using hnliving.web.Areas.Tools.Models;
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
    public class IntelligentController : BaseWebController
    {
        // GET: Invest/Stock/Intelligent
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Picker(int fid)
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

                return View(spe);
            }
            else
                return View(new StockPickEntity());
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
                return PromptView(Url.Action("List"), "删除成功！", true);
            else
                return PromptView(Url.Action("List"), "删除失败！", true);
        }

        public ActionResult GetProcess(string guid)
        {
            return GetAjaxProcess(guid);
        }
    }
}