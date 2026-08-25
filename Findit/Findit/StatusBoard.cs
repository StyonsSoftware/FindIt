//this class is a shared place for all threads to post whether they are finished or not
using System;
using System.Linq;
using System.Threading;

namespace Findit
{
  public class StatusBoard
  {
    public StatusBoard(int searchThreadCount)
    {
      Array.Resize(ref GrepComplete, searchThreadCount);
    }

    //polled by threads that are not the one setting it.  without 'volatile' the JIT is
    //entitled to hoist the read out of a loop, in which case the thread never notices the
    //flag changing.
    public volatile bool FileFindingComplete = false;

    private volatile bool m_Halt;
    private readonly CancellationTokenSource m_Cancel = new CancellationTokenSource();

    public bool Halt
    {
      get { return m_Halt; }
      set
      {
        m_Halt = value;
        if (value)
        {
          //a search thread with nothing to do is now *blocked* waiting for the next file
          //rather than spinning on a flag, so it cannot see m_Halt change on its own.
          //Cancelling the token is what wakes it up and sends it home.
          m_Cancel.Cancel();
        }
      }
    }

    //search threads wait on this while the queue is empty.  see StatusBoard.Halt.
    public CancellationToken CancelToken
    {
      get { return m_Cancel.Token; }
    }

    public int FilesToBeSearchedCount = 0;
    //how many files have been looked at.  Written only by the GUI thread, which works it
    //out by adding up each search thread's own tally when it repaints - see
    //frmMain.RefreshProgressBar.  It used to be incremented by every search thread once per
    //file, which is a contended write thousands of times a second to produce a number that
    //is looked at four times a second.
    public int FilesSearched = 0;
    public bool[] GrepComplete = { };  //written with Volatile.Write - see Grepper.Search
    public string LastSearchedFolder;
    public string UserFacingError = string.Empty;

    //is there anybody left to take a file off the queue?
    //
    //the queue has a ceiling, so the walker waits when it fills up.  If every search thread
    //has already stopped - they all hit an exception, say - nobody is ever going to empty
    //it again, and a walker waiting for room it will never get would hang the search with
    //no way out but killing the application.
    public bool AllGreppersFinished
    {
      get { return GrepComplete.All(t => t); }
    }
    public bool AllDone
    {
      get
      {
        //we are finished when:
        //1: they clicked cancel
        //2: we have identified every possible search target and actually searched them all.
        return Halt || (FileFindingComplete && AllGreppersFinished);
      }
    }
  }
}