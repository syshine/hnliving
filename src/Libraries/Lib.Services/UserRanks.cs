﻿using System;
using System.Collections.Generic;

using Lib.Core;

namespace Lib.Services
{
    /// <summary>
    /// 用户等级操作管理类
    /// </summary>
    public partial class UserRanks
    {
        /// <summary>
        /// 获得用户等级列表
        /// </summary>
        /// <returns></returns>
        public static List<UserRankInfo> GetUserRankList()
        {
            List<UserRankInfo> userRankList = null;// Lib.Core.MngCache.Get(CacheKeys.MALL_USERRANK_LIST) as List<UserRankInfo>;
            if (userRankList == null)
            {
                userRankList = Lib.Data.UserRanks.GetUserRankList();
                //Lib.Core.MngCache.Insert(CacheKeys.MALL_USERRANK_LIST, userRankList);
            }
            return userRankList;
        }

        /// <summary>
        /// 返回系统级用户等级列表
        /// </summary>
        /// <returns></returns>
        public static List<UserRankInfo> GetSystemUserRankList()
        {
            List<UserRankInfo> userRankList = new List<UserRankInfo>();
            foreach (UserRankInfo userRankInfo in GetUserRankList())
            {
                if (userRankInfo.System == 1)
                    userRankList.Add(userRankInfo);
            }

            return userRankList;
        }

        /// <summary>
        /// 返回用户级用户等级列表
        /// </summary>
        /// <returns></returns>
        public static List<UserRankInfo> GetCustomerUserRankList()
        {
            List<UserRankInfo> userRankList = new List<UserRankInfo>();
            foreach (UserRankInfo userRankInfo in GetUserRankList())
            {
                if (userRankInfo.System == 0)
                    userRankList.Add(userRankInfo);
            }

            return userRankList;
        }

        /// <summary>
        /// 获得用户等级
        /// </summary>
        /// <param name="userRid">用户等级id</param>
        /// <returns></returns>
        public static UserRankInfo GetUserRankById(int userRid)
        {
            if (userRid > 0)
            {
                foreach (UserRankInfo userRankInfo in GetUserRankList())
                {
                    if (userRid == userRankInfo.UserRid)
                        return userRankInfo;
                }
            }
            return null;
        }

        /// <summary>
        /// 获得用户等级id
        /// </summary>
        /// <param name="title">用户等级标题</param>
        /// <returns></returns>
        public static int GetUserRidByTitle(string title)
        {
            if (!string.IsNullOrWhiteSpace(title))
            {
                foreach (UserRankInfo userRankInfo in GetUserRankList())
                {
                    if (userRankInfo.Title == title)
                        return userRankInfo.UserRid;
                }
            }
            return -1;
        }

        /// <summary>
        /// 判断用户等级是否为被禁用等级
        /// </summary>
        /// <param name="userRid">用户等级id</param>
        /// <returns></returns>
        public static bool IsBanUserRank(int userRid)
        {
            return userRid == 1 ? true : false;
        }

        /// <summary>
        /// 获得最低用户等级
        /// </summary>
        /// <returns></returns>
        public static UserRankInfo GetLowestUserRank()
        {
            foreach (UserRankInfo userRankInfo in GetUserRankList())
            {
                if (userRankInfo.System == 0)
                    return userRankInfo;
            }
            return null;
        }



        /// <summary>
        /// 是否内容编辑者
        /// </summary>
        /// <param name="uid">用户id</param>
        /// <returns></returns>
        public static bool IsContentEditor(int uid)
        {
            if (uid > 0)
            {
                PartUserInfo partUserInfo = Users.GetPartUserById(uid);

                // 系统管理员和内容管理员可以编辑
                if ((int)UserRankInfo.UserRank.SYSTEM == partUserInfo.UserRid
                    || (int)UserRankInfo.UserRank.CONTENT == partUserInfo.UserRid)
                {
                    return true;
                }
                else
                    return false;
            }
            else
                return false;
        }
    }
}
