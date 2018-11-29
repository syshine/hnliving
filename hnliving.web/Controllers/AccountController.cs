﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Threading.Tasks;


using Lib.Core;
using Lib.Services;
using System.Text;
//using BrnMall.Web.Framework;
using hnliving.web.Models;

namespace hnliving.web.Controllers
{
    public class AccountController : Controller
    {
        // GET: Account
        public ActionResult Index()
        {
            return View();
        }

        // 登陆
        // GET: /Account/Login
        [AllowAnonymous]
        public ActionResult Login()
        {
            string returnUrl = WebHelper.GetQueryString("returnUrl");
            if (returnUrl.Length == 0)
                returnUrl = "/";

            return View();
        }

        // 登陆
        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel model, string returnUrl)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            if (model.Email.Length == 0)
                return View(model);
            else if (model.Email.Length > 10)
                return RedirectToLocal(returnUrl);
            else
                return View(model);

            #region //  登陆
            ////ajax请求
            //string accountName = WebHelper.GetFormString("ShadowName");
            //string password = WebHelper.GetFormString("password");
            //string verifyCode = WebHelper.GetFormString("verifyCode");
            //int isRemember = WebHelper.GetFormInt("isRemember");

            //StringBuilder errorList = new StringBuilder("[");
            ////验证账户名
            //if (string.IsNullOrWhiteSpace(accountName))
            //{
            //    errorList.AppendFormat("{0}\"key\":\"{1}\",\"msg\":\"{2}\"{3},", "{", "accountName", "账户名不能为空", "}");
            //}
            //else if (accountName.Length < 4 || accountName.Length > 50)
            //{
            //    errorList.AppendFormat("{0}\"key\":\"{1}\",\"msg\":\"{2}\"{3},", "{", "accountName", "账户名必须大于3且不大于50个字符", "}");
            //}
            //else if ((!SecureHelper.IsSafeSqlString(accountName, false)))
            //{
            //    errorList.AppendFormat("{0}\"key\":\"{1}\",\"msg\":\"{2}\"{3},", "{", "accountName", "账户名不存在", "}");
            //}

            ////验证密码
            //if (string.IsNullOrWhiteSpace(password))
            //{
            //    errorList.AppendFormat("{0}\"key\":\"{1}\",\"msg\":\"{2}\"{3},", "{", "password", "密码不能为空", "}");
            //}
            //else if (password.Length < 4 || password.Length > 32)
            //{
            //    errorList.AppendFormat("{0}\"key\":\"{1}\",\"msg\":\"{2}\"{3},", "{", "password", "密码必须大于3且不大于32个字符", "}");
            //}

            //////验证验证码
            ////if (CommonHelper.IsInArray(WorkContext.PageKey, WorkContext.MallConfig.VerifyPages))
            ////{
            ////    if (string.IsNullOrWhiteSpace(verifyCode))
            ////    {
            ////        errorList.AppendFormat("{0}\"key\":\"{1}\",\"msg\":\"{2}\"{3},", "{", "verifyCode", "验证码不能为空", "}");
            ////    }
            ////    else if (verifyCode.ToLower() != Sessions.GetValueString(WorkContext.Sid, "verifyCode"))
            ////    {
            ////        errorList.AppendFormat("{0}\"key\":\"{1}\",\"msg\":\"{2}\"{3},", "{", "verifyCode", "验证码不正确", "}");
            ////    }
            ////}

            ////当以上验证全部通过时
            //PartUserInfo partUserInfo = null;
            //if (errorList.Length == 1)
            //{
            //    partUserInfo = Users.GetPartUserByName(accountName);
            //    if (partUserInfo == null)
            //        errorList.AppendFormat("{0}\"key\":\"{1}\",\"msg\":\"{2}\"{3},", "{", "accountName", "用户名不存在", "}");

            //    if (partUserInfo != null)
            //    {
            //        if (Users.CreateUserPassword(password, partUserInfo.Salt) != partUserInfo.Password)//判断密码是否正确
            //        {
            //            //LoginFailLogs.AddLoginFailTimes(WorkContext.IP, DateTime.Now);//增加登录失败次数
            //            errorList.AppendFormat("{0}\"key\":\"{1}\",\"msg\":\"{2}\"{3},", "{", "password", "密码不正确", "}");
            //        }
            //        else if (partUserInfo.UserRid == 1)//当用户等级是禁止访问等级时
            //        {
            //            if (partUserInfo.LiftBanTime > DateTime.Now)//达到解禁时间
            //            {
            //                UserRankInfo userRankInfo = UserRanks.GetUserRankByCredits(partUserInfo.PayCredits);
            //                Users.UpdateUserRankByUid(partUserInfo.Uid, userRankInfo.UserRid);
            //                partUserInfo.UserRid = userRankInfo.UserRid;
            //            }
            //            else
            //            {
            //                errorList.AppendFormat("{0}\"key\":\"{1}\",\"msg\":\"{2}\"{3},", "{", "accountName", "您的账号当前被锁定,不能访问", "}");
            //            }
            //        }
            //    }
            //}

            //if (errorList.Length > 1)//验证失败时
            //{
            //    return View("error", "登录成功");
            //}
            //else//验证成功时
            //{

            //    return View("success", "登录成功");
            //}
            #endregion

            //// 这不会计入到为执行帐户锁定而统计的登录失败次数中
            //// 若要在多次输入错误密码的情况下触发帐户锁定，请更改为 shouldLockout: true
            //var result = await SignInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, shouldLockout: false);
            //switch (result)
            //{
            //    case SignInStatus.Success:
            //        return RedirectToLocal(returnUrl);
            //    case SignInStatus.LockedOut:
            //        return View("Lockout");
            //    case SignInStatus.RequiresVerification:
            //        return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = model.RememberMe });
            //    case SignInStatus.Failure:
            //    default:
            //        ModelState.AddModelError("", "无效的登录尝试。");
            //        return View(model);
            //}
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Home");
        }
    }
}