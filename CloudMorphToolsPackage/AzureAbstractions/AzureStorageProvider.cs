using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using CloudAbstractions;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.StorageClient;

namespace AzureAbstractions
{
    // How to Use the Blob Storage Service - http://www.windowsazure.com/en-us/develop/net/how-to-guides/blob-storage/
    public class AzureStorageProvider : IStorageProvider
    {
        private CloudStorageAccount cloudStorageAccount;
        private CloudBlobClient blobStorage;

        public AzureStorageProvider()
        {
            // Retrieve storage account from connection-string
            //cloudStorageAccount = CloudStorageAccount.DevelopmentStorageAccount;
            //cloudStorageAccount = CloudStorageAccount.Parse(RoleEnvironment.GetConfigurationSettingValue("StorageConnectionString"));
            cloudStorageAccount = CloudStorageAccount.Parse("DefaultEndpointsProtocol=http;AccountName=bmaigorcloudmorphstorage;AccountKey=60VMAzM8UiGe5NuL5t6IMpXm8mo2qiuKkBJY5CQg8Pz2XtF0Dm57tVDP4oTvmlvc8AV8Gh4HWSFz9fmxeszVDA==");
            //storageAccount = CloudStorageAccount.Parse(RoleEnvironment.GetConfigurationSettingValue("StorageConnectionString"));

            // Create the blob client
            blobStorage = cloudStorageAccount.CreateCloudBlobClient();
        }

        public AzureStorageProvider(CloudStorageAccount storageAccount)
        {
            cloudStorageAccount = storageAccount;

            // Create the blob client
            blobStorage = cloudStorageAccount.CreateCloudBlobClient();
        }

        public IEnumerable<IBucket> Buckets
        {
            get
            {
                // Retrieve reference to a previously created container
                CloudBlobContainer container = blobStorage.GetContainerReference("mycontainer");

                // Loop over blobs within the container and output the URI to each of them
                return container.ListBlobs().Select(blobItem => new AzureBucket() {Name = blobItem.Container.Name }); 
            }
        }

        // Primary Key = 60VMAzM8UiGe5NuL5t6IMpXm8mo2qiuKkBJY5CQg8Pz2XtF0Dm57tVDP4oTvmlvc8AV8Gh4HWSFz9fmxeszVDA==
        // Secondary Key = s+kNBQqwJR+1LPSeG1D5bWOVgWJ/snaxgP2Ar0K5jkrJYz0IOOde4wpXAy0lIb9TpYqtIQS6SVJhRQ1UobtxcQ==

        public IBucket CreateBucket(string bucketName)
        {
            var blobContainer = blobStorage.GetContainerReference("bmaigorcloudmorphstorage");
            blobContainer.CreateIfNotExist();

            var containerPermissions = new BlobContainerPermissions();
            containerPermissions.PublicAccess = BlobContainerPublicAccessType.Blob;
            blobContainer.SetPermissions(containerPermissions);

            return new AzureBucket() { Name = bucketName, CloudBlobClient = blobStorage.GetContainerReference(bucketName) };
        }

        public string AddToBucket(string bucketName, string path)
        {
            var blobContainer = blobStorage.GetContainerReference(bucketName);
            blobContainer.CreateIfNotExist();

            var containerPermissions = new BlobContainerPermissions
                                           {
                                               PublicAccess = BlobContainerPublicAccessType.Blob
                                           };
            blobContainer.SetPermissions(containerPermissions);

            var blobReference = blobContainer.GetBlobReference(Path.GetFileName(path));
            blobReference.Properties.ContentType = "image/jpeg";
            blobReference.UploadFile(path);

            return blobReference.Uri.ToString();

            //var blob = _cloudBlobContainer.GetBlobReference(blobStorage.GetContainerReference(bucketName));
            //blob.UploadByteArray(data);
        }

        public string AddToBucket(string bucketName, string id, Stream stream)
        {
            var blobContainer = blobStorage.GetContainerReference(bucketName);
            blobContainer.CreateIfNotExist();

            var containerPermissions = new BlobContainerPermissions
            {
                PublicAccess = BlobContainerPublicAccessType.Blob
            };
            blobContainer.SetPermissions(containerPermissions);

            var blobReference = blobContainer.GetBlobReference(id);
            blobReference.Properties.ContentType = "image/jpeg";
            blobReference.UploadFromStream(stream);

            return blobReference.Uri.ToString();
        }

        public void GetFromBucket(string bucketName, string objectId, Stream receiverStream)
        {
            // Retrieve reference to a previously created container
            CloudBlobContainer container = blobStorage.GetContainerReference("bmaigorcloudmorphstorage");

            // Retrieve reference to a blob named "myblob"
            CloudBlob blob = container.GetBlobReference("myblob");

            // Save blob contents to disk
/*
            using (var fileStream = System.IO.File.OpenWrite(@"path\myfile"))
            {
                blob.DownloadToStream(fileStream);
            } 
*/
            blob.DownloadToStream(receiverStream);
        }

        public void DeleteFromBucket(string bucketName, string objectId)
        {
            var blobContainer = blobStorage.GetContainerReference("bmaigorcloudmorphstorage");

            var blobReference = blobContainer.GetBlobReference(objectId);


            blobReference.DeleteIfExists();
        }

        public bool ExistInBucket(string bucketName, string objectId)
        {
            throw new NotImplementedException();
        }
    }
}