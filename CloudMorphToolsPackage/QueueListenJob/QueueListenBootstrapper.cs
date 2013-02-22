using System;
using System.IO;
using Topshelf.Configuration.Dsl;
using Topshelf.Shelving;
using log4net.Config;

namespace QueueListenJob
{
    public class QueueListenBootstrapper : Bootstrapper<QueueListenService>
    {
        public void InitializeHostedService(IServiceConfigurator<QueueListenService> cfg)
        {
            cfg.HowToBuildService(n => new QueueListenService());
            cfg.WhenStarted(s =>
            {
                string configFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "QueueListenJob.log4net.config");
                XmlConfigurator.Configure(new FileInfo(configFilePath));
                s.Start();
            });
            cfg.WhenStopped(s => s.Stop());
        }
    }
}