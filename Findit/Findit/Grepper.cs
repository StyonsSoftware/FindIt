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
using System.Runtime.InteropServices;
using System.Text;

namespace Findit
{
    public class Grepper
    {
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

        private UserParameters _userPrefs;
        private int _threadIndex;
        private int _lastFileSearched = 0;

        public PerfStat perfStats;

        public Grepper(UserParameters prefs, int threadIndex)
        {
            RecordExceptions = prefs.Verbosity > 0;
            RecordNotifications = prefs.Verbosity > 1;
            if ((prefs.FileNamePatterns != null) && (0 == prefs.FileNamePatterns.Length))
            {
                string[] defaultPattern = { "*.*" };
                prefs.FileNamePatterns = defaultPattern;
            }

            if ((prefs.SearchExcludeFiles != null) && (0 == prefs.SearchExcludeFiles.Length))
            {
                string[] defaultExcludePattern = {""};
                prefs.SearchExcludeFiles = defaultExcludePattern;
            }
            _userPrefs = prefs;
            _threadIndex = threadIndex;
            Globals.statBoard.GrepComplete[_threadIndex] = false;
            InitializePerformanceStats();
        }

        private void InitializePerformanceStats()
        {
            perfStats.FilesMatched = 0;
            perfStats.FilesUnmatched = 0;
            perfStats.LinesSearched = 0;
            perfStats.FileErrorCount = 0;
            perfStats.BinarySkipped = 0;
        }

        public void Search()
        {
            //this method is going to be running in a thread.
            //it needs to watch for new files thrown onto the queue to be processed.
            //when it sees new files, it needs to grab them and search them.
            //any matches get thrown onto SearchResults, where someone else can pick them up
            try
            {
                //the list of files to be searched is being built by someone else.
                //that "someone else" will tell us when the list is complete by setting the FileFindingComplete flag.
                //until then, keep watching the list for any new entries.
                while (!Globals.statBoard.FileFindingComplete)
                {
                    int count = Globals.processorQueues[_threadIndex].filesToSearch.Count;
                    //go from wherever i left off to the (new) end of my to-do list
                    for (int i = _lastFileSearched; i < count; ++i)
                    {
                        if (Globals.processorQueues[_threadIndex].filesToSearch[i] != null)
                        {
                            string currentFilename = String.Empty;
                            try
                            {
                                currentFilename = Globals.processorQueues[_threadIndex].filesToSearch[i].file.FullName;
                            }
                            catch (System.IO.PathTooLongException)
                            {
                                //file names longer than 260 characters will generate this exception, which we ignore.
                                Globals.processorQueues[_threadIndex].filesToSearch[i].HasBeenSearched = true;
                            }
                            if(_userPrefs.OnlyFileNames)
                            {
                                RecordPositiveMatch(currentFilename, 0);
                                perfStats.FilesMatched = Globals.processorQueues[_threadIndex].filesToSearch.Count;
                                break;
                            }
                            if (!Globals.processorQueues[_threadIndex].filesToSearch[i].HasBeenSearched)
                            {
                                //ok, this file has not been searched.  search it, if possible.
                                Int64 matchingLineNumber = -1;
                                Int64 unmatchingLineNumber = -1;
                                Globals.statBoard.FilesSearched++;
                                Globals.statBoard.LastSearchedFolder = Globals.processorQueues[_threadIndex].filesToSearch[i].file.DirectoryName;
                                foreach (string term in _userPrefs.SearchStrings)
                                {
                                    matchingLineNumber = IsTextInFile(currentFilename, term, _userPrefs.CaseSensitive, _userPrefs.IncludeOffice);
                                    Globals.processorQueues[_threadIndex].filesToSearch[i].MatchLineNumber = matchingLineNumber;
                                    Globals.processorQueues[_threadIndex].filesToSearch[i].HasBeenSearched = true;
                                    if (-1 == matchingLineNumber)
                                    {
                                        break;  //both terms are required.  if one is missing, we can quit.
                                    }
                                }

                                //now make sure we dont have any unwanted terms
                                foreach(string term in _userPrefs.AbsentStrings)
                                {
                                    unmatchingLineNumber = IsTextInFile(currentFilename, term, _userPrefs.CaseSensitive, _userPrefs.IncludeOffice);
                                    if (-1 != unmatchingLineNumber)
                                    {
                                        break;  //the presence of any of these terms is enough to fail.
                                    }
                                }
                                if ((-1 < matchingLineNumber) && (-1 == unmatchingLineNumber))
                                {
                                    RecordPositiveMatch(currentFilename, matchingLineNumber);
                                }
                                else
                                {
                                    perfStats.FilesUnmatched++;
                                }
                            }
                            else
                            {
                                StoreException("File " + currentFilename + " had already been searched.");
                            }
                        }
                        else
                        {
                            StoreException("Index " + i.ToString() + " was null");
                        }
                    }
                    _lastFileSearched = count;
                }

                //now that the search is done, do one last pass to make sure we didnt miss anything
                foreach (QueuedFile qf in Globals.processorQueues[_threadIndex].filesToSearch)
                {
                    if (!qf.HasBeenSearched)
                    {
                        Globals.statBoard.FilesSearched++;
                        Int64 matchingLineNumber = -1;
                        Int64 unmatchingLineNumber = -1;

                        if (_userPrefs.OnlyFileNames)
                        {
                            RecordPositiveMatch(qf.file.FullName, 0);
                            perfStats.FilesMatched = Globals.processorQueues[_threadIndex].filesToSearch.Count;
                            break;
                        }
                        foreach (string term in _userPrefs.SearchStrings)
                        {
                            try
                            {
                                matchingLineNumber = IsTextInFile(qf.file.FullName, term, _userPrefs.CaseSensitive, _userPrefs.IncludeOffice);
                            }
                            catch (System.IO.PathTooLongException)
                            {
                                matchingLineNumber = -1;
                            }
                            qf.MatchLineNumber = matchingLineNumber;
                            qf.HasBeenSearched = true;
                            if (-1 == matchingLineNumber)
                            {
                                break;
                            }
                        }

                        //now make sure we dont have any unwanted terms
                        foreach (string term in _userPrefs.AbsentStrings)
                        {
                            try
                            {
                                unmatchingLineNumber = IsTextInFile(qf.file.FullName, term, _userPrefs.CaseSensitive, _userPrefs.IncludeOffice);
                            }
                            catch (System.IO.PathTooLongException)
                            {
                                matchingLineNumber = -1;
                            }
                            if (-1 != unmatchingLineNumber)
                            {
                                break;  //the presence of any of these terms is enough to fail.
                            }
                        }
                        if ((-1 < matchingLineNumber) && (-1 == unmatchingLineNumber))
                        {
                            RecordPositiveMatch(qf.file.FullName,matchingLineNumber);
                        }
                        else
                        {
                            perfStats.FilesUnmatched++;
                        }
                    }
                }
                Globals.statBoard.GrepComplete[_threadIndex] = true;
            }
            catch (System.IO.PathTooLongException e)
            {
                StoreException(e.Message);
            }
            catch (Exception e)
            {
                StoreException(e.Message);
                if (_threadIndex < Globals.statBoard.GrepComplete.Length)
                {
                    Globals.statBoard.GrepComplete[_threadIndex] = true;
                }
            }
        }

        [DllImport("kernel32.dll", CharSet = CharSet.Auto)]
        public static extern int GetShortPathName(
                 [MarshalAs(UnmanagedType.LPTStr)]
                   string path,
                 [MarshalAs(UnmanagedType.LPTStr)]
                   StringBuilder shortPath,
                 int shortPathLength
                 );

        public static string LongFileNameTo83Format(string longFileName)
        {
            StringBuilder shortPath = new StringBuilder(255);
            GetShortPathName(longFileName, shortPath, shortPath.Capacity);
            return shortPath.ToString();
        }

        private void RecordPositiveMatch(string currentFilename, Int64 lineNumber)
        {
            SearchResults[SearchResultCount].FileName = currentFilename;
            SearchResults[SearchResultCount].LineNumber = lineNumber;
            perfStats.FilesMatched++;
            SearchResultCount++;
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
                int currlinenum = 0;
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
                            currlinenum++;
                            if(!BinaryChecked && IsBinary(currentLine))
                            {
                                BinaryChecked = true;
                                if(includeOffice && IsOfficeDocument(filename))
                                {
                                    return -1;// IsTextInOfficeDocument(filename, searchtext, casesensitive, includeOffice);
                                }
                                else
                                {
                                    RecordBinaryFile(filename);
                                    return -1;
                                }
                            }
                            else if (-1 < currentLine.IndexOf(searchtext, (casesensitive ? StringComparison.CurrentCulture : StringComparison.CurrentCultureIgnoreCase)))
                            {
                                perfStats.LinesSearched += currlinenum;  //we stopped searching at this line #
                                return currlinenum;
                            }
                        }
                        else
                        {
                            RecordBinaryFile(filename);
                            return -1;
                        }
                    } while (!reader.EndOfStream);
                    perfStats.LinesSearched += currlinenum;  //we searched every line in this file
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

        private void RecordBinaryFile(string filename)
        {
            StoreNotification("Skipped binary file " + filename);
            perfStats.BinarySkipped++;
        }

        private bool IsOfficeDocument(string filename)
        {
            //pretty low-tech here
            if (System.IO.File.Exists(filename))
            {
                string[] dots = filename.Split('.');
                if (0 < dots.Length)
                {
                    string fileextension = dots[dots.Length - 1].ToUpper();
                    string[] officeextensions = { "DOCX", "XLSX", "PPTX", "DOC", "XLS", "PPT" };
                    foreach (string s in officeextensions)
                    {
                        if (fileextension == s)
                        {
                            return true;
                        }
                    }
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
            return false;
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
            if (RecordExceptions)
            {
                Exceptions[idxExceptions++] = exceptmsg;
            }
        }
    }
}