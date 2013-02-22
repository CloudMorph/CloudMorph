using System;
using JobAbstractions;

namespace PingJob
{
    public class JobPackage : IJobPackage<PingJob>
    {
        public JobPackage()
        {
            Console.WriteLine("Yes!!!");
        }

        public void InitializeHostedService(IRunnerConfigurator<PingJob> cfg)
        {
            cfg.HowToBuildJob(n => new PingJob());
            cfg.WhenStarted(s => s.Start() );
            cfg.WhenStopped(s => s.Stop());
        }
    }
}