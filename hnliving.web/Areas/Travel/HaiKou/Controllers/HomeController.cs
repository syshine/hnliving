﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace hnliving.web.Areas.Travel.HaiKou.Controllers
{
    public class HomeController : Controller
    {
        // GET: Travel/HaiKou/Home
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Travel description page.";

            return View();
        }
    }
}