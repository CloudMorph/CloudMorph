using System;
using System.IO;
using log4net.Config;
using Topshelf.Configuration.Dsl;
using Topshelf.Shelving;

namespace TopShelfJob
{
    public class TopShelfJobBootstrapper : Bootstrapper<JobService>
    {
        public void InitializeHostedService(IServiceConfigurator<JobService> cfg)
        {
            cfg.HowToBuildService(n => new JobService());
            cfg.WhenStarted(s =>
            {
                string configFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "TopShelfJob.log4net.config");
                XmlConfigurator.Configure(new FileInfo(configFilePath));
                s.Start();
            });
            cfg.WhenStopped(s => s.Stop());
        }
    }
}