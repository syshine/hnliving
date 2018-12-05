using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace hnliving.web.Areas.Study.Controllers.Program
{
    public class HomeController : Controller
    {
        const string path = "~/Areas/Study/Views/Program/Home/{0}.cshtml";

        // GET: Study/Program/Home
        public ActionResult Index()
        {
            //System.Diagnostics.StackTrace st = new System.Diagnostics.StackTrace();
            //string viewName = string.Format(path, st.GetFrame(0).GetMethod().Name);

            return View("~/Areas/Study/Views/Program/Home/Index.cshtml");
        }

        // GET: Study/Program/Home
        public ActionResult About()
        {
            return View();
        }
    }
}