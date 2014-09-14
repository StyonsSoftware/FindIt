using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Findit
{
    //structure to hold performance data.  each thread must keep their own records for thread-safety reasons.
    //(example: two threads increment the "files searched" counter at the same time, resulting in one of 
    //their increments being lost).
    public struct PerfStat
    {
        public int LinesSearched;   //# of lines actually scanned for results.  does not include lines beyond a "found" result.
        public int FilesMatched;    //# of files matching the search criteria.
        public int FilesUnmatched;  //# of files searched, but found wanting
        public int BinarySkipped;   //# of files skipped because they were binary files.
        public int FileErrorCount;  //# of errors encountered during the search operation
        public int TotalFilesProcessed
        {
            get
            {
                //at the end of a search, this total should match the # of files in the "to be searched" queue for a given thread
                return FilesMatched + FilesUnmatched + FileErrorCount;
            }
        }
    }
}