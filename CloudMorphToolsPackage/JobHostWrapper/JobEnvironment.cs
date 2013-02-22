using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using JobAbstractions;
using log4net;

namespace JobHostWrapper
{
    public class JobEnvironment
    {
        readonly ILog _log = LogManager.GetLogger(typeof(JobEnvironment));

        private readonly string _workFolder;

        public JobEnvironment(string workFolder)
        {
            _workFolder = workFolder;
        }

        //readonly ILog _log = Logger.Get("Topshelf.Shelf." + _serviceName);

        public IEnumerable<IUntypedRunnerConfigurator> GetJobsCollection()
        {
            // TODO: check the metadata file

            var _jobs = JobUtilities.PrepareEnvironment().GetLocalFiles(_workFolder).GetActivators(typeof(IJobPackage<>));

            foreach (var job in _jobs)
            {
                object jobPackage = job.Invoke();

                Type serviceType = jobPackage.GetType()
                    .GetInterfaces()
                    .First()
                    .GetGenericArguments()
                    .First();

                Type runnerConfiguratorType = typeof(RunnerConfigurator<>);

                Type[] typeArgs = { serviceType };

                Type makeme = runnerConfiguratorType.MakeGenericType(typeArgs);

                object configurator = Activator.CreateInstance(makeme);

                jobPackage.GetType().InvokeMember("InitializeHostedService", BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.Instance | BindingFlags.InvokeMethod, null, jobPackage, new object[] { configurator });

/*
                object jobVal = configurator.GetType().InvokeMember("ConstructJob", BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.Instance | BindingFlags.InvokeMethod, null, configurator, new object[] { "someJob" });

                configurator.GetType().InvokeMember("StartJob", BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.Instance | BindingFlags.InvokeMethod, null, configurator, new object[] { jobVal });
                configurator.GetType().InvokeMember("StopJob", BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.Instance | BindingFlags.InvokeMethod, null, configurator, new object[] { jobVal });
*/
                yield return (IUntypedRunnerConfigurator) configurator;
/*
                var jobVal = unTypedConfigurator.ConstructJob("someJob");
                unTypedConfigurator.StartJob(jobVal);
                unTypedConfigurator.StopJob(jobVal);
*/
            }
        }

    }
}