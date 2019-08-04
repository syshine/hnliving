using Lib.Core;
using Lib.Services;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace hnliving.web.Controllers
{
    public class HomeController : BaseWebController
    {
        // GET: Home
        public ActionResult Index()
        {
            List<UEditorEntity> lstUe = null;
            Hashtable htSort = null;

            try
            {
                // 获取内容的ID
                int uid = -1;// UserRanks.IsContentEditor(WorkContext.Uid) ? -1 : WorkContext.Uid;

                // 内容
                lstUe = UEditorSer.GetList(uid);
                ViewData["lstUe"] = lstUe;

                // 分类ID名称
                DataTable dtSort = UEditorSer.GetSort(uid);
                htSort = new Hashtable();
                foreach (DataRow dr in dtSort.Rows)
                {
                    htSort.Add(dr["sid"].ToString(), dr["sname"].ToString());
                }
                ViewData["htSort"] = htSort;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Home/Index" + ex.Message);
            }

            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}