using System;
using CloudAbstractions;
using CloudAbstractions.Messaging;
using Microsoft.WindowsAzure;

namespace AzureAbstractions
{
    public class AzureRealm : IRealm
    {
        private CloudStorageAccount _storageAccount;

        public AzureRealm()
        {
            //_storageAccount = CloudStorageAccount.Parse("DefaultEndpointsProtocol=http;AccountName=bmaigorcloudmorphstorage;AccountKey=60VMAzM8UiGe5NuL5t6IMpXm8mo2qiuKkBJY5CQg8Pz2XtF0Dm57tVDP4oTvmlvc8AV8Gh4HWSFz9fmxeszVDA==");
            _storageAccount = CloudStorageAccount.Parse(CloudConfigurationManager.GetSetting("DataConnectionString"));
        }

        public IRealmConfigurator Configure()
        {
            throw new NotImplementedException();
        }

        public IStorageProvider StorageProvider
        {
            get { return new AzureStorageProvider(_storageAccount); }
        }

        public IComputeInfrastructureProvider ComputeInfrastructureProvider
        {
            get { throw new NotImplementedException(); }
        }

        public IQueueProvider QueueProvider
        {
            get { return new AzureQueueProvider(_storageAccount); }
        }

        public IKvStorageProvider KvStorageProvider
        {
            get { return new AzureTableStorageProvider(_storageAccount); }
        }

        public IEnvironment Current
        {
            get { return new AzureEnvironment(); }
        }
    }
}