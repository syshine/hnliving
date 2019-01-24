using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace hnliving.web.Areas.H5.Controllers.Wx
{
    public class GzhController : BaseWebController
    {
        // GET: H5/Gzh
        public ActionResult Index()
        {
            // 微信信息
            //ViewBag.timestamp = TimeHelper.GetNowStamp();
            //ViewBag.nonceStr = Randoms.GetRandomValue(16, false);
            //ViewBag.AccessToken = Monitor.AccessToken;
            //ViewBag.apiTicket = Monitor.JsapiTicket;
            //ViewBag.index = Monitor.index;
            //ViewBag.indexA = Monitor.indexA;
            //ViewBag.msg = Monitor.msg;
            //ViewBag.ret = Monitor.ret;
            //ViewBag.retA = Monitor.retA;
            //string srcStr = string.Format("jsapi_ticket={0}&noncestr={1}&timestamp={2}&url={3}", Monitor.JsapiTicket, ViewBag.nonceStr, ViewBag.timestamp, WorkContext.Url);
            //string signature = "";
            //string resultMsg = EncryptHelper.SHA1Encrypt(out signature, srcStr);
            //ViewBag.srcStr = srcStr;
            //if (resultMsg == "")
            //{
            //    ViewBag.signature = signature;
            //}

            return View();
        }
    }
}