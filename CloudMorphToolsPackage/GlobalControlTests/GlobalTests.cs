using System;
using GlobalControl;
using NUnit.Framework;

namespace GlobalControlTests
{
    [TestFixture]
    public class GlobalTests
    {
        [Test]
        public void TestStatusUpdate()
        {
            var id = new Guid("4DB0B54C-9B89-474A-B8E4-2CB0011C2962");
            new GlobalRoster().UpdateInstanceStatus(id, "boom", "barm");
        }

        [Test]
        public void TestStatusUpdateJobs()
        {
            var id = new Guid("4DB0B54C-9B89-474A-B8E4-2CB0011C2962");
            new GlobalRoster().UpdateJobStatus(id, "boom", "barm");
        }
    }
}