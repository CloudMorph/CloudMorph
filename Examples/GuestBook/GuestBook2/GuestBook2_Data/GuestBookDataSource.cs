using System;
using System.Collections.Generic;
using System.Linq;
using AzureAbstractions;
using CloudAbstractions;
using CloudAbstractions.Security;
using Newtonsoft.Json.Linq;

namespace GuestBook_Data
{
    public class GuestBookDataSource
    {
        private static AzureRealm _azure;
        private static IKvStorageProvider _storage;
/*
        private GuestBookDataContext context;
*/

        static GuestBookDataSource()
        {
            // TODO: storageAccount = CloudStorageAccount.FromConfigurationSetting("DataConnectionString");

            ICredentials credentials = null;

            _azure = new AzureRealm();
            _storage = _azure.KvStorageProvider;

            _storage.CreateTable("GuestBookEntry1");

            // TODO:
            //_storage.CreateTablesFromModel(
            //    typeof(GuestBookDataContext),
            //    "",
            //    credentials);
        }

        public GuestBookDataSource()
        {
/*
            this.context = new GuestBookDataContext(storageAccount.TableEndpoint.AbsoluteUri, storageAccount.Credentials);
            this.context.RetryPolicy = RetryPolicies.Retry(3, TimeSpan.FromSeconds(1));
*/
        }

        public IEnumerable<GuestBookEntry> GetGuestBookEntries()
        {
/*
            var results = from g in this.context.GuestBookEntry
                          where g.PartitionKey == DateTime.UtcNow.ToString("MMddyyyy")
                          select g;
            return results;
*/
/*
            return from bag in _storage.GetAll("GuestBookEntry1")
                   select new GuestBookEntry()
                              {
                                  Id = bag["Id"].ToString(),
                                  GuestName = bag["GuestName"].ToString(),
                                  Message = bag["Message"].ToString()
                              };
*/
            var entriesG = _storage.GetAll("GuestBookEntry1").ToList();

/*
            var entries = from bag in entriesG
                          select new GuestBookEntry
                                     {
                                         Id = bag["Id"].ToString(),
                                         GuestName = bag["GuestName"].ToString(),
                                         Message = bag["Message"].ToString()
                                     };
            var result = entries.ToList();
*/
            var result = new List<GuestBookEntry>();

            foreach (var bag in entriesG)
            {
                var r = new GuestBookEntry
                {
                    Id = bag["RowKey"].ToString(),
                    GuestName = bag["GuestName"].ToString(),
                    Message = bag["Message"].ToString(),
                    Timestamp = bag["Timestamp"].ToString()
                };

                result.Add(r);
            }

            return result;
        }

        public void AddGuestBookEntry(GuestBookEntry newItem)
        {
            var item = JObject.FromObject(newItem);
            var properties = new Dictionary<string, string>();

            foreach (var property in item.Properties())
            {
                if (property.Name != "Id")
                {
                    var val = property.Value;
                    properties.Add(property.Name, val.ToString());
                }
            }

            _storage.Put("GuestBookEntry1", newItem.Id, properties);
        }

        public void UpdateImageThumbnail(string partitionKey, string rowKey, string thumbUrl)
        {
/*
            var results = from g in this.context.GuestBookEntry
                          where g.PartitionKey == partitionKey && g.RowKey == rowKey
                          select g;

            var entry = results.FirstOrDefault<GuestBookEntry>();
            entry.ThumbnailUrl = thumbUrl;
            this.context.UpdateObject(entry);
            this.context.SaveChanges();
*/
        }
    }
}
