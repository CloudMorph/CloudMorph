using System;
using System.IO;
using Topshelf.Configuration.Dsl;
using Topshelf.Shelving;
using log4net.Config;

namespace CloudMorphDeploymentSvc
{
    public class DeploymentServiceBootstrapper : Bootstrapper<DeploymentService>
    {
        public void InitializeHostedService(IServiceConfigurator<DeploymentService> cfg)
        {
            cfg.HowToBuildService(n => new DeploymentService());
            cfg.WhenStarted(s =>
            {
                string configFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "DeploymentService.log4net.config");
                XmlConfigurator.Configure(new FileInfo(configFilePath));

                s.Start();
            });
            cfg.WhenStopped(s => s.Stop());
        }
    }
}