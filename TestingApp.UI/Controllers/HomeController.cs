﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TestingApp.UI
{
    [AllowAnonymous]
    public partial class HomeController : Controller
    {
        public virtual ActionResult Index()
        {
            return View();
        }

        public virtual ActionResult About()
        {

            //test - adina
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public virtual ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}