using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace hnliving.web.Areas.Travel.HaiKou
{
    public class TravelHaikouAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Travel_HaiKou";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            //此路由不能删除
            context.MapRoute(name: "Travel_HaiKou_default",
                             url: "Travel/HaiKou/{controller}/{action}/{id}",
                             defaults: new { action = "Index", id = UrlParameter.Optional },
                             namespaces: new[] { "hnliving.web.Areas.Travel.HaiKou.Controllers" });



        }
    }
}