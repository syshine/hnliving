using System;
using System.Collections.Generic;
using System.IO;

namespace Lib.Core
{
    /// <summary>
    /// 配置管理类
    /// </summary>
    public partial class MngConfig
    {
        private static object _locker = new object();//锁对象

        private static IConfigStrategy _iconfigstrategy = null;//配置策略

        private static RDBSConfigInfo _rdbsconfiginfo = null;//关系数据库配置信息
        private static SiteConfigInfo _siteconfiginfo = null;//站点基本配置信息
        private static List<AccessConfigInfo> _lstaccessconfiginfo = null;//站点权限配置信息
        private static RedisNOSQLConfigInfo _redisnosqlconfiginfo = null;//redis非关系数据库配置信息
        private static RedisCacheConfigInfo _rediscacheconfiginfo = null;//redis缓存配置信息
        private static MemcachedCacheConfigInfo _memcachedcacheconfiginfo = null;//Memcached缓存配置信息
        private static MemcachedSessionConfigInfo _memcachedsessionconfiginfo = null;//Memcached会话状态配置信息

        static MngConfig()
        {
            try
            {
                string[] fileNameList = Directory.GetFiles(System.Web.HttpRuntime.BinDirectory, "hnliving.ConfigStrategy.*.dll", SearchOption.TopDirectoryOnly);
                var obj = Type.GetType(string.Format("hnliving.ConfigStrategy.{0}.ConfigStrategy, hnliving.ConfigStrategy.{0}", fileNameList[0].Substring(fileNameList[0].LastIndexOf("ConfigStrategy.") + 15).Replace(".dll", "")), false, true);
                _iconfigstrategy = (IConfigStrategy)Activator.CreateInstance(obj);
            }
            catch
            {
                throw new BaseException("创建'配置策略对象'失败,可能存在的原因:未将'配置策略程序集'添加到bin目录中;'配置策略程序集'文件名不符合'hnliving.ConfigStrategy.{策略名称}.dll'格式");
            }
            _rdbsconfiginfo = _iconfigstrategy.GetRDBSConfig();
            _siteconfiginfo = _iconfigstrategy.GetSiteConfig();
            Lstaccessconfiginfo = _iconfigstrategy.GetAccessConfig();
        }

        /// <summary>
        /// 关系数据库配置信息
        /// </summary>
        public static RDBSConfigInfo RDBSConfig
        {
            get { return _rdbsconfiginfo; }
        }

        /// <summary>
        /// 站点基本配置信息
        /// </summary>
        public static SiteConfigInfo SiteConfig
        {
            get { return _siteconfiginfo; }
        }

        /// <summary>
        /// 站点权限配置信息
        /// </summary>
        public static List<AccessConfigInfo> Lstaccessconfiginfo
        {
            get
            {
                return _lstaccessconfiginfo;
            }

            set
            {
                _lstaccessconfiginfo = value;
            }
        }

        /// <summary>
        /// Redis非关系型数据库配置信息
        /// </summary>
        public static RedisNOSQLConfigInfo RedisNOSQLConfig
        {
            get
            {
                if (_redisnosqlconfiginfo == null)
                {
                    lock (_locker)
                    {
                        if (_redisnosqlconfiginfo == null)
                        {
                            _redisnosqlconfiginfo = _iconfigstrategy.GetRedisNOSQLConfig();
                        }
                    }
                }
                return _redisnosqlconfiginfo;
            }
        }

        /// <summary>
        /// Redis缓存配置信息
        /// </summary>
        public static RedisCacheConfigInfo RedisCacheConfig
        {
            get
            {
                if (_rediscacheconfiginfo == null)
                {
                    lock (_locker)
                    {
                        if (_rediscacheconfiginfo == null)
                        {
                            _rediscacheconfiginfo = _iconfigstrategy.GetRedisCacheConfig();
                        }
                    }
                }
                return _rediscacheconfiginfo;
            }
        }

        /// <summary>
        /// Memcached缓存配置信息
        /// </summary>
        public static MemcachedCacheConfigInfo MemcachedCacheConfig
        {
            get
            {
                if (_memcachedcacheconfiginfo == null)
                {
                    lock (_locker)
                    {
                        if (_memcachedcacheconfiginfo == null)
                        {
                            _memcachedcacheconfiginfo = _iconfigstrategy.GetMemcachedCacheConfig();
                        }
                    }
                }
                return _memcachedcacheconfiginfo;
            }
        }

        /// <summary>
        /// Memcached会话状态配置信息
        /// </summary>
        public static MemcachedSessionConfigInfo MemcachedSessionConfig
        {
            get
            {
                if (_memcachedsessionconfiginfo == null)
                {
                    lock (_locker)
                    {
                        if (_memcachedsessionconfiginfo == null)
                        {
                            _memcachedsessionconfiginfo = _iconfigstrategy.GetMemcachedSessionConfig();
                        }
                    }
                }
                return _memcachedsessionconfiginfo;
            }
        }

        /// <summary>
        /// 保存站点配置信息
        /// </summary>
        public static void SaveSiteConfig(SiteConfigInfo siteConfigInfo)
        {
            lock (_locker)
            {
                if (_iconfigstrategy.SaveSiteConfig(siteConfigInfo))
                    _siteconfiginfo = siteConfigInfo;
            }
        }
    }
}
