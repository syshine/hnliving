using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;

namespace hnliving.web.Ashx
{
    /// <summary>
    /// TestAshx 的摘要说明
    /// </summary>
    public class TestAshx : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            string result = "Hello World Ashx";
            
            //post
            if ("POST" == context.Request.HttpMethod)
            {
                StringBuilder sbForm = new StringBuilder();
                foreach (string key in context.Request.Form.Keys)
                {
                    sbForm.Append("\r\n key=" + key + ",value=" + context.Request.Form[key].ToString());
                }
                string strForm = sbForm.ToString();
                if (!string.IsNullOrEmpty(strForm))
                {
                    result += "Form:" + strForm;
                }
            } else if ("GET" == context.Request.HttpMethod)
            {
                StringBuilder sbQuery = new StringBuilder();
                foreach (string key in context.Request.QueryString.Keys)
                {
                    sbQuery.Append("\r\n key=" + key + ",value=" + context.Request.QueryString[key].ToString());
                }
                string strQuery = sbQuery.ToString();
                if (!string.IsNullOrEmpty(strQuery))
                {
                    result += "QueryString:" + strQuery;
                }
            }

            System.Diagnostics.Debug.WriteLine(result);
            context.Response.Write(result);
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}