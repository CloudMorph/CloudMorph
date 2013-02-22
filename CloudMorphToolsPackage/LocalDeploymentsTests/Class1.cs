using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using ZeroMQ;
using ZMQ;

namespace LocalDeploymentsTests
{
    [TestFixture]
    public class Class1
    {
        [Test]
        public void TestZeroMqClient()
        {
            // ZMQ Context and client socket
            using (ZmqContext context = ZmqContext.Create())
            using (ZmqSocket client = context.CreateSocket(SocketType.REQ))
            {
                client.Connect("tcp://localhost:5555");

                string request = "Hello";
                for (int requestNum = 0; requestNum < 10; requestNum++)
                {
                    Console.WriteLine("Sending request {0}...", requestNum);
                    client.Send(request, Encoding.Unicode);

                    string reply = client.Receive(Encoding.Unicode);
                    Console.WriteLine("Received reply {0}: {1}", requestNum, reply);
                }
            }

        }
    }
}
