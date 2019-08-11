using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace hnliving.web.Areas.Invest.Controllers
{
    public class HomeController : BaseWebController
    {
        // GET: Invest/Home
        public ActionResult Index()
        {
            return View();
        }
    }
}