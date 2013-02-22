using System;
using System.Collections.Generic;
using JobAbstractions;
using log4net;

namespace JobHostWrapper
{
    public class JobHostWrapper
    {
        private readonly string _workFolder;
        readonly ILog _log = LogManager.GetLogger(typeof(JobHostWrapper));
        private List<Tuple<IUntypedRunnerConfigurator, object>> _jobs = new List<Tuple<IUntypedRunnerConfigurator, object>>();

        public JobHostWrapper(string workFolder)
        {
            _workFolder = workFolder;
        }

        public void Start()
        {
            _log.Info("JobWrapper Started");

            var jobsConfigurators = new JobEnvironment(_workFolder).GetJobsCollection();

            foreach (var jobConfig in jobsConfigurators)
            {
                try
                {
                    object job = jobConfig.ConstructJob(Guid.NewGuid().ToString());
                    jobConfig.StartJob(job);
                    _jobs.Add(new Tuple<IUntypedRunnerConfigurator, object>(jobConfig, job));
                }
                catch (Exception e)
                {
                    _log.Error("Failed to start job", e);
                }
            }

            if (_jobs.Count == 0)
                _log.Info("No jobs were found to load");
        }

        public void Stop()
        {
            foreach (var job in _jobs)
            {
                try
                {
                    job.Item1.StopJob(job.Item2);
                }
                catch (Exception e)
                {
                    _log.Error("Failed to stop job", e);
                }
            }

            _log.Info("JobWrapper Stopped");
        }
    }
}