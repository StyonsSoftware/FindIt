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
    public bool RecordExceptions
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

    private bool m_RecordNotifications;
    private bool m_RecordExceptions;

    private const Int64 c_CacheSize = 50000;
    //private const int c_CrippleWaitMs = 100; //higher # here == more pain when they don't register
    //private TimeSpan OneMs = new TimeSpan(TimeSpan.TicksPerMillisecond);

    public SearchResult[] SearchResults = new SearchResult[c_CacheSize];
    public int SearchResultCount;

    public string[] Exceptions = new string[c_CacheSize];
    private int idxExceptions;

    public string[] Notifications;
    private int idxNotifications;

    private UserParameters _userPrefs;
    private readonly int _threadIndex;
    private readonly StatusBoard _statBoard;
    private readonly FileQueue _queue;
    private bool _resultOverflowReported;

    public PerfStat perfStats;

    public Grepper(UserParameters prefs, int threadIndex, StatusBoard statBoard, FileQueue queue)
    {
      //the status board and the queue belong to one particular search and are handed to us.
      //we used to look both up through Globals on every single access, so a thread still
      //winding down from a cancelled search would read and write the *next* search's state
      //- and index off the end of it when that search used fewer threads.
      _statBoard = statBoard;
      _queue = queue;
      RecordExceptions = prefs.Verbosity > 0;
      RecordNotifications = prefs.Verbosity > 1;
      if ((prefs.FileNamePatterns != null) && (0 == prefs.FileNamePatterns.Length))
      {
        string[] defaultPattern = { "*.*" };
        prefs.FileNamePatterns = defaultPattern;
      }

      if ((prefs.SearchExcludeFiles != null) && (0 == prefs.SearchExcludeFiles.Length))
      {
        string[] defaultExcludePattern = { "" };
        prefs.SearchExcludeFiles = defaultExcludePattern;
      }
      _userPrefs = prefs;
      _threadIndex = threadIndex;
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
      //it takes files off the shared queue - waiting when the queue is empty - and searches
      //each one.  any matches get thrown onto SearchResults, where someone else can pick
      //them up
      try
      {
        SearchTheQueue();
      }
      catch (OperationCanceledException)
      {
        //they clicked cancel while we were waiting for the next file.  not an error.
      }
      catch (Exception e)
      {
        StoreException(e.Message);
      }
      finally
      {
        //the GUI decides a search is over once every thread has reported in right here.
        //this used to be the last statement of the try block, so *any* exception - and an
        //over-full results array threw one reliably - left this thread's slot false
        //forever.  the search then never ended and the app spun at full tilt until it was
        //killed.  reporting in belongs in a finally: every way out of this method is a way
        //this thread has stopped searching.
        System.Threading.Volatile.Write(ref _statBoard.GrepComplete[_threadIndex], true);
      }
    }

    private void SearchTheQueue()
    {
      //GetConsumingEnumerable blocks while the queue is empty and ends once the queue
      //builder has called CompleteAdding and we have drained what it left.
      //
      //this used to be a spin loop over a List<QueuedFile> that the builder was appending
      //to at the same time, followed by a second "did we miss anything" pass over the whole
      //list.  that burned a core per thread even with nothing to do, and raced with every
      //Add.  taking from the queue is now the only way a file gets searched, exactly once,
      //so HasBeenSearched and the catch-up pass are both gone.
      foreach (QueuedFile qf in _queue.filesToSearch.GetConsumingEnumerable(_statBoard.CancelToken))
      {
        if (_statBoard.Halt)
        {
          return;  //they clicked cancel
        }
        SearchOneFile(qf);
      }
    }

    private void SearchOneFile(QueuedFile qf)
    {
      string currentFilename;
      try
      {
        currentFilename = qf.file.FullName;
      }
      catch (System.IO.PathTooLongException)
      {
        //file names longer than 260 characters will generate this exception, which we ignore.
        perfStats.FileErrorCount++;
        return;
      }

      System.Threading.Interlocked.Increment(ref _statBoard.FilesSearched);
      try
      {
        _statBoard.LastSearchedFolder = qf.file.DirectoryName;
      }
      catch (System.IO.PathTooLongException)
      {
        //just the progress caption.  not worth failing the file over.
      }

      if (_userPrefs.OnlyFileNames)
      {
        //they only care that the name matched the file pattern, so there is nothing to read
        RecordPositiveMatch(currentFilename, 0);
        return;
      }

      TermMatch outcome = FindTermsInFile(currentFilename);
      if (outcome.Matched)
      {
        RecordPositiveMatch(currentFilename, outcome.LineNumber);
      }
      else
      {
        perfStats.FilesUnmatched++;
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

    private void RecordPositiveMatch(string currentFilename, long lineNumber)
    {
      if (SearchResultCount >= SearchResults.Length)
      {
        //the results array is a fixed size and we just filled it.  this used to walk
        //straight off the end, and the resulting exception killed the search thread before
        //it could report itself finished - so the whole app hung instead of just capping
        //the list.  say so once and keep going.
        if (!_resultOverflowReported)
        {
          _resultOverflowReported = true;
          _statBoard.UserFacingError = "More than " + SearchResults.Length.ToString()
              + " matches were found.  Only the first " + SearchResults.Length.ToString() + " are listed.";
        }
        return;
      }
      SearchResults[SearchResultCount].FileName = currentFilename;
      SearchResults[SearchResultCount].LineNumber = lineNumber;
      perfStats.FilesMatched++;
      SearchResultCount++;
    }

    //private Boolean IsFileInFileArray(ref System.IO.FileInfo[] arry, System.IO.FileInfo f)
    //{
    //    foreach (System.IO.FileInfo fileelement in arry)
    //    {
    //        if (fileelement.Name == f.Name)
    //        {
    //            return true;
    //        }
    //    }
    //    return false;
    //}

    //what one pass over one file concluded
    private struct TermMatch
    {
      public bool Matched;
      public long LineNumber;   //where the GUI should point its preview
    }

    /*
    Tracks which of the required terms have turned up and whether any forbidden one has, as
    the lines of a file are fed to it one at a time.

    This is here because a file used to be opened and read from the top once *per term* -
    a three-term search with two exclusions read every single file five times over.  Now the
    file is read once and every term is tested against each line as it goes past.
    //*/
    private class TermScanner
    {
      private readonly string[] _required;
      private readonly string[] _forbidden;
      private readonly StringComparison _comparison;
      private readonly long[] _firstLineOfRequired;   //0 means "not seen yet" - lines count from 1
      private int _requiredStillMissing;

      public TermScanner(string[] required, string[] forbidden, bool caseSensitive)
      {
        _required = required ?? new string[0];
        _forbidden = forbidden ?? new string[0];
        _comparison = caseSensitive ? StringComparison.CurrentCulture : StringComparison.CurrentCultureIgnoreCase;
        _firstLineOfRequired = new long[_required.Length];
        _requiredStillMissing = _required.Length;
      }

      public bool Disqualified { get; private set; }

      public bool Satisfied
      {
        get { return !Disqualified && (0 == _requiredStillMissing); }
      }

      //true once reading any further cannot change the answer.  note that finding every
      //required term is not enough on its own: if there are terms the file must NOT
      //contain, we have to read to the end to know it does not contain them.
      public bool AnswerIsFinal
      {
        get { return Disqualified || (Satisfied && (0 == _forbidden.Length)); }
      }

      //the old code reported whichever line satisfied the *last* search term, because it
      //overwrote the line number once per term as it worked through them.  keeping that, so
      //the preview pane lands where it always did.
      public long ReportableLineNumber
      {
        get
        {
          if (0 == _required.Length)
          {
            return 0;
          }
          return _firstLineOfRequired[_required.Length - 1];
        }
      }

      public void Feed(string line, long lineNumber)
      {
        foreach (string term in _forbidden)
        {
          if (Contains(line, term))
          {
            //the presence of any of these is enough to fail, and nothing later can undo it
            Disqualified = true;
            return;
          }
        }

        if (0 == _requiredStillMissing)
        {
          return;
        }

        for (int i = 0; i < _required.Length; ++i)
        {
          if ((0 == _firstLineOfRequired[i]) && Contains(line, _required[i]))
          {
            _firstLineOfRequired[i] = lineNumber;
            _requiredStillMissing--;
          }
        }
      }

      private bool Contains(string line, string term)
      {
        return -1 < line.IndexOf(term, _comparison);
      }
    }

    private TermMatch ResultOf(TermScanner scanner)
    {
      TermMatch result = new TermMatch();
      result.Matched = scanner.Satisfied;
      result.LineNumber = scanner.ReportableLineNumber;
      return result;
    }

    private TermMatch NoMatch()
    {
      return new TermMatch();
    }

    private TermMatch FindTermsInFile(string filename)
    {
      //one pass, every term at once
      try
      {
        StoreNotification("Searching file '" + filename + "'");
        TermScanner scanner = new TermScanner(_userPrefs.SearchStrings, _userPrefs.AbsentStrings, _userPrefs.CaseSensitive);
        long currlinenum = 0;

        using (System.IO.StreamReader reader = new System.IO.StreamReader(filename))
        {
          string currentLine;
          while ((currentLine = reader.ReadLine()) != null)
          {
            currlinenum++;

            //don't try to check binary files, and don't check for binariness > 1 time
            if (IsBinary(currentLine))
            {
              perfStats.LinesSearched += currlinenum;
              if (_userPrefs.IncludeOffice && IsOfficeDocument(filename))
              {
                return FindTermsInOfficeDocument(filename);
              }
              RecordBinaryFile(filename);
              return NoMatch();
            }

            scanner.Feed(currentLine, currlinenum);
            if (scanner.AnswerIsFinal)
            {
              break;  //we stopped reading at this line #
            }
          }
        }

        perfStats.LinesSearched += currlinenum;
        return ResultOf(scanner);
      }
      catch (System.IO.IOException)
      {
        //"File in use by another process" exception
        StoreException("'" + filename + "' is being used by another process.");
        perfStats.FileErrorCount++;
        return NoMatch();
      }
      catch (Exception e)
      {
        //all other exceptions
        StoreException("Exception in file '" + filename + "': '" + e.Message + "'");
        perfStats.FileErrorCount++;
        return NoMatch();
      }
    }

    private TermMatch FindTermsInOfficeDocument(string filename)
    {
      //same one pass, but the text has to come out through an IFilter first
      try
      {
        StoreNotification("Searching office document '" + filename + "'");
        TermScanner scanner = new TermScanner(_userPrefs.SearchStrings, _userPrefs.AbsentStrings, _userPrefs.CaseSensitive);

        string completeText;
        using (System.IO.TextReader reader = new EPocalipse.IFilter.FilterReader(filename))
        {
          completeText = reader.ReadToEnd();
        }

        long currlinenum = 0;
        foreach (string currentLine in completeText.Split('\n'))
        {
          currlinenum++;
          scanner.Feed(currentLine, currlinenum);
          if (scanner.AnswerIsFinal)
          {
            break;
          }
        }

        perfStats.LinesSearched += currlinenum;
        return ResultOf(scanner);
      }
      catch (System.IO.IOException)
      {
        //"File in use by another process" exception
        StoreException("'" + filename + "' is being used by another process.");
        perfStats.FileErrorCount++;
        return NoMatch();
      }
      catch (Exception e)
      {
        //all other exceptions
        StoreException("Exception in file '" + filename + "': '" + e.Message + "'");
        perfStats.FileErrorCount++;
        return NoMatch();
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
        return !((line == null) || (-1 == line.IndexOf("\0\0\0\0\0\0\0", StringComparison.Ordinal)));
      }
      catch (Exception e)
      {
        StoreException("Exception while trying to detect whether a file was binary: '" + e.Message + "'");
        return true;
      }
    }

    //both of these buffers are a fixed size, and both used to run off the end of it once a
    //verbose search got busy enough.  the exception that followed killed the search thread,
    //which then never reported itself finished, which hung the app.  a dropped diagnostic
    //message is not worth that.
    private void StoreNotification(string notifymsg)
    {
      if (RecordNotifications && (idxNotifications < Notifications.Length))
      {
        Notifications[idxNotifications++] = notifymsg;
      }
    }

    private void StoreException(string exceptmsg)
    {
      if (RecordExceptions && (idxExceptions < Exceptions.Length))
      {
        Exceptions[idxExceptions++] = exceptmsg;
      }
    }
  }
}