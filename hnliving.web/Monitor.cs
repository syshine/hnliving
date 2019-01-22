using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Timers;
using System.Net;
using System.IO;
using Lib.Core;

namespace hnliving.web
{
    public static class Monitor
    {
        static Timer mntTimer = null;
        private static string _access_token = "";
        private static string _expires_in = ""; // access_token有效时间(秒)
        private static string _timeout_time = "";   // access_token超时时间

        public static string AccessToken
        {
            get
            {
                return _access_token;
            }
        }

        public static string ExpiresIn
        {
            get
            {
                return _expires_in;
            }
        }

        public static string ReqestTime
        {
            get
            {
                return _timeout_time;
            }

            set
            {
                _timeout_time = value;
            }
        }

        public static void Init()
        {
            if (mntTimer == null)
            {
                mntTimer = new Timer(60000);//实例化Timer类，设置时间间隔为一分钟
                mntTimer.Elapsed += new ElapsedEventHandler(OnElapsedEvent);//到达时间的时候执行事件
                mntTimer.AutoReset = true;//设置是执行一次（false）还是一直执行(true)
                mntTimer.Enabled = true;//是否执行System.Timers.Timer.Elapsed事件
                mntTimer.Start();
            }
        }

        /// <summary>
        /// Timer的Elapsed事件执行的方法
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void OnElapsedEvent(Object sender, ElapsedEventArgs e)
        {
            try
            {
                if (_timeout_time != "")
                {
                    DateTime dt = DateTime.Parse(_timeout_time);
                    // 现在离超时时间3分钟内才获取access_token
                    if (DateTime.Now < dt.AddMinutes(-3))
                    {
                        return;
                    }
                }

                string result = HttpGet(GetRequestString());
                System.Diagnostics.Debug.WriteLine(result);
                EntityAccessToken eat = DeserializeAccessToken(result);
                _access_token = eat.access_token;
                _expires_in = eat.expires_in;
                double expires_in = double.Parse(eat.expires_in);
                _timeout_time = DateTime.Now.AddSeconds(expires_in).ToString();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
        }

        public static string GetRequestString()
        {
            string url = string.Format("https://api.weixin.qq.com/cgi-bin/token?grant_type=client_credential&appid={0}&secret={1}", MngConfig.SiteConfig.WxGzhAppId, MngConfig.SiteConfig.WxGzhAppSecret);
            return url;
        }

        public static string HttpGet(string url)
        {
            string result = string.Empty;
            try
            {
                HttpWebRequest wbRequest = (HttpWebRequest)WebRequest.Create(url);
                wbRequest.Method = "GET";
                HttpWebResponse wbResponse = (HttpWebResponse)wbRequest.GetResponse();
                using (Stream responseStream = wbResponse.GetResponseStream())
                {
                    using (StreamReader sReader = new StreamReader(responseStream))
                    {
                        result = sReader.ReadToEnd();
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);

            }
            return result;
        }

        public static EntityAccessToken DeserializeAccessToken(string jsonStr)
        {
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            EntityAccessToken eat = serializer.Deserialize<EntityAccessToken>(jsonStr);
            return eat;
        }
    }

    public class EntityAccessToken
    {
        public string access_token { get; set; }
        public string expires_in { get; set; }
    }
}