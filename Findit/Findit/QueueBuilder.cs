//this class throws things onto a queue - one for each processor
using System;
using System.Collections.Generic;

namespace Findit
{
  class QueueBuilder
  {
    private readonly int _searchThreadCount;
    private readonly string[] _filePatternsToExclude;
    private string _currentFolder;
    private readonly bool _recurse;

    public string[] FilePatternsToMatch { get; set; }

    //constructor
    public QueueBuilder(UserParameters uparms, int searchThreadCount)
    {
      //initialization
      _searchThreadCount = searchThreadCount;
      List<FileQueue> processorQueues = new List<FileQueue>();
      for (int i = 0; i < searchThreadCount; ++i)
      {
        processorQueues.Add(new FileQueue());
      }
      FilePatternsToMatch = uparms.FileNamePatterns;
      _filePatternsToExclude = uparms.SearchExcludeFiles;
      var rootFoldersToSearch = uparms.SearchPaths;
      _currentFolder = rootFoldersToSearch[0];
      _recurse = uparms.Recurse;
    }

    private bool IsFileInFileList(ref List<QueuedFile> filelist, System.IO.FileInfo f)
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
      BuildQueuesWithStartPoint(0, true);
    }

    private void BuildQueuesWithStartPoint(int q = 0, bool root = false)
    {
      if (Globals.statBoard.Halt)
      {
        return;
      }

      System.IO.DirectoryInfo folder = new System.IO.DirectoryInfo(_currentFolder);

      if (!folder.Exists)
      {
        Globals.statBoard.FileFindingComplete = true;
        return;
      }

      List<QueuedFile> excludeFiles = new List<QueuedFile>();
      if (_filePatternsToExclude != null)
      {
        foreach (string pattern in _filePatternsToExclude)
        {
          System.IO.FileInfo[] files = folder.GetFiles(pattern);

          foreach (System.IO.FileInfo f in files)
          {
            QueuedFile qf = new QueuedFile();
            qf.file = f;
            qf.HasBeenSearched = false;
            excludeFiles.Add(qf);
          }
        }
      }
      try
      {
        List<QueuedFile> includeFiles = new List<QueuedFile>();
        foreach (string pattern in FilePatternsToMatch)
        {
          try
          {
            System.IO.FileInfo[] files = folder.GetFiles(pattern);

            foreach (System.IO.FileInfo f in files)
            {
              if (!IsFileInFileList(ref excludeFiles, f))
              {
                QueuedFile qf = new QueuedFile();
                qf.file = f;
                qf.HasBeenSearched = false;
                includeFiles.Add(qf);
              }
            }

            for (int i = 0; i < includeFiles.Count; ++i)
            {
              q = (q > _searchThreadCount - 1 ? 0 : q);
              Globals.processorQueues[q++].filesToSearch.Add(includeFiles[i]);
              Globals.statBoard.FilesToBeSearchedCount++;
            }
          }
          catch
          {
            //ignore exceptions here (typically a "file name too long" error
          }
        }

        //now we have the full list of all files in this folder.  move on to sub-folders (if any).
        foreach (System.IO.DirectoryInfo subFolder in folder.GetDirectories())
        {
          _currentFolder = subFolder.FullName;

          //passing q as a param helps balance out the queues.
          //without that, all of the folders with small #s of files get lumped in the first few queues.
          //for small searches it doesnt matter much, but when you encompass many 10ks of files, it adds up to a big imbalance
          if (_recurse)
          {
            BuildQueuesWithStartPoint(q);  //recursive
          }
        }
        if (root)
        {
          Globals.statBoard.FileFindingComplete = true;
        }
      }
      catch (UnauthorizedAccessException unauth)
      {
        //Globals.statBoard.FileFindingComplete = true;
        Globals.statBoard.UserFacingError = unauth.Message;
      }
      catch (System.IO.PathTooLongException)
      {
        //Globals.statBoard.FileFindingComplete = true;
        //Globals.statBoard.UserFacingError = toolong.Message;
      }
    }
  }
}