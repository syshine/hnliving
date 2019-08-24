using hnliving.web.Areas.Tools.Models;
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
            List<UEditorInfo> lstUei = new List<UEditorInfo>();

            try
            {
                // 获取内容的ID
                int uid = -1;// UserRanks.IsContentEditor(WorkContext.Uid) ? -1 : WorkContext.Uid;

                // 内容
                lstUe = UEditorSer.GetList(new PageEntity(-1, 7), uid);

                // 分类ID名称
                DataTable dtSort = UEditorSer.GetSort(uid);
                
                foreach (UEditorEntity ue in lstUe)
                {
                    DataRow[] drs = dtSort.Select("sid=" + ue.Typeid);
                    string sname = drs.Length > 0 ? drs[0]["sname"].ToString() : "";
                    UEditorInfo uei = new UEditorInfo()
                    {
                        Id = ue.Id,
                        Typename = sname,
                        Title = ue.Title,
                        Create_time = ue.Create_time,
                        Update_time = ue.Update_time,
                        Tag = ue.Tag
                    };
                    lstUei.Add(uei);
                }
                ViewData["lstUei"] = lstUei;
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