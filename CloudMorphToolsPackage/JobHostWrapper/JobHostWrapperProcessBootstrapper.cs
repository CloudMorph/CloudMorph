using System;
using System.IO;
using log4net.Config;
using Topshelf;

namespace JobHostWrapper
{
    public class JobHostWrapperProcessBootstrapper
    {
        public void Run(string[] args = null)
        {
            string workFolder = Directory.GetCurrentDirectory();

            string logFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "log4net.config");

            XmlConfigurator.ConfigureAndWatch(new FileInfo(logFilePath));

            HostFactory.Run(x =>
            {
                x.Service<JobHostWrapper>(s =>
                {
                    s.SetServiceName("TownCrier");
                    s.ConstructUsing(name => new JobHostWrapper(workFolder));
                    s.WhenStarted(tc => tc.Start());
                    s.WhenStopped(tc => tc.Stop());
                });

                x.RunAsLocalSystem();

                x.SetDescription("JobHostWrapper Host");
                x.SetDisplayName("Stuff");
                x.SetServiceName("stuff");
            });
        }
    }
}