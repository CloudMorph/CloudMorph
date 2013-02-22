using System;
using System.IO;
using log4net;
using log4net.Config;
using Topshelf.Configuration.Dsl;
using Topshelf.Shelving;

namespace JobHostWrapper
{
    public class JobHostWrapperBootstrapper : Bootstrapper<JobHostWrapper>
    {
        public void InitializeHostedService(IServiceConfigurator<JobHostWrapper> cfg)
        {
            //cfg.Named(JobConfig.);
            cfg.HowToBuildService(s => new JobHostWrapper(AppDomain.CurrentDomain.BaseDirectory));
            cfg.WhenStarted(s =>
            {
                log4net.GlobalContext.Properties["PackageName"] = Path.GetFileName(AppDomain.CurrentDomain.BaseDirectory);
                ThreadContext.Properties["PackageName"] = Path.GetFileName(AppDomain.CurrentDomain.BaseDirectory);
                XmlConfigurator.Configure(new FileInfo(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "JobHostWrapper.log4net.config")));

                s.Start();
            });
            cfg.WhenStopped(h => h.Stop());
        }
    }
}