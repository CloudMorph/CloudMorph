using System;
using System.IO;
using Topshelf;
using Topshelf.Configuration.Dsl;
using Topshelf.Shelving;

namespace CloudMorphDeploymentService
{
    public class DeploymentServiceBootstrapper : Bootstrapper<DeploymentService>
    {
        public void InitializeHostedService(IServiceConfigurator<DeploymentService> cfg)
        {
            //cfg.ConstructUsing((description, name, coordinator) => new DeploymentService(description, coordinator));
            //cfg.HowToBuildService(n => new DeploymentService());
            cfg.WhenStarted(s =>
            {
                string configFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "DeploymentService.log4net.config");
                //XmlConfigurator.Configure(new FileInfo(configFilePath));
                s.Start();
            });
            cfg.WhenStopped(s => s.Stop());
        }
    }
}