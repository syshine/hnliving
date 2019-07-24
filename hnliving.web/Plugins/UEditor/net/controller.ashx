<%@ WebHandler Language="C#" Class="UEditorHandler" %>

using System;
using System.Web;
using System.IO;
using System.Collections;
using Newtonsoft.Json;

public class UEditorHandler : IHttpHandler
{
    public void ProcessRequest(HttpContext context)
    {
        Handler action = null;
        switch (context.Request["action"])
        {
            case "config":
                action = new ConfigHandler(context);
                break;
            case "uploadimage":
                action = new UploadHandler(context, new UploadConfig()
                {
                    AllowExtensions = BaiduConfig.GetStringList("imageAllowFiles"),
                    PathFormat = BaiduConfig.GetString("imagePathFormat"),
                    SizeLimit = BaiduConfig.GetInt("imageMaxSize"),
                    UploadFieldName = BaiduConfig.GetString("imageFieldName")
                });
                break;
            case "uploadscrawl":
                action = new UploadHandler(context, new UploadConfig()
                {
                    AllowExtensions = new string[] { ".png" },
                    PathFormat = BaiduConfig.GetString("scrawlPathFormat"),
                    SizeLimit = BaiduConfig.GetInt("scrawlMaxSize"),
                    UploadFieldName = BaiduConfig.GetString("scrawlFieldName"),
                    Base64 = true,
                    Base64Filename = "scrawl.png"
                });
                break;
            case "uploadvideo":
                action = new UploadHandler(context, new UploadConfig()
                {
                    AllowExtensions = BaiduConfig.GetStringList("videoAllowFiles"),
                    PathFormat = BaiduConfig.GetString("videoPathFormat"),
                    SizeLimit = BaiduConfig.GetInt("videoMaxSize"),
                    UploadFieldName = BaiduConfig.GetString("videoFieldName")
                });
                break;
            case "uploadfile":
                action = new UploadHandler(context, new UploadConfig()
                {
                    AllowExtensions = BaiduConfig.GetStringList("fileAllowFiles"),
                    PathFormat = BaiduConfig.GetString("filePathFormat"),
                    SizeLimit = BaiduConfig.GetInt("fileMaxSize"),
                    UploadFieldName = BaiduConfig.GetString("fileFieldName")
                });
                break;
            case "listimage":
                action = new ListFileManager(context, BaiduConfig.GetString("imageManagerListPath"), BaiduConfig.GetStringList("imageManagerAllowFiles"));
                break;
            case "listfile":
                action = new ListFileManager(context, BaiduConfig.GetString("fileManagerListPath"), BaiduConfig.GetStringList("fileManagerAllowFiles"));
                break;
            case "catchimage":
                action = new CrawlerHandler(context);
                break;
            default:
                action = new NotSupportedHandler(context);
                break;
        }
        action.Process();
    }

    public bool IsReusable
    {
        get
        {
            return false;
        }
    }
}