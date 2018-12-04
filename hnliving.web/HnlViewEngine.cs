using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace hnliving.web
{
    public sealed class HnlViewEngine : RazorViewEngine
    {
        public HnlViewEngine()
        {
            ViewLocationFormats = new[]
            {
                "~/Views/{1}/{0}.cshtml",
                "~/Views/Shared/{0}.cshtml",
                "~/Areas/Cate/Views/{1}/{0}.cshtml",
                "~/Areas/Study/Views/{1}/{0}.cshtml",
                "~/Areas/Travel/Views/{1}/{0}.cshtml",
                "~/Areas/Travel/HaiKou/Views/{1}/{0}.cshtml",
            };
        }
        public override ViewEngineResult FindView(ControllerContext controllerContext, string viewName, string masterName, bool useCache)
        {
            return base.FindView(controllerContext, viewName, masterName, useCache);
        }
    }
}