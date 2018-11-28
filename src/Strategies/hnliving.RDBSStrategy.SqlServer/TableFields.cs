using System;

namespace hnliving.RDBSStrategy.SqlServer
{
    /// <summary>
    /// SqlServer策略之表字段分部类
    /// </summary>
    internal partial class TableFields
    {
        /// <summary>
        /// 属性分组表
        /// </summary>
        public const string ATTRIBUTE_GROUPS = "[attrgroupid],[cateid],[name],[displayorder]";

        /// <summary>
        /// 属性表
        /// </summary>
        public const string ATTRIBUTES = "[attrid],[name],[cateid],[attrgroupid],[showtype],[isfilter],[displayorder]";

        /// <summary>
        /// 属性值表
        /// </summary>
        public const string ATTRIBUTE_VALUES = "[attrvalueid],[attrvalue],[isinput],[attrname],[attrdisplayorder],[attrshowtype],[attrvaluedisplayorder],[attrgroupid],[attrgroupname],[attrgroupdisplayorder],[attrid]";

        /// <summary>
        /// 被禁止的ip表
        /// </summary>
        public const string BANNEDIPS = "[id],[ip],[liftbantime]";

        /// <summary>
        /// 事件日志表
        /// </summary>
        public const string EVENTLOGS = "[id],[key],[title],[server],[executetime]";

        /// <summary>
        /// 筛选词表
        /// </summary>
        public const string FILTERWORDS = "[id],[match],[replace]";

        /// <summary>
        /// 友情链接表
        /// </summary>
        public const string FRIENDLINKS = "[id],[name],[title],[logo],[url],[target],[displayorder]";
        
        /// <summary>
        /// 登录失败日志表
        /// </summary>
        public const string LOGINFAILLOGS = "[id],[loginip],[failtimes],[lastlogintime]";
        
        /// <summary>
        /// 导航栏表
        /// </summary>
        public const string NAVS = "[id],[pid],[layer],[name],[title],[url],[target],[displayorder]";

        /// <summary>
        /// 新闻表
        /// </summary>
        public const string NEWS = "[newsid],[newstypeid],[isshow],[istop],[ishome],[displayorder],[addtime],[title],[url],[body]";

        /// <summary>
        /// 新闻类型表
        /// </summary>
        public const string NEWS_TYPES = "[newstypeid],[name],[displayorder]";
        
        /// <summary>
        /// 用户在线时间表
        /// </summary>
        public const string ONLINE_TIME = "[uid],[total],[year],[month],[week],[day],[updatetime]";

        /// <summary>
        /// 在线用户表
        /// </summary>
        public const string ONLINE_USERS = "[olid],[uid],[sid],[nickname],[ip],[regionid],[updatetime]";
        
        /// <summary>
        /// PV统计表
        /// </summary>
        public const string PVSTATS = "[recordid],[storeid],[category],[value],[count]";

        /// <summary>
        /// 全国行政区域表
        /// </summary>
        public const string REGIONS = "[regionid],[name],[spell],[shortspell],[displayorder],[parentid],[layer],[provinceid],[provincename],[cityid],[cityname]";
        
        /// <summary>
        /// 活动专题表
        /// </summary>
        public const string TOPICS = "[topicid],[starttime],[endtime],[isshow],[sn],[title],[headhtml],[bodyhtml]";

        /// <summary>
        /// 部分用户表
        /// </summary>
        public const string PARTUSERS = "[uid],[username],[email],[mobile],[password],[userrid],[storeid],[mallagid],[nickname],[avatar],[paycredits],[rankcredits],[verifyemail],[verifymobile],[liftbantime],[salt]";

        /// <summary>
        /// 用户细节表
        /// </summary>
        public const string USERDETAILS = "[uid],[lastvisittime],[lastvisitip],[lastvisitrgid],[registertime],[registerip],[registerrgid],[gender],[realname],[bday],[idcard],[regionid],[address],[bio]";

        /// <summary>
        /// 用户等级表
        /// </summary>
        public const string USER_RANKS = "[userrid],[system],[title],[avatar],[creditslower],[creditsupper],[limitdays]";
    }
}
