using System;
using System.Collections.Generic;

using Lib.Core;

namespace hnliving.web
{
    /// <summary>
    /// PC前台工作上下文类
    /// </summary>
    public class WebWorkContext
    {
       public SiteConfigInfo SiteConfig = MngConfig.SiteConfig;//站点配置信息

        public bool IsHttpAjax;//当前请求是否为ajax请求

        public string IP;//用户ip

        public int RegionId;//区域id

        public string Url;//当前url

        public string RawUrl;//当前url信息（不包括主机和端口）

        public string UrlReferrer;//上一次访问的url

        public string Sid;//用户sid

        public int Uid = -1;//用户id

        public string UserName;//用户名

        public string UserEmail;//用户邮箱

        public string UserMobile;//用户手机号

        public string NickName;//用户昵称

        public string Avatar;//用户头像

        public string Password;//用户密码

        public string EncryptPwd;//加密密码

        public List<string> ModulesId;//模块id

        public PartUserInfo PartUserInfo;//用户信息

        public int UserRid = -1;//用户等级id

        public UserRankInfo UserRankInfo;//用户等级信息

        public string UserRTitle;//用户等级标题

        public string Controller;//控制器

        public string Action;//动作方法

        public string PageKey;//页面标示符

        public int OnlineUserCount = 0;//在线总人数

        public int OnlineMemberCount = 0;//在线会员数

        public int OnlineGuestCount = 0;//在线游客数

        public string SearchWord;//搜索词

        public string WebpSuffix = "png"; // webp格式后缀，默认"png"，如果IsSupportWebp为真，则是"webp"
        public string WebpGifSuffix = "gif"; // webp格式后缀，默认"gif"，如果IsSupportWebp为真，则是"webp"

        private bool _isSupportWebp = false;//是否支持webp格式

        public bool IsSupportWebp
        {
            get
            {
                return _isSupportWebp;
            }

            set
            {
                _isSupportWebp = value;
                if (_isSupportWebp)
                {
                    WebpSuffix = "webp";
                    WebpGifSuffix = "webp";
                }
                else
                {
                    WebpSuffix = "png";
                    WebpGifSuffix = "gif";
                }
            }
        }
    }
}