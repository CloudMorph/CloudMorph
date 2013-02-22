using CloudAbstractions;

namespace AwsAbstractions
{
    public class AwsQueueMessage : IQueueMessage
    {
        public string Id { get; set; }
        public string Body { get; set; }
        public string ReceiptHandle { get; set; }
    }
}