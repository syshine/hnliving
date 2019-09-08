using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lib.Core
{
    public class StockDefine
    {
        /// <summary>
        /// 上海交易所简称
        /// </summary>
        private static string _EXCHANGE_SH = "SH";
        
        /// <summary>
        /// 深圳交易所简称
        /// </summary>
        private static string _EXCHANGE_SZ = "SZ";

        /// <summary>
        /// 指数简称
        /// </summary>
        private static string _EXCHANGE_ZS = "ZS";

        public static string EXCHANGE_SH
        {
            get
            {
                return _EXCHANGE_SH;
            }
        }

        public static string EXCHANGE_SZ
        {
            get
            {
                return _EXCHANGE_SZ;
            }
        }

        public static string EXCHANGE_ZS
        {
            get
            {
                return _EXCHANGE_ZS;
            }
        }
    }
}
