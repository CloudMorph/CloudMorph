using System;
using System.Threading;
using System.Threading.Tasks;
using AwsAbstractions;
using CloudAbstractions;
using CloudAbstractions.Messaging;
using log4net;

namespace QueueListenJob
{
    public class QueueListenService
    {
        private AwsRealm _realm;
        private IQueueProvider _queueProvider;
        private IQueue _queue;
        private ILog _log;

        public void Start()
        {
            _log = LogManager.GetLogger(typeof(QueueListenService));

            new Task(DelayedStart).Start();
        }

        public void DelayedStart()
        {
            _log.Info("Test");

            _realm = new AwsRealm();
            _queueProvider = _realm.QueueProvider;
            _queue = _queueProvider.GetQueueByUri(new Uri("https://queue.amazonaws.com/867587891196/testQueue"));

            //_queueProvider.SendMessage(_queue, "hello from TopShelfJob");
            while (true)
            {
                var msg = _queueProvider.ReceiveMessage(_queue);
                if (msg != null)
                {
                    _log.Info("Received message: " + msg.Body);
                    _queueProvider.DeleteMessage(_queue, msg);
                }
                Thread.Sleep(2000);
            }
            
        }

        public void Stop()
        {
            //_queueProvider.SendMessage(_queue, "bye from TopShelfJob");
        }
    }
}