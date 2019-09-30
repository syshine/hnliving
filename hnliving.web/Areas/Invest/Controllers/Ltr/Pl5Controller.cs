using hnliving.web.Areas.Invest.Models;
using Lib.Core;
using Lib.Services;
using LibLtr.Entity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace hnliving.web.Areas.Invest.Controllers.Ltr
{
    public class Pl5Controller : BaseWebController
    {
        // GET: Invest/Ltr/Pl5
        public ActionResult Index(int count = 50)
        {
            Lib.Services.Ltr ltr = new Lib.Services.Ltr();
            List<ShowLtrPl5Model> lstEntity = ltr.GetPl5Data(count);

            return View(lstEntity);
        }

        public ActionResult HighFrequency(int count = 100)
        {
            Lib.Services.Ltr ltr = new Lib.Services.Ltr();
            DataTable lstEntity = ltr.GetPl5HighFrequency(count);
            var json_data = CommonHelper.JsonSerializeObject(lstEntity);

            return Content(json_data);
        }

        public ActionResult GetData(int issue = -1, int count = 50)
        {
            Lib.Services.Ltr ltr = new Lib.Services.Ltr();
            List<ShowLtrPl5Model> lstEntity = ltr.GetPl5Data(count, issue);
            var json_data = CommonHelper.JsonSerializeObject(lstEntity);

            return Content(json_data);
        }

        public ActionResult SumNumb(int issue = -1)
        {
            Lib.Services.Ltr ltr = new Lib.Services.Ltr();
            EntitySumResult esr = ltr.GetPl5Sum(issue);

            // 转成Json格式给页面显示
            var json_esr = CommonHelper.JsonSerializeObject(esr);
            ViewData["json_data"] = json_esr;

            return View(esr);
        }

        [HttpGet]
        public ActionResult Add()
        {
            Pl5Entity entity = Lib.Services.Ltr.GetCurrentPl5();
            AddLtrPl5Model model = new AddLtrPl5Model()
            {
                Issue = entity.Issue,
                Dt = entity.Date,
                Numb = entity.Numb
            };

            // 禁用日期控件的周几
            DayOfWeek[] arrDow = Lib.Services.Ltr.GetPl5DayOfWeek();
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
        public ActionResult Add(AddLtrPl5Model model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            
            if(string.IsNullOrWhiteSpace(model.Numb) || model.Numb.Length != 5)
                return PromptView("", "号码错误！", true);

            char[] arr = model.Numb.ToCharArray();
            foreach (char c in arr)
            {
                if (c < 48 || c > 57) // 48是0的ascii码值, 57是9的ascii码值
                    return PromptView("", "号码错误！", true);
            }

            if (!UserRanks.IsContentEditor(WorkContext.Uid))
                return PromptView("Home/Index", "不能执行此操作！", true);


            Pl5Entity entity = new Pl5Entity()
            {
                Issue = model.Issue,
                Date = model.Dt.Date,
                Numb = model.Numb
            };
            
            switch(Lib.Services.Ltr.AddPl5Numb(entity))
            {
                case 1:
                    return PromptView(Url.Action("Index"), "新增成功！", true);

                case 2:
                    return PromptView("", "新增失败！号码错误！", true);

                case 3:
                    return PromptView("", "新增失败！期号已经存在！", true);

                default:
                    return PromptView("", "新增失败！", true);
            }
            
            //return View();
        }
    }
}