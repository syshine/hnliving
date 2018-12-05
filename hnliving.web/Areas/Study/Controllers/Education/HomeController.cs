using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace hnliving.web.Areas.Study.Controllers.Education
{
    public class HomeController : Controller
    {
        // GET: Study/Education/Home
        public ActionResult Index()
        {
            return View("~/Areas/Study/Views/Education/Home/Index.cshtml");
        }
    }
}