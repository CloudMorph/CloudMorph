using System;
using CloudAbstractions;
using Microsoft.WindowsAzure.StorageClient;

namespace AzureAbstractions
{
    public class AzureBucket : IBucket
    {
        public string Name { get; internal set; }
        public CloudBlobContainer CloudBlobClient { get; internal set; }
    }
}