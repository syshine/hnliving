﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace hnliving.web.Areas.Study.Controllers.CivilServant
{
    public class HomeController : Controller
    {
        // GET: Study/CivilServant/Home
        public ActionResult Index()
        {
            return View("~/Areas/Study/Views/CivilServant/Home/Index.cshtml");
        }
    }
}