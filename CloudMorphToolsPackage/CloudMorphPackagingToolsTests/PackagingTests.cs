using System.IO;
using AwsAbstractions;
using CloudMorphPackagingTools;
using Newtonsoft.Json.Linq;
using NUnit.Framework;

namespace CloudMorphPackagingToolsTests
{
    [TestFixture]
    public class PackagingTests
    {
        [Test]
        public void TestPackaging()
        {
            //string target = new Packager().PackageForDeployment(@"C:\Dev\BlueMetal\CloudAbstractions\MorphCloud\CloudMorphToolsPackage\PingPongJob\bin\Debug\PingPongJob.dll");
            string target = new Packager().PackageForDeployment(@"C:\Dev\Open Source\CloudAbstractions\CloudMorph\CloudMorphToolsPackage\TopShelfJob\bin\Debug\TopShelfJob.dll");

            //new Packager().Unpack(target);
        }

        [Test]
        public void UploadPackage()
        {
            string packagePath = @"C:\Dev\Open Source\CloudAbstractions\CloudMorph\CloudMorphToolsPackage\TopShelfJob\bin\TopShelfJob.zip";

            var realm = new AwsRealm();

            var storage = realm.StorageProvider;

            //storage.CreateBucket("igor-morphCloud-packages");

            string id = storage.AddToBucket("igor-morphCloud-packages", packagePath);
        }

        [Test]
        public void PublishPackageForDeployment()
        {
            //string packagePath = @"C:\Dev\BlueMetal\CloudAbstractions\MorphCloud\CloudMorphToolsPackage\PingPongJob\bin\PingPongJob.zip";
            string packagePath = @"C:\Dev\Open Source\CloudAbstractions\CloudMorph\CloudMorphToolsPackage\TopShelfJob\bin\TopShelfJob.zip";

            var realm = new AwsRealm();

            var storage = realm.StorageProvider;

            //string id = storage.AddToBucket("igor-morphCloud-packages", packagePath);

            string id = Path.GetFileName(packagePath);

            var queueProvider = realm.QueueProvider;

            var queue = queueProvider.GetQueueById("igor-cloudMorph-deployments");

            var o = JObject.FromObject(new { Command = "launch-job", PackageName = id});
            queueProvider.SendMessage(queue, o.ToString());
        }

        [Test]
        public void DownloadPackageFordeployment()
        {
            string packageId = "PingPongJob.zip";

            var realm = new AwsRealm();

            var storage = realm.StorageProvider;

            using (var fs = new FileStream(packageId, FileMode.Create, FileAccess.Write))
            {
                storage.GetFromBucket("igor-morphCloud-packages", packageId, fs);
            }
        }

        [Test]
        public void CompressFileList()
        {
            var files = new[] {@"C:\Temp\Juice\python23.dll", @"C:\Temp\Juice\pythoncom23.dll", @"fewoij\ewgowiej"};

            PackageCompressor.CompressFiles(files, @"C:\Temp", @"C:\Temp\temp.zip");
        }
    }
}