using System;
using System.Collections.Generic;
using System.Data.Services.Client;
using System.Data.Services.Common;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using CloudAbstractions;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.StorageClient;

namespace AzureAbstractions
{
    // REST APi - http://msdn.microsoft.com/en-us/library/windowsazure/dd179421

    public class AzureTableStorageProvider : IKvStorageProvider
    {
        private TableServiceContext _serviceContext;
        private CloudTableClient _tableClient;

        [DataServiceKey("PartitionKey", "RowKey")]
        public class GenericEntity
        {
            public string PartitionKey { get; set; }
            public string RowKey { get; set; }

            public Tuple<string, string>[] ExtendedProperties
            {
                set
                {
                    foreach (var val in value)
                    {
                        // TODO: What the hell?!!!
                        if (! properties.ContainsKey(val.Item1))
                            properties.Add(val.Item1, val.Item2);
                    }
                }
            }

            internal Dictionary<string, object> properties = new Dictionary<string, object>();

            internal object this[string key]
            {
                get
                {
                    return this.properties[key];
                }

                set
                {
                    this.properties[key] = value;
                }
            }

            public override string ToString()
            {
                // TODO: append each property   
                return "";
            }
        } 


        public AzureTableStorageProvider()
        {
            // Retrieve storage account from connection string
            var storageAccount = CloudStorageAccount.Parse(CloudConfigurationManager.GetSetting("DataConnectionString"));
            //var storageAccount = CloudStorageAccount.Parse("DefaultEndpointsProtocol=http;AccountName=bmaigorcloudmorphstorage;AccountKey=60VMAzM8UiGe5NuL5t6IMpXm8mo2qiuKkBJY5CQg8Pz2XtF0Dm57tVDP4oTvmlvc8AV8Gh4HWSFz9fmxeszVDA==");

            Initialize(storageAccount);
        }

        public AzureTableStorageProvider(CloudStorageAccount storageAccount)
        {
            Initialize(storageAccount);
        }

        private void Initialize(CloudStorageAccount storageAccount)
        {
            // Create the table client
            _tableClient = storageAccount.CreateCloudTableClient();

            // Get the data service context
            _serviceContext = _tableClient.GetDataServiceContext();
        }

        public void CreateTable(string tableName)
        {
            // Create the table if it doesn't exist
            _tableClient.CreateTableIfNotExist(tableName);
        }

        public void AddObject(string entitySetName, object data)
        {
            // Add the new customer to the people table
            _serviceContext.AddObject(entitySetName, data);
        }

        public void Commit()
        {
            // Commit the operation to the table service
            _serviceContext.SaveChangesWithRetries();
            //serviceContext.SaveChangesWithRetries(SaveChangesOptions.Batch);
        }

        public object GetObject(string tableName, string partitionKey, string rowKey)
        {
            // Return the entity with partition key of "Smith" and row key of "Jeff"
/*
            return (from e in _serviceContext.CreateQuery<object>("people")
                 where e.PartitionKey == "Smith" && e.RowKey == "Jeff"
                 select e).FirstOrDefault();
*/

            return null;
        }

        public void ReadGeneric(string tableName)
        {
            _serviceContext.IgnoreMissingProperties = true;

            _serviceContext.ReadingEntity += OnReadingEntity;

            var customers = from o in _serviceContext.CreateQuery<GenericEntity>(tableName) select o;

            Console.WriteLine("Rows from '{0}'", tableName);

            foreach (GenericEntity entity in customers)
            {
                Console.WriteLine(entity.ToString());
            }  

        }

        // Credit goes to Pablo from ADO.NET Data Service team 
        public void OnReadingEntity(object sender, ReadingWritingEntityEventArgs args)
        {
            // TODO: Make these statics   
            XNamespace AtomNamespace = "http://www.w3.org/2005/Atom";
            XNamespace AstoriaDataNamespace = "http://schemas.microsoft.com/ado/2007/08/dataservices";
            XNamespace AstoriaMetadataNamespace = "http://schemas.microsoft.com/ado/2007/08/dataservices/metadata";

            var entity = args.Entity as GenericEntity;
            if (entity == null)
            {
                return;
            }

            // read each property, type and value in the payload   
            var properties = args.Entity.GetType().GetProperties();
            var q = from p in args.Data.Element(AtomNamespace + "content")
                                    .Element(AstoriaMetadataNamespace + "properties")
                                    .Elements()
                    where properties.All(pp => pp.Name != p.Name.LocalName)
                    select new
                    {
                        Name = p.Name.LocalName,
                        IsNull = string.Equals("true", p.Attribute(AstoriaMetadataNamespace + "null") == null ? null : p.Attribute(AstoriaMetadataNamespace + "null").Value, StringComparison.OrdinalIgnoreCase),
                        TypeName = p.Attribute(AstoriaMetadataNamespace + "type") == null ? null : p.Attribute(AstoriaMetadataNamespace + "type").Value,
                        p.Value
                    };

            foreach (var dp in q)
            {
                entity[dp.Name] = GetTypedEdmValue(dp.TypeName, dp.Value, dp.IsNull);
            }
        }

        private static object GetTypedEdmValue(string type, string value, bool isnull)
        {
            if (isnull) return null;

            if (string.IsNullOrEmpty(type)) return value;

            switch (type)
            {
                case "Edm.String": return value;
                case "Edm.Byte": return Convert.ChangeType(value, typeof(byte));
                case "Edm.SByte": return Convert.ChangeType(value, typeof(sbyte));
                case "Edm.Int16": return Convert.ChangeType(value, typeof(short));
                case "Edm.Int32": return Convert.ChangeType(value, typeof(int));
                case "Edm.Int64": return Convert.ChangeType(value, typeof(long));
                case "Edm.Double": return Convert.ChangeType(value, typeof(double));
                case "Edm.Single": return Convert.ChangeType(value, typeof(float));
                case "Edm.Boolean": return Convert.ChangeType(value, typeof(bool));
                case "Edm.Decimal": return Convert.ChangeType(value, typeof(decimal));
                case "Edm.DateTime": return XmlConvert.ToDateTime(value, XmlDateTimeSerializationMode.RoundtripKind);
                case "Edm.Binary": return Convert.FromBase64String(value);
                case "Edm.Guid": return new Guid(value);

                default: throw new NotSupportedException("Not supported type " + type);
            }
        }   

        public void Put(string tableName, string id, Dictionary<string, string> properties, bool replace = false)
        {
            _serviceContext.WritingEntity += OnWritingEntity;

            var entity = new GenericEntity();
            foreach (var propertyPair in properties)
            {
                entity[propertyPair.Key] = propertyPair.Value;
            }

            entity.PartitionKey = "SomePartition";
            entity.RowKey = id;

            _serviceContext.AddObject(tableName, entity);
            _serviceContext.SaveChanges();

            _serviceContext.WritingEntity -= OnWritingEntity;
        }

        public IEnumerable<Dictionary<string, object>> GetAll(string domain)
        {
            _serviceContext.IgnoreMissingProperties = true;
            _serviceContext.MergeOption = MergeOption.NoTracking;

            _serviceContext.ReadingEntity += OnReadingEntity1;

            var customers = from o in _serviceContext.CreateQuery<GenericEntity>(domain) select o;

/*
            Console.WriteLine("Rows from '{0}'", domain);

            foreach (GenericEntity entity in customers)
            {
                Console.WriteLine(entity.ToString());
            }
*/
            var result = customers.ToList();
            
            var r = result.Select(c => c.properties);

            //_serviceContext.ReadingEntity -= OnReadingEntity1;

            return r;
        }

        private void OnReadingEntity1(object sender, ReadingWritingEntityEventArgs args)
        {
            XNamespace Atom = "http://www.w3.org/2005/Atom";
            XNamespace Meta = "http://schemas.microsoft.com/ado/2007/08/dataservices/metadata";
            XNamespace Data = "http://schemas.microsoft.com/ado/2007/08/dataservices";

            var entry = args.Entity as GenericEntity;

            if (entry == null)
                return;

            XElement properties = args.Data
                .Element(Atom + "content")
                .Element(Meta + "properties");

            //select metadata from the extended properties
            entry.ExtendedProperties = (from p in properties.Elements()
                                        where p.Name.Namespace == Data && !IsReservedPropertyName(p.Name.LocalName) && !string.IsNullOrEmpty(p.Value)
                                        select new Tuple<string, string>(p.Name.LocalName, p.Value)).ToArray();
        }

        private void OnWritingEntity(object sender, ReadingWritingEntityEventArgs args)
        {
            XNamespace Atom = "http://www.w3.org/2005/Atom";
            XNamespace Meta = "http://schemas.microsoft.com/ado/2007/08/dataservices/metadata";
            XNamespace Data = "http://schemas.microsoft.com/ado/2007/08/dataservices";

            var entity = args.Entity as GenericEntity;
            if (entity == null)
                return;
            
            //String combinedName = String.Format("{0}, {1}", city.Name, city.Country);
            //XElement xElement = new XElement(d + "CombinedName", combinedName);
            //XElement properties = args.Data.Descendants(m + "properties").First();
            //properties.Add(xElement);

            XElement Properties = args.Data
                    .Element(Atom + "content")
                    .Element(Meta + "properties");

            //add extended properties from the metadata
            foreach (var p in (from p in entity.properties
                                    where !IsReservedPropertyName(p.Key) && !string.IsNullOrEmpty((string)p.Value)
                                    select p))
            {
                Properties.Add(new XElement(Data + p.Key, p.Value));
            }
        }

        private bool IsReservedPropertyName(string key)
        {
            switch (key)
            {
                case "PartitionKey":
                //case "RowKey":
                    return true;
            }
            // TODO:
            return false;
        }
    }
}