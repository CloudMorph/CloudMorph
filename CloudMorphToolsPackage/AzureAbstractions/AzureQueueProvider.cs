using System;
using System.Collections.Generic;
using System.IO;
using CloudAbstractions;
using CloudAbstractions.Messaging;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.StorageClient;

namespace AzureAbstractions
{
    // NONTE:  The queue name HAVE TO BE the LOW CASE !!!!

    // How to Use the Queue Storage Service: http://www.windowsazure.com/en-us/develop/net/how-to-guides/queue-service/
    // Queue Service REST API http://msdn.microsoft.com/en-us/library/windowsazure/dd179363.aspx
    public class AzureQueueProvider : IQueueProvider
    {
        private CloudQueueClient _client;
        private CloudStorageAccount _storageAccount;

        public AzureQueueProvider()
        {
            //var accountAndKey = new StorageCredentialsAccountAndKey("cloudmorph", "Q7Vco66yV3hncM/piawqSHyiXZZ8meZdRMAMElOorgE/EHv4t8B18Nn7zUF/7nLu6WSMaZzmjaUo4LUHlWCfyw==");
            //var _storageAccount = new CloudStorageAccount(accountAndKey, true);

            // Retrieve storage account from connection-string
            //_storageAccount = CloudStorageAccount.Parse(RoleEnvironment.GetConfigurationSettingValue("StorageConnectionString"));
            //_storageAccount = CloudStorageAccount.Parse("DefaultEndpointsProtocol=http;AccountName=bmaigorcloudmorphstorage;AccountKey=Q7Vco66yV3hncM/piawqSHyiXZZ8meZdRMAMElOorgE/EHv4t8B18Nn7zUF/7nLu6WSMaZzmjaUo4LUHlWCfyw==");
            _storageAccount = CloudStorageAccount.Parse(CloudConfigurationManager.GetSetting("StorageConnectionString"));

            // Create the queue client
            _client = _storageAccount.CreateCloudQueueClient();
        }

        public AzureQueueProvider(CloudStorageAccount storageAccount)
        {
            _storageAccount = storageAccount;
            _client = _storageAccount.CreateCloudQueueClient();            
        }

        public IEnumerable<IQueue> Queues
        {
            get { throw new NotImplementedException(); }
        }

        public bool CreateQueue(string queueId)
        {
            // Retrieve a reference to a queue
            CloudQueue queue = _client.GetQueueReference(queueId);

            // Create the queue if it doesn't already exist
            queue.CreateIfNotExist();

            return true;
        }

        public void DeleteQueue(string queueId)
        {
            // Retrieve a reference to a queue
            CloudQueue queue = _client.GetQueueReference(queueId);

            // Delete the queue
            queue.Delete();
        }

        public void SendMessage(IQueue queue, string message)
        {
            // Retrieve a reference to a queue
            CloudQueue queueC = _client.GetQueueReference(queue.Id);

            // Create a message and add it to the queue
            var queueMessage = new CloudQueueMessage(message);
            queueC.AddMessage(queueMessage);
        }

        public IQueue GetQueueById(string queueId)
        {
            var queue = _client.GetQueueReference(queueId);
            //queue.CreateIfNotExist();

            if (queue.Exists())
                return new AzureQueue(queueId);

            throw new InvalidDataException();
        }

        public IQueueMessage ReceiveMessage(IQueue queue)
        {
            // Retrieve a reference to a queue
            CloudQueue queueC = _client.GetQueueReference(queue.Id);

            var message = queueC.GetMessage();

            if (message == null)
                return null;

            return new AzureQueueMessage(message);
        }

        public void DeleteMessage(IQueue queue, IQueueMessage message)
        {
            // Retrieve a reference to a queue
            CloudQueue queueC = _client.GetQueueReference(queue.Id);

            var azureMessage = (AzureQueueMessage) message;

            queueC.DeleteMessage(message.Id, azureMessage.ReceiptHandle);
        }

        public IQueue GetQueueByUri(Uri uri)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<string> GetQueueList(string prefix = null)
        {
            IEnumerable<CloudQueue> queues;

            queues = prefix != null ? _client.ListQueues(prefix) : _client.ListQueues();

            foreach (var queue in queues)
            {
                Console.WriteLine(queue.Name);
                yield return queue.Name;
            }
        }
    }
}