using System.IO;
using CloudMorphConsole.Messages;
using CloudMorphPackagingTools;

namespace CloudMorphConsole.Handlers
{
    public class PackageHandler : Handles<PackageMessage>
    {
        public string PackagePath { get; private set; }

        public void Handle(PackageMessage message)
        {
            if (!Directory.Exists(message.PackageFolderDestination))
                Directory.CreateDirectory(message.PackageFolderDestination);

            PackagePath = Path.Combine(message.PackageFolderDestination, message.UniquePackageName) + ".zip";

            if (File.Exists(PackagePath))
                File.Delete(PackagePath);

            message.SourceMap.Compress(PackagePath);

            //string target = new Packager().PackageForDeployment(@"C:\Dev\BlueMetal\CloudAbstractions\MorphCloud\CloudMorphToolsPackage\PingPongJob\bin\Debug\PingPongJob.dll");

        }

        public void MergePackages(string left, string right, string uniquPackageName1)
        {
            PackageCompressor.AddPackageContents(left, right, uniquPackageName1);
        }
    }
}