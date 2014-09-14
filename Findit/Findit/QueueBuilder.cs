//this class throws things onto a queue - one for each processor
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Findit
{
    class QueueBuilder
    {        
        private int _searchThreadCount = 0;
        private string[] _filePatternsToMatch = {};
        private string[] _rootFoldersToSearch = { };
        private string _currentFolder = String.Empty;
        
        //constructor
        public QueueBuilder(string[] rootFoldersToSearch, string[] filePatternsToMatch, int searchThreadCount)
        {
            //initialization
            _searchThreadCount = searchThreadCount;
            List<FileQueue> processorQueues = new List<FileQueue>();
            for (int i = 0; i < searchThreadCount; ++i)
            {
                processorQueues.Add(new FileQueue());
            }
            _filePatternsToMatch = filePatternsToMatch;
            _rootFoldersToSearch = rootFoldersToSearch;
            _currentFolder = _rootFoldersToSearch[0];
        }

        public void BuildQueues()
        {
            int q = 0;
            System.IO.DirectoryInfo folder = new System.IO.DirectoryInfo(_currentFolder);
            if (!folder.Exists)
            {
                Globals.statBoard.FileFindingComplete = true;
                return;
            }
            foreach (string pattern in _filePatternsToMatch)
            {
                System.IO.FileInfo[] files = folder.GetFiles(pattern);
                List<QueuedFile> filesMatchingThisPattern = new List<QueuedFile>();
                foreach (System.IO.FileInfo f in files)
                {
                    QueuedFile qf = new QueuedFile();
                    qf.file = f;
                    qf.HasBeenSearched = false;
                    filesMatchingThisPattern.Add(qf);
                }

                for(int i = 0; i < filesMatchingThisPattern.Count; ++i)
                {
                    q = (q > _searchThreadCount - 1 ? 0 : q);
                    Globals.processorQueues[q++].filesToSearch.Add(filesMatchingThisPattern[i]);
                }
            }

            //now we have the full list of all files in this folder.  move on to sub-folders (if any).
            foreach (System.IO.DirectoryInfo subFolder in folder.GetDirectories())
            {
                _currentFolder = subFolder.FullName;
                BuildQueuesWithoutFinish();
            }

            Globals.statBoard.FileFindingComplete = true;
        }

        public void BuildQueuesWithoutFinish(int q = 0)
        {
            if(Globals.statBoard.Halt)
            {
                return;
            }
            System.IO.DirectoryInfo folder = new System.IO.DirectoryInfo(_currentFolder);
            foreach (string pattern in _filePatternsToMatch)
            {
                System.IO.FileInfo[] files = folder.GetFiles(pattern);
                List<QueuedFile> filesMatchingThisPattern = new List<QueuedFile>();
                foreach(System.IO.FileInfo f in files)
                {
                    QueuedFile qf = new QueuedFile();
                    qf.file = f;
                    qf.HasBeenSearched = false;
                    filesMatchingThisPattern.Add(qf);
                }
                for (int i = 0; i < filesMatchingThisPattern.Count; ++i)
                {
                    q = (q > _searchThreadCount - 1 ? 0 : q);
                    Globals.processorQueues[q++].filesToSearch.Add(filesMatchingThisPattern[i]);
                }
            }

            //now we have the full list of all files in this folder.  move on to sub-folders (if any).
            foreach (System.IO.DirectoryInfo subFolder in folder.GetDirectories())
            {
                //passing q as a param helps balance out the queues.
                //without that, all of the folders with small #s of files get lumped in the first few queues.
                //for small searches it doesnt matter much, but when you encompass many 10ks of files, it adds up to a big imbalance
                _currentFolder = subFolder.FullName;
                BuildQueuesWithoutFinish(q);  //recursive
            }
        }
    }
}