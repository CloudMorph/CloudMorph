using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Net;
using System.Threading;
using AzureAbstractions;
using CloudAbstractions;
using CloudAbstractions.Messaging;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.ServiceRuntime;
using Microsoft.WindowsAzure.StorageClient;

namespace GuestBook2_WorkerRole
{
    public class WorkerRole : RoleEntryPoint
    {
        //private CloudQueue queue;
        //private CloudBlobContainer container;
        private IStorageProvider _storage;
        private IQueueProvider _queueProvider;
        private IQueue _queue;

        public override void Run()
        {
            Trace.TraceInformation("Listening for queue messages...");

            while (true)
            {
                try
                {
                    // retrieve a new message from the queue
                    var msg = this._queueProvider.ReceiveMessage(_queue);
                    if (msg != null)
                    {
                    }
                    else
                    {
                        System.Threading.Thread.Sleep(1000);
                    }
                }
                catch (StorageClientException e)
                {
                    Trace.TraceError("Exception when processing queue item. Message: '{0}'", e.Message);
                    System.Threading.Thread.Sleep(5000);
                }
            }
        }

        public override bool OnStart()
        {
            // Set the maximum number of concurrent connections 
            ServicePointManager.DefaultConnectionLimit = 12;

            // read storage account configuration settings
            CloudStorageAccount.SetConfigurationSettingPublisher((configName, configSetter) =>
            {
                configSetter(RoleEnvironment.GetConfigurationSettingValue(configName));
            });

            var _azure = new AzureRealm();

            // initialize blob storage
            _storage = _azure.StorageProvider;
            _storage.CreateBucket("guestbookpics1");

            // initialize queue storage
            _queueProvider = _azure.QueueProvider;
            _queueProvider.CreateQueue("guestthumbs");
            _queue = _queueProvider.GetQueueById("guestthumbs");

            Trace.TraceInformation("Creating container and queue...");

            return base.OnStart();
        }

        public void ProcessImage(Stream input, Stream output)
        {
            int width;
            int height;
            var originalImage = new Bitmap(input);

            if (originalImage.Width > originalImage.Height)
            {
                width = 128;
                height = 128 * originalImage.Height / originalImage.Width;
            }
            else
            {
                height = 128;
                width = 128 * originalImage.Width / originalImage.Height;
            }

            Bitmap thumbnailImage = null;

            try
            {
                thumbnailImage = new Bitmap(width, height);

                using (Graphics graphics = Graphics.FromImage(thumbnailImage))
                {
                    graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                    graphics.SmoothingMode = SmoothingMode.AntiAlias;
                    graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;
                    graphics.DrawImage(originalImage, 0, 0, width, height);
                }

                thumbnailImage.Save(output, ImageFormat.Jpeg);
            }
            finally
            {
                if (thumbnailImage != null)
                {
                    thumbnailImage.Dispose();
                }
            }
        }
    }
}
