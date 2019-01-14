using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;

namespace hnliving.web.WeChat
{
    public class MiniProgram
    {
        /// <summary>
        /// 获取access_token  正常会返回{"access_token": "ACCESS_TOKEN", "expires_in": 7200}
        /// </summary>
        /// <returns></returns>
        public string JsCode2Session()
        {
            string appid = "wx*******";
            string secret = "secret**********";
            string JsCode2SessionUrl = "https://api.weixin.qq.com/cgi-bin/token?grant_type=client_credential&appid={0}&secret={1}";
            var url = string.Format(JsCode2SessionUrl, appid, secret);
            var str = GetFunction(url);
            try
            {
                //JsonData jo = JsonMapper.ToObject(str);
                //string access_token = jo["access_token"].ToString();
                //return access_token;
                return "";
            }
            catch (Exception ex)
            {
                return "";
            }
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public string GetFunction(string url)
        {
            string serviceAddress = url;
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(serviceAddress);
            request.Method = "GET";
            request.ContentType = "textml;charset=UTF-8";
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            Stream myResponseStream = response.GetResponseStream();
            StreamReader myStreamReader = new StreamReader(myResponseStream, System.Text.Encoding.UTF8);
            string retString = myStreamReader.ReadToEnd();
            myStreamReader.Close();
            myResponseStream.Close();
            return retString;
        }
    }
}