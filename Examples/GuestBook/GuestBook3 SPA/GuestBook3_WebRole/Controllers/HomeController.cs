using System;
using System.IO;
using System.Net;
using System.Web.Mvc;

namespace GuestBook3_WebRole.Controllers
{
    public class HomeController : Controller
    {
        // This action handles the form POST and the upload
        [HttpPost]
        public ActionResult Index()
        {
            var req = Request.Files;

            var userName = Request.Form["userName"];

            foreach (string file in Request.Files)
            {
                var hpf = Request.Files[file];
                if (hpf == null || hpf.ContentLength == 0)
                    continue;
                string savedFileName = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, Path.GetFileName(hpf.FileName));
                hpf.SaveAs(savedFileName);
            }

            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }
    }
}
