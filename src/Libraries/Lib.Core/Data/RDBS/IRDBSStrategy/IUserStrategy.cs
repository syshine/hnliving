using System;
using System.Data;

namespace Lib.Core
{
    /// <summary>
    /// 关系数据库策略之用户分部接口
    /// </summary>
    public partial interface IRDBSStrategy
    {
        #region 在线用户

        /// <summary>
        /// 创建在线用户
        /// </summary>
        int CreateOnlineUser(OnlineUserInfo onlineUserInfo);

        /// <summary>
        /// 更新在线用户ip
        /// </summary>
        /// <param name="olId">在线用户id</param>
        /// <param name="ip">ip</param>
        void UpdateOnlineUserIP(int olId, string ip);

        /// <summary>
        /// 更新在线用户uid
        /// </summary>
        /// <param name="olId">在线用户id</param>
        /// <param name="ip">uid</param>
        void UpdateOnlineUserUid(int olId, int uid);

        /// <summary>
        /// 获得在线用户
        /// </summary>
        /// <param name="sid">sessionId</param>
        /// <returns></returns>
        IDataReader GetOnlineUserBySid(string sid);

        /// <summary>
        /// 获得在线用户数量
        /// </summary>
        /// <param name="userType">在线用户类型</param>
        /// <returns></returns>
        int GetOnlineUserCount(int userType);

        /// <summary>
        /// 删除在线用户
        /// </summary>
        /// <param name="sid">sessionId</param>
        void DeleteOnlineUserBySid(string sid);

        /// <summary>
        /// 删除过期在线用户
        /// </summary>
        /// <param name="onlineUserExpire">过期时间</param>
        void DeleteExpiredOnlineUser(int onlineUserExpire);

        /// <summary>
        /// 重置在线用户表
        /// </summary>
        void ResetOnlineUserTable();

        /// <summary>
        /// 获得在线用户列表
        /// </summary>
        /// <param name="pageSize">每页数</param>
        /// <param name="pageNumber">当前页数</param>
        /// <param name="locationType">位置类型(0代表省,1代表市,2代表区或县)</param>
        /// <param name="locationId">位置id</param>
        /// <returns></returns>
        IDataReader GetOnlineUserList(int pageSize, int pageNumber, int locationType, int locationId);

        /// <summary>
        /// 获得在线用户数量
        /// </summary>
        /// <param name="locationType">位置类型(0代表省,1代表市,2代表区或县)</param>
        /// <param name="locationId">位置id</param>
        /// <returns></returns>
        int GetOnlineUserCount(int locationType, int locationId);

        #endregion

        #region 用户

        /// <summary>
        /// 获得部分用户
        /// </summary>
        /// <param name="uid">用户id</param>
        /// <returns></returns>
        IDataReader GetPartUserById(int uid);

        /// <summary>
        /// 获得用户
        /// </summary>
        /// <param name="uid">用户id</param>
        /// <returns></returns>
        IDataReader GetUserById(int uid);

        /// <summary>
        /// 获得用户细节
        /// </summary>
        /// <param name="uid">用户id</param>
        /// <returns></returns>
        IDataReader GetUserDetailById(int uid);

        /// <summary>
        /// 获得部分用户
        /// </summary>
        /// <param name="userName">用户名</param>
        /// <returns></returns>
        IDataReader GetPartUserByName(string userName);

        /// <summary>
        /// 获得部分用户
        /// </summary>
        /// <param name="email">用户邮箱</param>
        /// <returns></returns>
        IDataReader GetPartUserByEmail(string email);

        /// <summary>
        /// 获得部分用户
        /// </summary>
        /// <param name="mobile">用户手机</param>
        /// <returns></returns>
        IDataReader GetPartUserByMobile(string mobile);

        /// <summary>
        /// 获得用户id
        /// </summary>
        /// <param name="userName">用户名</param>
        /// <returns></returns>
        int GetUidByUserName(string userName);

        /// <summary>
        /// 获得用户id
        /// </summary>
        /// <param name="email">用户邮箱</param>
        /// <returns></returns>
        int GetUidByEmail(string email);

        /// <summary>
        /// 获得用户id
        /// </summary>
        /// <param name="mobile">用户手机</param>
        /// <returns></returns>
        int GetUidByMobile(string mobile);

        /// <summary>
        /// 创建用户
        /// </summary>
        /// <returns></returns>
        int CreateUser(UserInfo userInfo);

        /// <summary>
        /// 更新用户
        /// </summary>
        /// <returns></returns>
        void UpdateUser(UserInfo userInfo);

        /// <summary>
        /// 更新部分用户
        /// </summary>
        /// <returns></returns>
        void UpdatePartUser(PartUserInfo partUserInfo);

        /// <summary>
        /// 更新用户细节
        /// </summary>
        /// <returns></returns>
        void UpdateUserDetail(UserDetailInfo userDetailInfo);

        /// <summary>
        /// 更新用户最后访问
        /// </summary>
        /// <param name="uid">用户id</param>
        /// <param name="visitTime">访问时间</param>
        /// <param name="ip">ip</param>
        /// <param name="regionId">区域id</param>
        void UpdateUserLastVisit(int uid, DateTime visitTime, string ip, int regionId);

        /// <summary>
        /// 后台获得用户列表
        /// </summary>
        /// <param name="pageSize">每页数</param>
        /// <param name="pageNumber">当前页数</param>
        /// <param name="condition">条件</param>
        /// <returns></returns>
        DataTable AdminGetUserList(int pageSize, int pageNumber, string condition);

        /// <summary>
        /// 后台获得用户列表条件
        /// </summary>
        /// <param name="userName">用户名</param>
        /// <param name="email">邮箱</param>
        /// <param name="mobile">手机</param>
        /// <param name="userRid">用户等级</param>
        /// <param name="mallAGid">商城管理员组</param>
        /// <returns></returns>
        string AdminGetUserListCondition(string userName, string email, string mobile, int userRid, int mallAGid);

        /// <summary>
        /// 后台获得用户列表数量
        /// </summary>
        /// <param name="condition">条件</param>
        /// <returns></returns>
        int AdminGetUserCount(string condition);

        /// <summary>
        /// 获得用户等级下用户的数量
        /// </summary>
        /// <param name="userRid">用户等级id</param>
        /// <returns></returns>
        int GetUserCountByUserRid(int userRid);

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
        bool UpdateUser(int uid, string userName, string nickName, string avatar, int gender, string realName, DateTime bday, string idCard, int regionId, string address, string bio);

        /// <summary>
        /// 更新用户邮箱
        /// </summary>
        /// <param name="uid">用户id.</param>
        /// <param name="email">邮箱</param>
        void UpdateUserEmailByUid(int uid, string email);

        /// <summary>
        /// 更新用户手机
        /// </summary>
        /// <param name="uid">用户id.</param>
        /// <param name="mobile">手机</param>
        void UpdateUserMobileByUid(int uid, string mobile);

        /// <summary>
        /// 更新用户密码
        /// </summary>
        /// <param name="uid">用户id.</param>
        /// <param name="password">密码</param>
        void UpdateUserPasswordByUid(int uid, string password);

        /// <summary>
        /// 更新用户解禁时间
        /// </summary>
        /// <param name="uid">用户id</param>
        /// <param name="liftBanTime">解禁时间</param>
        void UpdateUserLiftBanTimeByUid(int uid, DateTime liftBanTime);

        /// <summary>
        /// 更新用户等级
        /// </summary>
        /// <param name="uid">用户id</param>
        /// <param name="userRid">用户等级id</param>
        void UpdateUserRankByUid(int uid, int userRid);

        /// <summary>
        /// 更新用户在线时间
        /// </summary>
        /// <param name="uid">用户id</param>
        /// <param name="onlineTime">在线时间</param>
        /// <param name="updateTime">更新时间</param>
        void UpdateUserOnlineTime(int uid, int onlineTime, DateTime updateTime);

        /// <summary>
        /// 通过注册ip获得注册时间
        /// </summary>
        /// <param name="registerIP">注册ip</param>
        /// <returns></returns>
        DateTime GetRegisterTimeByRegisterIP(string registerIP);

        /// <summary>
        /// 获得用户最后访问时间
        /// </summary>
        /// <param name="uid">用户id</param>
        /// <returns></returns>
        DateTime GetUserLastVisitTimeByUid(int uid);

        #endregion

        #region 用户等级

        /// <summary>
        /// 获得用户等级列表
        /// </summary>
        /// <returns></returns>
        IDataReader GetUserRankList();

        /// <summary>
        /// 创建用户等级
        /// </summary>
        void CreateUserRank(UserRankInfo userRankInfo);

        /// <summary>
        /// 删除用户等级
        /// </summary>
        /// <param name="userRid">用户等级id</param>
        void DeleteUserRankById(int userRid);

        /// <summary>
        /// 更新用户等级
        /// </summary>
        void UpdateUserRank(UserRankInfo userRankInfo);

        #endregion
        
        #region 登录失败日志

        /// <summary>
        /// 获得登录失败日志
        /// </summary>
        /// <param name="loginIP">登录IP</param>
        /// <returns></returns>
        IDataReader GetLoginFailLogByIP(long loginIP);

        /// <summary>
        /// 增加登录失败次数
        /// </summary>
        /// <param name="loginIP">登录IP</param>
        /// <param name="loginTime">登录时间</param>
        void AddLoginFailTimes(long loginIP, DateTime loginTime);

        /// <summary>
        /// 删除登录失败日志
        /// </summary>
        /// <param name="loginIP">登录IP</param>
        void DeleteLoginFailLogByIP(long loginIP);

        #endregion
    }
}
