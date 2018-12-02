using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Optimization;

namespace hnliving.web
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            //RegisterView(); //注册视图访问规则
        }

        //protected void RegisterView()
        //{
        //    ViewEngines.Engines.Clear();
        //    ViewEngines.Engines.Add(new Areas.AreasViewEngine());
        //}
    }
}
