using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Amazon.Runtime;
using Amazon.S3;
using Amazon.S3.Model;
using CloudAbstractions;

namespace AwsAbstractions
{
    // http://awsdocs.s3.amazonaws.com/S3/latest/s3-dg.pdf
    // How to Use the Blob Storage Service - http://www.windowsazure.com/en-us/develop/net/how-to-guides/blob-storage/
    public class AwsStorageProvider : IStorageProvider
    {
        private readonly AWSCredentials _credentials;
        private readonly AmazonS3Client _awsClient;

        public AwsStorageProvider(AWSCredentials credentials)
        {
            _credentials = credentials;
            _awsClient = new AmazonS3Client(_credentials);
        }

        public IEnumerable<IBucket> Buckets
        {
            get
            {
                //var request = new ListBucketsRequest()

                var response = _awsClient.ListBuckets();

                return response.Buckets.Select(bucket => new AwsBucket
                                                             {
                                                                 Name = bucket.BucketName
                                                             });
            }
        }

        public IBucket CreateBucket(string name)
        {
            var request = new PutBucketRequest()
                .WithBucketName(name);

            var responce = _awsClient.PutBucket(request);

            return new AwsBucket() {Name = name};
        }

        public string AddToBucket(string name, string path)
        {
            var request = new PutObjectRequest()
                .WithBucketName(name)
                .WithFilePath(path)
                .WithKey(Path.GetFileName(path))
                //.WithContentType("image/jpeg");
                .WithContentType("'binary/octet-stream");

            var response = _awsClient.PutObject(request);

            return Path.GetFileName(path);
        }

        public string AddToBucket(string bucketName, string id, Stream stream)
        {
            throw new NotImplementedException();
        }

        public void GetFromBucket(string bucketName, string objectId, Stream receiverStream)
        {
            GetObjectRequest request = new GetObjectRequest()
                .WithBucketName(bucketName)
                .WithKey(objectId);
            var response = _awsClient.GetObject(request);

            response.ResponseStream.CopyTo(receiverStream);
        }

        public void DeleteFromBucket(string bucketName, string objectId)
        {
            var request = new DeleteObjectRequest()
                .WithBucketName(bucketName)
                .WithKey(objectId);

            var response = _awsClient.DeleteObject(request);
        }

        public bool ExistInBucket(string bucketName, string objectId)
        {
            ListObjectsRequest request = new ListObjectsRequest()
                .WithBucketName(bucketName)
                .WithDelimiter("/");
                //.WithPrefix(objectId);

            while (true)
            {
                using (var response = _awsClient.ListObjects(request))
                {
                    //int totalNumberOfObjects = response.S3Objects.Count;
                    if (response.S3Objects.Any(entry => StringComparer.InvariantCultureIgnoreCase.Compare(entry.Key, objectId) == 0))
                    {
                        return true;
                    }

                    if (response.IsTruncated)
                    {
                        request.Marker = response.NextMarker;

                        request.MaxKeys = 1000;
                    }
                    else
                    {
                        break;
                    }
                }
            }

            return false;
        }

        // http://docs.amazonwebservices.com/AmazonS3/2006-03-01/dev/HLuploadFileDotNet.html
        void UploadFile()
        {
            /*
                        TransferUtilityUploadRequest tr = new TransferUtilityUploadRequest()
                            .WithBucketName("test_mydomain_com")
                            .WithFilePath(((FileInfo)lvi.Tag).FullName)
                            .WithTimeout(5 * 60 * 1000);
                        TransferUtility tu = new TransferUtility(a);

                        tu.Upload(tr);
            */
        }
    }
}