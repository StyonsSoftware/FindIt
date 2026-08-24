//this class throws things onto a queue - one for each processor
using System;
using System.Collections.Generic;

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
      FilePatternsToMatch = uparms.FileNamePatterns;
      _filePatternsToExclude = uparms.SearchExcludeFiles;
      var rootFoldersToSearch = uparms.SearchPaths;
      _rootFolder = rootFoldersToSearch[0];
      _recurse = uparms.Recurse;
    }

    private bool IsFileInFileList(List<QueuedFile> filelist, System.IO.FileInfo f)
    {
      foreach (QueuedFile fileelement in filelist)
      {
        if (fileelement.file.Name == f.Name)
        {
          return true;
        }
      }
      return false;
    }

    public void BuildQueues()
    {
      //BuildQueues has no args because it has to be threadable.
      try
      {
        BuildQueuesInFolder(_rootFolder);
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

      System.IO.DirectoryInfo folder = new System.IO.DirectoryInfo(folderName);
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

      foreach (System.IO.DirectoryInfo subFolder in SubFoldersOf(folder))
      {
        BuildQueuesInFolder(subFolder.FullName);
      }
    }

    private void QueueMatchingFilesInFolder(System.IO.DirectoryInfo folder)
    {
      //what are we told to skip in this folder?
      List<QueuedFile> excludeFiles = FilesMatchingPatterns(folder, _filePatternsToExclude);

      foreach (string pattern in (FilePatternsToMatch ?? Util.EmptyStringArray()))
      {
        //this list has to be built fresh for each pattern.  it used to be shared by all of
        //them while the loop below always started over at index 0, so the first pattern's
        //files got queued again once per additional pattern.
        List<QueuedFile> includeFiles = FilesMatchingPatterns(folder, new string[] { pattern });

        foreach (QueuedFile qf in includeFiles)
        {
          if (!IsFileInFileList(excludeFiles, qf.file))
          {
            //no dealing files out to a particular thread any more: whichever searcher is
            //free takes the next one off the shared queue.
            _queue.filesToSearch.Add(qf);
            _statBoard.FilesToBeSearchedCount++;
          }
        }
      }
    }

    private List<QueuedFile> FilesMatchingPatterns(System.IO.DirectoryInfo folder, string[] patterns)
    {
      //whatever we cannot enumerate is skipped, not thrown.  the exclude-pattern scan used
      //to sit outside any try at all, so one folder we lacked permission to read threw all
      //the way out to the *caller*, which then silently abandoned every sibling folder it
      //had not walked yet - and at the top of the tree, hung the search outright.
      List<QueuedFile> result = new List<QueuedFile>();
      if (patterns == null)
      {
        return result;
      }

      foreach (string pattern in patterns)
      {
        try
        {
          foreach (System.IO.FileInfo f in folder.GetFiles(pattern))
          {
            QueuedFile qf = new QueuedFile();
            qf.file = f;
            qf.HasBeenSearched = false;
            result.Add(qf);
          }
        }
        catch (UnauthorizedAccessException unauth)
        {
          _statBoard.UserFacingError = unauth.Message;
        }
        catch (System.IO.PathTooLongException)
        {
          //names longer than 260 characters.  nothing to do but skip them.
        }
        catch (System.IO.IOException)
        {
          //folder went away mid-walk, network drive dropped, and friends.
        }
      }
      return result;
    }

    private List<System.IO.DirectoryInfo> SubFoldersOf(System.IO.DirectoryInfo folder)
    {
      try
      {
        return new List<System.IO.DirectoryInfo>(folder.GetDirectories());
      }
      catch (UnauthorizedAccessException unauth)
      {
        //a folder we are not allowed to look inside is not a reason to give up on the rest
        //of the search, which is exactly what used to happen.
        _statBoard.UserFacingError = unauth.Message;
        return new List<System.IO.DirectoryInfo>();
      }
      catch (System.IO.PathTooLongException)
      {
        return new List<System.IO.DirectoryInfo>();
      }
      catch (System.IO.IOException)
      {
        return new List<System.IO.DirectoryInfo>();
      }
    }
  }
}
