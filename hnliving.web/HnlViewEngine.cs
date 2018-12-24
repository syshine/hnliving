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
                "~/Views/Areas/{1}/{0}.cshtml",
            };

            AreaViewLocationFormats = new[]
            {

                "~/Areas/{2}/Views/{1}/{0}.cshtml",
                "~/Areas/{2}/Views/Shared/{0}.cshtml",

                "~/Areas/{2}/Views/CivilServant/{1}/{0}.cshtml",
                "~/Areas/{2}/Views/Education/{1}/{0}.cshtml",
                "~/Areas/{2}/Views/Festival/{1}/{0}.cshtml",
                "~/Areas/{2}/Views/Program/{1}/{0}.cshtml",

                "~/Areas/{2}/Views/CivilServant/Shared/{0}.cshtml",
                "~/Areas/{2}/Views/Education/Shared/{0}.cshtml",
                "~/Areas/{2}/Views/Festival/Shared/{0}.cshtml",
                "~/Areas/{2}/Views/Program/Shared/{0}.cshtml",
                
            };
        }
        public override ViewEngineResult FindView(ControllerContext controllerContext, string viewName, string masterName, bool useCache)
        {
            return base.FindView(controllerContext, viewName, masterName, useCache);
        }

        public static void RegisterView()
        {
            ViewEngines.Engines.Clear();
            ViewEngines.Engines.Add(new HnlViewEngine());
        }
    }
}