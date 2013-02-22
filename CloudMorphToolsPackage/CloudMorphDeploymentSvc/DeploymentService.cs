using System;
using System.Configuration;
using System.IO;
using System.Threading.Tasks;
using AwsAbstractions;
using CloudMorphPackagingTools;
using log4net;

namespace CloudMorphDeploymentSvc
{
    public class DeploymentService
    {
        private ILog _log;

        public void Start()
        {
            new Task(DelayedStart).Start();
        }

        public void DelayedStart()
        {
            _log = LogManager.GetLogger(typeof(DeploymentService));

            //            _queueProvider = _realm.QueueProvider;
            //            _queueDeploymentsCommands = _queueProvider.GetQueueById("igor-cloudMorph-deployments");

            try
            {
                var realm = new AwsRealm();

                var environment = realm.Current;

                var metadata = environment.GetMetadata();

                string packageName = null;

                if (metadata.TryGetValue("metadata", out packageName))
                {
                    _log.Info("Package Name: " + packageName);
                }
                else
                {
                    _log.Warn("Can't retreive metadata.");

                    packageName = ConfigurationManager.AppSettings["packageName"];
                }

                _log.Info("Target to download the package: " + packageName);

                var storage = realm.StorageProvider;

                const string deplDir = @"Packages";
                if (!Directory.Exists(deplDir))
                    Directory.CreateDirectory(deplDir);

                string packagePath = Path.Combine(deplDir, packageName);
                if (File.Exists(packagePath))
                {
                    _log.Info("The package was already deployed. Exiting ...");
                    return;
                }

                using (var package = File.Create(packagePath))
                {
                    storage.GetFromBucket("igor-morphCloud-packages", packageName, package);
                }

                string serviceLocation = Path.Combine(@"Services", Path.GetFileNameWithoutExtension(packageName));
                _log.Info("Unpacking package to: " + serviceLocation);

                if (!Directory.Exists(serviceLocation))
                    Directory.CreateDirectory(serviceLocation);

                PackageCompressor.Decompress(packagePath, serviceLocation);
            }
            catch (Exception e)
            {
                _log.Error("Failed to get the package", e);
            }
        }


        public void Stop()
        {
        } 
    }
}