//this class is a shared place for all threads to post whether they are finished or not
using System;
using System.Linq;

namespace Findit
{
  public class StatusBoard
  {
    public StatusBoard(int searchThreadCount)
    {
      Array.Resize(ref GrepComplete, searchThreadCount);
    }
    public bool FileFindingComplete = false;
    public int FilesToBeSearchedCount = 0;
    public int FilesSearched = 0;  //many threads will be hitting this counter, so it will only be approximate until the end.
    public bool[] GrepComplete = { };
    public string LastSearchedFolder;
    public bool Halt = false;
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