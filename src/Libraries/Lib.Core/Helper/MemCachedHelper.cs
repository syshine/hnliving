using System;
using Memcached.ClientLibrary;
using log4net;

namespace Lib.Core
{
    /// <summary>
    /// 说    明： MemCachedHelper MemCached内存数据库操作类
    /// </summary>
    public class MemCachedHelper
    {
        static SockIOPool pool = null;
        static ILog log = LogManager.GetLogger("MemCachedLog");
        static string sysName = string.Empty;

        /// <summary>
        /// 存储
        /// </summary>
        /// <param name="name">键名</param>
        /// <param name="obj">对象</param>
        /// <param name="expiry">失效日期</param>
        public bool Set_date(string name, object obj,int tick)
        {
            if (pool == null)
            {
                log.Info("memcached连接服务未打开，禁止访问!");
                return false;
            }

            try
            {
                MemcachedClient mc = new MemcachedClient();
                mc.EnableCompression = false;
                //是否增加过期时间
                if (tick == -1)
                {
                    return mc.Set(TName(name), obj);
                }
                else
                    return mc.Set(TName(name), obj, new DateTime(DateTime.Now.AddMilliseconds(tick).Ticks)); 

                //return mc.Set(TName(name), obj,new DateTime( DateTime.Now.AddMilliseconds(30000).Ticks)); 

            }
            catch (Exception ex)
            {
                log.Debug("Set错误！", ex);
                return false;
            }
        }


        /// <summary>
        /// 存储
        /// </summary>
        /// <param name="name">键名</param>
        /// <param name="obj">对象</param>
        /// <returns></returns>
        public bool Set(string name, object obj)
        {
            return Set_date(name, obj, -1);
        }

        /// <summary>
        /// 获取
        /// </summary>
        /// <param name="name">键名</param>
        /// <returns>对象</returns>
        public object Get(string name)
        {
            if (pool == null)
            {
                log.Info("memcached连接服务未打开，禁止访问!");
                return null;
            }

            try
            {
                MemcachedClient mc = new MemcachedClient();
                mc.EnableCompression = false;
                return mc.Get(TName(name));
            }
            catch(Exception ex)
            {
                log.Debug("Get错误！", ex);
                return null;
            }
        }

        /// <summary>
        /// 验证键是否存在
        /// </summary>
        /// <param name="name">键名</param>
        /// <returns>true or false</returns>
        public bool IsExists(string name)
        {
            if (pool == null)
            {
                log.Info("memcached连接服务未打开，禁止访问!");
                return false;
            }

            try
            {
                MemcachedClient mc = new MemcachedClient();
                mc.EnableCompression = false;
                return mc.KeyExists(TName(name));
            }
            catch(Exception ex)
            {
                log.Debug("IsExists错误！", ex);
                return false;
            }

        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="name">键名</param>
        public bool Del(string name)
        {
            if (pool == null)
            {
                log.Info("memcached连接服务未打开，禁止访问!");
                return false;
            }

            try
            {
                MemcachedClient mc = new MemcachedClient();
                mc.EnableCompression = false;
                return mc.Delete(TName(name));
            }
            catch (Exception ex)
            {
                log.Debug("Del错误！", ex);
                return false;
            }
        }

        /// <summary>
        /// 存储
        /// </summary>
        /// <param name="name">键名</param>
        /// <param name="table">表名、域名</param>
        /// <param name="obj">对象</param>
        /// <returns></returns>
        public void Set(string name, string table, object obj)
        {
            Set(table + "@#@" + name, obj);
        }

        /// <summary>
        /// 获取
        /// </summary>
        /// <param name="name">键名</param>
        /// <param name="table">表名、域名</param>
        /// <returns></returns>
        public object Get(string name, string table) 
        {
            return Get(table + "@#@" + name);
        }

        /// <summary>
        /// 验证键是否存在
        /// </summary>
        /// <param name="name">键名</param>
        /// <returns>表名、域名</returns>
        public bool IsExists(string name, string table)
        {
            return IsExists(table + "@#@" + name);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="name">键名</param>
        /// <returns>表名、域名</returns>
        public bool Del(string name, string table)
        {
            return Del(table + "@#@" + name);
        }

        /// <summary>
        /// 清空
        /// </summary>
        /// <returns></returns>
        public bool DelFull()
        {
            return FlushAll();
        }

        /// <summary>
        /// 打开memcatched连接服务，该方法在应用启动时仅需调用一次！
        /// </summary>
        /// <param name="mcci">MemcachedCache配置信息</param>
        /// <param name="sysName">系统标示</param>
        public static void Open(MemcachedCacheConfigInfo mcci, string _sysName)
        {
            if (pool == null && mcci != null)
            {
                if (string.IsNullOrEmpty(_sysName))
                {
                    log.Error("系统标示名称不能为空！");
                }

                //设置系统标示
                sysName = _sysName;

                try
                {
                    //创建memcatched服务器连接服务；
                    pool = SockIOPool.GetInstance();

                    pool.SetServers(mcci.ServerList.ToArray());

                    pool.InitConnections = mcci.MinPoolSize;
                    pool.MinConnections = mcci.MinPoolSize;
                    pool.MaxConnections = mcci.MaxPoolSize;

                    pool.SocketConnectTimeout = mcci.ConnectionTimeOut;
                    pool.SocketTimeout = mcci.SocketTimeout;

                    pool.MaintenanceSleep = mcci.MaintenanceSleep;
                    pool.Failover = true;

                    pool.Nagle = false;
                    pool.Initialize();

                    //pool.SetServers(serverList.Split(new char[] { ';', '；' }, StringSplitOptions.RemoveEmptyEntries));

                    //pool.InitConnections = 10;
                    //pool.MinConnections = 10;
                    //pool.MaxConnections = 300;

                    //pool.SocketConnectTimeout = 1000;
                    //pool.SocketTimeout = 3000;

                    //pool.MaintenanceSleep = 30;
                    //pool.Failover = true;

                    //pool.Nagle = false;
                    //pool.Initialize();

                    log.Info("打开memcatched连接服务-成功!");
                }
                catch (Exception ex)
                {
                    log.Error("打开memcatched连接服务-失败!", ex);
                }
            }
        }


        /// <summary>
        /// 打开memcatched连接服务，该方法在应用启动时仅需调用一次！
        /// </summary>
        /// <param name="serverList">服务地址连接，多个地址使用“；”分开</param>
        /// <param name="sysName">系统标示</param>
        public static void Open(string serverList,string _sysName)
        {
            if (pool == null && !string.IsNullOrEmpty(serverList))
            {
                if(string.IsNullOrEmpty(_sysName))
                {
                    log.Error("系统标示名称不能为空！");
                }

                //设置系统标示
                sysName = _sysName;

                try
                {
                    //创建memcatched服务器连接服务；
                    pool = SockIOPool.GetInstance();
                    pool.SetServers(serverList.Split(new char[] { ';', '；' }, StringSplitOptions.RemoveEmptyEntries));

                    pool.InitConnections = 10;
                    pool.MinConnections = 10;
                    pool.MaxConnections = 300;

                    pool.SocketConnectTimeout = 1000;
                    pool.SocketTimeout = 3000;

                    pool.MaintenanceSleep = 30;
                    pool.Failover = true;

                    pool.Nagle = false;
                    pool.Initialize();

                    log.Info("打开memcatched连接服务-成功!");
                }
                catch(Exception ex)
                {
                    log.Error("打开memcatched连接服务-失败!", ex);
                }
            }
        }

        /// <summary>
        /// 关闭memcatched连接服务，该方案在应用关闭时调用！
        /// </summary>
        public static void Stop()
        {
            if (pool != null)
            {
                try
                {
                    SockIOPool.GetInstance().Shutdown();
                    log.Info("关闭memcatched连接服务-成功!");
                }
                catch (Exception ex)
                {
                    log.Error("关闭memcatched连接服务-失败!", ex);
                }
            }
        }

        /// <summary>
        /// 转换键名
        /// </summary>
        /// <param name="keyName">键名</param>
        /// <returns></returns>
        private string TName(string keyName)
        {
            return sysName + "_" + keyName.Trim();
        }

        /// <summary>
        /// 清空memcatched数据
        /// </summary>
        /// <returns>是否成功</returns>
        private static bool FlushAll()
        {
            try
            {
                MemcachedClient mc = new MemcachedClient();
                mc.FlushAll();

                return true;
            }
            catch(Exception ex)
            {
                log.Debug("FlushAll", ex);
                return false;
            }

        }

        /// <summary>
        /// 判断数据库连接状态
        /// </summary>
        /// <returns></returns>
        public static bool ifOpen()
        {
            if (pool == null)
                return false;
            return true;
        }

    }
}
