using System;
using AwsAbstractions;
using CloudAbstractions;
using CloudAbstractions.Messaging;
using NUnit.Framework;

namespace AwsAbstractionsTests
{
    [TestFixture]
    public class QueueManagementTests
    {
        private IRealm realm;
        private IQueueProvider queueProvider;

        [SetUp]
        public void Setup()
        {
            realm = new AwsRealm();

            queueProvider = realm.QueueProvider;
        }

        [Test]
        public void CreateQueue()
        {
            queueProvider.CreateQueue("igor-testQueue");
        }

        [Test]
        public void ListsQueues()
        {
            foreach (var queue in queueProvider.Queues)
            {
                Console.WriteLine(queue.Id);
            }
        }

        [Test]
        public void SendMessage()
        {
            var queue = queueProvider.GetQueueById("https://queue.amazonaws.com/995435520609/tetsQueue");

            queueProvider.SendMessage(queue, "hello to you");
        }

        [Test]
        public void ReadAndDeleteMessage()
        {
            var queue = queueProvider.GetQueueById("https://queue.amazonaws.com/995435520609/tetsQueue");

            var message = queueProvider.ReceiveMessage(queue);

            if (message != null)
                queueProvider.DeleteMessage(queue, message);
        }

        [Test]
        public void ClearQueue()
        {
            var queue = queueProvider.GetQueueByUri(new Uri("https://queue.amazonaws.com/995435520609/igor-testQueue"));

            bool doSome = false;
            do
            {
                var message = queueProvider.ReceiveMessage(queue);

                if (message != null)
                {
                    queueProvider.DeleteMessage(queue, message);
                    doSome = true;
                }
                else
                {
                    doSome = false;
                }
            } while (doSome);
        }
    }
}