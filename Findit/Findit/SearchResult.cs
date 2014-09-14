using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Findit
{
    //structure of search results (we will have a big array of these)
    public struct SearchResult
    {
        public string FileName;
        public Int64 LineNumber;
    }
}
