using System.IO;
using JobAbstractions;
using JobHostWrapper;
using NUnit.Framework;

namespace JobHostWrapperTests
{
    [TestFixture]
    public class Class1
    {
        [Test]
        public void TestJobInit()
        {
            var jobWrapper = new JobHostWrapper.JobHostWrapper(Directory.GetCurrentDirectory());

            jobWrapper.Start();

            jobWrapper.Stop();
        }

        [Test]
        public void LoadJobIntoAppDomainForMetadata()
        {
            const string workFolder = @"C:\Dev\BlueMetal\CloudAbstractions\CloudMorph\CloudMorphToolsPackage\PingJob\bin\Debug\PingJob.dll";
            var jobs = JobUtilities.PrepareEnvironment().GetLocalFiles(workFolder).GetActivatorsInAppDomain(typeof(IJobPackage<>));
        }
    }
}
