using System;
using System.Collections.Generic;
using System.IO;
using CloudAbstractions;

namespace Storage.FileSystem
{
    /// <summary>
    /// This is a 100% draft of the idea. 0% tested and 0% coverage
    /// </summary>
    public class FileSystemBlobStorage : IStorageProvider
    {
        public string StorageFolder { get; private set; }

        public FileSystemBlobStorage() : this("BlobStorage")
        {
        }

        public FileSystemBlobStorage(string storagePath)
        {
            // TODO: initialization will not happen in the constructor

            StorageFolder = storagePath;

            if (!Directory.Exists(StorageFolder))
                Directory.CreateDirectory(StorageFolder);
        }

        public IEnumerable<IBucket> Buckets
        {
            get { throw new NotImplementedException(); }
        }

        public IBucket CreateBucket(string bucketName)
        {
            string location = Path.Combine(StorageFolder, bucketName);

            if (! Directory.Exists(location))
                Directory.CreateDirectory(location);

            return new FileSystemBucket(location) {Name = bucketName};
        }

        public string AddToBucket(string bucketName, string path)
        {
            if (File.Exists(path))
            {
                var bucket = (FileSystemBucket)CreateBucket(bucketName);

                File.Copy(path, bucket.Path);
            }

            return null;
        }

        public string AddToBucket(string bucketName, string id, Stream stream)
        {
            throw new NotImplementedException();
        }

        public void GetFromBucket(string bucketName, string objectId, Stream receiverStream)
        {
            if (ExistInBucket(bucketName, objectId))
            {
                using (var readStream = File.OpenRead(Path.Combine(StorageFolder, bucketName, objectId)))
                {
                    readStream.CopyTo(receiverStream);
                }
            }
        }

        public void DeleteFromBucket(string bucketName, string objectId)
        {
            if (ExistInBucket(bucketName, objectId))
                File.Delete(Path.Combine(StorageFolder, bucketName, objectId));
        }

        public bool ExistInBucket(string bucketName, string objectId)
        {
            return File.Exists(Path.Combine(StorageFolder, bucketName, objectId));
        }
    }
}