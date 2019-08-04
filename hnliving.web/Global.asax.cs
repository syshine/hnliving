using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Optimization;
using Lib.Core;

namespace hnliving.web
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            HnlViewEngine.RegisterView();//注册多级目录扩展

            //Monitor.Init(); //初始化监控

            log4net.Config.XmlConfigurator.Configure();

            //打开缓存服务连接
            if (MngConfig.SiteConfig.EnableMemcache)
            {
                MemCachedHelper.Open(MngConfig.MemcachedCacheConfig, "hnl");
            }
        }

        protected void Application_End(object sender, EventArgs e)
        {
            //关闭缓存服务连接
            if (MngConfig.SiteConfig.EnableMemcache)
            {
                MemCachedHelper.Stop();
            }
        }
    }
}
