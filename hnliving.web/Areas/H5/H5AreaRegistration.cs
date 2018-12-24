using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace hnliving.web.Areas.H5
{
    public class H5AreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "H5";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {

            context.MapRoute(
                name: "H5_Festival",
                url: "H5/Festival/{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional },
                namespaces: new[] { "hnliving.web.Areas.H5.Controllers.Festival" }
            );
            context.MapRoute(
                name: "H5_Business",
                url: "H5/Business/{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional },
                namespaces: new[] { "hnliving.web.Areas.H5.Controllers.Business" }
            );

            context.MapRoute(
                name: "H5_Demo",
                url: "H5/Demo/{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional },
                namespaces: new[] { "hnliving.web.Areas.H5.Controllers.Demo" }
            );

            //此路由不能删除
            context.MapRoute(name: "H5_default",
                             url: "H5/{controller}/{action}/{id}",
                             defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional },
                             namespaces: new[] { "hnliving.web.Areas.H5.Controllers" });

        }
    }
}