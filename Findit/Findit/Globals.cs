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
        //one queue shared by every search thread - see ProcessorFileQueue.cs
        public static FileQueue fileQueue = new FileQueue();
        public static int RecommendedSearchThreadCount = Math.Max(Environment.ProcessorCount - 3 , 1);
    }
}
