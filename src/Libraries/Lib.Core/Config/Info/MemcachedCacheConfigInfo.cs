using System;
using System.Collections.Generic;

namespace Lib.Core
{
    /// <summary>
    /// Memcached缓存配置信息类
    /// </summary>
    [Serializable]
    public class MemcachedCacheConfigInfo : IConfigInfo
    {
        private List<string> _serverlist;//服务器列表
        private int _minpoolsize;//连接池最小连接数
        private int _maxpoolsize;//连接池最大连接数
        private int _connectiontimeout;//连接超时时间
        private int _sockettimeout;//查询超时时间
        private int _maintenancesleep;//设置维护线程运行的睡眠时间

        /// <summary>
        /// 服务器列表
        /// </summary>
        public List<string> ServerList
        {
            get { return _serverlist; }
            set { _serverlist = value; }
        }
        /// <summary>
        /// 连接池最小连接数
        /// </summary>
        public int MinPoolSize
        {
            get { return _minpoolsize; }
            set { _minpoolsize = value > 0 ? value : 10; }
        }
        /// <summary>
        /// 连接池最大连接数
        /// </summary>
        public int MaxPoolSize
        {
            get { return _maxpoolsize; }
            set { _maxpoolsize = value > 0 ? value : 20; }
        }
        /// <summary>
        /// 连接超时时间
        /// </summary>
        public int ConnectionTimeOut
        {
            get { return _connectiontimeout; }
            set { _connectiontimeout = value > 0 ? value : 1000; }
        }
        /// <summary>
        /// 查询超时时间
        /// </summary>
        public int SocketTimeout
        {
            get { return _sockettimeout; }
            set { _sockettimeout = value > 0 ? value : 3000; }
        }
        /// <summary>
        /// 设置维护线程运行的睡眠时间
        /// </summary>
        public int MaintenanceSleep
        {
            get { return _maintenancesleep; }
            set { _maintenancesleep = value > 0 ? value : 100; }
        }
    }
}
