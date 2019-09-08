using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lib.Core
{
    [Serializable]
    public class StockKLineData
    {
        public StockKLineData()
        {
            INFO = new List<object>();
            RIGHT = new List<string>();
            NOW = new List<string>();
            MIN = new List<string>();
            TICK = new List<string>();
            DAY = new List<List<object>>();
            DAY5 = new List<List<object>>();
            M5 = new List<string>();
            SEER = new List<string>();
        }

        /// <summary>
        /// INFO
        /// 例：['SZ', '300545', '联得装备', '300545,LDZB', 3, 2, 2, 19.60, 21.56, 17.64]
        /// [交易所, 股票代码, 股票名称, 代码和简称，]
        /// </summary>
        public List<object> INFO { get; set; }

        /// <summary>
        /// RIGHT 派息和送股的信息
        /// 例：[20170511, 0, 2000, 0, 0],[20180412, 10000, 2000, 0, 0]
        /// </summary>
        public List<string> RIGHT { get; set; }

        /// <summary>
        /// NOW
        /// </summary>
        public List<string> NOW { get; set; }

        /// <summary>
        /// MIN
        /// 例：[0, 19.86, 19.86, 19.62, 19.80, 20500, 406200],//分钟线
        /// [第几分钟（从0开始）, 开盘,最高,最低,收盘,到目前成交量,到目前成交额]
        /// </summary>
        public List<string> MIN { get; set; }

        /// <summary>
        /// TICK
        /// </summary>
        public List<string> TICK { get; set; }

        /// <summary>
        /// DAY (因为数组里的元素类型不同，所以要定义成object)
        /// 例：[20180823, 19.86, 20.11, 19.62, 19.79, 858900, 17111600] // 858900是股不是手
        /// [日期, 开盘,最高,最低,收盘,成交量,成交额]
        /// </summary>
        public List<List<object>> DAY { get; set; }

        /// <summary>
        /// DAY5
        /// </summary>
        public List<List<object>> DAY5 { get; set; }

        /// <summary>
        /// M5
        /// </summary>
        public List<string> M5 { get; set; }

        /// <summary>
        /// DAY5
        /// </summary>
        public List<string> SEER { get; set; }
    }

    ///// <summary>
    ///// 数据格式
    ///// </summary>
    //public class StockKLineFormat
    //{
    //    /// <summary>
    //    /// 基础信息
    //    /// 需改，后面几个参数还未明白其意思
    //    /// </summary>
    //    private static string _INFO = "{0},{1},{2},{1},{3},3,2,2,19.60,21.56,17.64";

    //    /// <summary>
    //    /// 日线数据
    //    /// [日期, 开盘,最高,最低,收盘,成交量(股),成交额]
    //    /// </summary>
    //    private static string _DAY = "[{0}, {1}, {2}, {3}, {4}, {5}, {6}]";

    //    /// <summary>
    //    /// 分钟线数据
    //    /// [第几分钟, 开盘,最高,最低,收盘,到目前成交量,到目前成交额]
    //    /// </summary>
    //    private static string _MIN = "[{0}, {1}, {2}, {3}, {4}, {5}, {6}]";

    //    public static string INFO
    //    {
    //        get
    //        {
    //            return _INFO;
    //        }
    //    }

    //    public static string DAY
    //    {
    //        get
    //        {
    //            return _DAY;
    //        }
    //    }

    //    public static string MIN
    //    {
    //        get
    //        {
    //            return _MIN;
    //        }
    //    }
    //}
}
