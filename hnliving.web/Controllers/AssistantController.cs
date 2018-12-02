using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace hnliving.web.Controllers
{
    public class AssistantController : Controller
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
    }
}