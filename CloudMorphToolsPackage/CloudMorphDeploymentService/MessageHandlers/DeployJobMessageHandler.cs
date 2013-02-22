using System;
using System.IO;
using AwsAbstractions;
using CloudMorphDeploymentService.Messages;
using CloudMorphPackagingTools;

namespace CloudMorphDeploymentService.MessageHandlers
{
    public class DeployJobMessageHandler : Handles<DeployJobMessage>
    {
        public void Handle(DeployJobMessage message)
        {
            var realm = new AwsRealm();
            var storage = realm.StorageProvider;

            if (!Directory.Exists(Configuration.DownloadFolder))
                Directory.CreateDirectory(Configuration.DownloadFolder);

            string packageDestinationPath = Path.Combine(Configuration.DownloadFolder, message.PackageName);

            using (var fs = new FileStream(packageDestinationPath, FileMode.Create, FileAccess.Write))
            {
                storage.GetFromBucket(Configuration.PackageStorageDomain, message.PackageName, fs);
            }

            string guid = Guid.NewGuid().ToString();
            string jobDeploymentFolder = Path.Combine(Configuration.DeploymentFolder, guid);

            if (!Directory.Exists(jobDeploymentFolder))
                Directory.CreateDirectory(jobDeploymentFolder);

            PackageCompressor.Decompress(packageDestinationPath, jobDeploymentFolder);

            // Add the Job Wrapper
            PackageCompressor.Decompress("JobHostWrapper.zip", jobDeploymentFolder);
        }
    }
}