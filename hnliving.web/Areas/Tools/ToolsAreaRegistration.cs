using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace hnliving.web.Areas.Tools
{
    public class ToolsAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Tools";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            
            context.MapRoute(
                name: "Tools_Program",
                url: "Tools/Program/{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional },
                namespaces: new[] { "hnliving.web.Areas.Tools.Controllers.Program" }
            );

            //此路由不能删除
            context.MapRoute(name: "Tools_default",
                             url: "Tools/{controller}/{action}/{id}",
                             defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional },
                             namespaces: new[] { "hnliving.web.Areas.Tools.Controllers" });

        }
    }
}