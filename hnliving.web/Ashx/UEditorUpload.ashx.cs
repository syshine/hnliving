using Lib.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace hnliving.web.Ashx
{
    /// <summary>
    /// UEditorUpload 的摘要说明
    /// </summary>
    public class UEditorUpload : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            string result = "UEditorUpload";
            string action = "";

            if ("POST" == context.Request.HttpMethod) //(context.Request.Params["method"] == "post")
            {
                try
                {
                    Console.WriteLine("content:" + context.Request.Form["content"]);
                    Console.WriteLine("type:" + context.Request.Form["type"]);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }

                string operation = WebHelper.GetQueryString("action");
                //string result = "";
                switch (operation)
                {
                    case "uploadimage":
                        HttpPostedFile file = context.Request.Files[0];
                        System.Diagnostics.Debug.WriteLine(file.FileName);
                        //result = "{"
                        //    + "\"state\": \"SUCCESS\","
                        //    + "\"original\": \"" + file.FileName + "\","
                        //    + "\"size\": \""+ file.ContentLength + "\","
                        //    + "\"title\": \""+ file.FileName + "\","
                        //    + "\"type\": \".jpg\","
                        //    + "\"url\": \"/ueditor/asp/upload/image/20190715/1465731377326075274.jpg\""
                        //    + "}";
                        result = "{"
                            + "'state': 'SUCCESS',"
                            + "'original': '" + file.FileName + "',"
                            + "'size': '" + file.ContentLength + "',"
                            + "'title': '" + file.FileName + "',"
                            + "'type': '.jpg',"
                            + "'url': '/ueditor/asp/upload/image/20190715/1465731377326075274.jpg'"
                            + "}";
                        //"上传成功";// Uploads.SaveUploadStoreRankAvatar(file);
                        context.Response.Write(result);
                        break;

                    default:
                        context.Response.Write("失败");
                        break;
                }
                return;
            }
            else if ("GET" == context.Request.HttpMethod)
            {
                StringBuilder sbQuery = new StringBuilder();
                foreach (string key in context.Request.QueryString.Keys)
                {
                    sbQuery.Append("key=" + key + ",value=" + context.Request.QueryString[key].ToString());
                }
                string strQuery = sbQuery.ToString();
                if (!string.IsNullOrEmpty(strQuery))
                {
                    result += "\r\n QueryString:" + strQuery;
                }

                if(context.Request.QueryString["action"].ToString() == "config")
                {
                    string config = "{ "
                                    + " \"imageActionName\": \"uploadimage\","
                                    + " \"imageUrl\": \"http://localhost/tools/program/ueditor/uploadimage?action=uploadimage\","
                                    + " \"imagePath\": \"/ueditor/asp/\","
                                    + " \"imageFieldName\": \"upfile\","
                                    + " \"imageMaxSize\": 2048,"
                                    + " \"imageAllowFiles\": [\".png\", \".jpg\", \".jpeg\", \".gif\", \".bmp\"]"
                                    + "}";

                    System.Diagnostics.Debug.WriteLine(config);
                    context.Response.Write(config);
                    return;
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