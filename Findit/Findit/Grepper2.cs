using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Findit
{
    class Grepper2
    {
        //structure to hold user parameters
        public struct UserParameters2
        {
            public int Verbosity;
            public Boolean ShowPerfStats;
            public string SearchPath;
            public string[] SearchStrings;
            public string SearchExtension;
            public string SearchExcludeFiles;
            public Boolean IncludeLineNumbers;
            public Boolean Recurse;
            public Boolean CaseSensitive;
            //setting 'remind' to true will have no effect in a gui app.
            //it is a command-line app feature.
            public Boolean Remind;
            public string[] AbsentStrings;
            public Boolean Crippled;
            public Boolean OnlyFileNames;
        }

        //structure of search results (we will have a big array of these)
        public struct SearchResult2
        {
            public string FileName;
            public Int64 LineNumber;
        }

        private UserParameters2 UserPrefs;

        public List<string> FilesToSearch = new List<string>();

        public Grepper2(UserParameters2 prefs)
        {
            if (0 == prefs.SearchExtension.Trim().Length)
            {
                prefs.SearchExtension = "*.*";
            }
            if (0 == prefs.SearchExcludeFiles.Trim().Length)
            {
                prefs.SearchExcludeFiles = "";
            }
            UserPrefs = prefs;
        }

        public void Search()
        {
            try
            {
                //rubber hits road in here
                ListMatchesInFolder(UserPrefs.SearchPath);
            }
            catch (Exception e)
            {
            }
        }

        private void ListMatchesInFolder(string FolderToSearch)
        {
            //this is where the fluff comes to an end and we actually search for text in files
            try
            {
                foreach (string currFile in FilesToSearch)
                {
                    Int64 MatchLine = -1;
                    Int64 UnmatchLine = -1;
                    
                    foreach (string s in UserPrefs.SearchStrings)
                    {
                        MatchLine = IsTextInFile(currFile, s, UserPrefs.CaseSensitive);
                        if (-1 == MatchLine)
                        {
                            break;
                        }
                    }

                    foreach (string s in UserPrefs.AbsentStrings)
                    {
                        if (0 < s.Trim().Length)
                        {
                            UnmatchLine = IsTextInFile(currFile, s, UserPrefs.CaseSensitive);
                            if (-1 != UnmatchLine)
                            {
                                break;
                            }
                        }
                    }

                }
            }
            catch (Exception e)
            {
            }
        }

        private Int64 IsTextInFile(string filename, string searchtext, Boolean casesensitive)
        {
            //look for a match in a file, quit as soon as you find it
            try
            {
                Int64 currlinenum = 0;
                System.IO.StreamReader reader = new System.IO.StreamReader(filename);
                Boolean BinaryChecked = false;
                try
                {
                    string currentLine = "";
                    do
                    {
                        currentLine = reader.ReadLine();

                        //don't try to check binary files, and don't check for binariness > 1 time
                        if (currentLine != null)
                        {
                            if (!casesensitive)
                            {
                                currentLine = currentLine.ToUpper();
                                searchtext = searchtext.ToUpper();
                            }

                            currlinenum++;
                            if (!BinaryChecked && IsBinary(currentLine))
                            {
                                BinaryChecked = true;
                                return -1;
                            }

                            if (-1 < currentLine.IndexOf(searchtext))
                            {
                                return currlinenum;
                            }
                        }
                    } while (!reader.EndOfStream);
                    return -1;
                }
                finally
                {
                    reader.Close();
                    reader.Dispose();
                }
            }
            catch (System.IO.IOException)
            {
                return -1;
            }
            catch (Exception e)
            {
                return -1;
            }
        }

        private Boolean IsBinary(string line)
        {
            try
            {
                //lots of consecutive nulls indicate a binary file
                //if the line itself is empty, then assume it is text
                return !((line == null) || (-1 == line.IndexOf("\0\0\0\0\0\0\0")));
            }
            catch (Exception e)
            {
                return true;
            }
        }
    }
}
