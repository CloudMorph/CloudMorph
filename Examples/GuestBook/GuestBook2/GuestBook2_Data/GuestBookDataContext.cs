using System.Linq;

namespace GuestBook_Data
{
    public class GuestBookDataContext1
    {
/*
        public GuestBookDataContext(string baseAddress, Microsoft.WindowsAzure.StorageCredentials credentials)
            : base(baseAddress, credentials)
        {
        }
*/

        public IQueryable<GuestBookEntry> GuestBookEntry
        {
            get
            {
                //return this.CreateQuery<GuestBookEntry>("GuestBookEntry");
                return null;
            }
        }
    }
}
