using CloudAbstractions;

namespace CloudMorphPackagingTools
{
    public class DeploymentTool
    {
        public void UploadToRealm(IRealm realm, string sourcePackagePath, string destinationBucketPath)
        {
            var storage = realm.StorageProvider;

            storage.AddToBucket(destinationBucketPath, sourcePackagePath);
        }
    }
}