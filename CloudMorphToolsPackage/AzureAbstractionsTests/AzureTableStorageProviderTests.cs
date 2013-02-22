using System;
using System.Collections.Generic;
using AzureAbstractions;
using Microsoft.WindowsAzure.StorageClient;
using NUnit.Framework;

namespace AzureAbstractionsTests
{
    [TestFixture]
    public class AzureTableStorageProviderTests
    {
        public class CustomerEntity : TableServiceEntity
        {
            public CustomerEntity(string lastName, string firstName)
            {
                this.PartitionKey = lastName;
                this.RowKey = firstName;
            }

            public CustomerEntity() { }

            public string Email { get; set; }

            public string PhoneNumber { get; set; }
        }

        [Test]
        public void StoreData()
        {
            var storage = new AzureTableStorageProvider();

            const string tableName = "people";
            storage.CreateTable(tableName);

            // Create a new customer entity
            var customer1 = new CustomerEntity("Harp", "Walter")
                                {
                                    Email = "Walter@contoso.com", 
                                    PhoneNumber = "425-555-0101"
                                };

            // Add the new customer to the people table
            storage.AddObject(tableName, customer1);

            // Commit the operation to the table service
            storage.Commit();

            //storage.GetObject();
        }

        [Test]
        public void SaveGenericEntity()
        {
            var storage = new AzureTableStorageProvider();

            string tblName = "customers";
            storage.CreateTable(tblName);

            var bag = new Dictionary<string, string> 
            {
                { "Name", "Igor Moochnick" }, 
                { "Marital_Status", "married" }
            };
            var id = Guid.NewGuid();
            storage.Put(tblName, id.ToString(), bag);
            storage.Commit();
        }
    }
}