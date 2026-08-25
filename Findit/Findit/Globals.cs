//a place for data structures shared across threads
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Findit
{
    public static class Globals
    {
        public static StatusBoard statBoard;
        //one queue shared by every search thread - see ProcessorFileQueue.cs
        public static FileQueue fileQueue = new FileQueue();

        //This used to be ProcessorCount - 3, which is two things at once, and both wrong.
        //
        //On a four core machine it is 1: a search that could have had four files open at a
        //time had one, and spent its life waiting on the disk with three idle cores beside
        //it.  On a twenty-eight core machine it is 25, and measured over a real source tree
        //25 threads took *three times as long* as 8 - a search is mostly waiting on reads,
        //so past a certain point the extra threads add contention and nothing else.
        //
        //Timed over ~7,000 files, the curve flattens at four threads and is at its best
        //around eight; beyond that it turns back upwards.  So: one per core, up to eight.
        //
        //Clamped to what the slider in the options dialog will accept: it is offered to the
        //user as the recommended setting, and a recommendation it cannot be set to is not
        //much of a recommendation.
        private const int c_ThreadsWorthHaving = 8;

        public static int RecommendedSearchThreadCount =
            GUIPreferences.ClampThreadCount(Math.Min(Environment.ProcessorCount, c_ThreadsWorthHaving));
    }
}
