using System;
using System.Text;
using System.Collections;

namespace Lib.Core
{
    /// <summary>
    /// 字符串帮助类
    /// </summary>
    public class TimeHelper
    {
        /// <summary>
        /// 获取时间戳
        /// </summary>
        /// <returns></returns>
        public static string GetTimeStamp(DateTime time)
        {
            long ts = ConvertDateTimeToInt(time);
            return ts.ToString();
        }
        /// <summary>
        /// 获取当前时间戳
        /// </summary>
        /// <param name="cnt">时间戳位数</param>
        /// <returns></returns>
        public static string GetNowStamp(int cnt = 10)
        {
            switch(cnt)
            {
                case 10:
                    return ((DateTime.Now.ToUniversalTime().Ticks - 621355968000000000) / 10000000).ToString();

                case 13:
                    return ((DateTime.Now.ToUniversalTime().Ticks - 621355968000000000) / 10000).ToString();

                default:
                    return (DateTime.Now.ToUniversalTime().Ticks - 621355968000000000).ToString();
            }
            //return GetTimeStamp(DateTime.Now);
        }
        /// <summary>  
        /// 将c# DateTime时间格式转换为Unix时间戳格式  
        /// </summary>  
        /// <param name="time">时间</param>  
        /// <returns>long</returns>  
        public static long ConvertDateTimeToInt(DateTime time)
        {
            DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1, 0, 0, 0, 0));
            long t = (time.Ticks - startTime.Ticks) / 10000;   //除10000调整为13位      
            return t;
        }
        /// <summary>        
        /// 时间戳转为C#格式时间        
        /// </summary>        
        /// <param name=”timeStamp”></param>        
        /// <returns></returns>        
        private DateTime ConvertStringToDateTime(string timeStamp)
        {
            DateTime dtStart = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
            long lTime = long.Parse(timeStamp + "0000");
            TimeSpan toNow = new TimeSpan(lTime);
            return dtStart.Add(toNow);
        }

    }
}
