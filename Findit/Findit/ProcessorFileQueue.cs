using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Findit
{
    public class FileQueue
    {
        public List<QueuedFile> filesToSearch;
        public FileQueue()
        {
            filesToSearch = new List<QueuedFile>();
            filesToSearch.Clear();
        }
    }
}
