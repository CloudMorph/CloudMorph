using System;
using System.Collections.Generic;
using System.Linq;
using Amazon.Runtime;
using Amazon.SQS;
using Amazon.SQS.Model;
using CloudAbstractions;
using CloudAbstractions.Messaging;
using Attribute = Amazon.SQS.Model.Attribute;

namespace AwsAbstractions
{
    public class AwsQueueProvider : IQueueProvider
    {
        private readonly AWSCredentials _credentials;
        private readonly AmazonSQSClient _sqsClient;

        public AwsQueueProvider(AWSCredentials credentials)
        {
            _credentials = credentials;
            _sqsClient = new AmazonSQSClient(_credentials);
        }

        public IEnumerable<IQueue> Queues
        {
            get
            {
                var request = new ListQueuesRequest();

                var response = _sqsClient.ListQueues(request);

                return response.ListQueuesResult.QueueUrl.Select(queueUrl => new AwsQueue(queueUrl));
            }
        }

        public bool CreateQueue(string name)
        {
            var request = new CreateQueueRequest()
                .WithQueueName(name);
            _sqsClient.CreateQueue(request);

            return true;
        }

        public void DeleteQueue(string queueId)
        {
            throw new NotImplementedException();
        }

        public void SendMessage(IQueue queue, string message)
        {
            var request = new SendMessageRequest()
                .WithQueueUrl(queue.Id)
                .WithDelaySeconds(0)
                .WithMessageBody(message);
            _sqsClient.SendMessage(request);
        }

        public IQueue GetQueueById(string queueId)
        {
            GetQueueUrlRequest request = new GetQueueUrlRequest()
                .WithQueueName(queueId);
            var response = _sqsClient.GetQueueUrl(request);

            return new AwsQueue(response.GetQueueUrlResult.QueueUrl);
        }

        public IQueue GetQueueByUri(Uri uri)
        {
            return new AwsQueue(uri.ToString());
        }

        public IQueueMessage ReceiveMessage(IQueue queue)
        {
            ReceiveMessageRequest request = new ReceiveMessageRequest()
                .WithQueueUrl(queue.Id)
                .WithMaxNumberOfMessages(1)
                .WithAttributeName(new []{"All"});
            var response = _sqsClient.ReceiveMessage(request);

            return response.ReceiveMessageResult.Message
                        .Select(message => new AwsQueueMessage { Id = message.MessageId, Body = message.Body, ReceiptHandle = message.ReceiptHandle })
                        .DefaultIfEmpty(null)
                        .FirstOrDefault();
        }

        public void DeleteMessage(IQueue queue, IQueueMessage message)
        {
            var awsMessage = (AwsQueueMessage)message;

            DeleteMessageRequest request = new DeleteMessageRequest()
                .WithQueueUrl(queue.Id)
                .WithReceiptHandle(awsMessage.ReceiptHandle);
            _sqsClient.DeleteMessage(request);
        }

        public void DeleteBatch(IQueue queue, IEnumerable<IQueueMessage> messageBatch)
        {
            var request = new DeleteMessageBatchRequest();

            messageBatch.Cast<AwsQueueMessage>()
                .Select(message => new DeleteMessageBatchRequestEntry() { Id = message.Id, ReceiptHandle = message.ReceiptHandle})
                .ForEach(entity => request.Entries.Add(entity));

            _sqsClient.DeleteMessageBatch(request);
        }
    }
}