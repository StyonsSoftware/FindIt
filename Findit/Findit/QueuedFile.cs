using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Findit
{
    public class QueuedFile
    {
        public System.IO.FileInfo file;
        public bool HasBeenSearched;
        public long MatchLineNumber;
    }
}
