using System;
using System.Collections.Generic;
using AwsAbstractions;
using CloudAbstractions;

namespace GlobalControl
{
    public class GlobalRoster
    {
        public IEnumerable<IRealm> GetRealms()
        {
            throw new NotImplementedException();
        }

        public void AddRealm(string category)
        {
            throw new NotImplementedException();
        }

        public void UpdateInstanceStatus(Guid id, string instanceName, string status)
        {
            IRealm realm = new AwsRealm();
            IKvStorageProvider kvStorage = realm.KvStorageProvider;

            var bag = new Dictionary<string, string>();
            bag.Add("Name", instanceName);
            bag.Add("Status", status);
            kvStorage.Put("igor-cloudMorph-roster", id.ToString(), bag, true);
        }

        public void UpdateJobStatus(Guid id, string jobDescription, string status)
        {
            IRealm realm = new AwsRealm();
            IKvStorageProvider kvStorage = realm.KvStorageProvider;

            var bag = new Dictionary<string, string>();
            bag.Add("Description", jobDescription);
            bag.Add("Status", status);
            kvStorage.Put("igor-cloudMorph-roster-jobs", id.ToString(), bag, true);
        }
    }
}