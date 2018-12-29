using System;
using System.Data;
using System.Collections.Generic;

using Lib.Core;

namespace Lib.Data
{
    /// <summary>
    /// 用户等级数据访问类
    /// </summary>
    public partial class UserRanks
    {
        /// <summary>
        /// 获得用户等级列表
        /// </summary>
        /// <returns></returns>
        public static List<UserRankInfo> GetUserRankList()
        {
            List<UserRankInfo> userRankList = new List<UserRankInfo>();
            IDataReader reader = Lib.Core.MngData.RDBS.GetUserRankList();
            while(reader.Read())
            {
                UserRankInfo userRankInfo = new UserRankInfo();
                userRankInfo.UserRid = TypeHelper.ObjectToInt(reader["userrid"]);
                userRankInfo.System = TypeHelper.ObjectToInt(reader["system"]);
                userRankInfo.Title = reader["title"].ToString();
                userRankInfo.Avatar = reader["avatar"].ToString();
                userRankInfo.LimitDays = TypeHelper.ObjectToInt(reader["limitdays"]);
                userRankList.Add(userRankInfo);
            }
            reader.Close();
            return userRankList;
        }

        /// <summary>
        /// 创建用户等级
        /// </summary>
        public static void CreateUserRank(UserRankInfo userRankInfo)
        {
            Lib.Core.MngData.RDBS.CreateUserRank(userRankInfo);
        }

        /// <summary>
        /// 删除用户等级
        /// </summary>
        /// <param name="userRid">用户等级id</param>
        public static void DeleteUserRankById(int userRid)
        {
            Lib.Core.MngData.RDBS.DeleteUserRankById(userRid);
        }

        /// <summary>
        /// 更新用户等级
        /// </summary>
        public static void UpdateUserRank(UserRankInfo userRankInfo)
        {
            Lib.Core.MngData.RDBS.UpdateUserRank(userRankInfo);
        }
    }
}
