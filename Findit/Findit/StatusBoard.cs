//this class is a shared place for all threads to post whether they are finished or not
using System;

namespace Findit
{
    public class StatusBoard
    {
        public StatusBoard(int searchThreadCount)
        {
            Array.Resize(ref GrepComplete, searchThreadCount);
        }
        public bool FileFindingComplete = false;
        public bool[] GrepComplete = { };
        public string LastSearchedFolder;
        public bool Halt = false;
        public bool AllDone
        {
            get 
            {
                if (Halt) { return true; }
                if (!FileFindingComplete)
                {
                    return false;
                }
                else
                {
                    for (int i = 0; i < GrepComplete.Length; ++i)
                    {
                        if (!GrepComplete[i])
                        {
                            return false;
                        }
                    }
                }
                return true;
            }
        }
    }
}