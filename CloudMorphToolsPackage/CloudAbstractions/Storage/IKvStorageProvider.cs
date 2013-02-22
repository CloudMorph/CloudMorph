using System.Collections.Generic;

namespace CloudAbstractions
{
    public interface IKvStorageProvider
    {
        void CreateTable(string domain);
        void Put(string domain, string id, Dictionary<string, string> properties, bool replace = false);
        IEnumerable<Dictionary<string, object>> GetAll(string domain);
    }
}