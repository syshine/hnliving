using System;
using System.Collections.Generic;

namespace Lib.Core
{
    /// <summary>
    /// hnliving配置策略接口
    /// </summary>
    public partial interface IConfigStrategy
    {
        /// <summary>
        /// 获得关系数据库配置
        /// </summary>
        RDBSConfigInfo GetRDBSConfig();

        /// <summary>
        /// 保存站点基本配置
        /// </summary>
        /// <param name="configInfo">站点基本配置信息</param>
        /// <returns>是否保存成功</returns>
        bool SaveSiteConfig(SiteConfigInfo configInfo);

        /// <summary>
        /// 获得站点基本配置
        /// </summary>
        SiteConfigInfo GetSiteConfig();

        /// <summary>
        /// 获得站点权限配置
        /// </summary>
        List<AccessConfigInfo> GetAccessConfig();

        /// <summary>
        /// 获得Redis非关系型数据库配置
        /// </summary>
        RedisNOSQLConfigInfo GetRedisNOSQLConfig();

        /// <summary>
        /// 获得RedisCache缓存配置
        /// </summary>
        /// <returns></returns>
        RedisCacheConfigInfo GetRedisCacheConfig();

        /// <summary>
        /// 获得Memcached缓存配置
        /// </summary>
        MemcachedCacheConfigInfo GetMemcachedCacheConfig();

        /// <summary>
        /// 获得Memcached会话状态配置
        /// </summary>
        MemcachedSessionConfigInfo GetMemcachedSessionConfig();
    }
}
