using System;
using System.Collections.Generic;
using System.IO;
using CloudAbstractions;

namespace Storage.Raven
{
    public class RavenBlobStorage : IStorageProvider
    {
        public RavenBlobStorage()
        {
            //_store = new DocumentStore { Url = "http://localhost:8080" };
/*
            _store = new EmbeddableDocumentStore { DataDirectory = "Data" };
            _store.Initialize();
*/
        }

/*
        public IBlobDocument GetBlob(string blobName)
        {
            var dbCommands = _session.Advanced.DatabaseCommands;
            //context.Response.StatusCode = (int)HttpStatusCode.NotFound;
            var attachement = dbCommands.GetAttachment(blobName);
            var dataStream = attachement.Data();
            var buff = new MemoryStream();
            dataStream.CopyTo(buff);
            //var format = attachement.Metadata["Format"];);

            _session.Store(new MetalCloudPackage { Name = blobName, AttachmentReference = blobName });
            _session.SaveChanges();

            return new RavenDbBlob() { Id = blobName, Data = buff.ToArray() };
        }

        public void SaveBlob(string blobName, byte[] data)
        {
            var dbCommands = _session.Advanced.DatabaseCommands;
            // We can freely choose an id.
            // I've decided to use the document id to which the attachement belongs to
            var optionalMetaData = new RavenJObject();
            //optionalMetaData["Format"] = "PNG";
            dbCommands.PutAttachment(blobName, Guid.NewGuid(), new MemoryStream(data), optionalMetaData);

            // To update a document just use the put command again
            dbCommands.PutAttachment("blogposts/3073", Guid.NewGuid(),
                File.ReadAllBytes(@"C:\Users\Gamlor\Desktop\newerImage.png"),
                new RavenJObject());

            dbCommands.DeleteAttachment(post.Id, null);
        }
*/
        public IEnumerable<IBucket> Buckets
        {
            get { throw new NotImplementedException(); }
        }

        public IBucket CreateBucket(string bucketName)
        {
            throw new NotImplementedException();
        }

        public string AddToBucket(string bucketName, string path)
        {
            throw new NotImplementedException();
        }

        public string AddToBucket(string bucketName, string id, Stream stream)
        {
            throw new NotImplementedException();
        }

        public void GetFromBucket(string bucketName, string objectId, Stream receiverStream)
        {
            throw new NotImplementedException();
        }

        public void DeleteFromBucket(string bucketName, string objectId)
        {
            throw new NotImplementedException();
        }

        public bool ExistInBucket(string bucketName, string objectId)
        {
            throw new NotImplementedException();
        }
    }
}