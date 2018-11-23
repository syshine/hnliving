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
            //此路由不能删除
            context.MapRoute("Study_default",
                              "Study/{controller}/{action}/{id}",
                              new { action = "Index", id = UrlParameter.Optional });

        }
    }
}