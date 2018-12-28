using System;
using System.Web;

using Lib.Core;

namespace Lib.Services
{
    public partial class Utils
    {
        #region  加密/解密

        /// <summary>
        /// AES加密
        /// </summary>
        /// <param name="encryptStr">加密字符串</param>
        public static string AESEncrypt(string encryptStr)
        {
            return SecureHelper.AESEncrypt(encryptStr, MngConfig.SiteConfig.SecretKey);
        }

        /// <summary>
        /// AES解密
        /// </summary>
        /// <param name="decryptStr">解密字符串</param>
        public static string AESDecrypt(string decryptStr)
        {
            return SecureHelper.AESDecrypt(decryptStr, MngConfig.SiteConfig.SecretKey);
        }

        #endregion

        #region Cookie

        /// <summary>
        /// 获得用户sid
        /// </summary>
        /// <returns></returns>
        public static string GetSidCookie()
        {
            return WebHelper.GetCookie("hnlsid");
        }

        /// <summary>
        /// 设置用户sid
        /// </summary>
        public static void SetSidCookie(string sid)
        {
            HttpCookie cookie = HttpContext.Current.Request.Cookies["hnlsid"];
            if (cookie == null)
                cookie = new HttpCookie("hnlsid");

            cookie.Value = sid;
            cookie.Expires = DateTime.Now.AddDays(15);
            string cookieDomain = MngConfig.SiteConfig.CookieDomain;
            if (cookieDomain.Length != 0)
                cookie.Domain = cookieDomain;

            HttpContext.Current.Response.AppendCookie(cookie);
        }

        /// <summary>
        /// 获得用户id
        /// </summary>
        /// <returns></returns>
        public static int GetUidCookie()
        {
            return TypeHelper.StringToInt(GetHnlCookie("uid"), -1);
        }

        /// <summary>
        /// 设置用户id
        /// </summary>
        public static void SetUidCookie(int uid)
        {
            SetHnlCookie("uid", uid.ToString());
        }

        /// <summary>
        /// 获得cookie密码
        /// </summary>
        /// <returns></returns>
        public static string GetCookiePassword()
        {
            return WebHelper.UrlDecode(GetHnlCookie("password"));
        }

        /// <summary>
        /// 解密cookie密码
        /// </summary>
        /// <param name="cookiePassword">cookie密码</param>
        /// <returns></returns>
        public static string DecryptCookiePassword(string cookiePassword)
        {
            return AESDecrypt(cookiePassword).Trim();
        }

        /// <summary>
        /// 设置cookie密码
        /// </summary>
        public static void SetCookiePassword(string password)
        {
            SetHnlCookie("password", WebHelper.UrlEncode(AESEncrypt(password)));
        }

        /// <summary>
        /// 设置用户
        /// </summary>
        /// <param name="uid">用户id</param>
        /// <param name="password">密码</param>
        /// <param name="sid">sid</param>
        /// <param name="expires">过期时间</param>
        public static void SetUserCookie(PartUserInfo partUserInfo, int expires)
        {
            HttpCookie cookie = HttpContext.Current.Request.Cookies["hnl"];
            if (cookie == null)
                cookie = new HttpCookie("hnl");

            cookie.Values["uid"] = partUserInfo.Uid.ToString();
            cookie.Values["password"] = WebHelper.UrlEncode(AESEncrypt(partUserInfo.Password));
            if (expires > 0)
            {
                cookie.Values["expires"] = expires.ToString();
                cookie.Expires = DateTime.Now.AddDays(expires);
            }
            string cookieDomain = MngConfig.SiteConfig.CookieDomain;
            if (cookieDomain.Length != 0)
                cookie.Domain = cookieDomain;

            HttpContext.Current.Response.AppendCookie(cookie);
        }

        /// <summary>
        /// 获得cookie
        /// </summary>
        /// <param name="key">键</param>
        /// <returns></returns>
        public static string GetHnlCookie(string key)
        {
            return WebHelper.GetCookie("hnl", key);
        }

        /// <summary>
        /// 设置cookie
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        public static void SetHnlCookie(string key, string value)
        {
            HttpCookie cookie = HttpContext.Current.Request.Cookies["hnl"];
            if (cookie == null)
                cookie = new HttpCookie("hnl");

            cookie[key] = value;

            int expires = TypeHelper.StringToInt(cookie.Values["expires"]);
            if (expires > 0)
                cookie.Expires = DateTime.Now.AddDays(expires);

            string cookieDomain = MngConfig.SiteConfig.CookieDomain;
            if (cookieDomain.Length != 0)
                cookie.Domain = cookieDomain;

            HttpContext.Current.Response.AppendCookie(cookie);
        }

        #endregion
    }
}
