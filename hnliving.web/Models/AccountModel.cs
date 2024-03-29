﻿using System;
using System.Collections.Generic;

using Lib.Core;

namespace hnliving.web.Models
{
    /// <summary>
    /// 登录模型类
    /// </summary>
    public class LoginModel
    {
        /// <summary>
        /// 返回地址
        /// </summary>
        public string ReturnUrl { get; set; }
        /// <summary>
        /// 影子账号名
        /// </summary>
        public string ShadowName { get; set; }
        /// <summary>
        /// 是否允许记住用户
        /// </summary>
        public bool IsRemember { get; set; }
        /// <summary>
        /// 是否启用验证码
        /// </summary>
        public bool IsVerifyCode { get; set; }
        /// <summary>
        /// 开放授权插件
        /// </summary>
        //public List<PluginInfo> OAuthPluginList { get; set; }
    }

    /// <summary>
    /// 注册模型类
    /// </summary>
    public class RegisterModel
    {
        /// <summary>
        /// 返回地址
        /// </summary>
        public string ReturnUrl { get; set; }
        /// <summary>
        /// 影子账号名
        /// </summary>
        public string ShadowName { get; set; }
        /// <summary>
        /// 是否启用验证码
        /// </summary>
        public bool IsVerifyCode { get; set; }
    }

    /// <summary>
    /// 找回密码模型类
    /// </summary>
    public class FindPwdModel
    {
        /// <summary>
        /// 影子账号名
        /// </summary>
        public string ShadowName { get; set; }
        /// <summary>
        /// 是否启用验证码
        /// </summary>
        public bool IsVerifyCode { get; set; }
    }

    /// <summary>
    /// 选择找回密码方式模型类
    /// </summary>
    public class SelectFindPwdTypeModel
    {
        public PartUserInfo PartUserInfo { get; set; }
    }

    /// <summary>
    /// 重置密码模型类
    /// </summary>
    public class ResetPwdModel
    {
        public string V { get; set; }
    }


    /// <summary>
    /// 用户信息模型类
    /// </summary>
    public class UserInfoModel
    {
        /// <summary>
        /// 用户信息
        /// </summary>
        public UserInfo UserInfo { get; set; }
        /// <summary>
        /// 用户等级信息
        /// </summary>
        public UserRankInfo UserRankInfo { get; set; }
    }
}