using System;
using System.IO;
using System.Net;
using AzureAbstractions;
using CloudAbstractions;
using CloudAbstractions.Messaging;
using GuestBook_Data;

namespace GuestBook_WebRole
{
    public partial class _Default : System.Web.UI.Page
    {
        private static bool storageInitialized = false;
        private static object gate = new object();
        private IStorageProvider _storage;
        private IQueueProvider _queue;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                this.Timer1.Enabled = true;
            }
        }

        protected void SignButton_Click(object sender, EventArgs e)
        {
            if (this.FileUpload1.HasFile)
            {
                this.InitializeStorage();

                // upload the image to blob storage
                string uniqueBlobName = string.Format("guestbookpics/image_{0}{1}", Guid.NewGuid().ToString(), Path.GetExtension(this.FileUpload1.FileName));
                string blobUri = _storage.AddToBucket("guestbookpics1", uniqueBlobName, this.FileUpload1.FileContent);
                System.Diagnostics.Trace.TraceInformation("Uploaded image '{0}' to blob storage as '{1}'", this.FileUpload1.FileName, uniqueBlobName);

                // create a new entry in table storage
                var entry = new GuestBookEntry() { GuestName = this.NameTextBox.Text, Message = this.MessageTextBox.Text, PhotoUrl = blobUri, ThumbnailUrl = blobUri };
                var ds = new GuestBookDataSource();
                ds.AddGuestBookEntry(entry);
                System.Diagnostics.Trace.TraceInformation("Added entry {0} in table storage for guest '{1}'", entry.Id, entry.GuestName);

                // queue a message to process the image
                var queue = _queue.GetQueueById("guestthumbs");
                var message = string.Format("{0},{1}", blobUri, entry.Id);
                _queue.SendMessage(queue, message);
                System.Diagnostics.Trace.TraceInformation("Queued message to process blob '{0}'", uniqueBlobName);
            }

            this.NameTextBox.Text = string.Empty;
            this.MessageTextBox.Text = string.Empty;

            this.DataList1.DataBind();
        }

        protected void Timer1_Tick(object sender, EventArgs e)
        {
            this.DataList1.DataBind();
        }

        private void InitializeStorage()
        {
            if (storageInitialized)
            {
                return;
            }

            lock (gate)
            {
                if (storageInitialized)
                {
                    return;
                }

                try
                {
                    // create blob container for images
                    var _azure = new AzureRealm();
                    _storage = _azure.StorageProvider;
                    _storage.CreateBucket("guestbookpics1");
                    //_storage.CreateIfNotExist();

                    // configure container for public access
                    //var permissions = container.GetPermissions();
                    //permissions.PublicAccess = BlobContainerPublicAccessType.Container;
                    //container.SetPermissions(permissions);

                    // create queue to communicate with worker role
                    _queue = _azure.QueueProvider;
                    _queue.CreateQueue("guestthumbs");
                    //queue.CreateIfNotExist();
                }
                catch (WebException)
                {
                    throw new WebException("Storage services initialization failure. "
                        + "Check your storage account configuration settings. If running locally, "
                        + "ensure that the Development Storage service is running.");
                }

                storageInitialized = true;
            }
        }
    }
}
