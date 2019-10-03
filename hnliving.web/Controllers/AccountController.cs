using System;
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
    public class AccountController : BaseWebController
    {
        // GET: Account
        public ActionResult Index()
        {
            return View();
        }

        // 登陆
        // /Account/Login
        [AllowAnonymous]
        //[ValidateAntiForgeryToken]
        public ActionResult Login()
        {
            string returnUrl = WebHelper.GetQueryString("returnUrl");
            if (returnUrl.Length == 0)
                returnUrl = "/";

            if (WorkContext.Uid > 0)
                return PromptView(returnUrl, "您已经登录，无须重复登录!");

            //get请求
            if (WebHelper.IsGet())
            {
                LoginModel model = new LoginModel();

                model.ReturnUrl = returnUrl;
                model.ShadowName = WorkContext.SiteConfig.ShadowName;
                model.IsRemember = WorkContext.SiteConfig.IsRemember == 1;
                //model.IsVerifyCode = CommonHelper.IsInArray(WorkContext.PageKey, WorkContext.SiteConfig.VerifyPages);
                //model.OAuthPluginList = Plugins.GetOAuthPluginList();

                return View(model);
            }

            #region 登陆
            ////ajax请求
            string accountName = WebHelper.GetFormString(WorkContext.SiteConfig.ShadowName);
            string password = WebHelper.GetFormString("password");
            string verifyCode = WebHelper.GetFormString("verifyCode");
            int isRemember = WebHelper.GetFormInt("isRemember");

            StringBuilder errorList = new StringBuilder("[");
            //验证账户名
            if (string.IsNullOrWhiteSpace(accountName))
            {
                errorList.AppendFormat("{0}\"key\":\"{1}\",\"msg\":\"{2}\"{3},", "{", "accountName", "账户名不能为空", "}");
            }
            else if (accountName.Length < 4 || accountName.Length > 50)
            {
                errorList.AppendFormat("{0}\"key\":\"{1}\",\"msg\":\"{2}\"{3},", "{", "accountName", "账户名必须大于3且不大于50个字符", "}");
            }
            else if ((!SecureHelper.IsSafeSqlString(accountName, false)))
            {
                errorList.AppendFormat("{0}\"key\":\"{1}\",\"msg\":\"{2}\"{3},", "{", "accountName", "账户名不存在", "}");
            }

            //验证密码
            if (string.IsNullOrWhiteSpace(password))
            {
                errorList.AppendFormat("{0}\"key\":\"{1}\",\"msg\":\"{2}\"{3},", "{", "password", "密码不能为空", "}");
            }
            else if (password.Length < 6 || password.Length > 32)
            {
                errorList.AppendFormat("{0}\"key\":\"{1}\",\"msg\":\"{2}\"{3},", "{", "password", "密码必须大于5且不大于32个字符", "}");
            }

            //验证验证码
            if (CommonHelper.IsInArray(WorkContext.PageKey, WorkContext.SiteConfig.VerifyPages))
            {
                if (string.IsNullOrWhiteSpace(verifyCode))
                {
                    errorList.AppendFormat("{0}\"key\":\"{1}\",\"msg\":\"{2}\"{3},", "{", "verifyCode", "验证码不能为空", "}");
                }
                else if (verifyCode.ToLower() != Sessions.GetValueString(WorkContext.Sid, "verifyCode"))
                {
                    errorList.AppendFormat("{0}\"key\":\"{1}\",\"msg\":\"{2}\"{3},", "{", "verifyCode", "验证码不正确", "}");
                }
            }

            //当以上验证全部通过时
            PartUserInfo partUserInfo = null;
            if (errorList.Length == 1)
            {
                partUserInfo = Users.GetPartUserByName(accountName);
                if (partUserInfo == null)
                    errorList.AppendFormat("{0}\"key\":\"{1}\",\"msg\":\"{2}\"{3},", "{", "accountName", "用户名不存在", "}");

                if (partUserInfo != null)
                {
                    if (Users.CreateUserPassword(password, partUserInfo.Salt) != partUserInfo.Password)//判断密码是否正确
                    {
                        //LoginFailLogs.AddLoginFailTimes(WorkContext.IP, DateTime.Now);//增加登录失败次数
                        errorList.AppendFormat("{0}\"key\":\"{1}\",\"msg\":\"{2}\"{3},", "{", "password", "密码不正确", "}");
                    }
                    else if (partUserInfo.UserRid == 1)//当用户等级是禁止访问等级时
                    {
                        if (partUserInfo.LiftBanTime > DateTime.Now)//达到解禁时间
                        {
                            //UserRankInfo userRankInfo = UserRanks.GetUserRankByCredits(partUserInfo.PayCredits);
                            //Users.UpdateUserRankByUid(partUserInfo.Uid, userRankInfo.UserRid);
                            //partUserInfo.UserRid = userRankInfo.UserRid;
                        }
                        else
                        {
                            errorList.AppendFormat("{0}\"key\":\"{1}\",\"msg\":\"{2}\"{3},", "{", "accountName", "您的账号当前被锁定,不能访问", "}");
                        }
                    }
                }
            }

            if (/*accountName == "admin@qq.com" && password == "admin" && */errorList.Length <= 1)
            {
                //将用户信息写入cookie中
                Utils.SetUserCookie(partUserInfo, (WorkContext.SiteConfig.IsRemember == 1 && isRemember == 1) ? 30 : -1);

                return AjaxResult("success", "登录成功"); //RedirectToLocal(returnUrl);
            }
            else
            {
                return AjaxResult("error", errorList.Remove(errorList.Length - 1, 1).Append("]").ToString(), true);
            }

            //if (errorList.Length > 1)//验证失败时
            //{
            //    return AjaxResult("error", "登录失败");
            //}
            //else//验证成功时
            //{

            //    return AjaxResult("success", "登录成功");
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

        /// <summary>
        /// 退出
        /// </summary>
        public ActionResult Logout()
        {
            if (WorkContext.Uid > 0)
            {
                WebHelper.DeleteCookie("hnl");
                Sessions.RemoverSession(WorkContext.Sid);
            }
            return Redirect("/");
        }



        /// <summary>
        /// 注册
        /// </summary>
        public ActionResult Register()
        {
            string returnUrl = WebHelper.GetQueryString("returnUrl");
            if (returnUrl.Length == 0)
                returnUrl = "/";

            if (WorkContext.SiteConfig.RegType.Length == 0)
                return PromptView(returnUrl, "网站目前已经关闭注册功能!");
            if (WorkContext.Uid > 0)
                return PromptView(returnUrl, "你已经是网站的注册用户，无需再注册!");
            if (WorkContext.SiteConfig.RegTimeSpan > 0)
            {
                DateTime registerTime = Users.GetRegisterTimeByRegisterIP(WorkContext.IP);
                if ((DateTime.Now - registerTime).Minutes <= WorkContext.SiteConfig.RegTimeSpan)
                    return PromptView(returnUrl, "你注册太频繁，请间隔一定时间后再注册!");
            }

            //get请求
            if (WebHelper.IsGet())
            {
                RegisterModel model = new RegisterModel();

                model.ReturnUrl = returnUrl;
                model.ShadowName = WorkContext.SiteConfig.ShadowName;
                model.IsVerifyCode = CommonHelper.IsInArray(WorkContext.PageKey, WorkContext.SiteConfig.VerifyPages);

                return View(model);
            }

            //ajax请求
            string accountName = WebHelper.GetFormString(WorkContext.SiteConfig.ShadowName).Trim().ToLower();
            string password = WebHelper.GetFormString("password");
            string confirmPwd = WebHelper.GetFormString("confirmPwd");
            string verifyCode = WebHelper.GetFormString("verifyCode");
            string pwdQuestion = WebHelper.GetFormString("pwdQuestion");
            string pwdAnswer = WebHelper.GetFormString("pwdAnswer");

            StringBuilder errorList = new StringBuilder("[");
            #region 验证

            //账号验证
            if (string.IsNullOrWhiteSpace(accountName))
            {
                errorList.AppendFormat("{0}\"key\":\"{1}\",\"msg\":\"{2}\"{3},", "{", "accountName", "账户名不能为空", "}");
            }
            else if (accountName.Length < 4 || accountName.Length > 50)
            {
                errorList.AppendFormat("{0}\"key\":\"{1}\",\"msg\":\"{2}\"{3},", "{", "accountName", "账户名必须大于3且不大于50个字符", "}");
            }
            else if (accountName.Contains(" "))
            {
                errorList.AppendFormat("{0}\"key\":\"{1}\",\"msg\":\"{2}\"{3},", "{", "accountName", "账户名中不允许包含空格", "}");
            }
            else if (accountName.Contains(":"))
            {
                errorList.AppendFormat("{0}\"key\":\"{1}\",\"msg\":\"{2}\"{3},", "{", "accountName", "账户名中不允许包含冒号", "}");
            }
            else if (accountName.Contains("<"))
            {
                errorList.AppendFormat("{0}\"key\":\"{1}\",\"msg\":\"{2}\"{3},", "{", "accountName", "账户名中不允许包含'<'符号", "}");
            }
            else if (accountName.Contains(">"))
            {
                errorList.AppendFormat("{0}\"key\":\"{1}\",\"msg\":\"{2}\"{3},", "{", "accountName", "账户名中不允许包含'>'符号", "}");
            }
            else if ((!SecureHelper.IsSafeSqlString(accountName, false)))
            {
                errorList.AppendFormat("{0}\"key\":\"{1}\",\"msg\":\"{2}\"{3},", "{", "accountName", "账户名不符合系统要求", "}");
            }
            else if (CommonHelper.IsInArray(accountName, WorkContext.SiteConfig.ReservedName, "\n"))
            {
                errorList.AppendFormat("{0}\"key\":\"{1}\",\"msg\":\"{2}\"{3},", "{", "accountName", "此账户名不允许被注册", "}");
            }
            else if (FilterWords.IsContainWords(accountName))
            {
                errorList.AppendFormat("{0}\"key\":\"{1}\",\"msg\":\"{2}\"{3},", "{", "accountName", "账户名包含禁止单词", "}");
            }

            //密码验证
            if (string.IsNullOrWhiteSpace(password))
            {
                errorList.AppendFormat("{0}\"key\":\"{1}\",\"msg\":\"{2}\"{3},", "{", "password", "密码不能为空", "}");
            }
            else if (password.Length < 6 || password.Length > 32)
            {
                errorList.AppendFormat("{0}\"key\":\"{1}\",\"msg\":\"{2}\"{3},", "{", "password", "密码必须大于5且不大于32个字符", "}");
            }
            else if (password != confirmPwd)
            {
                errorList.AppendFormat("{0}\"key\":\"{1}\",\"msg\":\"{2}\"{3},", "{", "password", "两次输入的密码不一样", "}");
            }

            //密码提示问题验证
            if (string.IsNullOrWhiteSpace(pwdQuestion))
            {
                errorList.AppendFormat("{0}\"key\":\"{1}\",\"msg\":\"{2}\"{3},", "{", "pwdQuestion", "密码提示问题不能为空", "}");
            }
            else if (pwdQuestion.Length < 6 || pwdQuestion.Length > 30)
            {
                errorList.AppendFormat("{0}\"key\":\"{1}\",\"msg\":\"{2}\"{3},", "{", "pwdQuestion", "密码提示问题必须大于5且不大于30个字符", "}");
            }

            //密码提示答案验证
            if (string.IsNullOrWhiteSpace(pwdAnswer))
            {
                errorList.AppendFormat("{0}\"key\":\"{1}\",\"msg\":\"{2}\"{3},", "{", "pwdAnswer", "密码提示答案不能为空", "}");
            }
            else if (pwdAnswer.Length < 6 || pwdAnswer.Length > 30)
            {
                errorList.AppendFormat("{0}\"key\":\"{1}\",\"msg\":\"{2}\"{3},", "{", "pwdAnswer", "密码提示答案必须大于5且不大于30个字符", "}");
            }

            //验证码验证
            if (CommonHelper.IsInArray(WorkContext.PageKey, WorkContext.SiteConfig.VerifyPages))
            {
                if (string.IsNullOrWhiteSpace(verifyCode))
                {
                    errorList.AppendFormat("{0}\"key\":\"{1}\",\"msg\":\"{2}\"{3},", "{", "verifyCode", "验证码不能为空", "}");
                }
                else if (verifyCode.ToLower() != Sessions.GetValueString(WorkContext.Sid, "verifyCode"))
                {
                    errorList.AppendFormat("{0}\"key\":\"{1}\",\"msg\":\"{2}\"{3},", "{", "verifyCode", "验证码不正确", "}");
                }
            }

            //其它验证
            int gender = WebHelper.GetFormInt("gender");
            if (gender < 0 || gender > 2)
                errorList.AppendFormat("{0}\"key\":\"{1}\",\"msg\":\"{2}\"{3},", "{", "gender", "请选择正确的性别", "}");

            string nickName = WebHelper.GetFormString("nickName");
            if (nickName.Length > 10)
            {
                errorList.AppendFormat("{0}\"key\":\"{1}\",\"msg\":\"{2}\"{3},", "{", "nickName", "昵称的长度不能大于10", "}");
            }
            else if (FilterWords.IsContainWords(nickName))
            {
                errorList.AppendFormat("{0}\"key\":\"{1}\",\"msg\":\"{2}\"{3},", "{", "nickName", "昵称中包含禁止单词", "}");
            }

            if (WebHelper.GetFormString("realName").Length > 5)
            {
                errorList.AppendFormat("{0}\"key\":\"{1}\",\"msg\":\"{2}\"{3},", "{", "realName", "真实姓名的长度不能大于5", "}");
            }

            string bday = WebHelper.GetFormString("bday");
            if (bday.Length == 0)
            {
                string bdayY = WebHelper.GetFormString("bdayY");
                string bdayM = WebHelper.GetFormString("bdayM");
                string bdayD = WebHelper.GetFormString("bdayD");
                bday = string.Format("{0}-{1}-{2}", bdayY, bdayM, bdayD);
            }
            if (bday.Length > 0 && bday != "--" && !ValidateHelper.IsDate(bday))
                errorList.AppendFormat("{0}\"key\":\"{1}\",\"msg\":\"{2}\"{3},", "{", "bday", "请选择正确的日期", "}");

            string idCard = WebHelper.GetFormString("idCard");
            if (idCard.Length > 0 && !ValidateHelper.IsIdCard(idCard))
            {
                errorList.AppendFormat("{0}\"key\":\"{1}\",\"msg\":\"{2}\"{3},", "{", "idCard", "请输入正确的身份证号", "}");
            }

            //int regionId = WebHelper.GetFormInt("regionId");
            //if (regionId > 0)
            //{
            //    if (Regions.GetRegionById(regionId) == null)
            //        errorList.AppendFormat("{0}\"key\":\"{1}\",\"msg\":\"{2}\"{3},", "{", "regionId", "请选择正确的地址", "}");
            //    if (WebHelper.GetFormString("address").Length > 75)
            //    {
            //        errorList.AppendFormat("{0}\"key\":\"{1}\",\"msg\":\"{2}\"{3},", "{", "address", "详细地址的长度不能大于75", "}");
            //    }
            //}

            if (WebHelper.GetFormString("bio").Length > 150)
            {
                errorList.AppendFormat("{0}\"key\":\"{1}\",\"msg\":\"{2}\"{3},", "{", "bio", "简介的长度不能大于150", "}");
            }

            //当以上验证都通过时
            UserInfo userInfo = null;
            if (errorList.Length == 1)
            {
                //if (ValidateHelper.IsEmail(accountName))//验证邮箱
                //{
                //    if (!WorkContext.SiteConfig.RegType.Contains("2"))
                //    {
                //        errorList.AppendFormat("{0}\"key\":\"{1}\",\"msg\":\"{2}\"{3},", "{", "accountName", "不能使用邮箱注册", "}");
                //    }
                //    else
                //    {
                //        string emailProvider = CommonHelper.GetEmailProvider(accountName);
                //        if (WorkContext.SiteConfig.AllowEmailProvider.Length != 0 && (!CommonHelper.IsInArray(emailProvider, WorkContext.SiteConfig.AllowEmailProvider, "\n")))
                //        {
                //            errorList.AppendFormat("{0}\"key\":\"{1}\",\"msg\":\"{2}\"{3},", "{", "accountName", "不能使用'" + emailProvider + "'类型的邮箱", "}");
                //        }
                //        else if (CommonHelper.IsInArray(emailProvider, WorkContext.SiteConfig.BanEmailProvider, "\n"))
                //        {
                //            errorList.AppendFormat("{0}\"key\":\"{1}\",\"msg\":\"{2}\"{3},", "{", "accountName", "不能使用'" + emailProvider + "'类型的邮箱", "}");
                //        }
                //        else if (Users.IsExistEmail(accountName))
                //        {
                //            errorList.AppendFormat("{0}\"key\":\"{1}\",\"msg\":\"{2}\"{3},", "{", "accountName", "邮箱已经存在", "}");
                //        }
                //        else
                //        {
                //            userInfo = new UserInfo();
                //            userInfo.UserName = string.Empty;
                //            userInfo.Email = accountName;
                //            userInfo.Mobile = string.Empty;
                //        }
                //    }
                //}
                //else if (ValidateHelper.IsMobile(accountName))//验证手机
                //{
                //    if (!WorkContext.SiteConfig.RegType.Contains("3"))
                //    {
                //        errorList.AppendFormat("{0}\"key\":\"{1}\",\"msg\":\"{2}\"{3},", "{", "accountName", "不能使用手机注册", "}");
                //    }
                //    else if (Users.IsExistMobile(accountName))
                //    {
                //        errorList.AppendFormat("{0}\"key\":\"{1}\",\"msg\":\"{2}\"{3},", "{", "accountName", "手机号已经存在", "}");
                //    }
                //    else
                //    {
                //        userInfo = new UserInfo();
                //        userInfo.UserName = string.Empty;
                //        userInfo.Email = string.Empty;
                //        userInfo.Mobile = accountName;
                //    }
                //}
                //else//验证用户名
                {
                    if (!WorkContext.SiteConfig.RegType.Contains("1"))
                    {
                        errorList.AppendFormat("{0}\"key\":\"{1}\",\"msg\":\"{2}\"{3},", "{", "accountName", "不能使用用户名注册", "}");
                    }
                    else if (accountName.Length > 20)
                    {
                        errorList.AppendFormat("{0}\"key\":\"{1}\",\"msg\":\"{2}\"{3},", "{", "accountName", "用户名长度不能超过20个字符", "}");
                    }
                    else if (Lib.Services.Users.IsExistUserName(accountName))
                    {
                        errorList.AppendFormat("{0}\"key\":\"{1}\",\"msg\":\"{2}\"{3},", "{", "accountName", "用户名已经存在", "}");
                    }
                    else
                    {
                        userInfo = new UserInfo();
                        userInfo.UserName = accountName;
                        userInfo.Email = string.Empty;
                        userInfo.Mobile = string.Empty;
                    }
                }
            }

            #endregion

            if (errorList.Length > 1)//验证失败
            {
                return AjaxResult("error", errorList.Remove(errorList.Length - 1, 1).Append("]").ToString(), true);
            }
            else//验证成功
            {
                #region 绑定用户信息

                userInfo.Salt = Randoms.CreateRandomValue(6);
                userInfo.Password = Users.CreateUserPassword(password, userInfo.Salt);
                userInfo.UserRid = UserRanks.GetLowestUserRank().UserRid;
                if (nickName.Length > 0)
                    userInfo.NickName = WebHelper.HtmlEncode(nickName);
                else
                    userInfo.NickName = "hn" + Randoms.CreateRandomValue(7);
                userInfo.Avatar = "";
                userInfo.RankCredits = 0;
                userInfo.VerifyEmail = 0;
                userInfo.VerifyMobile = 0;
                userInfo.Modules_id = "";
                userInfo.Pwdquestion = WebHelper.HtmlEncode(pwdQuestion);
                userInfo.Pwdanswer = WebHelper.HtmlEncode(pwdAnswer);

                userInfo.LastVisitIP = WorkContext.IP;
                userInfo.LastVisitRgId = WorkContext.RegionId;
                userInfo.LastVisitTime = DateTime.Now;
                userInfo.RegisterIP = WorkContext.IP;
                userInfo.RegisterRgId = WorkContext.RegionId;
                userInfo.RegisterTime = DateTime.Now;

                userInfo.Gender = WebHelper.GetFormInt("gender");
                userInfo.RealName = WebHelper.HtmlEncode(WebHelper.GetFormString("realName"));
                userInfo.Bday = bday.Length > 0 ? TypeHelper.StringToDateTime(bday) : new DateTime(1900, 1, 1);
                userInfo.IdCard = WebHelper.GetFormString("idCard");
                userInfo.RegionId = WebHelper.GetFormInt("regionId");
                userInfo.Address = WebHelper.HtmlEncode(WebHelper.GetFormString("address"));
                userInfo.Bio = WebHelper.HtmlEncode(WebHelper.GetFormString("bio"));

                #endregion

                //创建用户
                userInfo.Uid = Users.CreateUser(userInfo);

                //添加用户失败
                if (userInfo.Uid < 1)
                    return AjaxResult("exception", "创建用户失败,请联系管理员");
                
                //将用户信息写入cookie
                Utils.SetUserCookie(userInfo, 0);

                //同步上下文
                WorkContext.Uid = userInfo.Uid;
                WorkContext.UserName = userInfo.UserName;
                WorkContext.UserEmail = userInfo.Email;
                WorkContext.UserMobile = userInfo.Mobile;
                WorkContext.NickName = userInfo.NickName;

                return AjaxResult("success", "注册成功");
            }
        }



        #region 用户信息

        /// <summary>
        /// 用户信息
        /// </summary>
        public ActionResult UserInfo()
        {
            UserInfoModel model = new UserInfoModel();

            model.UserInfo = Users.GetUserById(WorkContext.Uid);
            model.UserRankInfo = WorkContext.UserRankInfo;

            return View(model);
        }

        /// <summary>
        /// 编辑用户信息
        /// </summary>
        public ActionResult EditUser()
        {
            string userName = WebHelper.GetFormString("userName");
            string nickName = WebHelper.GetFormString("nickName");
            string avatar = WebHelper.GetFormString("avatar");
            string realName = WebHelper.GetFormString("realName");
            int gender = WebHelper.GetFormInt("gender");
            string idCard = WebHelper.GetFormString("idCard");
            string bday = WebHelper.GetFormString("bday");
            int regionId = WebHelper.GetFormInt("regionId");
            string address = WebHelper.GetFormString("address");
            string bio = WebHelper.GetFormString("bio");

            StringBuilder errorList = new StringBuilder("[");
            //验证用户名
            if (WorkContext.UserName.Length == 0 && userName.Length > 0)
            {
                if (userName.Length < 4 || userName.Length > 10)
                {
                    errorList.AppendFormat("{0}\"key\":\"{1}\",\"msg\":\"{2}\"{3},", "{", "userName", "用户名必须大于3且不大于10个字符", "}");
                }
                else if (userName.Contains(" "))
                {
                    errorList.AppendFormat("{0}\"key\":\"{1}\",\"msg\":\"{2}\"{3},", "{", "userName", "用户名中不允许包含空格", "}");
                }
                else if (userName.Contains(":"))
                {
                    errorList.AppendFormat("{0}\"key\":\"{1}\",\"msg\":\"{2}\"{3},", "{", "userName", "用户名中不允许包含冒号", "}");
                }
                else if (userName.Contains("<"))
                {
                    errorList.AppendFormat("{0}\"key\":\"{1}\",\"msg\":\"{2}\"{3},", "{", "userName", "用户名中不允许包含'<'符号", "}");
                }
                else if (userName.Contains(">"))
                {
                    errorList.AppendFormat("{0}\"key\":\"{1}\",\"msg\":\"{2}\"{3},", "{", "userName", "用户名中不允许包含'>'符号", "}");
                }
                else if ((!SecureHelper.IsSafeSqlString(userName)))
                {
                    errorList.AppendFormat("{0}\"key\":\"{1}\",\"msg\":\"{2}\"{3},", "{", "userName", "账户名不符合系统要求", "}");
                }
                else if (CommonHelper.IsInArray(userName, WorkContext.SiteConfig.ReservedName, "\n"))
                {
                    errorList.AppendFormat("{0}\"key\":\"{1}\",\"msg\":\"{2}\"{3},", "{", "userName", "用户名已经存在", "}");
                }
                else if (FilterWords.IsContainWords(userName))
                {
                    errorList.AppendFormat("{0}\"key\":\"{1}\",\"msg\":\"{2}\"{3},", "{", "userName", "用户名包含禁止单词", "}");
                }
                else if (Users.IsExistUserName(userName))
                {
                    errorList.AppendFormat("{0}\"key\":\"{1}\",\"msg\":\"{2}\"{3},", "{", "userName", "用户名已经存在", "}");
                }
            }
            else
            {
                userName = WorkContext.UserName;
            }

            //验证昵称
            if (nickName.Length > 10)
            {
                errorList.AppendFormat("{0}\"key\":\"{1}\",\"msg\":\"{2}\"{3},", "{", "nickName", "昵称的长度不能大于10", "}");
            }
            else if (FilterWords.IsContainWords(nickName))
            {
                errorList.AppendFormat("{0}\"key\":\"{1}\",\"msg\":\"{2}\"{3},", "{", "nickName", "昵称中包含禁止单词", "}");
            }

            //验证真实姓名
            if (realName.Length > 5)
            {
                errorList.AppendFormat("{0}\"key\":\"{1}\",\"msg\":\"{2}\"{3},", "{", "realName", "真实姓名的长度不能大于5", "}");
            }

            //验证性别
            if (gender < 0 || gender > 2)
                errorList.AppendFormat("{0}\"key\":\"{1}\",\"msg\":\"{2}\"{3},", "{", "gender", "请选择正确的性别", "}");

            //验证身份证号
            if (idCard.Length > 0 && !ValidateHelper.IsIdCard(idCard))
            {
                errorList.AppendFormat("{0}\"key\":\"{1}\",\"msg\":\"{2}\"{3},", "{", "idCard", "请输入正确的身份证号", "}");
            }

            //验证出生日期
            if (bday.Length == 0)
            {
                string bdayY = WebHelper.GetFormString("bdayY");
                string bdayM = WebHelper.GetFormString("bdayM");
                string bdayD = WebHelper.GetFormString("bdayD");
                bday = string.Format("{0}-{1}-{2}", bdayY, bdayM, bdayD);
            }
            if (bday.Length > 0 && bday != "--" && !ValidateHelper.IsDate(bday))
                errorList.AppendFormat("{0}\"key\":\"{1}\",\"msg\":\"{2}\"{3},", "{", "bday", "请选择正确的日期", "}");

            //验证详细地址
            if (address.Length > 75)
            {
                errorList.AppendFormat("{0}\"key\":\"{1}\",\"msg\":\"{2}\"{3},", "{", "address", "详细地址的长度不能大于75", "}");
            }

            //验证简介
            if (bio.Length > 150)
            {
                errorList.AppendFormat("{0}\"key\":\"{1}\",\"msg\":\"{2}\"{3},", "{", "bio", "简介的长度不能大于150", "}");
            }

            if (errorList.Length == 1)
            {
                if (bday.Length == 0 || bday == "--")
                    bday = "1900-1-1";

                if (regionId < 1)
                    regionId = 0;

                Users.UpdateUser(WorkContext.Uid, userName, WebHelper.HtmlEncode(nickName), WebHelper.HtmlEncode(avatar), gender, WebHelper.HtmlEncode(realName), TypeHelper.StringToDateTime(bday), idCard, regionId, WebHelper.HtmlEncode(address), WebHelper.HtmlEncode(bio));
                return AjaxResult("success", "信息更新成功");
            }
            else
            {
                return AjaxResult("error", errorList.Remove(errorList.Length - 1, 1).Append("]").ToString(), true);
            }
        }

        #endregion


        /// <summary>
        /// 重定向到本网站页面，如果不是本网站，则返回主页，用于避免跳转攻击
        /// </summary>
        /// <param name="returnUrl"></param>
        /// <returns></returns>
        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl) && returnUrl.Length > 1 && returnUrl.StartsWith("/")
                && !returnUrl.StartsWith("//") && !returnUrl.StartsWith("/\\"))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Home");
        }
    }
}