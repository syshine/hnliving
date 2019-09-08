using Lib.Core;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace hnliving.web.Areas.Invest.Controllers.Stock
{
    public class IntelligentController : BaseWebController
    {
        // GET: Invest/Stock/Intelligent
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Picker()
        {
            return View();
        }

        /// <summary>
        /// 选股
        /// </summary>
        /// <param name="param"></param>
        /// <param name="guid"></param>
        /// <returns></returns>
        public ActionResult Pick(StockPickEntity param, string guid = "")
        {
            ResultEntity result;

            StockPickEntity pickEntity = param;
            #region //
            //StockPickEntity pickEntity = new StockPickEntity()
            //{
            //    PCHGEnable = pchgEnable,
            //    Days = days,
            //    IsRise = isRise,
            //    Operator = Operator,
            //    PCHG = pchg,
            //    PriceEnable = priceEnable,
            //    PriceLow = priceLow,
            //    PriceHigh = priceHigh,
            //    MCAPEnable = mcapEnable,
            //    MCAPLow = mcapLow * 100000000,
            //    MCAPHigh = mcapHigh * 100000000
            //};
            #endregion

            // 挑选
            result = Lib.Services.Stock.Pick(param, guid);

            // 转成json格式返回
            string strResult = JsonConvert.SerializeObject(result);
            return Content(strResult);
        }

        public ActionResult GetProcess(string guid)
        {
            return GetAjaxProcess(guid);
        }
    }
}