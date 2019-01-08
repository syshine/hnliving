using System;
using System.Text;
using System.Data;
using System.Data.Common;

using Lib.Core;

namespace hnliving.RDBSStrategy.SqlServer
{
    /// <summary>
    /// SqlServer策略之用户分部类
    /// </summary>
    public partial class RDBSStrategy : IRDBSStrategy
    {
        #region 在线用户

        /// <summary>
        /// 创建在线用户
        /// </summary>
        public int CreateOnlineUser(OnlineUserInfo onlineUserInfo)
        {
            DbParameter[] parms = {
									   GenerateInParam("@uid",SqlDbType.Int,4,onlineUserInfo.Uid),
									   GenerateInParam("@sid",SqlDbType.Char,16,onlineUserInfo.Sid),
                                       GenerateInParam("@nickname",SqlDbType.NChar,20,onlineUserInfo.NickName),	
                                       GenerateInParam("@ip",SqlDbType.Char,15,onlineUserInfo.IP),	
                                       GenerateInParam("@regionid",SqlDbType.SmallInt,2,onlineUserInfo.RegionId),	
									   GenerateInParam("@updatetime",SqlDbType.DateTime,8,onlineUserInfo.UpdateTime)
								   };
            return TypeHelper.ObjectToInt(RDBSHelper.ExecuteScalar(CommandType.StoredProcedure,
                                                                   string.Format("{0}createonlineuser", RDBSHelper.RDBSTablePre),
                                                                   parms));
        }

        /// <summary>
        /// 更新在线用户ip
        /// </summary>
        /// <param name="olId">在线用户id</param>
        /// <param name="ip">ip</param>
        public void UpdateOnlineUserIP(int olId, string ip)
        {
            DbParameter[] parms = {
									   GenerateInParam("@ip",SqlDbType.Char,15,ip),
									   GenerateInParam("@olid",SqlDbType.Int,4,olId)
								   };
            RDBSHelper.ExecuteNonQuery(CommandType.StoredProcedure,
                                       string.Format("{0}updateonlineuserip", RDBSHelper.RDBSTablePre),
                                       parms);
        }

        /// <summary>
        /// 更新在线用户uid
        /// </summary>
        /// <param name="olId">在线用户id</param>
        /// <param name="ip">uid</param>
        public void UpdateOnlineUserUid(int olId, int uid)
        {
            DbParameter[] parms = {
									   GenerateInParam("@uid",SqlDbType.Int,4,uid),
									   GenerateInParam("@olid",SqlDbType.Int,4,olId)
								   };
            RDBSHelper.ExecuteNonQuery(CommandType.StoredProcedure,
                                       string.Format("{0}updateonlineuseruid", RDBSHelper.RDBSTablePre),
                                       parms);
        }

        /// <summary>
        /// 获得在线用户
        /// </summary>
        /// <param name="sid">sessionId</param>
        /// <returns></returns>
        public IDataReader GetOnlineUserBySid(string sid)
        {
            DbParameter[] parms = {
                                    GenerateInParam("@sid",SqlDbType.Char,16,sid)
                                  };

            return RDBSHelper.ExecuteReader(CommandType.StoredProcedure,
                                            string.Format("{0}getonlineuserbysid", RDBSHelper.RDBSTablePre),
                                            parms);
        }

        /// <summary>
        /// 获得在线用户数量
        /// </summary>
        /// <param name="userType">在线用户类型</param>
        /// <returns></returns>
        public int GetOnlineUserCount(int userType)
        {
            DbParameter[] parms = {
                                      GenerateInParam("@usertype",SqlDbType.Int,4,userType)
                                   };
            return TypeHelper.ObjectToInt(RDBSHelper.ExecuteScalar(CommandType.StoredProcedure,
                                                                   string.Format("{0}getonlineuercount", RDBSHelper.RDBSTablePre),
                                                                   parms));
        }

        /// <summary>
        /// 删除在线用户
        /// </summary>
        /// <param name="sid">sessionId</param>
        public void DeleteOnlineUserBySid(string sid)
        {
            DbParameter[] parms = { 
                                        GenerateInParam("@sid", SqlDbType.Char, 16, sid)
                                    };
            RDBSHelper.ExecuteNonQuery(CommandType.StoredProcedure,
                                       string.Format("{0}deleteonlineuserbysid", RDBSHelper.RDBSTablePre),
                                       parms);
        }

        /// <summary>
        /// 删除过期在线用户
        /// </summary>
        /// <param name="onlineUserExpire">过期时间</param>
        public void DeleteExpiredOnlineUser(int onlineUserExpire)
        {
            DbParameter[] parms = { 
                                    GenerateInParam("@expiretime", SqlDbType.DateTime, 8, DateTime.Now.AddMinutes(onlineUserExpire * -1))
                                  };
            RDBSHelper.ExecuteNonQuery(CommandType.StoredProcedure,
                                       string.Format("{0}deleteexpiredonlineuser", RDBSHelper.RDBSTablePre),
                                       parms);
        }

        /// <summary>
        /// 重置在线用户表
        /// </summary>
        public void ResetOnlineUserTable()
        {
            RDBSHelper.ExecuteNonQuery(CommandType.Text,
                                       string.Format("TRUNCATE TABLE [{0}onlineusers]",
                                       RDBSHelper.RDBSTablePre));
        }

        /// <summary>
        /// 获得在线用户列表
        /// </summary>
        /// <param name="pageSize">每页数</param>
        /// <param name="pageNumber">当前页数</param>
        /// <param name="locationType">位置类型(0代表省,1代表市,2代表区或县)</param>
        /// <param name="locationId">位置id</param>
        /// <returns></returns>
        public IDataReader GetOnlineUserList(int pageSize, int pageNumber, int locationType, int locationId)
        {
            string condition = GetOnlineUserListCondition(locationType, locationId);
            bool noCondition = string.IsNullOrWhiteSpace(condition);
            string commandText;
            if (pageNumber == 1)
            {
                if (noCondition)
                    commandText = string.Format("SELECT TOP {0} {2} FROM [{1}onlineusers] ORDER BY [olid] DESC",
                                                pageSize,
                                                RDBSHelper.RDBSTablePre,
                                                TableFields.ONLINE_USERS);

                else
                    commandText = string.Format("SELECT TOP {0} {3} FROM [{1}onlineusers] WHERE {2} ORDER BY [olid] DESC",
                                                pageSize,
                                                RDBSHelper.RDBSTablePre,
                                                condition,
                                                TableFields.ONLINE_USERS);
            }
            else
            {
                if (noCondition)
                    commandText = string.Format("SELECT TOP {0} {3} FROM [{1}onlineusers] WHERE [olid] NOT IN (SELECT TOP {2} [olid] FROM [{1}onlineusers] ORDER BY [olid] DESC) ORDER BY [olid] DESC",
                                                pageSize,
                                                RDBSHelper.RDBSTablePre,
                                                (pageNumber - 1) * pageSize,
                                                TableFields.ONLINE_USERS);
                else
                    commandText = string.Format("SELECT TOP {0} {4} FROM [{1}onlineusers] WHERE [olid] NOT IN (SELECT TOP {2} [olid] FROM [{1}onlineusers] WHERE {3} ORDER BY [olid] DESC) AND {3} ORDER BY [olid] DESC",
                                                pageSize,
                                                RDBSHelper.RDBSTablePre,
                                                (pageNumber - 1) * pageSize,
                                                condition,
                                                TableFields.ONLINE_USERS);
            }

            return RDBSHelper.ExecuteReader(CommandType.Text, commandText);
        }

        /// <summary>
        /// 获得在线用户数量
        /// </summary>
        /// <param name="locationType">位置类型(0代表省,1代表市,2代表区或县)</param>
        /// <param name="locationId">位置id</param>
        /// <returns></returns>
        public int GetOnlineUserCount(int locationType, int locationId)
        {
            string condition = GetOnlineUserListCondition(locationType, locationId);
            string commandText;
            if (string.IsNullOrWhiteSpace(condition))
                commandText = string.Format("SELECT COUNT(olid) FROM [{0}onlineusers]", RDBSHelper.RDBSTablePre);
            else
                commandText = string.Format("SELECT COUNT(olid) FROM [{0}onlineusers] WHERE {1}", RDBSHelper.RDBSTablePre, condition);
            return TypeHelper.ObjectToInt(RDBSHelper.ExecuteScalar(CommandType.Text, commandText));
        }

        /// <summary>
        /// 获得在线用户列表条件
        /// </summary>
        /// <param name="locationType">位置类型(0代表省,1代表市,2代表区或县)</param>
        /// <param name="locationId">位置id</param>
        /// <returns></returns>
        private string GetOnlineUserListCondition(int locationType, int locationId)
        {
            if (locationId > 0)
            {
                if (locationType == 0)
                {
                    return string.Format(" [regionid] IN (SELECT [regionid] FROM [{0}regions] WHERE [provinceid]={1})", RDBSHelper.RDBSTablePre, locationId);
                }
                else if (locationType == 1)
                {
                    return string.Format(" [regionid] IN (SELECT [regionid] FROM [{0}regions] WHERE [cityid]={1})", RDBSHelper.RDBSTablePre, locationId);
                }
                else if (locationType == 2)
                {
                    return string.Format(" [regionid]={0}", locationId);
                }
            }

            return "";
        }

        #endregion

        #region 开放授权

        /// <summary>
        /// 获得用户id
        /// </summary>
        /// <param name="openId">开放id</param>
        /// <param name="server">服务商</param>
        /// <returns></returns>
        public int GetUidByOpenIdAndServer(string openId, string server)
        {
            DbParameter[] parms = {
									GenerateInParam("@openid",SqlDbType.Char,50,openId),
                                    GenerateInParam("@server",SqlDbType.Char,10,server)	
								   };
            return TypeHelper.ObjectToInt(RDBSHelper.ExecuteScalar(CommandType.StoredProcedure,
                                                                   string.Format("{0}getuidbyopenidandserver", RDBSHelper.RDBSTablePre),
                                                                   parms));
        }

        #endregion

        #region 用户

        /// <summary>
        /// 获得部分用户
        /// </summary>
        /// <param name="uid">用户id</param>
        /// <returns></returns>
        public IDataReader GetPartUserById(int uid)
        {
            DbParameter[] parms = {
									   GenerateInParam("@uid",SqlDbType.Int,4, uid)
								   };
            return RDBSHelper.ExecuteReader(CommandType.StoredProcedure,
                                            string.Format("{0}getpartuserbyid", RDBSHelper.RDBSTablePre),
                                            parms);
        }

        /// <summary>
        /// 获得用户
        /// </summary>
        /// <param name="uid">用户id</param>
        /// <returns></returns>
        public IDataReader GetUserById(int uid)
        {
            DbParameter[] parms = {
									   GenerateInParam("@uid",SqlDbType.Int,4, uid)
								   };
            return RDBSHelper.ExecuteReader(CommandType.StoredProcedure,
                                            string.Format("{0}getuserbyid", RDBSHelper.RDBSTablePre),
                                            parms);
        }

        /// <summary>
        /// 获得用户细节
        /// </summary>
        /// <param name="uid">用户id</param>
        /// <returns></returns>
        public IDataReader GetUserDetailById(int uid)
        {
            DbParameter[] parms = {
									   GenerateInParam("@uid",SqlDbType.Int,4, uid)
								   };
            return RDBSHelper.ExecuteReader(CommandType.StoredProcedure,
                                            string.Format("{0}getuserdetailbyid", RDBSHelper.RDBSTablePre),
                                            parms);
        }

        /// <summary>
        /// 获得部分用户
        /// </summary>
        /// <param name="userName">用户名</param>
        /// <returns></returns>
        public IDataReader GetPartUserByName(string userName)
        {
            DbParameter[] parms = {
									   GenerateInParam("@username",SqlDbType.NChar,20, userName)
								   };
            return RDBSHelper.ExecuteReader(CommandType.StoredProcedure,
                                            string.Format("{0}getpartuserbyname", RDBSHelper.RDBSTablePre),
                                            parms);
        }

        /// <summary>
        /// 获得部分用户
        /// </summary>
        /// <param name="email">用户邮箱</param>
        /// <returns></returns>
        public IDataReader GetPartUserByEmail(string email)
        {
            DbParameter[] parms = {
									   GenerateInParam("@email",SqlDbType.Char,50, email)
								   };
            return RDBSHelper.ExecuteReader(CommandType.StoredProcedure,
                                            string.Format("{0}getpartuserbyemail", RDBSHelper.RDBSTablePre),
                                            parms);
        }

        /// <summary>
        /// 获得部分用户
        /// </summary>
        /// <param name="mobile">用户手机</param>
        /// <returns></returns>
        public IDataReader GetPartUserByMobile(string mobile)
        {
            DbParameter[] parms = {
									   GenerateInParam("@mobile",SqlDbType.Char,15, mobile)
								   };
            return RDBSHelper.ExecuteReader(CommandType.StoredProcedure,
                                            string.Format("{0}getpartuserbymobile", RDBSHelper.RDBSTablePre),
                                            parms);
        }

        /// <summary>
        /// 获得用户id
        /// </summary>
        /// <param name="userName">用户名</param>
        /// <returns></returns>
        public int GetUidByUserName(string userName)
        {
            DbParameter[] parms = {
									   GenerateInParam("@username",SqlDbType.NChar,20, userName)
								   };
            return TypeHelper.ObjectToInt(RDBSHelper.ExecuteScalar(CommandType.StoredProcedure,
                                                                   string.Format("{0}getuidbyusername", RDBSHelper.RDBSTablePre),
                                                                   parms), -1);
        }

        /// <summary>
        /// 获得用户id
        /// </summary>
        /// <param name="email">用户邮箱</param>
        /// <returns></returns>
        public int GetUidByEmail(string email)
        {
            DbParameter[] parms = {
									   GenerateInParam("@email",SqlDbType.Char,50, email)
								   };
            return TypeHelper.ObjectToInt(RDBSHelper.ExecuteScalar(CommandType.StoredProcedure,
                                                                   string.Format("{0}getuidbyemail", RDBSHelper.RDBSTablePre),
                                                                   parms), -1);
        }

        /// <summary>
        /// 获得用户id
        /// </summary>
        /// <param name="mobile">用户手机</param>
        /// <returns></returns>
        public int GetUidByMobile(string mobile)
        {
            DbParameter[] parms = {
									   GenerateInParam("@mobile",SqlDbType.Char,15, mobile)
								   };
            return TypeHelper.ObjectToInt(RDBSHelper.ExecuteScalar(CommandType.StoredProcedure,
                                                                   string.Format("{0}getuidbymobile", RDBSHelper.RDBSTablePre),
                                                                   parms), -1);
        }

        /// <summary>
        /// 创建用户
        /// </summary>
        /// <returns></returns>
        public int CreateUser(UserInfo userInfo)
        {
            DbParameter[] parms = {
									   GenerateInParam("@username",SqlDbType.NChar,20,userInfo.UserName),
									   GenerateInParam("@email",SqlDbType.Char,50,userInfo.Email),
                                       GenerateInParam("@mobile",SqlDbType.Char,15,userInfo.Mobile),
									   GenerateInParam("@password",SqlDbType.Char,32,userInfo.Password),
									   GenerateInParam("@userrid",SqlDbType.SmallInt,2,userInfo.UserRid),
									   GenerateInParam("@nickname",SqlDbType.NChar,20,userInfo.NickName),
									   GenerateInParam("@avatar",SqlDbType.Char,40,userInfo.Avatar),
									   GenerateInParam("@rankcredits",SqlDbType.Int,4,userInfo.RankCredits),
									   GenerateInParam("@verifyemail",SqlDbType.TinyInt,1,userInfo.VerifyEmail),
									   GenerateInParam("@verifymobile",SqlDbType.TinyInt,1,userInfo.VerifyMobile),
									   GenerateInParam("@liftbantime",SqlDbType.DateTime,8,userInfo.LiftBanTime),
                                       GenerateInParam("@salt",SqlDbType.NChar,6,userInfo.Salt),
                                       GenerateInParam("@modules_id",SqlDbType.NChar,userInfo.Modules_id.Length,userInfo.Modules_id),
                                       GenerateInParam("@lastvisittime",SqlDbType.DateTime,8,userInfo.LastVisitTime),
                                       GenerateInParam("@lastvisitip",SqlDbType.Char,15,userInfo.LastVisitIP),
                                       GenerateInParam("@lastvisitrgid",SqlDbType.SmallInt,2,userInfo.LastVisitRgId),
									   GenerateInParam("@registertime",SqlDbType.DateTime,8,userInfo.RegisterTime),
                                       GenerateInParam("@registerip",SqlDbType.Char,15,userInfo.RegisterIP),
                                       GenerateInParam("@registerrgid",SqlDbType.SmallInt,2,userInfo.RegisterRgId),
									   GenerateInParam("@gender",SqlDbType.TinyInt,1,userInfo.Gender),
                                       GenerateInParam("@realname",SqlDbType.NVarChar,10,userInfo.RealName),
									   GenerateInParam("@bday",SqlDbType.DateTime,8,userInfo.Bday),
                                       GenerateInParam("@idcard",SqlDbType.VarChar,18,userInfo.IdCard),
									   GenerateInParam("@regionid",SqlDbType.SmallInt,2,userInfo.RegionId),
									   GenerateInParam("@address",SqlDbType.NVarChar,150,userInfo.Address),
									   GenerateInParam("@bio",SqlDbType.NVarChar,300,userInfo.Bio)
								   };

            return TypeHelper.ObjectToInt(RDBSHelper.ExecuteScalar(CommandType.StoredProcedure,
                                                                      string.Format("{0}createuser", RDBSHelper.RDBSTablePre),
                                                                      parms), -1);
        }

        /// <summary>
        /// 更新用户
        /// </summary>
        /// <returns></returns>
        public void UpdateUser(UserInfo userInfo)
        {
            DbParameter[] parms = {
									   GenerateInParam("@username",SqlDbType.NChar,20,userInfo.UserName),
									   GenerateInParam("@email",SqlDbType.Char,50,userInfo.Email),
                                       GenerateInParam("@mobile",SqlDbType.Char,15,userInfo.Mobile),
									   GenerateInParam("@password",SqlDbType.Char,32,userInfo.Password),
									   GenerateInParam("@userrid",SqlDbType.SmallInt,2,userInfo.UserRid),
									   GenerateInParam("@nickname",SqlDbType.NChar,20,userInfo.NickName),
									   GenerateInParam("@avatar",SqlDbType.Char,40,userInfo.Avatar),
									   GenerateInParam("@rankcredits",SqlDbType.Int,4,userInfo.RankCredits),
									   GenerateInParam("@verifyemail",SqlDbType.TinyInt,1,userInfo.VerifyEmail),
									   GenerateInParam("@verifymobile",SqlDbType.TinyInt,1,userInfo.VerifyMobile),
									   GenerateInParam("@liftbantime",SqlDbType.DateTime,8,userInfo.LiftBanTime),
                                       GenerateInParam("@salt",SqlDbType.NChar,6,userInfo.Salt),
                                       GenerateInParam("@modules_id",SqlDbType.NChar,userInfo.Modules_id.Length,userInfo.Modules_id),
                                       GenerateInParam("@lastvisittime",SqlDbType.DateTime,8,userInfo.LastVisitTime),
                                       GenerateInParam("@lastvisitip",SqlDbType.Char,15,userInfo.LastVisitIP),
                                       GenerateInParam("@lastvisitrgid",SqlDbType.SmallInt,2,userInfo.LastVisitRgId),
									   GenerateInParam("@registertime",SqlDbType.DateTime,8,userInfo.RegisterTime),
                                       GenerateInParam("@registerip",SqlDbType.Char,15,userInfo.RegisterIP),
                                       GenerateInParam("@registerrgid",SqlDbType.SmallInt,2,userInfo.RegisterRgId),
									   GenerateInParam("@gender",SqlDbType.TinyInt,1,userInfo.Gender),
                                       GenerateInParam("@realname",SqlDbType.NVarChar,10,userInfo.RealName),
									   GenerateInParam("@bday",SqlDbType.DateTime,8,userInfo.Bday),
                                       GenerateInParam("@idcard",SqlDbType.VarChar,18,userInfo.IdCard),
									   GenerateInParam("@regionid",SqlDbType.SmallInt,2,userInfo.RegionId),
									   GenerateInParam("@address",SqlDbType.NVarChar,150,userInfo.Address),
									   GenerateInParam("@bio",SqlDbType.NVarChar,300,userInfo.Bio),
									   GenerateInParam("@uid",SqlDbType.Int,4,userInfo.Uid)
								   };

            RDBSHelper.ExecuteScalar(CommandType.StoredProcedure,
                                     string.Format("{0}updateuser", RDBSHelper.RDBSTablePre),
                                     parms);
        }

        /// <summary>
        /// 更新部分用户
        /// </summary>
        /// <returns></returns>
        public void UpdatePartUser(PartUserInfo partUserInfo)
        {
            DbParameter[] parms = {
									   GenerateInParam("@username",SqlDbType.NChar,20,partUserInfo.UserName),
									   GenerateInParam("@email",SqlDbType.Char,50,partUserInfo.Email),
                                       GenerateInParam("@mobile",SqlDbType.Char,15,partUserInfo.Mobile),
									   GenerateInParam("@password",SqlDbType.Char,32,partUserInfo.Password),
									   GenerateInParam("@userrid",SqlDbType.SmallInt,2,partUserInfo.UserRid),
									   GenerateInParam("@nickname",SqlDbType.NChar,20,partUserInfo.NickName),
									   GenerateInParam("@avatar",SqlDbType.Char,40,partUserInfo.Avatar),
									   GenerateInParam("@rankcredits",SqlDbType.Int,4,partUserInfo.RankCredits),
									   GenerateInParam("@verifyemail",SqlDbType.TinyInt,1,partUserInfo.VerifyEmail),
									   GenerateInParam("@verifymobile",SqlDbType.TinyInt,1,partUserInfo.VerifyMobile),
									   GenerateInParam("@liftbantime",SqlDbType.DateTime,8,partUserInfo.LiftBanTime),
                                       GenerateInParam("@salt",SqlDbType.NChar,6,partUserInfo.Salt),
                                       GenerateInParam("@modules_id",SqlDbType.NChar,partUserInfo.Modules_id.Length,partUserInfo.Modules_id),
                                       GenerateInParam("@uid",SqlDbType.Int,4,partUserInfo.Uid)
								   };

            RDBSHelper.ExecuteScalar(CommandType.StoredProcedure,
                                     string.Format("{0}updatepartuser", RDBSHelper.RDBSTablePre),
                                     parms);
        }

        /// <summary>
        /// 更新用户细节
        /// </summary>
        /// <returns></returns>
        public void UpdateUserDetail(UserDetailInfo userDetailInfo)
        {
            DbParameter[] parms = {
                                       GenerateInParam("@lastvisittime",SqlDbType.DateTime,8,userDetailInfo.LastVisitTime),
                                       GenerateInParam("@lastvisitip",SqlDbType.Char,15,userDetailInfo.LastVisitIP),
                                       GenerateInParam("@lastvisitrgid",SqlDbType.SmallInt,2,userDetailInfo.LastVisitRgId),
									   GenerateInParam("@registertime",SqlDbType.DateTime,8,userDetailInfo.RegisterTime),
                                       GenerateInParam("@registerip",SqlDbType.Char,15,userDetailInfo.RegisterIP),
                                       GenerateInParam("@registerrgid",SqlDbType.SmallInt,2,userDetailInfo.RegisterRgId),
									   GenerateInParam("@gender",SqlDbType.TinyInt,1,userDetailInfo.Gender),
                                       GenerateInParam("@realname",SqlDbType.NVarChar,10,userDetailInfo.RealName),
									   GenerateInParam("@bday",SqlDbType.DateTime,8,userDetailInfo.Bday),
                                       GenerateInParam("@idcard",SqlDbType.VarChar,18,userDetailInfo.IdCard),
									   GenerateInParam("@regionid",SqlDbType.SmallInt,2,userDetailInfo.RegionId),
									   GenerateInParam("@address",SqlDbType.NVarChar,150,userDetailInfo.Address),
									   GenerateInParam("@bio",SqlDbType.NVarChar,300,userDetailInfo.Bio),
									   GenerateInParam("@uid",SqlDbType.Int,4,userDetailInfo.Uid)
								   };

            RDBSHelper.ExecuteScalar(CommandType.StoredProcedure,
                                     string.Format("{0}updateuserdetail", RDBSHelper.RDBSTablePre),
                                     parms);
        }

        /// <summary>
        /// 更新用户最后访问
        /// </summary>
        /// <param name="uid">用户id</param>
        /// <param name="visitTime">访问时间</param>
        /// <param name="ip">ip</param>
        /// <param name="regionId">区域id</param>
        public void UpdateUserLastVisit(int uid, DateTime visitTime, string ip, int regionId)
        {
            DbParameter[] parms = {
									   GenerateInParam("@uid",SqlDbType.Int,4,uid),
									   GenerateInParam("@visittime",SqlDbType.DateTime,8,visitTime),
                                       GenerateInParam("@ip",SqlDbType.Char,15,ip),
									   GenerateInParam("@regionid",SqlDbType.SmallInt,2,regionId)
								   };
            RDBSHelper.ExecuteNonQuery(CommandType.StoredProcedure,
                                       string.Format("{0}updateuserlastvisit", RDBSHelper.RDBSTablePre),
                                       parms);
        }

        /// <summary>
        /// 后台获得用户列表
        /// </summary>
        /// <param name="pageSize">每页数</param>
        /// <param name="pageNumber">当前页数</param>
        /// <param name="condition">条件</param>
        /// <returns></returns>
        public DataTable AdminGetUserList(int pageSize, int pageNumber, string condition)
        {
            bool noCondition = string.IsNullOrWhiteSpace(condition);
            string commandText;
            if (pageNumber == 1)
            {
                if (noCondition)
                    commandText = string.Format("SELECT TOP {0} [{1}users].[uid],[{1}users].[username],[{1}users].[email],[{1}users].[mobile],[{1}users].[userrid],[{1}users].[storeid],[{1}users].[mallagid],[{1}users].[nickname],[{1}users].[paycredits],[{1}users].[rankcredits],[{1}userdetails].[lastvisittime],[{1}userdetails].[lastvisitip],[{1}userdetails].[registertime],[{1}userdetails].[gender],[{1}userdetails].[realname],[{1}userranks].[title] AS [utitle],[{1}malladmingroups].[title] AS [atitle] FROM [{1}users] LEFT JOIN [{1}userdetails] ON [{1}userdetails].[uid] = [{1}users].[uid]  LEFT JOIN [{1}userranks] ON [{1}userranks].[userrid]=[{1}users].[userrid]  LEFT JOIN [{1}malladmingroups] ON [{1}malladmingroups].[mallagid]=[{1}users].[mallagid] ORDER BY [{1}users].[uid] DESC",
                                                pageSize,
                                                RDBSHelper.RDBSTablePre);

                else
                    commandText = string.Format("SELECT TOP {0} [{1}users].[uid],[{1}users].[username],[{1}users].[email],[{1}users].[mobile],[{1}users].[userrid],[{1}users].[storeid],[{1}users].[mallagid],[{1}users].[nickname],[{1}users].[paycredits],[{1}users].[rankcredits],[{1}userdetails].[lastvisittime],[{1}userdetails].[lastvisitip],[{1}userdetails].[registertime],[{1}userdetails].[gender],[{1}userdetails].[realname],[{1}userranks].[title] AS [utitle],[{1}malladmingroups].[title] AS [atitle] FROM [{1}users] LEFT JOIN [{1}userdetails] ON [{1}userdetails].[uid] = [{1}users].[uid]  LEFT JOIN [{1}userranks] ON [{1}userranks].[userrid]=[{1}users].[userrid]  LEFT JOIN [{1}malladmingroups] ON [{1}malladmingroups].[mallagid]=[{1}users].[mallagid] WHERE {2} ORDER BY [{1}users].[uid] DESC",
                                                pageSize,
                                                RDBSHelper.RDBSTablePre,
                                                condition);
            }
            else
            {
                if (noCondition)
                    commandText = string.Format("SELECT TOP {0} [{1}users].[uid],[{1}users].[username],[{1}users].[email],[{1}users].[mobile],[{1}users].[userrid],[{1}users].[storeid],[{1}users].[mallagid],[{1}users].[nickname],[{1}users].[paycredits],[{1}users].[rankcredits],[{1}userdetails].[lastvisittime],[{1}userdetails].[lastvisitip],[{1}userdetails].[registertime],[{1}userdetails].[gender],[{1}userdetails].[realname],[{1}userranks].[title] AS [utitle],[{1}malladmingroups].[title] AS [atitle] FROM [{1}users],[{1}userdetails],[{1}userranks],[{1}malladmingroups]  WHERE [{1}userdetails].[uid] = [{1}users].[uid] AND  [{1}userranks].[userrid]=[{1}users].[userrid] AND  [{1}malladmingroups].[mallagid]=[{1}users].[mallagid] AND [{1}users].[uid] < (SELECT min([uid])  FROM (SELECT TOP {2} [uid] FROM [{1}users] ORDER BY [uid] DESC) AS temp ) ORDER BY [{1}users].[uid] DESC",
                                                pageSize,
                                                RDBSHelper.RDBSTablePre,
                                                (pageNumber - 1) * pageSize);
                else
                    commandText = string.Format("SELECT TOP {0} [{1}users].[uid],[{1}users].[username],[{1}users].[email],[{1}users].[mobile],[{1}users].[userrid],[{1}users].[storeid],[{1}users].[mallagid],[{1}users].[nickname],[{1}users].[paycredits],[{1}users].[rankcredits],[{1}userdetails].[lastvisittime],[{1}userdetails].[lastvisitip],[{1}userdetails].[registertime],[{1}userdetails].[gender],[{1}userdetails].[realname],[{1}userranks].[title] AS [utitle],[{1}malladmingroups].[title] AS [atitle] FROM [{1}users],[{1}userdetails],[{1}userranks],[{1}malladmingroups]  WHERE [{1}userdetails].[uid] = [{1}users].[uid] AND  [{1}userranks].[userrid]=[{1}users].[userrid] AND  [{1}malladmingroups].[mallagid]=[{1}users].[mallagid] AND [{1}users].[uid] < (SELECT min([uid])  FROM (SELECT TOP {2} [uid] FROM [{1}users] WHERE {3} ORDER BY [uid] DESC) AS temp ) AND {3} ORDER BY [{1}users].[uid] DESC",
                                                pageSize,
                                                RDBSHelper.RDBSTablePre,
                                                (pageNumber - 1) * pageSize,
                                                condition);
            }

            return RDBSHelper.ExecuteDataset(CommandType.Text, commandText).Tables[0];
        }

        /// <summary>
        /// 后台获得用户列表条件
        /// </summary>
        /// <param name="userName">用户名</param>
        /// <param name="email">邮箱</param>
        /// <param name="mobile">手机</param>
        /// <param name="userRid">用户等级</param>
        /// <param name="mallAGid">商城管理员组</param>
        /// <returns></returns>
        public string AdminGetUserListCondition(string userName, string email, string mobile, int userRid, int mallAGid)
        {
            StringBuilder condition = new StringBuilder();

            if (!string.IsNullOrWhiteSpace(userName) && SecureHelper.IsSafeSqlString(userName))
                condition.AppendFormat(" AND [{1}users].[username] like '{0}%' ", userName, RDBSHelper.RDBSTablePre);

            if (!string.IsNullOrWhiteSpace(email) && SecureHelper.IsSafeSqlString(email, false))
                condition.AppendFormat(" AND [{1}users].[email] like '{0}%' ", email, RDBSHelper.RDBSTablePre);

            if (!string.IsNullOrWhiteSpace(mobile) && SecureHelper.IsSafeSqlString(mobile))
                condition.AppendFormat(" AND [{1}users].[mobile] like '{0}%' ", mobile, RDBSHelper.RDBSTablePre);

            if (userRid > 0)
                condition.AppendFormat(" AND [{1}users].[userrid] = {0} ", userRid, RDBSHelper.RDBSTablePre);

            if (mallAGid > 0)
                condition.AppendFormat(" AND [{1}users].[mallagid] = {0} ", mallAGid, RDBSHelper.RDBSTablePre);

            return condition.Length > 0 ? condition.Remove(0, 4).ToString() : "";
        }

        /// <summary>
        /// 后台获得用户列表数量
        /// </summary>
        /// <param name="condition">条件</param>
        /// <returns></returns>
        public int AdminGetUserCount(string condition)
        {
            string commandText;
            if (string.IsNullOrWhiteSpace(condition))
                commandText = string.Format("SELECT COUNT([{0}users].[uid]) FROM [{0}users]", RDBSHelper.RDBSTablePre);
            else
                commandText = string.Format("SELECT COUNT([{0}users].[uid]) FROM [{0}users] WHERE {1}", RDBSHelper.RDBSTablePre, condition);

            return TypeHelper.ObjectToInt(RDBSHelper.ExecuteScalar(CommandType.Text, commandText), 0);
        }

        /// <summary>
        /// 获得用户等级下用户的数量
        /// </summary>
        /// <param name="userRid">用户等级id</param>
        /// <returns></returns>
        public int GetUserCountByUserRid(int userRid)
        {
            DbParameter[] parms = {
                                        GenerateInParam("@userrid", SqlDbType.SmallInt, 2, userRid)    
                                    };
            string commandText = string.Format("SELECT COUNT([uid]) FROM [{0}users] WHERE [userrid]=@userrid",
                                                RDBSHelper.RDBSTablePre);
            return TypeHelper.ObjectToInt(RDBSHelper.ExecuteNonQuery(CommandType.Text, commandText, parms));
        }

        /// <summary>
        /// 更新用户
        /// </summary>
        /// <param name="uid">用户id</param>
        /// <param name="userName">用户名</param>
        /// <param name="nickName">昵称</param>
        /// <param name="avatar">头像</param>
        /// <param name="gender">性别</param>
        /// <param name="realName">真实名称</param>
        /// <param name="bday">出生日期</param>
        /// <param name="idCard">The id card.</param>
        /// <param name="regionId">区域id</param>
        /// <param name="address">所在地</param>
        /// <param name="bio">简介</param>
        /// <returns></returns>
        public bool UpdateUser(int uid, string userName, string nickName, string avatar, int gender, string realName, DateTime bday, string idCard, int regionId, string address, string bio)
        {
            DbParameter[] parms = {
									   GenerateInParam("@username",SqlDbType.NChar,20,userName),
									   GenerateInParam("@nickname",SqlDbType.NChar,20,nickName),
									   GenerateInParam("@avatar",SqlDbType.Char,40,avatar),
									   GenerateInParam("@gender",SqlDbType.TinyInt,1,gender),
                                       GenerateInParam("@realname",SqlDbType.NVarChar,10,realName),
									   GenerateInParam("@bday",SqlDbType.DateTime,8,bday),
									   GenerateInParam("@idcard",SqlDbType.VarChar,18,idCard),
									   GenerateInParam("@regionid",SqlDbType.SmallInt,2,regionId),
									   GenerateInParam("@address",SqlDbType.NVarChar,150,address),
									   GenerateInParam("@bio",SqlDbType.NVarChar,300,bio),
									   GenerateInParam("@uid",SqlDbType.Int,4,uid),
								   };

            return RDBSHelper.ExecuteNonQuery(CommandType.StoredProcedure,
                                              string.Format("{0}updateucenteruser", RDBSHelper.RDBSTablePre),
                                              parms) > 0;
        }

        /// <summary>
        /// 更新用户邮箱
        /// </summary>
        /// <param name="uid">用户id.</param>
        /// <param name="email">邮箱</param>
        public void UpdateUserEmailByUid(int uid, string email)
        {
            DbParameter[] parms = {
									   GenerateInParam("@uid",SqlDbType.Int,4, uid),
									   GenerateInParam("@email",SqlDbType.Char,50, email)
								   };
            RDBSHelper.ExecuteNonQuery(CommandType.StoredProcedure,
                                       string.Format("{0}updateuseremailbyuid", RDBSHelper.RDBSTablePre),
                                       parms);
        }

        /// <summary>
        /// 更新用户手机
        /// </summary>
        /// <param name="uid">用户id.</param>
        /// <param name="mobile">手机</param>
        public void UpdateUserMobileByUid(int uid, string mobile)
        {
            DbParameter[] parms = {
									   GenerateInParam("@uid",SqlDbType.Int,4, uid),
									   GenerateInParam("@mobile",SqlDbType.Char,15, mobile)
								   };
            RDBSHelper.ExecuteNonQuery(CommandType.StoredProcedure,
                                       string.Format("{0}updateusermobilebyuid", RDBSHelper.RDBSTablePre),
                                       parms);
        }

        /// <summary>
        /// 更新用户密码
        /// </summary>
        /// <param name="uid">用户id.</param>
        /// <param name="password">密码</param>
        public void UpdateUserPasswordByUid(int uid, string password)
        {
            DbParameter[] parms = {
									   GenerateInParam("@uid",SqlDbType.Int,4, uid),
									   GenerateInParam("@password",SqlDbType.Char,32, password)
								   };
            RDBSHelper.ExecuteNonQuery(CommandType.StoredProcedure,
                                       string.Format("{0}updateuserpasswordbyuid", RDBSHelper.RDBSTablePre),
                                       parms);
        }

        /// <summary>
        /// 更新用户解禁时间
        /// </summary>
        /// <param name="uid">用户id</param>
        /// <param name="liftBanTime">解禁时间</param>
        public void UpdateUserLiftBanTimeByUid(int uid, DateTime liftBanTime)
        {
            DbParameter[] parms = {
									   GenerateInParam("@uid",SqlDbType.Int,4, uid),
									   GenerateInParam("@liftbantime",SqlDbType.DateTime,8, liftBanTime)
								   };
            RDBSHelper.ExecuteNonQuery(CommandType.StoredProcedure,
                                       string.Format("{0}updateuserliftbantimebyuid", RDBSHelper.RDBSTablePre),
                                       parms);
        }

        /// <summary>
        /// 更新用户等级
        /// </summary>
        /// <param name="uid">用户id</param>
        /// <param name="userRid">用户等级id</param>
        public void UpdateUserRankByUid(int uid, int userRid)
        {
            DbParameter[] parms = {
									   GenerateInParam("@uid",SqlDbType.Int,4, uid),
									   GenerateInParam("@userrid",SqlDbType.SmallInt,2, userRid)
								   };
            RDBSHelper.ExecuteNonQuery(CommandType.StoredProcedure,
                                       string.Format("{0}updateuserrankbyuid", RDBSHelper.RDBSTablePre),
                                       parms);
        }

        /// <summary>
        /// 更新用户在线时间
        /// </summary>
        /// <param name="uid">用户id</param>
        /// <param name="onlineTime">在线时间</param>
        /// <param name="updateTime">更新时间</param>
        public void UpdateUserOnlineTime(int uid, int onlineTime, DateTime updateTime)
        {
            DbParameter[] parms = {
                                    GenerateInParam("@uid", SqlDbType.Int, 4, uid),
                                    GenerateInParam("@onlinetime", SqlDbType.Int, 4, onlineTime),
                                    GenerateInParam("@updatetime", SqlDbType.DateTime, 8, updateTime)
                                   };
            string commandText = string.Format("UPDATE [{0}onlinetime] SET [total]=[total]+@onlinetime,[year]=[year]+@onlinetime,[month]=[month]+@onlinetime,[week]=[week]+@onlinetime,[day]=[day]+@onlinetime,[updatetime]=@updatetime WHERE [uid]=@uid",
                                                RDBSHelper.RDBSTablePre);
            RDBSHelper.ExecuteNonQuery(CommandType.Text, commandText, parms);
        }

        /// <summary>
        /// 通过注册ip获得注册时间
        /// </summary>
        /// <param name="registerIP">注册ip</param>
        /// <returns></returns>
        public DateTime GetRegisterTimeByRegisterIP(string registerIP)
        {
            DbParameter[] parms = {
									GenerateInParam("@registerip",SqlDbType.Char,15, registerIP)
								   };
            return TypeHelper.ObjectToDateTime(RDBSHelper.ExecuteScalar(CommandType.StoredProcedure,
                                                                        string.Format("{0}getregistertimebyregisterip", RDBSHelper.RDBSTablePre),
                                                                        parms), DateTime.Now.AddDays(-1));
        }

        /// <summary>
        /// 获得用户最后访问时间
        /// </summary>
        /// <param name="uid">用户id</param>
        /// <returns></returns>
        public DateTime GetUserLastVisitTimeByUid(int uid)
        {
            DbParameter[] parms = {
									GenerateInParam("@uid",SqlDbType.Int,4, uid)
								   };
            return TypeHelper.ObjectToDateTime(RDBSHelper.ExecuteScalar(CommandType.StoredProcedure,
                                                                        string.Format("{0}getuserlastvisittimebyuid", RDBSHelper.RDBSTablePre),
                                                                        parms));
        }

        #endregion

        #region 用户等级

        /// <summary>
        /// 获得用户等级列表
        /// </summary>
        /// <returns></returns>
        public IDataReader GetUserRankList()
        {
            string commandText = string.Format("SELECT {1} FROM [{0}userranks] ORDER BY [system] DESC",
                                                RDBSHelper.RDBSTablePre,
                                                TableFields.USER_RANKS);
            return RDBSHelper.ExecuteReader(CommandType.Text, commandText);
        }

        /// <summary>
        /// 创建用户等级
        /// </summary>
        public void CreateUserRank(UserRankInfo userRankInfo)
        {
            DbParameter[] parms = {
                                        GenerateInParam("@system", SqlDbType.Int, 4, userRankInfo.System),
                                        GenerateInParam("@title", SqlDbType.NChar,50,userRankInfo.Title),
                                        GenerateInParam("@avatar", SqlDbType.Char,50,userRankInfo.Avatar),
                                        GenerateInParam("@limitdays", SqlDbType.Int,4,userRankInfo.LimitDays)
                                    };
            string commandText = string.Format("INSERT INTO [{0}userranks]([system],[title],[avatar],[limitdays]) VALUES(@system,@title,@avatar,@creditslower,@creditsupper,@limitdays)",
                                                RDBSHelper.RDBSTablePre);
            RDBSHelper.ExecuteNonQuery(CommandType.Text, commandText, parms);
        }

        /// <summary>
        /// 删除用户等级
        /// </summary>
        /// <param name="userRid">用户等级id</param>
        public void DeleteUserRankById(int userRid)
        {
            DbParameter[] parms = {
                                        GenerateInParam("@userrid", SqlDbType.SmallInt, 2, userRid)    
                                    };
            string commandText = string.Format("DELETE FROM [{0}userranks] WHERE [userrid]=@userrid",
                                                RDBSHelper.RDBSTablePre);
            RDBSHelper.ExecuteNonQuery(CommandType.Text, commandText, parms);
        }

        /// <summary>
        /// 更新用户等级
        /// </summary>
        public void UpdateUserRank(UserRankInfo userRankInfo)
        {
            DbParameter[] parms = {
                                        GenerateInParam("@system", SqlDbType.Int, 4, userRankInfo.System),
                                        GenerateInParam("@title", SqlDbType.NChar,50,userRankInfo.Title),
                                        GenerateInParam("@avatar", SqlDbType.Char,50,userRankInfo.Avatar),
                                        GenerateInParam("@limitdays", SqlDbType.Int,4,userRankInfo.LimitDays),
                                        GenerateInParam("@userrid", SqlDbType.SmallInt, 2, userRankInfo.UserRid)    
                                    };

            string commandText = string.Format("UPDATE [{0}userranks] SET [system]=@system,[title]=@title,[avatar]=@avatar,[limitdays]=@limitdays WHERE [userrid]=@userrid",
                                                RDBSHelper.RDBSTablePre);
            RDBSHelper.ExecuteNonQuery(CommandType.Text, commandText, parms);
        }

        #endregion


        #region 登录失败日志

        /// <summary>
        /// 获得登录失败日志
        /// </summary>
        /// <param name="loginIP">登录IP</param>
        /// <returns></returns>
        public IDataReader GetLoginFailLogByIP(long loginIP)
        {
            DbParameter[] parms = {
                                    GenerateInParam("@loginip",SqlDbType.BigInt,8, loginIP)
                                  };
            return RDBSHelper.ExecuteReader(CommandType.StoredProcedure,
                                            string.Format("{0}getloginfaillogbyip", RDBSHelper.RDBSTablePre),
                                            parms);
        }

        /// <summary>
        /// 增加登录失败次数
        /// </summary>
        /// <param name="loginIP">登录IP</param>
        /// <param name="loginTime">登录时间</param>
        public void AddLoginFailTimes(long loginIP, DateTime loginTime)
        {
            DbParameter[] parms = {
                                    GenerateInParam("@loginip",SqlDbType.BigInt,8, loginIP),
                                    GenerateInParam("@lastlogintime",SqlDbType.DateTime,8, loginTime)
                                    };
            string commandText = string.Format("UPDATE [{0}loginfaillogs] SET [failtimes]=[failtimes]+1,[lastlogintime]=@lastlogintime WHERE [loginip]=@loginip",
                                                RDBSHelper.RDBSTablePre);
            if (RDBSHelper.ExecuteNonQuery(CommandType.Text, commandText, parms) < 1)
            {
                commandText = string.Format("INSERT INTO [{0}loginfaillogs]([loginip],[failtimes],[lastlogintime]) VALUES(@loginip,1,@lastlogintime)",
                                             RDBSHelper.RDBSTablePre);
                RDBSHelper.ExecuteNonQuery(CommandType.Text, commandText, parms);
            }
        }

        /// <summary>
        /// 删除登录失败日志
        /// </summary>
        /// <param name="loginIP">登录IP</param>
        public void DeleteLoginFailLogByIP(long loginIP)
        {
            DbParameter[] parms = {
                                    GenerateInParam("@loginip",SqlDbType.BigInt,8, loginIP)
                                   };
            RDBSHelper.ExecuteNonQuery(CommandType.StoredProcedure,
                                       string.Format("{0}deleteloginfaillogbyip", RDBSHelper.RDBSTablePre),
                                       parms);
        }

        #endregion
    }
}
