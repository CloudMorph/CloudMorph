using LocalAbstractions;
using NUnit.Framework;
using Storage.FileSystem;

namespace LocalDeploymentsTests
{
    [TestFixture]
    public class LocalConfigurationTests
    {
        [Test]
        public void ConfiguringFileSystemStorageAsStorageProvider()
        {
            var local = new LocalRealm();

            local.Configure().WithStorageProvider(provider => new FileSystemBlobStorage());
        }
    }
}