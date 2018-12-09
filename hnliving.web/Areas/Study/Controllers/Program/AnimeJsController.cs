using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace hnliving.web.Areas.Study.Controllers.Program
{
    public class AnimeJsController : Controller
    {
        // GET: Study/Program/AnimeJs
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 动画目标
        /// </summary>
        /// <returns></returns>
        public ActionResult Targets()
        {
            return View();
        }

        /// <summary>
        /// 属性目标
        /// </summary>
        /// <returns></returns>
        public ActionResult Attribute()
        {
            return View();
        }

        /// <summary>
        /// 动画参数
        /// </summary>
        /// <returns></returns>
        public ActionResult Parameter()
        {
            return View();
        }

        /// <summary>
        /// 函数返回动画参数
        /// </summary>
        /// <returns></returns>
        public ActionResult FuncRetPar()
        {
            return View();
        }

        /// <summary>
        /// 方向和循环
        /// </summary>
        /// <returns></returns>
        public ActionResult DirAndLoop()
        {
            return View();
        }

        /// <summary>
        /// 动画赋值
        /// </summary>
        /// <returns></returns>
        public ActionResult Assign()
        {
            return View();
        }

        /// <summary>
        /// 时间轴
        /// </summary>
        /// <returns></returns>
        public ActionResult TimeLine()
        {
            return View();
        }
    }
}