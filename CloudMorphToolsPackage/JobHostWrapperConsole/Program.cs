using System;
using System.IO;
using log4net.Config;
using Topshelf;

namespace JobHostWrapperConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            new JobHostWrapper.JobHostWrapperProcessBootstrapper().Run(args);
        }
    }
}
