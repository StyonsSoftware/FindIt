/*
The queue of files waiting to be searched.

There is one shared queue, not one per processor as there used to be.  BlockingCollection
hands the next batch of files to whichever search thread asks for one, so a thread that
lands on a folder full of enormous files no longer leaves the others idle - which is what
the old round-robin dealing of files into per-thread lists was trying, and failing, to
achieve.

The other two things it buys us:
 - search threads block while the queue is empty instead of spinning on a flag, so an
   idle thread costs nothing rather than a whole core.
 - it is safe to add to while several threads are taking from it.  The List<QueuedFile>
   it replaced was not: the builder appending while a searcher indexed into it could hand
   that searcher a stale backing array or an entry that was not populated yet.

Files travel in batches rather than one at a time.  Handing them over singly meant a
trip through the collection's lock and, whenever the searchers had caught up with the
walker - which is most of the time, because reading a file takes far longer than listing
one - a wait handle and a kernel transition per file.  That overhead is what stopped the
search getting any faster past a handful of threads and made it get *worse* past a dozen:
the threads spent their time being woken up rather than reading.  See QueueBuilder for how
batches are filled and when they get flushed early.

There is a ceiling on it.  The walk over a folder tree runs far ahead of the threads
reading the files it finds, and unbounded that meant a search over a large drive built a
queue of millions of entries in memory before a fraction of them had been looked at.  The
ceiling is generous enough that the searchers never wait on the walker for real work; it
just stops the backlog growing without limit.  See QueueBuilder.TryQueue for how the walker
gets out of a full queue when the search is cancelled.
//*/
using System.Collections.Concurrent;

namespace Findit
{
    public class FileQueue
    {
        //batches, not files: enough backlog that no search thread finds it empty in
        //practice, small enough that the queue is measured in megabytes not gigabytes
        private const int c_MaxQueuedBatches = 4096;

        public BlockingCollection<QueuedFile[]> filesToSearch =
            new BlockingCollection<QueuedFile[]>(c_MaxQueuedBatches);

        //is anybody waiting on us right now?  The walker uses this to decide whether to
        //send a part-filled batch off early rather than hold on to it - see
        //QueueBuilder.QueueOneFile.
        public bool IsEmpty
        {
            get { return 0 == filesToSearch.Count; }
        }
    }
}
