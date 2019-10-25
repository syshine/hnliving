using hnliving.web.Areas.Tools.Models;
using hnliving.web.Models;
using Lib.Core;
using Lib.Services;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace hnliving.web.Areas.Invest.Controllers.Stock
{
    public class FormulaController : BaseWebController
    {
        // GET: Invest/Formula
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 添加公式
        /// </summary>
        [HttpGet]
        public ActionResult Add()
        {
            // 获取内容的ID
            int uid = UserRanks.IsContentEditor(WorkContext.Uid) ? -1 : WorkContext.Uid;

            //StockPickEntity model = new StockPickEntity();
            AddStockFormulaModel model = new AddStockFormulaModel()
            {
                GroupId = 1,
                UseFormerComplexRights = true,
                PCHGEnable = false,
                Days = 2,
                IsRise = "0",
                Operator = "<=",
                PCHG = 3,
                PriceEnable = false,
                PriceLow = 5,
                PriceHigh = 100,
                MCAPEnable = false,
                MCAPLow = 0,
                MCAPHigh = 100,
                FormulaEnable = true,
                PreAvgLines = "5,10,20,30",
                PreDays = 20,
                Formula = ""
            };

            // 分类
            DataTable dtGroup = Lib.Services.Stock.GetFormulaGroup(uid);
            List<SelectListItem> lstGroup = new List<SelectListItem>();
            foreach (DataRow dr in dtGroup.Rows)
            {
                lstGroup.Add(new SelectListItem() { Text = dr["gname"].ToString(), Value = dr["gid"].ToString() });
            }
            SelectList slGroup = new SelectList(lstGroup, "Value", "Text");
            ViewData["slGroup"] = slGroup;

            List<SelectListItem> lstRise = new List<SelectListItem>();
            lstRise.Add(new SelectListItem() { Text = "上涨", Value = "1" });
            lstRise.Add(new SelectListItem() { Text = "下跌", Value = "0" });
            SelectList slRise = new SelectList(lstRise, "Value", "Text");
            ViewData["slRise"] = slRise;

            List<SelectListItem> lstOperator = new List<SelectListItem>();
            lstOperator.Add(new SelectListItem() { Text = "不超过", Value = "<=" });
            lstOperator.Add(new SelectListItem() { Text = "超过", Value = ">" });
            SelectList slOperator = new SelectList(lstOperator, "Value", "Text");
            ViewData["slOperator"] = slOperator;

            return View(model);
        }

        /// <summary>
        /// 添加公式
        /// </summary>
        [HttpPost]
        public ActionResult Add(AddStockFormulaModel model)
        {
            // 获取公式的ID
            int uid = UserRanks.IsContentEditor(WorkContext.Uid) ? -1 : WorkContext.Uid;

            StockPickEntity spe = new StockPickEntity()
            {
                Uid = uid,
                Groupid = model.GroupId,
                Fname = model.Name,
                UseFormerComplexRights = model.UseFormerComplexRights,
                PCHGEnable = model.PCHGEnable,
                Days = model.Days,
                IsRise = model.IsRise == "1" ? true : false,
                Operator = model.Operator,
                PCHG = model.PCHG,
                PriceEnable = model.PriceEnable,
                PriceLow = model.PriceLow,
                PriceHigh = model.PriceHigh,
                MCAPEnable = model.MCAPEnable,
                MCAPLow = model.MCAPLow,
                MCAPHigh = model.MCAPHigh,
                FormulaEnable = model.FormulaEnable,
                PreAvgLines = model.PreAvgLines,
                PreDays = model.PreDays,
                Formula = (model.Formula != null) ? model.Formula : ""
            };

            if (Lib.Services.Stock.AddFormula(spe) > 0)
                return PromptView("List", "添加成功！", true);
            else
                return PromptView("List", "添加失败！", true);

            //return View(model);
        }

        /// <summary>
        /// 编辑
        /// </summary>
        [HttpGet]
        public ActionResult Edit(int fid)
        {
            if (fid > 0)
            {
                // 获取内容的ID
                int uid = UserRanks.IsContentEditor(WorkContext.Uid) ? -1 : WorkContext.Uid;

                StockPickEntity spe = Lib.Services.Stock.GetFormulaById(fid);

                if (spe.Uid != uid)
                {
                    return PromptView("不能查看此内容");
                }

                EditStockFormulaModel model = new EditStockFormulaModel()
                {
                    FId = spe.Fid,
                    GroupId = spe.Groupid,
                    Name = spe.Fname,
                    UseFormerComplexRights = spe.UseFormerComplexRights,
                    PCHGEnable = spe.PCHGEnable,
                    Days = spe.Days,
                    IsRise = spe.IsRise ? "1" : "0",
                    Operator = spe.Operator,
                    PCHG = spe.PCHG,
                    PriceEnable = spe.PriceEnable,
                    PriceLow = spe.PriceLow,
                    PriceHigh = spe.PriceHigh,
                    MCAPEnable = spe.MCAPEnable,
                    MCAPLow = spe.MCAPLow,
                    MCAPHigh = spe.MCAPHigh,
                    FormulaEnable = spe.FormulaEnable,
                    PreAvgLines = spe.PreAvgLines,
                    PreDays = spe.PreDays,
                    Formula = spe.Formula
                };

                // 分类
                DataTable dtGroup = Lib.Services.Stock.GetFormulaGroup(uid);
                List<SelectListItem> lstGroup = new List<SelectListItem>();
                foreach (DataRow dr in dtGroup.Rows)
                {
                    lstGroup.Add(new SelectListItem() { Text = dr["gname"].ToString(), Value = dr["gid"].ToString() });
                }
                SelectList slGroup = new SelectList(lstGroup, "Value", "Text");
                ViewData["slGroup"] = slGroup;

                List<SelectListItem> lstRise = new List<SelectListItem>();
                lstRise.Add(new SelectListItem() { Text = "上涨", Value = "1" });
                lstRise.Add(new SelectListItem() { Text = "下跌", Value = "0" });
                SelectList slRise = new SelectList(lstRise, "Value", "Text");
                ViewData["slRise"] = slRise;

                List<SelectListItem> lstOperator = new List<SelectListItem>();
                lstOperator.Add(new SelectListItem() { Text = "不超过", Value = "<=" });
                lstOperator.Add(new SelectListItem() { Text = "超过", Value = ">" });
                SelectList slOperator = new SelectList(lstOperator, "Value", "Text");
                ViewData["slOperator"] = slOperator;

                return View(model);
            }
            else
                return View(new StockPickEntity());
        }


        /// <summary>
        /// 编辑
        /// </summary>
        [HttpPost]
        public ActionResult Edit(EditStockFormulaModel model)
        {
            // 获取内容的ID
            int uid = UserRanks.IsContentEditor(WorkContext.Uid) ? -1 : WorkContext.Uid;

            StockPickEntity spe = new StockPickEntity()
            {
                Fid = model.FId,
                Uid = uid,
                Groupid = model.GroupId,
                Fname = model.Name,
                UseFormerComplexRights = model.UseFormerComplexRights,
                PCHGEnable = model.PCHGEnable,
                Days = model.Days,
                IsRise = model.IsRise == "1" ? true : false,
                Operator = model.Operator,
                PCHG = model.PCHG,
                PriceEnable = model.PriceEnable,
                PriceLow = model.PriceLow,
                PriceHigh = model.PriceHigh,
                MCAPEnable = model.MCAPEnable,
                MCAPLow = model.MCAPLow,
                MCAPHigh = model.MCAPHigh,
                FormulaEnable = model.FormulaEnable,
                PreAvgLines = model.PreAvgLines,
                PreDays = model.PreDays,
                Formula = (model.Formula != null) ? model.Formula : ""
            };

            //UEditorSer.Update(uee);

            //return View(uee);
            if (Lib.Services.Stock.UpdateFormula(spe) > 0)
                return PromptView("Edit?fid=" + model.FId, "保存成功！", true);
            else
                return PromptView("Edit?fid=" + model.FId, "保存失败！", true);
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

        /// <summary>
        /// 公式列表
        /// </summary>
        /// <param name="groupid"></param>
        /// <param name="pageSize"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        public ActionResult List(int groupid = -1, int pageSize = 10, int page = 1)
        {
            // 获取内容的ID
            int uid = UserRanks.IsContentEditor(WorkContext.Uid) ? -1 : WorkContext.Uid;

            // 分页
            PageEntity pg = new PageEntity(0, pageSize, page);

            // StockPickEntity实体列表
            List<StockPickEntity> lstSpe = Lib.Services.Stock.GetFormulaList(pg, uid, groupid);


            // 分组ID名称
            DataTable dtGroup = Lib.Services.Stock.GetFormulaGroup(uid);
            List<KeyValuePair<string, string>> lstGroup = new List<KeyValuePair<string, string>>();
            foreach (DataRow dr in dtGroup.Rows)
            {
                lstGroup.Add(new KeyValuePair<string, string>(dr["gid"].ToString(), dr["gname"].ToString()));
                if (groupid >= 0 && dr["gid"].ToString() == groupid.ToString())
                {
                    ViewBag.GroupId = groupid;
                    ViewBag.GroupName = dr["gname"].ToString();
                }
            }
            if (groupid <= 0)
            {
                ViewBag.GroupId = -1;
                ViewBag.GroupName = "全部";
            }

            // UEditor信息列表
            List<StockFormulaInfo> lstSfi = new List<StockFormulaInfo>();
            foreach (StockPickEntity spe in lstSpe)
            {
                DataRow[] drs = dtGroup.Select("gid=" + spe.Groupid);
                string gname = drs.Length > 0 ? drs[0]["gname"].ToString() : "";
                StockFormulaInfo sfi = new StockFormulaInfo()
                {
                    Fid = spe.Fid,
                    GroupName = gname,
                    Fname = spe.Fname,
                    Create_time = spe.CreateTime,
                    Update_time = spe.UpdateTime
                };
                lstSfi.Add(sfi);
            }

            // 列表模型
            StockFormulaListModel model = new StockFormulaListModel();
            model.StockFormulaList = lstSfi;
            model.GroupList = lstGroup;
            model.PageModel = new PageModel(pg.Pagesize, pg.Pageindex, pg.Totalcount);

            return View(model);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="fid"></param>
        /// <returns></returns>
        public ActionResult DeleteFormula(int fid)
        {
            // 获取内容的ID
            int uid = UserRanks.IsContentEditor(WorkContext.Uid) ? -1 : WorkContext.Uid;

            if (Lib.Services.Stock.DeleteFormulaById(uid, fid) > 0)
                return PromptView("List", "删除成功！", true);
            else
                return PromptView("List", "删除失败！", true);
        }

        #region 分组
        /// <summary>
        /// 分组列表
        /// </summary>
        /// <returns></returns>
        public ActionResult Group()
        {
            // 获取内容的ID
            int uid = UserRanks.IsContentEditor(WorkContext.Uid) ? -1 : WorkContext.Uid;

            DataTable dt = Lib.Services.Stock.GetFormulaGroup(uid);

            return View(dt);
        }

        /// <summary>
        /// 新增分组
        /// </summary>
        [HttpPost]
        public ActionResult AddGroup(string name)
        {
            // 获取内容的ID
            int uid = UserRanks.IsContentEditor(WorkContext.Uid) ? -1 : WorkContext.Uid;
            
            if (Lib.Services.Stock.AddFormulaGroup(uid, name) > 0)
                return PromptView("Group", "新增分组成功！", true);
            else
                return PromptView("Group", "新增分组失败！", true);
        }

        /// <summary>
        /// 删除分组
        /// </summary>
        public ActionResult DeleteGroup(int gid)
        {
            // 获取内容的ID
            int uid = UserRanks.IsContentEditor(WorkContext.Uid) ? -1 : WorkContext.Uid;

            if (Lib.Services.Stock.DeleteFormulaGroupById(uid, gid) > 0)
                return PromptView("Group", "删除成功！", true);
            else
                return PromptView("Group", "删除失败！", true);
        }
        #endregion
    }
}