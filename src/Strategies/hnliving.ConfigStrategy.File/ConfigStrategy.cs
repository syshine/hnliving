using System;

using Lib.Core;

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
        private readonly string _redisnosqlconfigfilepath = "/App_Data/redisnosql.config";//redis非关系型数据库配置信息文件路径

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
        /// 获得Redis非关系型数据库配置
        /// </summary>
        public RedisNOSQLConfigInfo GetRedisNOSQLConfig()
        {
            return (RedisNOSQLConfigInfo)LoadConfigInfo(typeof(RedisNOSQLConfigInfo), IOHelper.GetMapPath(_redisnosqlconfigfilepath));
        }
    }
}
