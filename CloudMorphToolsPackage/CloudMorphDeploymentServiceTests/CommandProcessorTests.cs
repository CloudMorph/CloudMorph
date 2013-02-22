using Newtonsoft.Json.Linq;
using NUnit.Framework;

namespace CloudMorphDeploymentServiceTests
{
    [TestFixture]
    public class CommandProcessorTests
    {
        [Test]
        public void TestDeploymentMessage()
        {
            string packageId = "PingPongJob.zip";
            var o = JObject.FromObject(new { Command = "launch-job", PackageName = packageId });

            string msg = o.ToString();

            //new CommandProcessor(null).Process(msg);
        }
    }
}