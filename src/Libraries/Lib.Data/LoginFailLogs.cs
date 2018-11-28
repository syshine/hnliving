﻿using System;
using System.Data;

using Lib.Core;

namespace Lib.Data
{
    /// <summary>
    /// 登录失败日志数据访问类
    /// </summary>
    public partial class LoginFailLogs
    {
        /// <summary>
        /// 获得登录失败日志
        /// </summary>
        /// <param name="loginIP">登录IP</param>
        /// <returns></returns>
        public static LoginFailLogInfo GetLoginFailLogByIP(long loginIP)
        {
            LoginFailLogInfo loginFailLogInfo = null;
            IDataReader reader = Lib.Core.MngData.RDBS.GetLoginFailLogByIP(loginIP);
            if (reader.Read())
            {
                loginFailLogInfo = new LoginFailLogInfo();
                loginFailLogInfo.Id = TypeHelper.ObjectToInt(reader["id"]);
                loginFailLogInfo.LoginIP = Convert.ToInt64(reader["loginip"]);
                loginFailLogInfo.FailTimes = TypeHelper.ObjectToInt(reader["failtimes"]);
                loginFailLogInfo.LastLoginTime = TypeHelper.ObjectToDateTime(reader["lastlogintime"]);
            }
            reader.Close();
            return loginFailLogInfo;
        }

        /// <summary>
        /// 增加登录失败次数
        /// </summary>
        /// <param name="loginIP">登录IP</param>
        /// <param name="loginTime">登录时间</param>
        public static void AddLoginFailTimes(long loginIP, DateTime loginTime)
        {
            Lib.Core.MngData.RDBS.AddLoginFailTimes(loginIP, loginTime);
        }

        /// <summary>
        /// 删除登录失败日志
        /// </summary>
        /// <param name="loginIP">登录IP</param>
        public static void DeleteLoginFailLogByIP(long loginIP)
        {
            Lib.Core.MngData.RDBS.DeleteLoginFailLogByIP(loginIP);
        }
    }
}
