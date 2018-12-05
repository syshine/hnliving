using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace hnliving.web.Areas.Study
{
    public class StudyAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Study";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {

            context.MapRoute(
                name: "Study_Civil_Servant",
                url: "Study/CivilServant/{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional },
                namespaces: new[] { "hnliving.web.Areas.Study.Controllers.CivilServant" }
            );
            context.MapRoute(
                name: "Study_Program",
                url: "Study/Program/{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional },
                namespaces: new[] { "hnliving.web.Areas.Study.Controllers.Program" }
            );

            context.MapRoute(
                name: "Study_Education",
                url: "Study/Education/{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional },
                namespaces: new[] { "hnliving.web.Areas.Study.Controllers.Education" }
            );

            //此路由不能删除
            context.MapRoute(name: "Study_default",
                             url: "Study/{controller}/{action}/{id}",
                             defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional },
                             namespaces: new[] { "hnliving.web.Areas.Study.Controllers" });

        }
    }
}