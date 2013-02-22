namespace JobAbstractions
{
    public interface IJobPackage<TJob> where TJob : class
    {
        void InitializeHostedService(IRunnerConfigurator<TJob> cfg);
    }
}