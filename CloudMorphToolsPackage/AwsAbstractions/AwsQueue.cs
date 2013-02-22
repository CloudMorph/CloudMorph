using System;
using CloudAbstractions;

namespace AwsAbstractions
{
    public class AwsQueue : IQueue
    {
        public AwsQueue(string id)
        {
            this.Id = id;
        }

        public string Id { get; internal set; }
    }
}