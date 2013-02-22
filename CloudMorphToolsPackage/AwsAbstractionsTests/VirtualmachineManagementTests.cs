using AwsAbstractions;
using CloudAbstractions;
using NUnit.Framework;

namespace AwsAbstractionsTests
{
    [TestFixture]
    public class VirtualmachineManagementTests
    {
        [Test]
        public void LaunchVmInstance()
        {
            IRealm realm = new AwsRealm();

            var infrastructureProvider = realm.ComputeInfrastructureProvider;

            infrastructureProvider.LaunchInstance("IgorTest-instance 2");
        }
    }
}