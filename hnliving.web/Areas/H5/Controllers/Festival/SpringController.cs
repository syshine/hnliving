using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Lib.Core;
using Lib.Services;

namespace hnliving.web.Areas.H5.Controllers.Festival
{
    public class SpringController : BaseWebController
    {
        // GET: H5/Festival/Spring
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Y2019(string nf, string nt, string bgm)
        {
            string key = "14789632";

            // 发起人名字
            ViewBag.NameFrom = "";
            if (!string.IsNullOrWhiteSpace(nf))
            {
                string destString = "";
                string retMsg = EncryptHelper.DesDecrypt(out destString, nf, key, key);

                if (retMsg == "")
                {
                    ViewBag.NameFrom = destString;
                }
            }

            // 祝福人名字
            ViewBag.NameTo = "";
            if (!string.IsNullOrWhiteSpace(nt))
            {
                string destString = "";
                string retMsg = EncryptHelper.DesDecrypt(out destString, nt, key, key);

                if (retMsg == "")
                {
                    ViewBag.NameTo = destString;
                }
            }

            // 背景音乐
            ViewBag.bgm = "";
            if (!string.IsNullOrWhiteSpace(bgm))
            {
                string destString = "";
                string retMsg = EncryptHelper.DesDecrypt(out destString, bgm, key, key);

                if (retMsg == "")
                {
                    ViewBag.bgm = destString;
                }
            }

            return View();
        }
    }
}