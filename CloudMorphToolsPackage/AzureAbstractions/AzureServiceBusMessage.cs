using System;
using CloudAbstractions;
using Microsoft.ServiceBus.Messaging;

namespace AzureAbstractions
{
    public class AzureServiceBusMessage : IQueueMessage
    {
        public AzureServiceBusMessage(BrokeredMessage message)
        {
            BrokeredMessage = message;
        }

        public BrokeredMessage BrokeredMessage { get; internal set; }

        public string Id
        {
            get { return BrokeredMessage.MessageId; }
        }

        public string Body
        {
            get { return BrokeredMessage.GetBody<string>(); }
            set { throw new NotImplementedException(); }
        }
    }
}