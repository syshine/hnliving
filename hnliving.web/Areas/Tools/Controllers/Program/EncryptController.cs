using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Lib.Core;

namespace hnliving.web.Areas.Tools.Controllers.Program
{
    public class EncryptController : BaseWebController
    {
        // GET: Tools/Program/Encrypt
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// DES页面
        /// </summary>
        /// <returns></returns>
        public ActionResult DES()
        {
            return View();
        }


        /// <summary>
        /// DES加密
        /// </summary>
        /// <param name="content">内容</param>
        /// <param name="key"></param>
        /// <param name="iv"></param>
        /// <returns></returns>
        public ActionResult DesEncrypt(string content, string key, string iv = "")
        {
            key = key.PadRight(8, '0');
            if (iv == "")
                iv = key;

            string destString = "";
            string retMsg = EncryptHelper.DesEncrypt(out destString, content, key, iv);

            if (retMsg != "")
                return Content(retMsg);
            else
                return Content(destString);
        }

        /// <summary>
        /// DES解密
        /// </summary>
        /// <param name="content">内容</param>
        /// <param name="key"></param>
        /// <param name="iv"></param>
        /// <returns></returns>
        public ActionResult DesDecrypt(string content, string key, string iv = "")
        {
            key = key.PadRight(8, '0');
            if (iv == "")
                iv = key;

            string destString = "";
            string retMsg = EncryptHelper.DesDecrypt(out destString, content, key, iv);

            if (retMsg != "")
                return Content(retMsg);
            else
                return Content(destString);
        }


        /// <summary>
        /// MD5页面
        /// </summary>
        /// <returns></returns>
        public ActionResult MD5()
        {
            return View();
        }
        
        /// <summary>
        /// MD5加密
        /// </summary>
        /// <param name="content">内容</param>
        /// <param name="flag">16位加密：0;32位加密：1</param>
        /// <returns></returns>
        public ActionResult Md5Encrypt(string content, string flag = "0")
        {
            string destString = "";
            string retMsg = "";
            if (flag == "0")
                retMsg = EncryptHelper.Md5Encrypt16(out destString, content);
            else
                retMsg = EncryptHelper.Md5Encrypt32(out destString, content);

            if (retMsg != "")
                return Content(retMsg);
            else
                return Content(destString);
        }


        /// <summary>
        /// Xor页面
        /// </summary>
        /// <returns></returns>
        public ActionResult Xor()
        {
            return View();
        }

        /// <summary>
        /// 异或加密
        /// </summary>
        /// <param name="content">内容</param>
        /// <param name="key">异或键</param>
        /// <returns></returns>
        public ActionResult XorEncrypt(string content, string key)
        {
            string destString = "";
            string retMsg = EncryptHelper.XorEncrypt(out destString, content, int.Parse(key));
            
            if (retMsg != "")
                return Content(retMsg);
            else
                return Content(destString);
        }
    }
}