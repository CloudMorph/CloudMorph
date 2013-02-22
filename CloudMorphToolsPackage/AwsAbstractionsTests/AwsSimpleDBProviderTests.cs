using System;
using System.Collections.Generic;
using AwsAbstractions;
using NUnit.Framework;

namespace AwsAbstractionsTests
{
    [TestFixture]
    public class AwsSimpleDBProviderTests
    {
        [Test]    
        public void SaveData()
        {
            var realm = new AwsRealm();

            var provider = realm.KvStorageProvider;

            string tblName = "customers";
            provider.CreateTable(tblName);

            var bag = new Dictionary<string, string> 
            {
                { "Name", "Igor Moochnick" }, 
                { "Marital Status", "married" }
            };
            var id = Guid.NewGuid();
            provider.Put(tblName, id.ToString(), bag, true);
        }
    }
}