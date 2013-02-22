using System;
using CloudAbstractions;

namespace Storage.FileSystem
{
    public class FileSystemBucket : IBucket
    {
        internal FileSystemBucket(string path)
        {
            Path = path;
        }

        public string Name { get; internal set; }

        internal string Path { get; private set; }
    }
}