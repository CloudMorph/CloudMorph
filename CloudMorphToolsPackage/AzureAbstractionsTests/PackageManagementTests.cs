using System.IO;
using AzureAbstractions;
using CloudAbstractions;
using NUnit.Framework;

namespace AzureAbstractionsTests
{
    [TestFixture]
    public class PackageManagementTests
    {
        [Test]
        public void UploadPackage()
        {
            IRealm realm = new AzureRealm();

            var storage = realm.StorageProvider;

            /*
                        var buckets = storage.Buckets;

                        foreach (var bucket in buckets)
                        {
                            Console.WriteLine(bucket.Name);
                        }
            */

            string bucketName = "igor_another_test";
            //var bucket1 = storage.CreateBucket(bucketName);

            string filePath = @"C:\Temp\kinect.jpg";
            storage.AddToBucket(bucketName, filePath);

            //storage.DeleteFromBucket(bucketName, Path.GetFileName(filePath));
        }
    }
}