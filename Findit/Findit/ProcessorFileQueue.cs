/*
The queue of files waiting to be searched.

There is one shared queue, not one per processor as there used to be.  BlockingCollection
hands the next file to whichever search thread asks for one, so a thread that lands on a
folder full of enormous files no longer leaves the others idle - which is what the old
round-robin dealing of files into per-thread lists was trying, and failing, to achieve.

The other two things it buys us:
 - search threads block while the queue is empty instead of spinning on a flag, so an
   idle thread costs nothing rather than a whole core.
 - it is safe to add to while several threads are taking from it.  The List<QueuedFile>
   it replaced was not: the builder appending while a searcher indexed into it could hand
   that searcher a stale backing array or an entry that was not populated yet.
//*/
using System.Collections.Concurrent;

namespace Findit
{
    public class FileQueue
    {
        public BlockingCollection<QueuedFile> filesToSearch = new BlockingCollection<QueuedFile>();
    }
}
