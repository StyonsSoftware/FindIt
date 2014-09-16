using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Findit
{
    public struct UserParameters
    {
        public int Verbosity;
        public Boolean ShowPerfStats;
        public string[] SearchPaths;
        public string[] SearchStrings;
        public string[] FileNamePatterns;
        public string[] SearchExcludeFiles;
        public Boolean IncludeLineNumbers;
        public Boolean Recurse;
        public Boolean CaseSensitive;
        //setting 'remind' to true will have no effect in a gui app.
        //it is a command-line app feature.
        public Boolean Remind;
        public string[] AbsentStrings;
        public Boolean Crippled;
        public Boolean OnlyFileNames;
        public Boolean IncludeOffice;

        public void Print()
        {
            Console.WriteLine("Folders:");
            foreach(string s in SearchPaths)
            {
                Console.WriteLine(s);
            }
            Console.WriteLine("Terms:");
            foreach (string s in SearchStrings)
            {
                Console.WriteLine(s);
            }
            Console.WriteLine("File patterns:");
            foreach (string s in FileNamePatterns)
            {
                Console.WriteLine(s);
            }
        }

    }
}
