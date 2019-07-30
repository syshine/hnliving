using System;

namespace Lib.Core
{
    /// <summary>
    /// Memcached缓存配置信息类
    /// </summary>
    [Serializable]
    public class RedisCacheConfigInfo : IConfigInfo
    {
        private string _host;//主机地址
        private int _port = 6379;//端口

        /// <summary>
        /// 主机地址
        /// </summary>
        public string Host
        {
            get
            {
                return _host;
            }

            set
            {
                _host = value != "" ? value : "127.0.0.1" ;
            }
        }

        /// <summary>
        /// 端口
        /// </summary>
        public int Port
        {
            get
            {
                return _port;
            }

            set
            {
                _port = value;
            }
        }
    }
}
