using hnliving.web.Areas.Tools.Models;
using hnliving.web.Models;
using Lib.Core;
using Lib.Services;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace hnliving.web.Areas.Tools.Controllers.Program
{
    public class UEditorController : BaseWebController
    {
        // GET: Tools/Program/UEditor
        public ActionResult Index()
        {
            return View();
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

            #region //排序
            //// 排序
            //lstUe.Sort((left, right) =>
            //{
            //    if (left.Typeid > right.Typeid)
            //        return 1;
            //    else if (left.Typeid == right.Typeid)
            //    {
            //        if (left.Update_time > right.Update_time)
            //            return -1;
            //        else if (left.Update_time == right.Update_time)
            //            return 0;
            //        else
            //            return 1;
            //    }
            //    else
            //        return -1;
            //});
            #endregion

            // 分类ID名称
            DataTable dtSort = UEditorSer.GetSort(uid);
            List<KeyValuePair<string, string>> lstType = new List<KeyValuePair<string, string>>();
            foreach (DataRow dr in dtSort.Rows)
            {
                lstType.Add(new KeyValuePair<string, string>(dr["sid"].ToString(), dr["sname"].ToString()));
                if(typeid >= 0 && dr["sid"].ToString() == typeid.ToString())
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
            foreach(UEditorEntity ue in lstUe)
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

        /// <summary>
        /// 添加
        /// </summary>
        [HttpGet]
        public ActionResult Add()
        {
            // 获取内容的ID
            int uid = UserRanks.IsContentEditor(WorkContext.Uid) ? -1 : WorkContext.Uid;

            AddUEditorModel model = new AddUEditorModel();

            // 分类
            DataTable dtSort = UEditorSer.GetSort(uid);
            List<SelectListItem> lstType = new List<SelectListItem>();
            foreach (DataRow dr in dtSort.Rows)
            {
                lstType.Add(new SelectListItem() { Text = dr["sname"].ToString(), Value = dr["sid"].ToString() });
            }
            SelectList slType = new SelectList(lstType, "Value", "Text");
            ViewData["slType"] = slType;

            return View(model);
        }

        /// <summary>
        /// 添加
        /// </summary>
        [HttpPost]
        public ActionResult Add(AddUEditorModel model)
        {
            // 获取内容的ID
            int uid = UserRanks.IsContentEditor(WorkContext.Uid) ? -1 : WorkContext.Uid;

            UEditorEntity uee = new UEditorEntity()
            {
                Uid = uid,
                Title = model.Title,
                Typeid = model.TypeId,
                Ue_content = model.Content,
            };

            if (UEditorSer.Add(uee) > 0)
                return PromptView(Url.Action("List"), "添加成功！", true);
            else
                return PromptView(Url.Action("List"), "添加失败！", true);

            //return View(model);
        }

        /// <summary>
        /// 编辑
        /// </summary>
        [HttpGet]
        public ActionResult Edit(int ueid = -1)
        {
            // 获取内容的ID
            int uid = UserRanks.IsContentEditor(WorkContext.Uid) ? -1 : WorkContext.Uid;

            UEditorEntity uee = UEditorSer.GetById(ueid);

            if (uee.Uid != uid)
            {
                return PromptView("不能操作此内容");
            }

            EditUEditorModel model = new EditUEditorModel()
            {
                UeId = ueid,
                Title = uee.Title,
                TypeId = uee.Typeid,
                Content = uee.Ue_content,
            };

            // 分类
            DataTable dtSort = UEditorSer.GetSort(uid);
            List<SelectListItem> lstType = new List<SelectListItem>();
            foreach(DataRow dr in dtSort.Rows)
            {
                lstType.Add(new SelectListItem() { Text = dr["sname"].ToString(), Value = dr["sid"].ToString() });
            }
            SelectList slType = new SelectList(lstType, "Value", "Text");
            ViewData["slType"] = slType;

            return View(model);
        }

        /// <summary>
        /// 编辑
        /// </summary>
        [HttpPost]
        public ActionResult Edit(EditUEditorModel model, int ueid = -1)
        {
            // 获取内容的ID
            int uid = UserRanks.IsContentEditor(WorkContext.Uid) ? -1 : WorkContext.Uid;

            UEditorEntity uee = new UEditorEntity()
            {
                Id = ueid,
                Uid = uid,
                Title = model.Title,
                Typeid = model.TypeId,
                Ue_content = model.Content,
            };

            //UEditorSer.Update(uee);

            //return View(uee);
            if (UEditorSer.Update(uee) > 0)
                return PromptView(Url.Action("Edit", new { ueid = ueid }), "保存成功！", true);
            else
                return PromptView(Url.Action("Edit", new { ueid = ueid }), "保存失败！", true);
        }

        /// <summary>
        /// 删除
        /// </summary>
        public ActionResult Delete(int ueid)
        {
            // 获取内容的ID
            int uid = UserRanks.IsContentEditor(WorkContext.Uid) ? -1 : WorkContext.Uid;

            if (UEditorSer.DeleteById(uid, ueid) > 0)
                return PromptView(Url.Action("List"), "删除成功！", true);
            else
                return PromptView(Url.Action("List"), "删除失败！", true);
        }

        #region 分类
        /// <summary>
        /// 分类列表
        /// </summary>
        /// <returns></returns>
        public ActionResult Sort()
        {
            // 获取内容的ID
            int uid = UserRanks.IsContentEditor(WorkContext.Uid) ? -1 : WorkContext.Uid;

            DataTable dt = UEditorSer.GetSort(uid);

            return View(dt);
        }

        /// <summary>
        /// 新增分类
        /// </summary>
        [HttpPost]
        public ActionResult AddSort(string title)
        {
            // 获取内容的ID
            int uid = UserRanks.IsContentEditor(WorkContext.Uid) ? -1 : WorkContext.Uid;

            if (UEditorSer.AddSort(uid, title) > 0)
                return PromptView(Url.Action("Sort"), "新增分类成功！", true);
            else
                return PromptView(Url.Action("Sort"), "新增分类失败！", true);
        }

        /// <summary>
        /// 删除分类
        /// </summary>
        public ActionResult DeleteSort(int sid)
        {
            // 获取内容的ID
            int uid = UserRanks.IsContentEditor(WorkContext.Uid) ? -1 : WorkContext.Uid;

            if (UEditorSer.DeleteSortById(uid, sid) > 0)
                return PromptView(Url.Action("Sort"), "删除成功！", true);
            else
                return PromptView(Url.Action("Sort"), "删除失败！", true);
        }
        #endregion

        #region //
        //public ActionResult Detail()
        //{
        //    string ueid = WebHelper.GetQueryString("ueid");
        //    if(!string.IsNullOrWhiteSpace(ueid))
        //    {
        //        ViewBag.ueid = int.Parse(ueid);
        //        ViewBag.uecontent = UEditorSer.GetContentById(ViewBag.ueid);
        //    }
        //    return View();
        //}

        //public ActionResult Save(string content, int id = -1, int type = 0)
        //{
        //    if (id <= 0) // 新建
        //    {
        //        if (UEditorSer.Add(content, type) > 0)
        //            return AjaxResult("success", "保存成功！");
        //        else
        //            return AjaxResult("error", "保存错误！");
        //    }
        //    else // 修改
        //    {
        //        if (UEditorSer.Update(id, content) > 0)
        //            return AjaxResult("success", "修改成功！");
        //        else
        //            return AjaxResult("error", "修改错误！");
        //    }
        //}

        //public ActionResult uploadimage()
        //{
        //    string operation = WebHelper.GetQueryString("action");
        //    string result = "";
        //    switch (operation)
        //    {
        //        case "uploadimage":
        //            HttpPostedFileBase file = Request.Files[0];
        //            System.Diagnostics.Debug.WriteLine(file.FileName);
        //            result = "上传成功";// Uploads.SaveUploadStoreRankAvatar(file);
        //            break;

        //        default:
        //            return HttpNotFound();
        //            //break;
        //    }

        //    return Content(result);
        //}

        //public ActionResult Upload()
        //{
        //    string operation = WebHelper.GetQueryString("action");
        //    string result = "";
        //    switch (operation)
        //    {
        //        case "uploadimage":
        //            HttpPostedFileBase file = Request.Files[0];
        //            System.Diagnostics.Debug.WriteLine(file.FileName);
        //            result = "上传成功";// Uploads.SaveUploadStoreRankAvatar(file);
        //            break;

        //        default:
        //            return HttpNotFound();
        //            //break;
        //    }

        //    return Content(result);
        //}
        #endregion
    }
}