namespace CloudAbstractions
{
    public enum InstanceSize
    {
        Small,
        Medium,
        Large,
        ExLarge
    }

    public interface IComputeInfrastructureProvider
    {
        void LaunchInstance(string instanceName, InstanceSize size = InstanceSize.Small, string metadata = null);
    }
}