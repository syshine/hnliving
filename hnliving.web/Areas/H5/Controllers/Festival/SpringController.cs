using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Lib.Core.Helper;

namespace hnliving.web.Areas.H5.Controllers.Festival
{
    public class SpringController : BaseWebController
    {
        // GET: H5/Festival/Spring
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Y2019(string name)
        {
            ViewBag.Name = "";
            if (!string.IsNullOrWhiteSpace(name))
            {
                string key = "14789630";

                string destString = "";
                string retMsg = EncryptHelper.DesDecrypt(out destString, name, key, key);

                if (retMsg == "")
                {
                    ViewBag.Name = destString;
                }
            }

            return View();
        }
    }
}