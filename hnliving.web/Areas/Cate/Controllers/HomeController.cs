using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace hnliving.web.Areas.Cate.Controllers
{
    public class HomeController : Controller
    {
        // GET: Cate/Home
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Cate description page.";

            return View();
        }
    }
}