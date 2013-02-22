namespace JobAbstractions
{
    public delegate TJob JobFactory<TJob>(string name)
        where TJob : class;

    public interface IRunnerConfigurator<TJob> where TJob : class
    {
        void HowToBuildJob(JobFactory<TJob> jobFactory);
        void WhenStarted(System.Action<TJob> startAction);
        void WhenStopped(System.Action<TJob> stopAction);
    }
}