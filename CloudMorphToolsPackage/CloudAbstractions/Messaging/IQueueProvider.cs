using System;
using System.Collections.Generic;

namespace CloudAbstractions.Messaging
{
    public interface IQueueProvider
    {
        IEnumerable<IQueue> Queues { get; }
        bool CreateQueue(string queueId);
        void DeleteQueue(string queueId);
        void SendMessage(IQueue queue, string message);
        IQueue GetQueueById(string queueId);
        IQueueMessage ReceiveMessage(IQueue queue);
        void DeleteMessage(IQueue queue, IQueueMessage message);
        IQueue GetQueueByUri(Uri uri);
    }
}