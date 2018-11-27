using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace hnliving.web.Areas.Travel
{
    public class TravelAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Travel";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            //此路由不能删除
            context.MapRoute("Travel_default",
                              "Travel/{controller}/{action}/{id}",
                              new { action = "Index", id = UrlParameter.Optional });

        }
    }
}