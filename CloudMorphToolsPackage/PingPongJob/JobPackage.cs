using CloudAbstractions;
using JobAbstractions;

namespace PingPongJob
{
    public class JobPackage : IJobPackage<Job>
    {
        public void InitializeHostedService(IRunnerConfigurator<Job> cfg)
        {
            cfg.HowToBuildJob(n => new Job());
            //cfg.WhenStarted(s => s.Start() );
            //cfg.WhenStopped(s => s.Stop());
        }
    }
}