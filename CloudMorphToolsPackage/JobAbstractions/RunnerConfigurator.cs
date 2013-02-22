using System;

namespace JobAbstractions
{
    public class RunnerConfigurator<TJob> : IRunnerConfigurator<TJob>, IUntypedRunnerConfigurator where TJob : class
    {
        private Action<TJob> _startAction;
        private Action<TJob> _stopAction;
        private JobFactory<TJob> _jobFactory;

        public void HowToBuildJob(JobFactory<TJob> jobFactory)
        {
            _jobFactory = jobFactory;
        }

        public void WhenStarted(Action<TJob> startAction)
        {
            _startAction = startAction;
        }

        public void WhenStopped(Action<TJob> stopAction)
        {
            _stopAction = stopAction;
        }

        public object ConstructJob(string name)
        {
            return _jobFactory(name);
        }

        public void StartJob(object job)
        {
            _startAction((TJob)job);
        }

        public void StopJob(object job)
        {
            _stopAction((TJob)job);
        }
    }
}