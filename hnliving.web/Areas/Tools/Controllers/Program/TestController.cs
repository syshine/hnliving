using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Lib.Core;

namespace hnliving.web.Areas.Tools.Controllers.Program
{
    public class TestController : BaseWebController
    {
        // GET: Tools/Program/Test
        public ActionResult Index()
        {
            return View();
        }

        #region MemCache
        public ActionResult MemCache()
        {
            return View();
        }

        public ActionResult SetMemCache(string key, string value)
        {
            try
            {
                if (!MngConfig.SiteConfig.EnableMemcache)
                    return Content("Memcache没有开启！");

                MemCachedHelper mch = new MemCachedHelper();

                if(mch.Set(key, value))
                    return Content("设置成功！");
                else
                    return Content("设置失败！");
            }
            catch (Exception ex)
            {
                return Content("设置错误！错误内容如下：\r\n" + ex.Message);
            }
        }

        public ActionResult GetMemCache(string key)
        {
            try
            {
                if (!MngConfig.SiteConfig.EnableMemcache)
                    return Content("Memcache没有开启！");

                MemCachedHelper mch = new MemCachedHelper();

                if (!string.IsNullOrWhiteSpace(key))
                {
                    object obj = mch.Get(key);
                    if (obj != null)
                    {
                        return Content(obj.ToString());
                    }
                    else
                        return Content("空值");
                }
                else
                    return Content("key不能为空！");
            }
            catch (Exception ex)
            {
                return Content("获取错误！错误内容如下：\r\n" + ex.Message);
            }
        }

        public ActionResult ClearMemCache()
        {
            try
            {
                if (!MngConfig.SiteConfig.EnableMemcache)
                    return Content("Memcache没有开启！");

                MemCachedHelper mch = new MemCachedHelper();

                if (mch.DelFull())
                    return Content("清空成功！");
                else
                    return Content("清空失败！");
            }
            catch (Exception ex)
            {
                return Content("清空失败！错误内容如下：\r\n" + ex.Message);
            }
        }
        #endregion

        #region Redis
        public ActionResult Redis()
        {
            return View();
        }

        public ActionResult SetRedis(string key, string value)
        {
            try
            {
                if(!MngConfig.SiteConfig.EnableRedis)
                    return Content("Redis没有开启！");

                if (RedisHelper.Set(key, value))
                    return Content("设置成功！");
                else
                    return Content("设置失败！");
            }
            catch (Exception ex)
            {
                return Content("设置错误！错误内容如下：\r\n" + ex.Message);
            }
        }

        public ActionResult GetRedis(string key)
        {
            try
            {
                if (!MngConfig.SiteConfig.EnableRedis)
                    return Content("Redis没有开启！");

                if (!string.IsNullOrWhiteSpace(key))
                {
                    string obj = RedisHelper.GetString(key);
                    if (obj != null)
                    {
                        return Content(obj);
                    }
                    else
                        return Content("空值");
                }
                else
                    return Content("key不能为空！");
            }
            catch (Exception ex)
            {
                return Content("获取错误！错误内容如下：\r\n" + ex.Message);
            }
        }

        public ActionResult ClearRedis()
        {
            try
            {
                if (!MngConfig.SiteConfig.EnableRedis)
                    return Content("Redis没有开启！");

                RedisHelper.Clear();
                return Content("清空成功！");
            }
            catch (Exception ex)
            {
                return Content("清空失败！错误内容如下：\r\n" + ex.Message);
            }

        }
        #endregion

        #region ActiveMQ 消息推送
        public ActionResult PushMsg()
        {
            return View();
        }

        public ActionResult Push(string msg = "")
        {
            try
            {
                //if (!MngConfig.SiteConfig.EnableActiveMQ)
                //    return Content("ActiveMQ没有开启！");

                var model = new
                {
                    ShopId = "222",//门店编码
                    proNum = msg,//库存
                    skuNo = "111",//sku
                };

                var guid = Guid.NewGuid().ToString();
                var method = "updatestoreproductkuc111";
                var lst = new List<object>();
                lst.Add(model);

                ActiveMQHelper.Send(guid, lst, method);

                return Content("推送成功！");
                //if (ActiveMQHelper)
                //    return Content("设置成功！");
                //else
                //    return Content("设置失败！");
            }
            catch (Exception ex)
            {
                return Content("推送错误！错误内容如下：\r\n" + ex.Message);
            }
        }
        #endregion
    }
}