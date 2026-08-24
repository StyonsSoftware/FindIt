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
    public int FilesSearched = 0;  //many threads will be hitting this counter, so it will only be approximate until the end.
    public bool[] GrepComplete = { };  //written with Volatile.Write - see Grepper.Search
    public string LastSearchedFolder;
    public string UserFacingError = string.Empty;
    public bool AllDone
    {
      get
      {
        //we are finished when:
        //1: they clicked cancel
        //2: we have identified every possible search target and actually searched them all.
        return Halt || (FileFindingComplete && GrepComplete.All(t => t));
      }
    }
  }
}