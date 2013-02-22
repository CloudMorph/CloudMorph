using System.Diagnostics;
using System.Net;
using System.Threading;
using JobHostWrapper;
using Microsoft.WindowsAzure.ServiceRuntime;

namespace WorkerRole1
{
    public class WorkerRole : RoleEntryPoint
    {
        private JobHostWrapperProcessBootstrapper _host;
        private readonly ManualResetEvent waitForStop = new ManualResetEvent(false);

        public override void Run()
        {
            // This is a sample worker implementation. Replace with your logic.
            Trace.WriteLine("$projectname$ entry point called", "Information");

            _host.Run();
            waitForStop.WaitOne();
        }

        public override void OnStop()
        {
            //_host.Stop();
            waitForStop.Set();

            base.OnStop();
        }

        public override bool OnStart()
        {
            _host = new JobHostWrapper.JobHostWrapperProcessBootstrapper(); //.Run(args);

            // Set the maximum number of concurrent connections 
            ServicePointManager.DefaultConnectionLimit = 12;

            // For information on handling configuration changes
            // see the MSDN topic at http://go.microsoft.com/fwlink/?LinkId=166357.

            return base.OnStart();
        }
    }
}
