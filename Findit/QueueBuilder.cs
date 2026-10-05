/*
Walks the folder tree and throws every file that matches the user's patterns onto the one
shared queue, where the search threads take them off.

Two things this deliberately does not do any more:

 - It does not build a List<FileInfo> of a folder's contents and then walk that list.  It
   streams the folder with EnumerateFiles, so the first file reaches a search thread while
   the folder is still being read, and it never holds a whole folder in memory.  It queues
   paths, not FileInfo objects: a FileInfo carries a normalised copy of the path and a
   block of attributes that nothing here ever asked for.

 - It does not test each of a folder's files against a List of the excluded ones, one at a
   time.  That was a scan of the whole exclude list per file - fine for a handful, quadratic
   once "*.dll;*.pdb" matched a few thousand files in an output folder.  The excluded names
   go into a HashSet and each file is one lookup.
//*/
using System;
using System.Collections.Generic;
using System.IO;

namespace Findit
{
  class QueueBuilder
  {
    private readonly string[] _filePatternsToExclude;
    private readonly string _rootFolder;
    private readonly bool _recurse;
    private readonly StatusBoard _statBoard;
    private readonly FileQueue _queue;

    public string[] FilePatternsToMatch { get; set; }

    //constructor
    public QueueBuilder(UserParameters uparms, StatusBoard statBoard, FileQueue queue)
    {
      //the status board and the queue belong to one particular search and are handed to us.
      //we used to reach through Globals for both, which meant a builder left over from a
      //cancelled search would keep filling in whatever the *next* search was using.
      _statBoard = statBoard;
      _queue = queue;
      FilePatternsToMatch = UsablePatterns(uparms.FileNamePatterns);
      _filePatternsToExclude = uparms.SearchExcludeFiles;
      var rootFoldersToSearch = uparms.SearchPaths;
      _rootFolder = rootFoldersToSearch[0];
      _recurse = uparms.Recurse;
    }

    //a blank file pattern matches nothing at all, so emptying the "File type" box used to
    //produce a search that walked the entire tree and reported no files - with no hint that
    //the box was the reason.  Blank means "no filter", which is "*".
    private static string[] UsablePatterns(string[] patterns)
    {
      if ((patterns == null) || (0 == patterns.Length))
      {
        return new string[] { "*" };
      }

      List<string> kept = new List<string>(patterns.Length);
      foreach (string pattern in patterns)
      {
        if (!string.IsNullOrEmpty(pattern) && (0 < pattern.Trim().Length))
        {
          kept.Add(pattern.Trim());
        }
      }

      if (0 == kept.Count)
      {
        kept.Add("*");
      }
      return kept.ToArray();
    }

    //how many files travel to the search threads together.  see FileQueue's notes on why
    //they travel together at all.
    private const int c_BatchSize = 64;

    private QueuedFile[] _batch = new QueuedFile[c_BatchSize];
    private int _batchCount;

    public void BuildQueues()
    {
      //BuildQueues has no args because it has to be threadable.
      try
      {
        BuildQueuesInFolder(_rootFolder);
        FlushBatch();  //whatever is left over at the end of the walk
      }
      catch (Exception e)
      {
        //nothing is allowed to escape this method.  an unhandled exception on a background
        //thread takes the entire application down with it.
        _statBoard.UserFacingError = e.Message;
      }
      finally
      {
        //this is the one and only place that says "the walk is over", and both of these
        //have to happen on every possible way out of it.
        //CompleteAdding is what lets the search threads stop waiting for another file and
        //finish; without it they block until the application is killed.
        _queue.filesToSearch.CompleteAdding();
        _statBoard.FileFindingComplete = true;
      }
    }

    private void BuildQueuesInFolder(string folderName)
    {
      if (_statBoard.Halt)
      {
        return;
      }

      DirectoryInfo folder = new DirectoryInfo(folderName);
      if (!folder.Exists)
      {
        //a folder that is missing is just nothing to search.  note what this does NOT do:
        //it used to declare the whole search complete from here, at any depth in the tree,
        //which turned the grep threads loose on a list that was still being added to.
        return;
      }

      QueueMatchingFilesInFolder(folder);

      if (!_recurse)
      {
        return;
      }

      foreach (DirectoryInfo subFolder in SubFoldersOf(folder))
      {
        BuildQueuesInFolder(subFolder.FullName);
      }
    }

    private void QueueMatchingFilesInFolder(DirectoryInfo folder)
    {
      //what are we told to skip in this folder?
      HashSet<string> excludedNames = NamesMatchingPatterns(folder, _filePatternsToExclude);

      //only needed when more than one include pattern is in play: "*.cs" and "*.c*" both
      //match the same file, and it used to be queued - and searched - once for each.
      HashSet<string> alreadyQueued = (1 < FilePatternsToMatch.Length)
          ? new HashSet<string>(StringComparer.OrdinalIgnoreCase)
          : null;

      foreach (string pattern in FilePatternsToMatch)
      {
        if (_statBoard.Halt)
        {
          return;
        }

        foreach (string fullPath in EnumerateFilesSafely(folder, pattern))
        {
          string name = Path.GetFileName(fullPath);
          if (excludedNames.Contains(name))
          {
            continue;
          }
          if ((alreadyQueued != null) && !alreadyQueued.Add(name))
          {
            continue;
          }

          //no dealing files out to a particular thread any more: whichever searcher is
          //free takes the next batch off the shared queue.
          QueuedFile qf = new QueuedFile();
          qf.FullName = fullPath;
          qf.FolderName = folder.FullName;
          _statBoard.FilesToBeSearchedCount++;
          if (!QueueOneFile(qf))
          {
            return;  //cancelled, or the queue is closed
          }
        }
      }
    }

    private bool QueueOneFile(QueuedFile qf)
    {
      _batch[_batchCount++] = qf;

      //send it on when the batch is full - or early, if there is nothing on the queue at
      //all, because that means a search thread is sitting there with nothing to do and
      //would rather have four files now than sixty-four in a moment.
      if ((c_BatchSize == _batchCount) || _queue.IsEmpty)
      {
        return FlushBatch();
      }
      return true;
    }

    private bool FlushBatch()
    {
      if (0 == _batchCount)
      {
        return true;
      }

      //hand over exactly what is filled in.  the batch buffer itself is reused, so the
      //searchers must not be given a reference to it.
      QueuedFile[] sending = new QueuedFile[_batchCount];
      Array.Copy(_batch, sending, _batchCount);
      _batchCount = 0;
      return TryQueue(sending);
    }

    //how long we are willing to wait for room on the queue before looking up to check that
    //there is still somebody down there taking files off it
    private const int c_QueueWaitMs = 250;

    private bool TryQueue(QueuedFile[] batch)
    {
      //the queue has a ceiling on it, so a walk over a huge tree cannot get millions of
      //entries ahead of the threads doing the searching and take the memory with it.
      //Adding to a full one waits - and the two ways out of that wait are the cancel token
      //and noticing that every search thread has stopped, which is the case where nothing
      //is ever going to make room again.
      while (true)
      {
        try
        {
          if (_queue.filesToSearch.TryAdd(batch, c_QueueWaitMs, _statBoard.CancelToken))
          {
            return true;
          }
        }
        catch (OperationCanceledException)
        {
          return false;  //they clicked cancel while we were waiting for room
        }
        catch (InvalidOperationException)
        {
          return false;  //CompleteAdding already called - nothing more is going to be searched
        }

        if (_statBoard.Halt || _statBoard.AllGreppersFinished)
        {
          return false;
        }
      }
    }

    //the *names* of the files in this folder matching any of these patterns.  names, not
    //paths: every one of them came out of the same folder, so the name is what tells them
    //apart, and that is all the caller compares.
    private HashSet<string> NamesMatchingPatterns(DirectoryInfo folder, string[] patterns)
    {
      //file names are matched the way Windows matches them, without regard to case
      HashSet<string> result = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
      if (patterns == null)
      {
        return result;
      }

      foreach (string pattern in patterns)
      {
        if (string.IsNullOrEmpty(pattern))
        {
          continue;
        }
        foreach (string fullPath in EnumerateFilesSafely(folder, pattern))
        {
          result.Add(Path.GetFileName(fullPath));
        }
      }
      return result;
    }

    //whatever we cannot enumerate is skipped, not thrown.  the exclude-pattern scan used
    //to sit outside any try at all, so one folder we lacked permission to read threw all
    //the way out to the *caller*, which then silently abandoned every sibling folder it
    //had not walked yet - and at the top of the tree, hung the search outright.
    //
    //the try cannot wrap the foreach directly - you cannot yield from inside one - so the
    //enumerator is stepped by hand and the exception is caught around each step.  This
    //matters: with EnumerateFiles the access check happens as the folder is read, which is
    //partway through the loop, not up front the way GetFiles used to fail.
    private IEnumerable<string> EnumerateFilesSafely(DirectoryInfo folder, string pattern)
    {
      IEnumerator<string> walker;
      try
      {
        walker = Directory.EnumerateFiles(folder.FullName, pattern).GetEnumerator();
      }
      catch (UnauthorizedAccessException unauth)
      {
        _statBoard.UserFacingError = unauth.Message;
        yield break;
      }
      catch (ArgumentException)
      {
        //a pattern Windows will not accept - stray wildcards, invalid path characters
        yield break;
      }
      catch (PathTooLongException)
      {
        //names longer than 260 characters.  nothing to do but skip them.
        yield break;
      }
      catch (IOException)
      {
        //folder went away mid-walk, network drive dropped, and friends.
        yield break;
      }

      using (walker)
      {
        while (true)
        {
          try
          {
            if (!walker.MoveNext())
            {
              yield break;
            }
          }
          catch (UnauthorizedAccessException unauth)
          {
            _statBoard.UserFacingError = unauth.Message;
            yield break;
          }
          catch (PathTooLongException)
          {
            yield break;
          }
          catch (IOException)
          {
            yield break;
          }
          yield return walker.Current;
        }
      }
    }

    private List<DirectoryInfo> SubFoldersOf(DirectoryInfo folder)
    {
      List<DirectoryInfo> result = new List<DirectoryInfo>();
      try
      {
        foreach (DirectoryInfo sub in folder.GetDirectories())
        {
          //a junction or a symlink points at a folder that is somewhere else, quite
          //possibly a folder we are already inside.  Following them meant that searching
          //C:\ walked "C:\Users\someone\Application Data" - which points back at its own
          //parent - over and over until the recursion ran out of stack or the path ran out
          //of room.  Windows' own tools skip these; so do we.
          if (0 != (sub.Attributes & FileAttributes.ReparsePoint))
          {
            continue;
          }
          result.Add(sub);
        }
        return result;
      }
      catch (UnauthorizedAccessException unauth)
      {
        //a folder we are not allowed to look inside is not a reason to give up on the rest
        //of the search, which is exactly what used to happen.
        _statBoard.UserFacingError = unauth.Message;
        return result;
      }
      catch (PathTooLongException)
      {
        return result;
      }
      catch (IOException)
      {
        return result;
      }
    }
  }
}
