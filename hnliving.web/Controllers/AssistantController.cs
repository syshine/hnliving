﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace hnliving.web.Controllers
{
    public class AssistantController : BaseWebController
    {
        // GET: Assistant
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Building()
        {
            return View();
        }

        public ActionResult NotFound()
        {
            Response.Status = "404 Not Found...";
            Response.StatusCode = 404;
            return View();
        }

        public ActionResult InternalServerError()
        {
            Response.Status = "500 Internal Server Error";
            Response.StatusCode = 500;
            ViewBag.Description = Response.StatusDescription;
            return View();
        }
    }
}