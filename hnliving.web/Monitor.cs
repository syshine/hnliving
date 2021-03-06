﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Timers;
using System.Net;
using System.IO;
using Lib.Core;
using System.Collections;

namespace hnliving.web
{
    /// <summary>
    /// 微信监控获取票据
    /// </summary>
    public static class Monitor
    {
        static Timer mntTimer = null;
        private static string _access_token = "";
        private static string _at_expires_in = ""; // access_token有效时间(秒)
        private static string _at_timeout_time = "";   // access_token超时时间

        private static string _jsapi_ticket = ""; // 调用微信JS接口的临时票据。正常情况下，jsapi_ticket的有效期为7200秒
        private static string _jt_expires_in = ""; // access_token有效时间(秒)
        private static string _jt_timeout_time = "";   // access_token超时时间

        public static string index = "-111";   // 执行次序
        public static string indexA = "-111";   // 执行次序
        public static string msg = "-";   // 
        public static string ret = "-";   // 
        public static string retA = "-";   // 

        public static string AccessToken
        {
            get
            {
                return _access_token;
            }
        }

        public static string AtExpiresIn
        {
            get
            {
                return _at_expires_in;
            }
        }

        public static string AtTimeoutTime
        {
            get
            {
                return _at_timeout_time;
            }

            set
            {
                _at_timeout_time = value;
            }
        }

        public static string JsapiTicket
        {
            get
            {
                return _jsapi_ticket;
            }

            set
            {
                _jsapi_ticket = value;
            }
        }

        public static string JtExpiresIn
        {
            get
            {
                return _jt_expires_in;
            }

            set
            {
                _jt_expires_in = value;
            }
        }

        public static string JtTimeoutTime
        {
            get
            {
                return _jt_timeout_time;
            }

            set
            {
                _jt_timeout_time = value;
            }
        }

        public static void Init()
        {
            index = "-2";
            if (mntTimer == null)
            {
                index = "-1";
                mntTimer = new Timer(10000);//实例化Timer类，设置时间间隔为一分钟
                mntTimer.Elapsed += new ElapsedEventHandler(OnElapsedEvent);//到达时间的时候执行事件
                mntTimer.AutoReset = true;//设置是执行一次（false）还是一直执行(true)
                mntTimer.Enabled = true;//是否执行System.Timers.Timer.Elapsed事件
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
                index = "0";
                #region 获取access_token
                bool bGetAccessToken = true;
                if (_at_timeout_time != "")
                {
                    index = "0.5";
                    DateTime dt = DateTime.Parse(_at_timeout_time);
                    // 现在离超时时间3分钟内才获取access_token
                    if (DateTime.Now < dt.AddMinutes(-3))
                    {
                        bGetAccessToken = false;
                    }
                }

                index = "1";
                indexA = "1";
                if (bGetAccessToken)
                {
                    index = "2";
                    indexA = "2";
                    string result = HttpGet(GetRequestString("access_token"));
                    retA = result;
                    System.Diagnostics.Debug.WriteLine(result);
                    EntityAccessToken eat = DeserializeAccessToken(result);
                    if (string.IsNullOrWhiteSpace(eat.errcode))
                    {
                        index = "3";
                        indexA = "3";
                        _access_token = eat.access_token;
                        _at_expires_in = eat.expires_in;
                        double expires_in = double.Parse(eat.expires_in);
                        _at_timeout_time = DateTime.Now.AddSeconds(expires_in).ToString();
                    }
                    else
                    {
                        index = "3.1";
                        indexA = "3.1";
                        msg = eat.errmsg;
                        System.Diagnostics.Debug.WriteLine(eat.errmsg);
                    }
                }
                #endregion

                #region 获取jsapi_ticket
                index = "20";
                bool bGetTicket = true;
                if (_jt_timeout_time != "")
                {
                    index = "20.5";
                    DateTime dt = DateTime.Parse(_jt_timeout_time);
                    // 现在离超时时间3分钟内才获取jsapi_ticket
                    if (DateTime.Now < dt.AddMinutes(-3))
                    {
                        bGetTicket = false;
                    }
                }

                index = "21";
                if (bGetTicket && _access_token != "")
                {
                    index = "22";
                    string result = HttpGet(GetRequestString("jsapi_ticket"));
                    ret = result;
                    System.Diagnostics.Debug.WriteLine(result);
                    Hashtable ht = DeserializeToHashtable(result);
                    if (ht["errcode"].ToString() == "0")
                    {
                        index = "23";
                        _jsapi_ticket = ht["ticket"].ToString();
                        _jt_expires_in = ht["expires_in"].ToString();
                        double expires_in = double.Parse(_jt_expires_in);
                        _jt_timeout_time = DateTime.Now.AddSeconds(expires_in).ToString();
                    }
                    else
                    {
                        index = "23.2";
                        msg = "errcode:" + ht["errcode"].ToString() + ";errmsg:" + ht["errmsg"].ToString();
                        System.Diagnostics.Debug.WriteLine(ht["errmsg"].ToString());
                    }
                }
                #endregion
            }
            catch (Exception ex)
            {
                index = "999";
                msg = ex.Message;
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
        }

        public static string GetRequestString(string method)
        {
            string url = "";
            switch(method)
            {
                case "access_token":
                    url = string.Format("https://api.weixin.qq.com/cgi-bin/token?grant_type=client_credential&appid={0}&secret={1}", MngConfig.SiteConfig.WxGzhAppId, MngConfig.SiteConfig.WxGzhAppSecret);
                    break;

                case "jsapi_ticket":
                    url = string.Format("https://api.weixin.qq.com/cgi-bin/ticket/getticket?access_token={0}&type=jsapi", _access_token);
                    break;

                default:
                    break;
            }
            
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

        public static Hashtable DeserializeToHashtable(string jsonStr)
        {
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            Hashtable ht = serializer.Deserialize<Hashtable>(jsonStr);
            return ht;
        }
    }

    public class EntityAccessToken
    {
        public string errcode { get; set; }
        public string errmsg { get; set; }
        public string access_token { get; set; }
        public string expires_in { get; set; }
    }
}