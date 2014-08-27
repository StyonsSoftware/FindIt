/*
 * The Grepper class is fundamentally about searching for a set of strings within text files.
 * All options are specified using a "UserParameters" object.
 * You execute a search using your parameters using the public 'Search' method.
 * 
 * A noteworthy option is the ability to specify strings that may *not* exist in the files
 * discovered by the search.
 * 
 * Search results get dumped into an array of type 'SearchResult', which is
 * just a big list of files that contain your string + the line # they occur on.
 * 
 * Returning them in this way makes it a lot easier to write threaded applications who
 * can start a search and just monitor the Pile O'Results periodically.
 * 
 * There is also some non-core functionality to help with debugging and general curiosity.
 * -A 'PerfStat' structure that collects performance data about the speed of the search
 * -A 'Notifications' array that records lots of (usually) useless notifications
 * -A 'LastSearchedFolder' field that is a gratuitous gimme to any GUI app that wants to give feedback
 * -An 'Exceptions' array that holds exceptions, mostly for debugging.
 *  Exceptions are usually just 'permission denied' errors if the user tried to search through folders
 *  where they don't have read access.
 *  
   Example usage:
            GrepTool.Grepper.UserParameters uparms = new GrepTool.Grepper.UserParameters();
            uparms.SearchPath = "C:\\some_folder";
            uparms.SearchStrings = {"File must contain this","And this"};
            uparms.Recurse = true;
            uparms.Verbosity = 0;
            uparms.CaseSensitive = false;
            uparms.IncludeLineNumbers = false;
            uparms.Remind = true;
            uparms.SearchExtension = "*.xml";
            uparms.ShowPerfStats = true;
            uparms.AbsentStrings = {"File cannot contain this","Or this"};
            uparms.Crippled = false;
            uparms.OnlyFileNames = false;

            gp = new GrepTool.Grepper(uparms);
            searchThread = new Thread(gp.Search);
            //at this point, a thread is running your search, and gp.SearchResult will have the list of results

//*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GrepTool
{
    public class Grepper
    {
        //structure to hold user parameters
        public struct UserParameters
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
            public Boolean IncludeOffice;
        }

        //structure to hold performance data
        public struct PerfStat
        {
            public float ElapsedSeconds;
            public Int64 FilesSearched;
            public Int64 LinesSearched;
            public Int64 Matches;
            public Int64 Skipped;
            public Int64 Errors;
        }

        //structure of search results (we will have a big array of these)
        public struct SearchResult
        {
            public string FileName;
            public Int64 LineNumber;
        }

        //do we record every single minute little detail?
        //if so, then they get their own array similar to search results
        public Boolean RecordNotifications
        {
            get
            {
                return m_RecordNotifications;
            }
            set
            {
                if (m_RecordNotifications)
                {
                    //we *were* recording before
                    if (!value)
                    {
                        //but now we are *not*, so clear the old stuff away
                        Array.Clear(Notifications, 0, Notifications.Length);
                    }
                }
                else
                {
                    //we were *not* recording before
                    if (value)
                    {
                        //but now we are, so create some storage space
                        Notifications = new string[c_CacheSize];
                        idxNotifications = 0;
                    }
                }
                m_RecordNotifications = value;
            }
        }

        //do we record every single exception?
        //if so, they get their own array similar to search results
        public Boolean RecordExceptions
        {
            get
            {
                return m_RecordExceptions;
            }
            set
            {
                if (m_RecordExceptions)
                {
                    //we *were* recording before
                    if (!value)
                    {
                        //but now we are *not*, so clear the old stuff away
                        Array.Clear(Exceptions, 0, Exceptions.Length);
                    }
                }
                else
                {
                    //we were *not* recording before
                    if (value)
                    {
                        //but now we are, so create some storage space
                        Exceptions = new string[c_CacheSize];
                        idxExceptions = 0;
                    }
                }
                m_RecordExceptions = value;
            }
        }

        //this is nice for callers to show progress (searching folder c:\1, c:\2, c:\2, etc)
        public string LastSearchedFolder = "";

        private Boolean m_RecordNotifications = false;
        private Boolean m_RecordExceptions = false;

        private const Int64 c_CacheSize = 50000;
        private const int c_CrippleWaitMs = 100; //higher # here == more pain when they don't register
        private TimeSpan OneMs = new TimeSpan(TimeSpan.TicksPerMillisecond);

        public SearchResult[] SearchResults = new SearchResult[c_CacheSize];
        public int SearchResultCount = 0;

        public string[] Exceptions = new string[c_CacheSize];
        private int idxExceptions = 0;

        public string[] Notifications;
        private int idxNotifications = 0;

        public PerfStat PerformanceStats;
        private UserParameters UserPrefs;
        private static System.Diagnostics.Stopwatch Swatch = new System.Diagnostics.Stopwatch();
      
        public Grepper(UserParameters prefs)
        {
            Swatch = new System.Diagnostics.Stopwatch();
            PerformanceStats.ElapsedSeconds = 0;
            PerformanceStats.FilesSearched = 0;
            PerformanceStats.LinesSearched = 0;
            PerformanceStats.Errors = 0;
            PerformanceStats.Skipped = 0;
            PerformanceStats.Matches = 0;
            RecordExceptions = prefs.Verbosity > 0;
            RecordNotifications = prefs.Verbosity > 1;
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
                PerformanceStats.ElapsedSeconds = 0;
                PerformanceStats.FilesSearched = 0;
                PerformanceStats.LinesSearched = 0;
                Swatch.Reset();
                Swatch.Start();

                //rubber hits road in here
                ListMatchesInFolder(UserPrefs.SearchPath);

                Swatch.Stop();
                PerformanceStats.ElapsedSeconds = (((float)(Swatch.ElapsedMilliseconds)) / 1000);
            }
            catch (Exception e)
            {
                StoreException(e.Message);
            }
        }

        private Boolean IsFileInFileArray(ref System.IO.FileInfo[] arry, System.IO.FileInfo f)
        {
            foreach (System.IO.FileInfo fileelement in arry)
            {
                if (fileelement.Name == f.Name)
                {
                    return true;
                }
            }
            return false;
        }

        private void ListMatchesInFolder(string FolderToSearch)
        {
            //this is where the fluff comes to an end and we actually search for text in files
            try
            {
                LastSearchedFolder = FolderToSearch;
                StoreNotification("Searching folder: '" + FolderToSearch + "'");
                System.IO.DirectoryInfo folder = new System.IO.DirectoryInfo(FolderToSearch);

                //get the list of matching files, and also the list of excluded files.
                //subtract the excluded from the matches, then search among that final list.
                System.IO.FileInfo[] ExcludedFiles = folder.GetFiles(UserPrefs.SearchExcludeFiles);
                System.IO.FileInfo[] IncludedFiles = folder.GetFiles(UserPrefs.SearchExtension);
                System.IO.FileInfo[] FilesToSearch = { };
                Array.Resize(ref FilesToSearch, IncludedFiles.Length);
                int idx = 0;
                foreach (System.IO.FileInfo f in IncludedFiles)
                {
                    if (!IsFileInFileArray(ref ExcludedFiles,f))
                    {
                        FilesToSearch[idx++] = f;
                    }
                }

                if (UserPrefs.OnlyFileNames)
                {
                    PerformanceStats.FilesSearched += folder.GetFiles().Length;
                }

                Array.Resize(ref FilesToSearch, idx);

                foreach (System.IO.FileInfo currFile in FilesToSearch)
                {
                    Int64 MatchLine = -1;
                    Int64 UnmatchLine = -1;

                    if (UserPrefs.OnlyFileNames)
                    {
                        PerformanceStats.ElapsedSeconds = (((float)(Swatch.ElapsedMilliseconds)) / 1000);
                        SearchResults[SearchResultCount].LineNumber = 0;
                        SearchResults[SearchResultCount].FileName = currFile.FullName;
                        SearchResultCount++;
                        PerformanceStats.Matches++;
                    }
                    else
                    {
                        foreach (string s in UserPrefs.SearchStrings)
                        {
                            MatchLine = IsTextInFile(currFile.FullName, s, UserPrefs.CaseSensitive, UserPrefs.IncludeOffice);
                            if (-1 == MatchLine)
                            {
                                break;
                            }
                        }

                        foreach (string s in UserPrefs.AbsentStrings)
                        {
                            if (0 < s.Trim().Length)
                            {
                                UnmatchLine = IsTextInFile(currFile.FullName, s, UserPrefs.CaseSensitive, UserPrefs.IncludeOffice);
                                if (-1 != UnmatchLine)
                                {
                                    StoreNotification("Found unwanted text " + s + " in file " + currFile.FullName);
                                    break;
                                }
                            }
                        }

                        PerformanceStats.FilesSearched++;
                        PerformanceStats.ElapsedSeconds = (((float)(Swatch.ElapsedMilliseconds)) / 1000);
                        if ((-1 < MatchLine) && (-1 == UnmatchLine))
                        {
                            string newline = currFile.FullName;
                            SearchResults[SearchResultCount].LineNumber = MatchLine;
                            SearchResults[SearchResultCount].FileName = newline;
                            SearchResultCount++;
                            PerformanceStats.Matches++;
                        }
                        else
                        {
                            StoreNotification("NOMATCH: " + currFile.FullName);
                        }

                        //force the search to run slowly when in 'crippled' mode.
                        //this is to support the idea of a limited functionality
                        //mode after a trial period has expired.
                        if (UserPrefs.Crippled)
                        {
                            WaitWithoutSleeping(c_CrippleWaitMs);
                        }
                    }
                }
                if (UserPrefs.Recurse)
                {
                    foreach (System.IO.DirectoryInfo subFolder in folder.GetDirectories())
                    {
                        ListMatchesInFolder(subFolder.FullName);  //recursive
                    }
                }
            }
            catch (Exception e)
            {
                StoreException("Exception in folder '" + FolderToSearch + "'" + 
                    ".  The exception was: '" + e.Message + "'");
            }
        }

        private void WaitWithoutSleeping(int MsToWait)
        {
            //since Grepper might be running in a thread, we cannot just call
            //Thread.Sleep, since that would indicate to a caller that we are
            //no longer active.  instead this function will simulate the 'sleep' function
            //by doing busywork until N milliseconds have passed

            DateTime waituntil = DateTime.Now;

            for (int i = 0; i < MsToWait; ++i)
            {
                waituntil = waituntil + OneMs;
            }

            while (DateTime.Now < waituntil)
            {
                //busywork
                int x = 0;
                ++x;
                if (x > 100000)
                {
                    break;
                }
            }
        }

        private Int64 IsTextInOfficeDocument(string filename, string searchtext, Boolean casesensitive, Boolean includeOffice)
        {
            //look for a match in a file, quit as soon as you find it
            try
            {
                Int64 currlinenum = 0;
                StoreNotification("Searching file '" + filename + "'");
                System.IO.TextReader reader = new EPocalipse.IFilter.FilterReader(filename);
                string[] completeText = reader.ReadToEnd().Split('\n');
                try
                {
                    if (0 < completeText.Length)
                    {
                        for (int i = 0; i < completeText.Length; ++i)
                        {
                            string currentLine = completeText[i];
                            PerformanceStats.LinesSearched++;
                            currlinenum++;
                            if (-1 < currentLine.IndexOf(searchtext, (casesensitive ? StringComparison.CurrentCulture : StringComparison.CurrentCultureIgnoreCase)))
                            {
                                return currlinenum;
                            }
                        }
                    }
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
                //"File in use by another process" exception
                StoreException("'" + filename + "' is being used by another process.");
                return -1;
            }
            catch (Exception e)
            {
                //all other exceptions
                StoreException("Exception in file '" + filename + "': '" + e.Message + "'");
                return -1;
            }
        }

        private Int64 IsTextInFile(string filename, string searchtext, Boolean casesensitive, Boolean includeOffice)
        {
            //look for a match in a file, quit as soon as you find it
            try
            {
                Int64 currlinenum = 0;
                StoreNotification("Searching file '" + filename + "'");
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
                            PerformanceStats.LinesSearched++;

                            currlinenum++;
                            if (!includeOffice && !BinaryChecked && IsBinary(currentLine))
                            {
                                StoreNotification("Skipped binary file " + filename);
                                PerformanceStats.Skipped++;
                                BinaryChecked = true;
                                return -1;
                            }
                            else if (includeOffice)
                            {
                                return IsTextInOfficeDocument(filename, searchtext, casesensitive, includeOffice);
                            }
                            if (-1 < currentLine.IndexOf(searchtext, (casesensitive ? StringComparison.CurrentCulture : StringComparison.CurrentCultureIgnoreCase)))
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
                //"File in use by another process" exception
                StoreException("'" + filename + "' is being used by another process.");
                return -1;
            }
            catch (Exception e)
            {
                //all other exceptions
                StoreException("Exception in file '" + filename + "': '" + e.Message + "'");
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
                StoreException("Exception while trying to detect whether a file was binary: '" + e.Message + "'");
                return true;
            }
        }
        
        private void StoreNotification(string notifymsg)
        {
            if (RecordNotifications)
            {
                Notifications[idxNotifications++] = notifymsg;
            }
        }

        private void StoreException(string exceptmsg)
        {
            //always record the count, but don't store the message unless they asked
            PerformanceStats.Errors++;
            if (RecordExceptions)
            {
                Exceptions[idxExceptions++] = exceptmsg;
            }
        }
    }
}
