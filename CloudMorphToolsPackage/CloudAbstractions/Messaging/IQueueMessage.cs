namespace CloudAbstractions
{
    public interface IQueueMessage
    {
        string Id { get; }
        string Body { get; set; }
    }
}