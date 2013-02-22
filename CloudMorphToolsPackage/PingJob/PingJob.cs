using System;
using CloudAbstractions.Configurations;
using CloudAbstractions.EnvironmentConfiguration;

namespace PingJob
{
    public class PingJob
    {
        private IEnvironment _environment;
        //private IOutQueue _outQueue;

        public void Start()
        {
            //_environment = environment;
            //_outQueue = _environment.GetOutQueue();
            Console.WriteLine("Job started");
        }

        public void Stop()
        {
            Console.WriteLine("Job stopped");
        }

        [MessageReceive]
        public void OnMessageReceived(string message)
        {
            //_outQueue.
        }
    }
}