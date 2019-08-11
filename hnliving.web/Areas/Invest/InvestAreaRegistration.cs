using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace hnliving.web.Areas.Invest
{
    public class InvestAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Invest";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {

            context.MapRoute(
                name: "Invest_Ltr",
                url: "Invest/Ltr/{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional },
                namespaces: new[] { "hnliving.web.Areas.Invest.Controllers.Ltr" }
            );
            context.MapRoute(
                name: "Invest_Stock",
                url: "Invest/Stock/{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional },
                namespaces: new[] { "hnliving.web.Areas.Invest.Controllers.Stock" }
            );

            //此路由不能删除
            context.MapRoute(name: "Invest_default",
                             url: "Invest/{controller}/{action}/{id}",
                             defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional },
                             namespaces: new[] { "hnliving.web.Areas.Invest.Controllers" });

        }
    }
}