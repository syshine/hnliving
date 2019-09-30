using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Optimization;
using Lib.Core;
using System.Diagnostics;

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

            // 开启sql server服务
            //StartSqlserver();

            // 监控Qxc
            if (MngConfig.SiteConfig.IsMonitorQxc)
            {
                TimingTask.MonitorQxc();
            }

            // 监控Pl5
            if (MngConfig.SiteConfig.IsMonitorPl5)
            {
                TimingTask.MonitorPl5();
            }
        }

        protected void Application_End(object sender, EventArgs e)
        {
            ////关闭缓存服务连接
            //if (MngConfig.SiteConfig.EnableMemcache)
            //{
            //    MemCachedHelper.Stop();
            //}
        }

        private void StartSqlserver()
        {
            Process proc = null;
            try
            {
                string targetDir = string.Format(@"E:\\sql server start.bat");//this is where testChange.bat lies
                proc = new Process();
                proc.StartInfo.WorkingDirectory = targetDir;
                proc.StartInfo.FileName = "Video.bat";
                proc.StartInfo.Arguments = string.Format("20");
                //proc.StartInfo.CreateNoWindow = true;
                proc.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;//这里设置DOS窗口不显示，经实践可行
                proc.Start();
                proc.WaitForExit();
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Exception Occurred :{0},{1}", ex.Message, ex.StackTrace.ToString());
            }
        }
    }
}
