using System;

using Lib.Core;
using System.Collections.Generic;

namespace hnliving.ConfigStrategy.File
{
    /// <summary>
    /// 基于文件的配置策略
    /// </summary>
    public partial class ConfigStrategy : IConfigStrategy
    {
        #region 私有字段

        private readonly string _rdbsconfigfilepath = "/App_Data/rdbs.config";//关系数据库配置信息文件路径
        private readonly string _siteconfigfilepath = "/App_Data/site.config";//站点基本配置信息文件路径
        private readonly string _accessconfigfilepath = "/App_Data/access.config";//站点权限配置信息文件路径
        private readonly string _redisnosqlconfigfilepath = "/App_Data/redisnosql.config";//redis非关系型数据库配置信息文件路径
        private readonly string _rediscacheconfigfilepath = "/App_Data/redis.config";//Redis缓存配置信息文件路径
        private readonly string _memcachedcacheconfigfilepath = "/App_Data/memcachedcache.config";//Memcached缓存配置信息文件路径
        private readonly string _memcachedsessionconfigfilepath = "/App_Data/memcachedsession.config";//Memcached会话状态配置信息文件路径

        #endregion

        #region 帮助方法

        /// <summary>
        /// 从文件中加载配置信息
        /// </summary>
        /// <param name="configInfoType">配置信息类型</param>
        /// <param name="configInfoFile">配置信息文件路径</param>
        /// <returns>配置信息</returns>
        private IConfigInfo LoadConfigInfo(Type configInfoType, string configInfoFile)
        {
            return (IConfigInfo)IOHelper.DeserializeFromXML(configInfoType, configInfoFile);
        }

        /// <summary>
        /// 从文件中加载配置信息
        /// </summary>
        /// <param name="configInfoType">配置信息类型</param>
        /// <param name="configInfoFile">配置信息文件路径</param>
        /// <returns>配置信息</returns>
        private List<AccessConfigInfo> LoadAccessConfigInfo(Type configInfoType, string configInfoFile)
        {
            return (List<AccessConfigInfo>)IOHelper.DeserializeFromXML(configInfoType, configInfoFile);
        }

        /// <summary>
        /// 将配置信息保存到文件中
        /// </summary>
        /// <param name="configInfo">配置信息</param>
        /// <param name="configInfoFile">保存路径</param>
        /// <returns>是否保存成功</returns>
        private bool SaveConfigInfo(IConfigInfo configInfo, string configInfoFile)
        {
            return IOHelper.SerializeToXml(configInfo, configInfoFile);
        }

        #endregion

        /// <summary>
        /// 获得关系数据库配置
        /// </summary>
        public RDBSConfigInfo GetRDBSConfig()
        {
            return (RDBSConfigInfo)LoadConfigInfo(typeof(RDBSConfigInfo), IOHelper.GetMapPath(_rdbsconfigfilepath));
        }

        /// <summary>
        /// 保存站点基本配置
        /// </summary>
        /// <param name="configInfo">站点基本配置信息</param>
        /// <returns>是否保存结果</returns>
        public bool SaveSiteConfig(SiteConfigInfo configInfo)
        {
            return SaveConfigInfo(configInfo, IOHelper.GetMapPath(_siteconfigfilepath));
        }

        /// <summary>
        /// 获得站点基本配置
        /// </summary>
        public SiteConfigInfo GetSiteConfig()
        {
            return (SiteConfigInfo)LoadConfigInfo(typeof(SiteConfigInfo), IOHelper.GetMapPath(_siteconfigfilepath));
        }

        /// <summary>
        /// 获得站点基本配置
        /// </summary>
        public List<AccessConfigInfo> GetAccessConfig()
        {
            return (List<AccessConfigInfo>)LoadAccessConfigInfo(typeof(List<AccessConfigInfo>), IOHelper.GetMapPath(_accessconfigfilepath));
        }

        /// <summary>
        /// 获得Redis非关系型数据库配置
        /// </summary>
        public RedisNOSQLConfigInfo GetRedisNOSQLConfig()
        {
            return (RedisNOSQLConfigInfo)LoadConfigInfo(typeof(RedisNOSQLConfigInfo), IOHelper.GetMapPath(_redisnosqlconfigfilepath));
        }

        /// <summary>
        /// 获得Redis缓存配置
        /// </summary>
        public RedisCacheConfigInfo GetRedisCacheConfig()
        {
            return (RedisCacheConfigInfo)LoadConfigInfo(typeof(RedisCacheConfigInfo), IOHelper.GetMapPath(_rediscacheconfigfilepath));
        }

        /// <summary>
        /// 获得Memcached缓存配置
        /// </summary>
        public MemcachedCacheConfigInfo GetMemcachedCacheConfig()
        {
            return (MemcachedCacheConfigInfo)LoadConfigInfo(typeof(MemcachedCacheConfigInfo), IOHelper.GetMapPath(_memcachedcacheconfigfilepath));
        }

        /// <summary>
        /// 获得Memcached会话状态配置
        /// </summary>
        public MemcachedSessionConfigInfo GetMemcachedSessionConfig()
        {
            return (MemcachedSessionConfigInfo)LoadConfigInfo(typeof(MemcachedSessionConfigInfo), IOHelper.GetMapPath(_memcachedsessionconfigfilepath));
        }
    }
}
