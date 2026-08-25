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
            uparms.OnlyFileNames = false;

            gp = new GrepTool.Grepper(uparms);
            searchThread = new Thread(gp.Search);
            //at this point, a thread is running your search, and gp.SearchResult will have the list of results

//*/
using System;
using System.Collections.Generic;
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

    //how much of a file we pull off the disk at a time.  the framework default is 1KB,
    //which is a syscall every few lines of a source file.
    private const int c_FileBufferBytes = 65536;

    //how much of the front of a file we look at to decide whether it is binary
    private const int c_BinarySniffBytes = 8192;

    //a run of this many NUL bytes says "binary".  two would be enough to rule out
    //UTF-16 text (whose NULs alternate with real characters), so four is comfortable.
    private const int c_BinaryNulRun = 4;

    public SearchResult[] SearchResults = new SearchResult[c_CacheSize];
    public int SearchResultCount;

    //only allocated if we are actually recording - see the RecordExceptions setter.  a
    //50,000 element array per thread per search is not free, and the default verbosity
    //never writes a single entry into it.
    public string[] Exceptions;
    private int idxExceptions;

    public string[] Notifications;
    private int idxNotifications;

    private UserParameters _userPrefs;
    private readonly int _threadIndex;
    private readonly StatusBoard _statBoard;
    private readonly FileQueue _queue;
    private bool _resultOverflowReported;

    //the search terms, worked out once for the whole search rather than once per file
    private readonly string[] _requiredTerms;
    private readonly string[] _forbiddenTerms;
    private readonly StringComparison _comparison;

    //reused for the binary sniff so we are not handing the GC an 8KB array per file
    private readonly byte[] _sniffBuffer = new byte[c_BinarySniffBytes];

    //the folder name we last put on the progress line - see SearchOneFile
    private string _lastFolderReported;

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

      //an empty term matches every line ever written.  one blank line left in the middle of
      //the "must not contain" box therefore disqualified every file in the search and
      //quietly returned nothing at all.  the GUI strips blanks when the box loses focus,
      //which is not the same as never letting one through.
      _requiredTerms = WithoutBlanks(prefs.SearchStrings);
      _forbiddenTerms = WithoutBlanks(prefs.AbsentStrings);

      //Ordinal, not CurrentCulture.  these are literal substrings, and a culture-aware
      //IndexOf walks a collation table for every comparison - it is several times slower
      //than the ordinal one for exactly the same answer on the searches this tool does.
      _comparison = prefs.CaseSensitive ? StringComparison.Ordinal : StringComparison.OrdinalIgnoreCase;

      InitializePerformanceStats();
    }

    private static string[] WithoutBlanks(string[] terms)
    {
      if (terms == null)
      {
        return new string[0];
      }
      List<string> kept = new List<string>(terms.Length);
      foreach (string term in terms)
      {
        if (!string.IsNullOrEmpty(term))
        {
          kept.Add(term);
        }
      }
      return kept.ToArray();
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
      //files arrive in batches - see FileQueue.  Taking them one at a time meant a lock and,
      //more often than not, a wait handle per file.
      foreach (QueuedFile[] batch in _queue.filesToSearch.GetConsumingEnumerable(_statBoard.CancelToken))
      {
        for (int i = 0; i < batch.Length; ++i)
        {
          if (_statBoard.Halt)
          {
            return;  //they clicked cancel
          }
          SearchOneFile(batch[i]);
        }
      }
    }

    private void SearchOneFile(QueuedFile qf)
    {
      //"which folder are we in" is a caption on the progress line, refreshed four times a
      //second.  Writing it per file meant every search thread wrote to the same field on the
      //same object several thousand times a second - and a field several cores are all
      //writing to is a cache line being dragged between them, which is why adding threads
      //past a handful used to make a search *slower* rather than faster.  Every file in a
      //folder has the same folder name, so only the change is worth reporting.
      if (!ReferenceEquals(_lastFolderReported, qf.FolderName))
      {
        _lastFolderReported = qf.FolderName;
        _statBoard.LastSearchedFolder = qf.FolderName;
      }

      //the count of files searched is *not* maintained here any more, for the same reason:
      //it was an interlocked increment on one shared counter per file, across every thread.
      //Each thread already keeps its own tally in perfStats, and the GUI adds those up when
      //it repaints - see frmMain.RefreshProgressBar.

      string currentFilename = qf.FullName;

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
      else if (!outcome.Errored)
      {
        //a file we could not read has already been counted under FileErrorCount.  counting
        //it here as well made TotalFilesProcessed - which adds the two together - larger
        //than the number of files there were, so a search over a folder with a few locked
        //files finished by reporting "1043 of 1000 files checked".
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
      //counted before the "is there room to list it" test.  A match we ran out of room to
      //display is still a file we looked at, and this tally is what the progress bar is
      //built from - leaving it out made the bar stall short of the end on any search that
      //filled the results list.
      perfStats.FilesMatched++;

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

      //the GUI reads this count and then reads everything below it, so the entry has to be
      //filled in before the count that publishes it - and has to be seen that way round by
      //the other thread.  A plain increment leaves the compiler and the processor free to
      //reorder the two, which would show the GUI an entry that is not written yet.
      System.Threading.Volatile.Write(ref SearchResultCount, SearchResultCount + 1);
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
      public bool Errored;      //we never got to read it - already counted as a file error
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

      //the term lists and the comparison arrive already worked out - they are the same for
      //every file in the search, and this gets built once per file
      public TermScanner(string[] required, string[] forbidden, StringComparison comparison)
      {
        _required = required;
        _forbidden = forbidden;
        _comparison = comparison;
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

    private TermMatch FileError()
    {
      TermMatch result = new TermMatch();
      result.Errored = true;
      return result;
    }

    private TermScanner NewScanner()
    {
      return new TermScanner(_requiredTerms, _forbiddenTerms, _comparison);
    }

    private TermMatch FindTermsInFile(string filename)
    {
      //an office document is decided by its name, before we open it.  it used to be routed
      //here only if reading it as text happened to trip the binary detector - and a .docx
      //is a zip, whose compressed bytes almost never contain the long run of NULs that
      //detector was looking for.  So "search Office documents" mostly did nothing at all:
      //the document was read as text, the terms were not found in the compressed bytes, and
      //the file was reported as not matching.
      if (IsOfficeDocument(filename))
      {
        if (_userPrefs.IncludeOffice)
        {
          return FindTermsInOfficeDocument(filename);
        }
        RecordBinaryFile(filename);
        return NoMatch();
      }

      //one pass, every term at once
      try
      {
        StoreNotification("Searching file '" + filename + "'");
        TermScanner scanner = NewScanner();
        long currlinenum = 0;

        //SequentialScan tells Windows how we are going to read this, and the large buffer
        //turns what was a syscall every kilobyte into one every 64.  the framework's own
        //default is 1KB and it shows on a tree of small source files.
        using (System.IO.FileStream stream = new System.IO.FileStream(filename, System.IO.FileMode.Open,
            System.IO.FileAccess.Read, System.IO.FileShare.ReadWrite, c_FileBufferBytes, System.IO.FileOptions.SequentialScan))
        {
          //binary detection happens twice, and both are worth having.
          //
          //First, before a byte is decoded, on the raw bytes at the front of the file.  Most
          //binaries are caught here for almost nothing, and it is also what stops ReadLine
          //being handed a 50MB executable with no line breaks in it and asked to produce
          //the whole thing as a single string.
          if (LooksBinary(stream))
          {
            RecordBinaryFile(filename);
            return NoMatch();
          }

          using (System.IO.StreamReader reader = new System.IO.StreamReader(stream, Encoding.UTF8, true, c_FileBufferBytes))
          {
            string currentLine;
            while ((currentLine = reader.ReadLine()) != null)
            {
              currlinenum++;

              //Second, per line - because a binary whose first few KB happen to look like
              //text still needs to be abandoned rather than searched to the end.  One NUL
              //character, not the run of seven this used to look for: a NUL does not occur
              //in real text, so one is all the evidence there is going to be, and looking
              //for a single character is far cheaper than a substring search per line.
              //Between them, these two skip binaries roughly a third of the way sooner than
              //the old check did, which is most of where this search got faster.
              if (-1 < currentLine.IndexOf('\0'))
              {
                perfStats.LinesSearched += currlinenum;  //we did read this far
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
        }

        perfStats.LinesSearched += currlinenum;
        return ResultOf(scanner);
      }
      catch (System.IO.IOException)
      {
        //"File in use by another process" exception
        StoreException("'" + filename + "' is being used by another process.");
        perfStats.FileErrorCount++;
        return FileError();
      }
      catch (Exception e)
      {
        //all other exceptions
        StoreException("Exception in file '" + filename + "': '" + e.Message + "'");
        perfStats.FileErrorCount++;
        return FileError();
      }
    }

    //true if the front of this file looks like something other than text.  leaves the
    //stream back at the beginning either way, so the caller can go on and read it.
    private bool LooksBinary(System.IO.FileStream stream)
    {
      int bytesRead = stream.Read(_sniffBuffer, 0, _sniffBuffer.Length);
      stream.Position = 0;

      //a run, not a single NUL: UTF-16 text is half NUL bytes by construction, and reading
      //one of those as a binary would be a regression on what the old line-by-line check did.
      int consecutiveNuls = 0;
      for (int i = 0; i < bytesRead; ++i)
      {
        if (0 == _sniffBuffer[i])
        {
          if (++consecutiveNuls >= c_BinaryNulRun)
          {
            return true;
          }
        }
        else
        {
          consecutiveNuls = 0;
        }
      }
      return false;
    }

    private TermMatch FindTermsInOfficeDocument(string filename)
    {
      //same one pass, but the text has to come out through an IFilter first
      try
      {
        StoreNotification("Searching office document '" + filename + "'");
        TermScanner scanner = NewScanner();

        //ReadToEnd, because FilterReader does not implement reading a line at a time -
        //ask it for one and it reports the document as empty.
        string completeText;
        using (System.IO.TextReader filter = new EPocalipse.IFilter.FilterReader(filename))
        {
          completeText = filter.ReadToEnd();
        }

        long currlinenum = 0;
        //a StringReader over the text we already have, rather than Split('\n').  Split
        //builds an array holding a second complete copy of the document, and the answer is
        //often settled a few lines in, so most of that copy was never going to be read.
        using (System.IO.StringReader lines = new System.IO.StringReader(completeText))
        {
          string currentLine;
          while ((currentLine = lines.ReadLine()) != null)
          {
            currlinenum++;
            scanner.Feed(currentLine, currlinenum);
            if (scanner.AnswerIsFinal)
            {
              break;
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
        return FileError();
      }
      catch (Exception e)
      {
        //all other exceptions
        StoreException("Exception in file '" + filename + "': '" + e.Message + "'");
        perfStats.FileErrorCount++;
        return FileError();
      }
    }

    private void RecordBinaryFile(string filename)
    {
      StoreNotification("Skipped binary file " + filename);
      perfStats.BinarySkipped++;
    }

    private static bool IsOfficeDocument(string filename)
    {
      //this is asked once per file, so it does not go to the disk to confirm the file is
      //there - we are holding it - and it does not split the whole path apart to get at the
      //extension either.
      return Util.IsOfficeExtension(filename);
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