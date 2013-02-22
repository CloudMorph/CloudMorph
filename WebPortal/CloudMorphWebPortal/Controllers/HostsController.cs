using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CloudMorphWebPortal.Controllers
{
    public class HostsController : Controller
    {
        //
        // GET: /Hosts/

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Host(string host)
        {
            return View();
        }

    }
}
