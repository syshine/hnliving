using System;
using System.Text;
using System.Web.Mvc;
using System.Web.Routing;
using System.Collections.Generic;

using Lib.Core;
using Lib.Services;

namespace hnliving.web
{
    /// <summary>
    /// PC前台基础控制器类
    /// </summary>
    public class BaseWebController : BaseController
    {
        //工作上下文
        public WebWorkContext WorkContext = new WebWorkContext();

        protected override void Initialize(RequestContext requestContext)
        {
            base.Initialize(requestContext);
            this.ValidateRequest = false;
            ViewBag.Url = WebHelper.GetUrl();

            WorkContext.IsHttpAjax = WebHelper.IsAjax();
            WorkContext.IP = WebHelper.GetIP();
            WorkContext.Url = WebHelper.GetUrl();
            WorkContext.RawUrl = WebHelper.GetRawUrl();
            WorkContext.UrlReferrer = WebHelper.GetUrlReferrer();
            WorkContext.IsSupportWebp = WebHelper.IsSupportFileType("image/webp");

            WorkContext.AccessTokenForGzh = "";
            WorkContext.AccessTokenForMp = "";

            //获得用户唯一标示符sid
            WorkContext.Sid = Utils.GetSidCookie();
            if (WorkContext.Sid.Length == 0)
            {
                //生成sid
                WorkContext.Sid = Sessions.GenerateSid();
                //将sid保存到cookie中
                Utils.SetSidCookie(WorkContext.Sid);
            }

            PartUserInfo partUserInfo;

            //获得用户id
            int uid = Utils.GetUidCookie();
            if (uid < 1)//当用户为游客时
            {
                //创建游客
                partUserInfo = Users.CreatePartGuest();
            }
            else//当用户为会员时
            {
                //获得保存在cookie中的密码
                string encryptPwd = Utils.GetCookiePassword();
                //防止用户密码被篡改为危险字符
                if (encryptPwd.Length == 0 || !SecureHelper.IsBase64String(encryptPwd))
                {
                    //创建游客
                    partUserInfo = Users.CreatePartGuest();
                    encryptPwd = string.Empty;
                    Utils.SetUidCookie(-1);
                    Utils.SetCookiePassword("");
                }
                else
                {
                    partUserInfo = Users.GetPartUserByUidAndPwd(uid, Utils.DecryptCookiePassword(encryptPwd));
                    if (partUserInfo != null)
                    {
                    }
                    else//当会员的账号或密码不正确时，将用户置为游客
                    {
                        partUserInfo = Users.CreatePartGuest();
                        encryptPwd = string.Empty;
                        Utils.SetUidCookie(-1);
                        Utils.SetCookiePassword("");
                    }
                }
                WorkContext.EncryptPwd = encryptPwd;
            }

            //设置用户等级
            //if (UserRanks.IsBanUserRank(partUserInfo.UserRid) && partUserInfo.LiftBanTime <= DateTime.Now)
            //{
            //    UserRankInfo userRankInfo = UserRanks.GetUserRankByCredits(partUserInfo.PayCredits);
            //    Users.UpdateUserRankByUid(partUserInfo.Uid, userRankInfo.UserRid);
            //    partUserInfo.UserRid = userRankInfo.UserRid;
            //}

            //当用户被禁止访问时重置用户为游客
            if (partUserInfo.UserRid == 1)
            {
                partUserInfo = Users.CreatePartGuest();
                WorkContext.EncryptPwd = string.Empty;
                Utils.SetUidCookie(-1);
                Utils.SetCookiePassword("");
            }

            WorkContext.PartUserInfo = partUserInfo;

            WorkContext.Uid = partUserInfo.Uid;
            WorkContext.UserName = partUserInfo.UserName;
            WorkContext.UserEmail = partUserInfo.Email;
            WorkContext.UserMobile = partUserInfo.Mobile;
            WorkContext.Password = partUserInfo.Password;
            WorkContext.NickName = partUserInfo.NickName;
            WorkContext.Avatar = partUserInfo.Avatar;
            WorkContext.ModulesId = new List<string>(partUserInfo.Modules_id.Split(','));

            WorkContext.UserRid = partUserInfo.UserRid;
            //WorkContext.UserRankInfo = UserRanks.GetUserRankById(partUserInfo.UserRid);
            //WorkContext.UserRTitle = WorkContext.UserRankInfo.Title;

            //设置当前控制器类名
            WorkContext.Controller = RouteData.Values["controller"].ToString().ToLower();
            //设置当前动作方法名
            WorkContext.Action = RouteData.Values["action"].ToString().ToLower();
            WorkContext.PageKey = string.Format("/{0}/{1}", WorkContext.Controller, WorkContext.Action);
            
            //在线会员数
            WorkContext.OnlineMemberCount = WorkContext.OnlineUserCount - WorkContext.OnlineGuestCount;
            //搜索词
            WorkContext.SearchWord = string.Empty;
        }

        protected override void OnAuthorization(AuthorizationContext filterContext)
        {
            //不能应用在子方法上
            if (filterContext.IsChildAction)
                return;

            var controllerName = (filterContext.RouteData.Values["controller"]).ToString().ToLower();
            var actionName = (filterContext.RouteData.Values["action"]).ToString().ToLower();
            var areaName = (filterContext.RouteData.DataTokens["area"] == null ? "" : filterContext.RouteData.DataTokens["area"]).ToString().ToLower();

            if (areaName != "" && WorkContext.UserRid != 2)    // 不是系统管理员
            {
                // 整个控制器权限
                AccessConfigInfo aciAllAction = MngConfig.Lstaccessconfiginfo.Find(value => value.Area.ToLower() == areaName
                                                                                && value.Control.ToLower() == controllerName
                                                                                && value.Action.ToLower() == "AllAction");


                // 没有权限访问
                //if (!WorkContext.ModulesId.Contains("0") && !WorkContext.ModulesId.Contains(controllerName) && !WorkContext.ModulesId.Contains(areaName))
                bool bAccess = false;
                if (aciAllAction != null)
                {
                    List<string> lstAll = new List<string>(aciAllAction.Value.Split(','));
                    if (lstAll.Contains(WorkContext.UserRid.ToString()))
                    {
                        bAccess = true;
                    }
                }
                else
                {
                    // 单个Action权限
                    AccessConfigInfo aci = MngConfig.Lstaccessconfiginfo.Find(value => value.Area.ToLower() == areaName
                                                                                    && value.Control.ToLower() == controllerName
                                                                                    && value.Action.ToLower() == actionName);

                    if (aci != null)
                    {
                        List<string> lst = new List<string>(aci.Value.Split(','));
                        // 0代表都可以访问
                        if (lst.Contains("0") || lst.Contains(WorkContext.UserRid.ToString()))
                        {
                            bAccess = true;
                        }
                    }
                    else
                    {
                        bAccess = false;
                    }
                }

                if(!bAccess)
                {
                    if (filterContext.HttpContext.Request.IsAjaxRequest())
                    {
                        //Ajax 输出错误信息给脚本吧
                        if (WorkContext.Uid > 0)
                            filterContext.Result = AjaxResult("error", "未找到");
                        else
                            filterContext.Result = AjaxResult("error", "请登录");
                    }
                    else
                    {
                        if (WorkContext.Uid > 0)
                            filterContext.Result = new RedirectResult("/"); // 跳转到首页
                        else
                            filterContext.Result = new RedirectResult("/account/login?returnUrl=" + filterContext.HttpContext.Request.RawUrl); // 跳转到登录页面
                    }
                    return;
                }
            }
        }

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            //不能应用在子方法上
            if (filterContext.IsChildAction)
                return;

            //当用户为会员时,更新用户的在线时间
            //if (WorkContext.Uid > 0)
            //    Users.UpdateUserOnlineTime(WorkContext.Uid);
        }

        protected override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            //不能应用在子方法上
            if (filterContext.IsChildAction)
                return;
        }

        /// <summary>
        /// 提示信息视图
        /// </summary>
        /// <param name="message">提示信息</param>
        /// <returns></returns>
        protected ViewResult PromptView(string message)
        {
            return View("Prompt", new PromptModel(message));
        }


        /// <summary>
        /// 获取进度
        /// </summary>
        /// <param name="guid"></param>
        /// <returns></returns>
        protected ActionResult GetAjaxProcess(string guid)
        {
            ResultEntity result = new ResultEntity();

            try
            {
                #region memcache
                //  从memcache获取进度
                if (MngConfig.SiteConfig.EnableMemcache)
                {
                    MemCachedHelper mch = new MemCachedHelper();
                    // 总行数
                    int total_cnt = Convert.ToInt32(mch.Get(guid + "_total_cnt"));

                    // 线程数
                    int thread_cnt = Convert.ToInt32(mch.Get(guid + "_thread_cnt"));

                    // 已执行行数
                    int done_cnt = 0;
                    for (int i = 0; i < thread_cnt; i++)
                    {
                        string name = guid + "_td_pk_" + (i + 1);
                        done_cnt += Convert.ToInt32(mch.Get(name));
                    }

                    ProcessEntity progress = null;

                    // 执行时间
                    string key = guid + "_exec_time";
                    if (mch.IsExists(key))
                    {
                        DateTime dtStart = (DateTime)mch.Get(key);
                        progress = new ProcessEntity(total_cnt, done_cnt, dtStart);
                    }
                    else
                    {
                        progress = new ProcessEntity(total_cnt, done_cnt);
                    }


                    result.SetSuccess(progress);
                }
                #endregion
                #region redis
                //  从redis获取进度
                else if (MngConfig.SiteConfig.EnableRedis)
                {
                    // 总行数
                    int total_cnt = Convert.ToInt32(RedisHelper.GetString(guid + "_total_cnt"));
                    int total_count123 = RedisHelper.Get<int>(guid + "_total_cnt");
                    string total_count = RedisHelper.GetString(guid + "_total_cnt");

                    // 线程数
                    int thread_cnt = Convert.ToInt32(RedisHelper.GetString(guid + "_thread_cnt"));

                    // 已执行行数
                    int done_cnt = 0;
                    for (int i = 0; i < thread_cnt; i++)
                    {
                        string name = guid + "_td_pk_" + (i + 1);
                        done_cnt += Convert.ToInt32(RedisHelper.GetString(name));
                    }

                    ProcessEntity progress = null;

                    // 执行时间
                    string key = guid + "_exec_time";
                    DateTime dtStart = RedisHelper.Get<DateTime>(key);
                    if (dtStart != null)
                    {
                        progress = new ProcessEntity(total_cnt, done_cnt, dtStart);
                    }
                    else
                    {
                        progress = new ProcessEntity(total_cnt, done_cnt);
                    }

                    result.SetSuccess(progress);
                }
                #endregion
                else
                {
                    result.Set("inexistence", null, "未开启");
                }
            }
            catch (Exception ex)
            {
                result.SetError(ex.Message);
            }

            // 转成json格式返回
            string strResult = Newtonsoft.Json.JsonConvert.SerializeObject(result);
            return Content(strResult);
        }
    }
}
