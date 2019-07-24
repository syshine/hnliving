using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace hnliving.web.Areas.Tools.Controllers.Program
{
    public class HttpController : BaseWebController
    {
        // GET: Tools/Http
        public ActionResult Index()
        {
            return View();
        }


        public ActionResult Ajax()
        {
            //List<SelectListItem> dataType = new List<SelectListItem> {
            //    new SelectListItem { Text = "xml", Value = "xml",Selected = true},
            //    new SelectListItem { Text = "html", Value = "html" },
            //    new SelectListItem { Text = "script", Value = "script" },
            //    new SelectListItem { Text = "json", Value = "json" },
            //    new SelectListItem { Text = "jsonp", Value = "jsonp" },
            //    new SelectListItem { Text = "text", Value = "text" }
            //};

            //ViewData["dataType"] = dataType;

            return View();
        }
    }
}