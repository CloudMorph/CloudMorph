using CloudAbstractions;

namespace AzureAbstractions
{
    public class AzureQueue : IQueue
    {
        public AzureQueue(string id)
        {
            this.Id = id;
        }

        public string Id { get; internal set; }
    }
}