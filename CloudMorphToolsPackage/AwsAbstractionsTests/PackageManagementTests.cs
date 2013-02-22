using System;
using System.IO;
using AwsAbstractions;
using CloudAbstractions;
using NUnit.Framework;

namespace AwsAbstractionsTests
{
    [TestFixture]
    public class PackageManagementTests
    {
        [Test]
        public void UploadPackage()
        {
            IRealm realm = new AwsRealm();

            var storage = realm.StorageProvider;

/*
            var buckets = storage.Buckets;

            foreach (var bucket in buckets)
            {
                Console.WriteLine(bucket.Name);
            }
*/

            string bucketName = "igor_another_test";
            var bucket1 = storage.CreateBucket(bucketName);

            string filePath = @"C:\Temp\kinect.jpg";
            storage.AddToBucket(bucketName, filePath);

            //storage.DeleteFromBucket(bucketName, Path.GetFileName(filePath));
        }
    }
}