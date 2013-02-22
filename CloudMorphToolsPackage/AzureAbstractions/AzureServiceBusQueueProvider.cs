using System;
using System.Collections.Generic;
using CloudAbstractions;
using CloudAbstractions.Messaging;
using Microsoft.ServiceBus;
using Microsoft.ServiceBus.Messaging;

namespace AzureAbstractions
{
    public class AzureServiceBusQueueProvider : IQueueProvider
    {
        public IEnumerable<IQueue> Queues
        {
            get { throw new NotImplementedException(); }
        }

        public bool CreateQueue(string queueId)
        {
            // Holds credentials and handles tokens from AZS
            TokenProvider tokenProvider = GetTokenProvider();

            Uri uri = GetServiceBusUri();

            var namespaceManager = new NamespaceManager(uri, tokenProvider);

            if (!namespaceManager.QueueExists(queueId))
            {
                namespaceManager.CreateQueue(queueId);
            }

            throw new NotImplementedException();
        }

        public void DeleteQueue(string queueId)
        {
            throw new NotImplementedException();
        }

        private Uri GetServiceBusUri()
        {
            throw new NotImplementedException();
        }

        private TokenProvider GetTokenProvider()
        {
            string issuer = ""; // ConfigurationManager.AppSettings["issuer"];
            string key = ""; //"ConfigurationManager.AppSettings["key"];

            return TokenProvider.CreateSharedSecretTokenProvider(issuer, key);
        }

        public void SendMessage(IQueue queue, string message)
        {
            var tokenProvider = GetTokenProvider();
            var uri = GetServiceBusUri();

            var factory = MessagingFactory.Create(uri, tokenProvider);
            var messageSender = factory.CreateMessageSender(queue.Id);

            var brokeredMessage = new BrokeredMessage(message);

            // TODO: support metadata
            // brokeredMessage.Properties["moreData"] = "metadata";

            // messageSender.BeginSend(brokeredMessage)
            messageSender.Send(brokeredMessage);
        }

        public IQueue GetQueueById(string queueId)
        {
            throw new NotImplementedException();
        }

        public IQueueMessage ReceiveMessage(IQueue queue)
        {
            var tokenProvider = GetTokenProvider();
            var uri = GetServiceBusUri();

            var factory = MessagingFactory.Create(uri, tokenProvider);
            var messageReceiver = factory.CreateMessageReceiver(queue.Id, ReceiveMode.PeekLock);

            var message = messageReceiver.Receive();

            return new AzureServiceBusMessage(message);
        }

        public void DeleteMessage(IQueue queue, IQueueMessage message)
        {
            var azureSbMessage = message as AzureServiceBusMessage;

            if (azureSbMessage == null)
                throw new InvalidOperationException("Can't delete non Azure Service Bus message");

            azureSbMessage.BrokeredMessage.Complete();
        }

        public IQueue GetQueueByUri(Uri uri)
        {
            throw new NotImplementedException();
        }
    }
}