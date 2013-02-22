using System.Collections.Generic;
using System.IO;

namespace CloudAbstractions
{
    public interface IStorageProvider
    {
        IEnumerable<IBucket> Buckets { get; }
        IBucket CreateBucket(string bucketName);
        string AddToBucket(string bucketName, string path);
        string AddToBucket(string bucketName, string id, Stream stream);
        void GetFromBucket(string bucketName, string objectId, Stream receiverStream);
        void DeleteFromBucket(string bucketName, string objectId);
        bool ExistInBucket(string bucketName, string objectId);
    }
}