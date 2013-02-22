using CloudAbstractions.Messaging;

namespace CloudAbstractions
{
    public interface IRealm
    {
        IRealmConfigurator Configure();
        IStorageProvider StorageProvider { get; }
        IComputeInfrastructureProvider ComputeInfrastructureProvider { get; }
        IQueueProvider QueueProvider { get; }
        IKvStorageProvider KvStorageProvider { get; }
        IEnvironment Current { get; }
    }
}