using ServiceStack.Redis;
using System;

namespace Lib.Core
{
    /// <summary>
    /// 说    明： RedisHelper Redis内存数据库操作类
    /// </summary>
    public class RedisHelper
    {
        private static RedisClient client = null;// new RedisClient("127.0.0.1", 6379);

        static RedisHelper()
        {
            string host = MngConfig.RedisCacheConfig.Host;
            int port = MngConfig.RedisCacheConfig.Port;

            client = new RedisClient(host, port);
        }

        public static bool SetString(string name, string value)
        {
            return client.Set<string>(name, value);
        }

        public static bool Set<T>(string name, T value)
        {
            return client.Set<T>(name, value);
        }

        public static string GetString(string name)
        {
            return client.Get<string>(name);
        }

        public static T Get<T>(string name)
        {
            return client.Get<T>(name);
        }

        public static long Del(string[] keys)
        {
            return client.Del(keys);
        }

        public static void Clear()
        {
            client.Del(client.GetAllKeys().ToArray());
        }

    }
}
