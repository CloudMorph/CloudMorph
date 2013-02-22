namespace JobAbstractions
{
    public interface IUntypedRunnerConfigurator
    {
        object ConstructJob(string name);
        void StartJob(object job);
        void StopJob(object job);
    }
}