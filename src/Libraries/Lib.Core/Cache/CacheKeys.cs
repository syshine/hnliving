using System;

namespace Lib.Core
{
    /// <summary>
    /// 缓存键
    /// </summary>
    public partial class CacheKeys
    {
        /// <summary>
        /// 在线游客数量缓存键
        /// </summary>
        public const string SITE_ONLINEUSER_GUESTCOUNT = "/Mall/OnlineGuestCount";
        /// <summary>
        /// 全部在线人数缓存键
        /// </summary>
        public const string SITE_ONLINEUSER_ALLUSERCOUNT = "/Mall/OnlineAllUserCount";

        /// <summary>
        /// 用户等级列表缓存键
        /// </summary>
        public const string SITE_USERRANK_LIST = "/Mall/UserRankList";

        /// <summary>
        /// 被禁止的IPHashSet缓存键
        /// </summary>
        public const string SITE_BANNEDIP_HASHSET = "/Mall/BannedIPHashSet";

        /// <summary>
        /// 筛选词正则列表缓存键
        /// </summary>
        public const string SITE_FILTERWORD_REGEXLIST = "/Mall/FilterWordRegexList";
    }
}
