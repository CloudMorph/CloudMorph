namespace CloudAbstractions.EnvironmentConfiguration
{
    public interface IEnvironment
    {
        IResource GetResource(string resourceId);
        IResource GetOutQueue();
        IResource GetInQueue();
    }
}