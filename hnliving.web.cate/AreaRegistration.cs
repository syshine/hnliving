using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace hnliving.web.cate
{
    public class AreaRegistration : System.Web.Mvc.AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "cate";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            //此路由不能删除
            context.MapRoute("Cate_default",
                              "Cate/{controller}/{action}/{id}",
                              new { controller = "home", action = "index", id = UrlParameter.Optional },
                              new[] { "hnliving.web.cate.Controllers" });
        }
    }
}