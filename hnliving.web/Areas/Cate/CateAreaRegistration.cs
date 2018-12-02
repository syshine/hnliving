using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace hnliving.web.Areas.Cate
{
    public class CateAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Cate";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute("Cate_default",
                              "Cate/{controller}/{action}/{id}",
                              new { controller = "home", action = "Index", id = UrlParameter.Optional });
            //此路由不能删除
            //context.MapRoute("Cate_default",
            //                  "Cate/{controller}/{action}/{id}",
            //                  new { action = "Index", id = UrlParameter.Optional });

        }
    }
}