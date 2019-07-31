using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace hnliving.web.Areas.Study.Controllers.Program
{
    public class AnimeJsController : BaseWebController
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

        /// <summary>
        /// 动画播放控制
        /// </summary>
        /// <returns></returns>
        public ActionResult Control()
        {
            return View();
        }

        /// <summary>
        /// 回调函数
        /// </summary>
        /// <returns></returns>
        public ActionResult Callbacks()
        {
            return View();
        }

        /// <summary>
        /// 异步对象
        /// </summary>
        /// <returns></returns>
        public ActionResult Async()
        {
            return View();
        }

        /// <summary>
        /// SVG ( Scalable Vector Graphics ) 可缩放矢量图形
        /// </summary>
        /// <returns></returns>
        public ActionResult SVG()
        {
            return View();
        }

        /// <summary>
        /// 动画缓冲效果
        /// </summary>
        /// <returns></returns>
        public ActionResult Easing()
        {
            return View();
        }
    }
}