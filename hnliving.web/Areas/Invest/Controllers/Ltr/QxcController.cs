using hnliving.web.Areas.Invest.Models;
using Lib.Core;
using Lib.Core.Domain.Ltr;
using Lib.Services;
using LibLtr.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace hnliving.web.Areas.Invest.Controllers.Ltr
{
    public class QxcController : BaseWebController
    {
        // GET: Invest/Ltr/Qxc
        public ActionResult Index(int count = 50)
        {
            Lib.Services.Ltr ltr = new Lib.Services.Ltr();
            List<ShowLtrQxcModel> lstEntity = ltr.GetQxcData(count);

            return View(lstEntity);
        }

        public ActionResult GetData(int issue = -1, int count = 10)
        {
            Lib.Services.Ltr ltr = new Lib.Services.Ltr();
            List<ShowLtrQxcModel> lstEntity = ltr.GetQxcData(count, issue);
            var json_data = CommonHelper.JsonSerializeObject(lstEntity);

            return Content(json_data);
        }

        public ActionResult SumNumb(int issue = -1)
        {
            Lib.Services.Ltr ltr = new Lib.Services.Ltr();
            EntitySumResult esr = ltr.GetQxcSum(issue);

            // 转成Json格式给页面显示
            var json_esr = CommonHelper.JsonSerializeObject(esr);
            ViewData["json_data"] = json_esr;

            return View(esr);
        }

        [HttpGet]
        public ActionResult Add()
        {
            QxcEntity entity = Lib.Services.Ltr.GetCurrentQxc();
            AddLtrQxcModel model = new AddLtrQxcModel()
            {
                Issue = entity.Issue,
                Dt = entity.Date,
                Numb = entity.Numb
            };

            // 禁用日期控件的周几
            DayOfWeek[] arrDow = Lib.Services.Ltr.GetQxcDayOfWeek();
            List<string> daysOfWeekDisablede = new List<string>();
            for(int i = 0; i < 7; i++)
            {
                if (!arrDow.Contains((DayOfWeek)i))
                    daysOfWeekDisablede.Add(i.ToString());
            }
            ViewData["daysOfWeekDisablede"] = string.Join(",", daysOfWeekDisablede);

            return View(model);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Add(AddLtrQxcModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            
            if(string.IsNullOrWhiteSpace(model.Numb) || model.Numb.Length != 7)
                return PromptView("", "号码错误！", true);

            char[] arr = model.Numb.ToCharArray();
            foreach (char c in arr)
            {
                if (c < 48 || c > 57) // 48是0的ascii码值, 57是9的ascii码值
                    return PromptView("", "号码错误！", true);
            }

            if (!UserRanks.IsContentEditor(WorkContext.Uid))
                return PromptView("Home/Index", "不能执行此操作！", true);


            QxcEntity entity = new QxcEntity()
            {
                Issue = model.Issue,
                Date = model.Dt.Date,
                Numb = model.Numb
            };
            
            switch(Lib.Services.Ltr.AddQxcNumb(entity))
            {
                case 1:
                    bool b = Lib.Services.Ltr.UpdateQxcData();
                    return PromptView(Url.Action("Index"), "新增分类成功！"+ b, true);

                case 2:
                    return PromptView("", "新增分类失败！号码错误！", true);

                case 3:
                    return PromptView("", "新增分类失败！期号已经存在！", true);

                default:
                    return PromptView("", "新增分类失败！", true);
            }
            
            //return View();
        }
    }
}