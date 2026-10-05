namespace Findit
{
    //one file waiting its turn on the shared queue.
    //
    //A path and the folder it came out of, and nothing else.  This used to be a class
    //holding a FileInfo, which is two allocations per file plus a normalised copy of the
    //path inside the FileInfo - on a tree with a million files in it, for a search that
    //only ever asked the FileInfo for its FullName.
    //
    //FolderName is carried rather than derived later: it is only wanted for the progress
    //caption, the walker already knows it, and working it back out of the path costs a
    //scan and an allocation per file.
    public struct QueuedFile
    {
        public string FullName;
        public string FolderName;
    }
}
