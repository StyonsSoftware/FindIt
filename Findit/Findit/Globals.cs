//a place for data structures shared across threads
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Findit
{
    public static class Globals
    {
        public static StatusBoard statBoard;
        public static List<FileQueue> processorQueues = new List<FileQueue>();
        public static int RecommendedSearchThreadCount = Math.Max(Environment.ProcessorCount - 3 , 1);
    }
}
