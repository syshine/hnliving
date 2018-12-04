using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace hnliving.web.Areas.Travel
{
    public class TravelSanYaAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Travel_SanYa";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            //此路由不能删除
            context.MapRoute("Travel_SanYa_default",
                              "Travel/SanYa/{controller}/{action}/{id}",
                              new { action = "Index", id = UrlParameter.Optional });

        }
    }
}