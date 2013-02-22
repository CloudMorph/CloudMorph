using System.IO;
using AwsAbstractions;
using CloudAbstractions;
using CloudMorphPackagingTools;
using NUnit.Framework;

namespace CloudMorphDeploymentServiceTests
{
    [TestFixture]
    public class LaunchComputeInstances
    {
        [Test]
        public void LaunchNewComputeInstance()
        {
            //
            string packageBitsSource =
                @"C:\Dev\Open Source\CloudAbstractions\CloudMorph\CloudMorphToolsPackage\TopShelfJob\bin\Debug\";

            string packagePath = @"TopShelfJob.zip";

            if (File.Exists(packagePath))
                File.Delete(packagePath);

            PackageCompressor.Compress(packageBitsSource, packagePath);

            var realm = new AwsRealm();

            var storage = realm.StorageProvider;

            if (storage.ExistInBucket("igor-morphCloud-packages", packagePath))
                storage.DeleteFromBucket("igor-morphCloud-packages", packagePath);

            storage.AddToBucket("igor-morphCloud-packages", packagePath);

            var compute = realm.ComputeInfrastructureProvider;

            compute.LaunchInstance("igor-compute1", InstanceSize.Small, "TopShelfJob.zip");
        }
    }
}