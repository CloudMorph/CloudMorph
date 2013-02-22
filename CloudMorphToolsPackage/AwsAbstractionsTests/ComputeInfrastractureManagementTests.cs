using AwsAbstractions;
using CloudAbstractions;
using NUnit.Framework;

namespace AwsAbstractionsTests
{
    [TestFixture]
    public class ComputeInfrastractureManagementTests
    {
        private IRealm realm;
        private IComputeInfrastructureProvider compute;

        [SetUp]
        public void Setup()
        {
            realm = new AwsRealm();

            compute = realm.ComputeInfrastructureProvider;
        }

        [Test]
        public void LaunchSmallComputeInstance()
        {
            compute.LaunchInstance("test");
        }
    }
}