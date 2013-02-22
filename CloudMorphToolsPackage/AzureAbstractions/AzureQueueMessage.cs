using CloudAbstractions;
using Microsoft.WindowsAzure.StorageClient;

namespace AzureAbstractions
{
    public class AzureQueueMessage : IQueueMessage
    {
        public string Id { get; internal set; }
        public string Body { get; set; }
        public string ReceiptHandle { get; set; }

        public AzureQueueMessage(CloudQueueMessage message)
        {
            Id = message.Id;
            Body = message.AsString;
            ReceiptHandle = message.PopReceipt;
        }
    } 
}