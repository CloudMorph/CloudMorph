﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MorphCloudWebConsole.Controllers
{
    public class HomeController : Controller
    {
        //
        // GET: /Home/

        public ActionResult Index()
        {
            return View();
        }
/*
        public string Index()
        {
            //return View();
            return "Hello from Home";
        }
*/

        public ActionResult About()
        {
            return View();
        }

    }
}
