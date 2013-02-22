using System;
using AwsAbstractions;
using CloudAbstractions;
using CloudAbstractions.Messaging;

namespace TopShelfJob
{
    public class JobService
    {
        private AwsRealm _realm;
        private IQueueProvider _queueProvider;
        private IQueue _queue;

        public void Start()
        {
            _realm = new AwsRealm();
            _queueProvider = _realm.QueueProvider;
            _queue = _queueProvider.GetQueueByUri(new Uri("https://queue.amazonaws.com/867587891196/testQueue"));

            _queueProvider.SendMessage(_queue, "hello from TopShelfJob");
        }

        public void Stop()
        {
            _queueProvider.SendMessage(_queue, "bye from TopShelfJob");
        }
    }
}