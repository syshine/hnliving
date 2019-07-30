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
                RedisHelper.Clear();
                return Content("清空成功！");
            }
            catch (Exception ex)
            {
                return Content("清空失败！错误内容如下：\r\n" + ex.Message);
            }

        }
        #endregion
    }
}