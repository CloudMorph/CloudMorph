using System.Collections.Generic;
using System.Web.Mvc;
using CloudMorphWebPortal.Models;

namespace CloudMorphWebPortal.Controllers
{
    public class PackagesController : Controller
    {
        //
        // GET: /Packages/

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Management()
        {
            var packages = new List<Package>
                               {
                                   new Package { Name = "Worker", Type = "worker-role" }, 
                                   new Package { Name = "Web", Type = "web-role" }
                               };

            return View(packages);
        }

        public ActionResult Upload()
        {
            return View();
        }

        public ActionResult Hosts()
        {
            return View();
        }
    }
}
