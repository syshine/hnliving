using Lib.Core;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace hnliving.web.Areas.Invest.Controllers.Stock
{
    public class HomeController : BaseWebController
    {
        // GET: Invest/Stock/Home
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// K线
        /// </summary>
        /// <returns></returns>
        public ActionResult KLine(string code)
        {
            ViewBag.Data = "{}";
            if(!string.IsNullOrWhiteSpace(code))
            {
                ResultEntity entity = Lib.Services.Stock.GetDetail(code);
                if(entity.IsSuccess)
                {
                    ViewBag.Data = JsonConvert.SerializeObject(entity.Data);
                }
            }
            return View();
        }

        public ActionResult Detail()
        {
            return View();
        }

        public ActionResult GetDetail(string code)
        {
            ResultEntity result;
            if (string.IsNullOrWhiteSpace(code))
            {
                result = ResultEntity.ParamsError("股票代码不能为空");
            }
            else
            {
                result = Lib.Services.Stock.GetDetail(code);
            }

            string strResult = JsonConvert.SerializeObject(result);
            return Content(strResult);
        }

        public ActionResult GetAverageLine(string code)
        {
            ResultEntity result;
            if (string.IsNullOrWhiteSpace(code))
            {
                result = ResultEntity.ParamsError("股票代码不能为空");
            }
            else
            {
                result = Lib.Services.Stock.GetAverageLine(code);
            }

            string strResult = JsonConvert.SerializeObject(result);
            return Content(strResult);
        }

        public ActionResult GetFormerComplexRights(string code)
        {
            ResultEntity result;
            if (string.IsNullOrWhiteSpace(code))
            {
                result = ResultEntity.ParamsError("股票代码不能为空");
            }
            else
            {
                result = Lib.Services.Stock.GetFormerComplexRights(code);
            }

            string strResult = JsonConvert.SerializeObject(result);
            return Content(strResult);
            //return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}