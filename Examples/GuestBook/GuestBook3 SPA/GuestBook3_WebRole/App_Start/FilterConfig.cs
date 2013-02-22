using System.Web;
using System.Web.Mvc;

namespace GuestBook3_WebRole
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}