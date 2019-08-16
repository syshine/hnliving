using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace hnliving.web.Areas.Invest.Controllers.Stock
{
    public class DownloadController : BaseWebController
    {
        // GET: Invest/Stock/Download
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GetData(string code, string start, string end)
        {
            string result =  Lib.Services.Stock.GetHistoryFromWeb(code, DateTime.Parse(start), DateTime.Parse(end));
            return Content(result);
        }

        public ActionResult GetStockBaseInfo(int page = 1, int count = int.MaxValue)
        {
            string result = Lib.Services.Stock.GetStockBaseInfo(page, count);
            //JObject json = JObject.Parse(result);
            //string[] values = json.Properties().Select(item => item.Value.ToString()).ToArray();

            return Content(result);
        }

        public ActionResult SaveStockBaseInfo(int page = 1, int count = int.MaxValue)
        {
            int dd = Lib.Services.Stock.SaveStockBaseInfo();
            
            return Content(dd.ToString());
        }

        public ActionResult GetStockHis(string code, string start, string end)
        {
            string s_type = code.Substring(0, 1) == "6" ? "0" : "1";
            string result = Lib.Services.Stock.GetStockHis(s_type, code, DateTime.Parse(start), DateTime.Parse(end));
            return Content(result);
        }

        public ActionResult SaveStockHis(string codes = "")
        {
            List<string> lstCode = codes != "" ? codes.Split(',').ToList() : null;
            string result = Lib.Services.Stock.SaveStockHis(lstCode);
            return Content(result);
        }
    }
}