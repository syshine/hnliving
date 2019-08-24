using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Lib.Core;
using Lib.Services;
using hnliving.web.Models;
using System.Data;
using hnliving.web.Areas.Tools.Models;

namespace hnliving.web.Controllers
{
    public class ContentController : BaseWebController
    {
        // GET: Content
        public ActionResult Index(int id = 0)
        {
            // 如果没有ID或者小于0，则跳转至列表
            if(id <= 0)
            {
                return RedirectToAction("list");
            }

            // 获取内容的ID
            int uid = UserRanks.IsContentEditor(WorkContext.Uid) ? -1 : WorkContext.Uid;

            UEditorEntity uee = UEditorSer.GetById(id);

            if (uee.Uid != uid)
            {
                return PromptView("没有此内容");
            }

            ShowUEditorModel model = new ShowUEditorModel()
            {
                UeId = id,
                Title = uee.Title,
                TypeId = uee.Typeid,
                TypeName = UEditorSer.GetName(uee.Typeid),
                Content = uee.Ue_content,
            };

            return View(model);
        }

        /// <summary>
        /// 文章列表
        /// </summary>
        /// <param name="typeid"></param>
        /// <param name="pageSize"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        public ActionResult List(int typeid = -1, int pageSize = 10, int page = 1)
        {
            // 获取内容的ID
            int uid = UserRanks.IsContentEditor(WorkContext.Uid) ? -1 : WorkContext.Uid;

            // 分页
            PageEntity pg = new PageEntity(0, pageSize, page);

            // UEditor实体列表
            List<UEditorEntity> lstUe = UEditorSer.GetList(pg, uid, typeid);

            // 分类ID名称
            DataTable dtSort = UEditorSer.GetSort(uid);
            List<KeyValuePair<string, string>> lstType = new List<KeyValuePair<string, string>>();
            foreach (DataRow dr in dtSort.Rows)
            {
                lstType.Add(new KeyValuePair<string, string>(dr["sid"].ToString(), dr["sname"].ToString()));
                if (typeid > 0 && dr["sid"].ToString() == typeid.ToString())
                {
                    ViewBag.TypeId = typeid;
                    ViewBag.TypeName = dr["sname"].ToString();
                }
            }
            if (typeid <= 0)
            {
                ViewBag.TypeId = -1;
                ViewBag.TypeName = "全部";
            }

            // UEditor信息列表
            List<UEditorInfo> lstUei = new List<UEditorInfo>();
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

            // 列表模型
            UEditorListModel model = new UEditorListModel();
            model.UEditorList = lstUei;
            model.TypeList = lstType;
            model.PageModel = new PageModel(pg.Pagesize, pg.Pageindex, pg.Totalcount);

            return View(model);
        }
    }
}