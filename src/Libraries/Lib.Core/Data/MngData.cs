using System;
using System.IO;

namespace Lib.Core
{
    /// <summary>
    /// 数据管理类
    /// </summary>
    public partial class MngData
    {
        private static IRDBSStrategy _irdbsstrategy = null;//关系型数据库策略

        private static object _locker = new object();//锁对象
        private static bool _enablednosql = false;//是否启用非关系型数据库
        private static IUserNOSQLStrategy _iusernosqlstrategy = null;//用户非关系型数据库策略

        static MngData()
        {
            try
            {
                string[] fileNameList = Directory.GetFiles(System.Web.HttpRuntime.BinDirectory, "HNLiving.RDBSStrategy.*.dll", SearchOption.TopDirectoryOnly);
                _irdbsstrategy = (IRDBSStrategy)Activator.CreateInstance(Type.GetType(string.Format("HNLiving.RDBSStrategy.{0}.RDBSStrategy, HNLiving.RDBSStrategy.{0}", fileNameList[0].Substring(fileNameList[0].LastIndexOf("RDBSStrategy.") + 13).Replace(".dll", "")),
                                                                                            false,
                                                                                            true));
            }
            catch
            {
                throw new BaseException("创建'关系数据库策略对象'失败,可能存在的原因:未将'关系数据库策略程序集'添加到bin目录中;'关系数据库策略程序集'文件名不符合'HNLiving.RDBSStrategy.{策略名称}.dll'格式");
            }
            _enablednosql = Directory.GetFiles(System.Web.HttpRuntime.BinDirectory, "HNLiving.NOSQLStrategy.*.dll", SearchOption.TopDirectoryOnly).Length > 0;
        }

        /// <summary>
        /// 关系型数据库
        /// </summary>
        public static IRDBSStrategy RDBS
        {
            get { return _irdbsstrategy; }
        }

        /// <summary>
        /// 用户非关系型数据库
        /// </summary>
        public static IUserNOSQLStrategy UserNOSQL
        {
            get
            {
                if (_enablednosql && MngConfig.RedisNOSQLConfig.User.Enabled == 1)
                {
                    if (_iusernosqlstrategy == null)
                    {
                        lock (_locker)
                        {
                            if (_iusernosqlstrategy == null)
                            {
                                try
                                {
                                    string[] fileNameList = Directory.GetFiles(System.Web.HttpRuntime.BinDirectory, "HNLiving.NOSQLStrategy.*.dll", SearchOption.TopDirectoryOnly);
                                    _iusernosqlstrategy = (IUserNOSQLStrategy)Activator.CreateInstance(Type.GetType(string.Format("HNLiving.NOSQLStrategy.{0}.UserNOSQLStrategy, HNLiving.NOSQLStrategy.{0}", fileNameList[0].Substring(fileNameList[0].LastIndexOf("NOSQLStrategy.") + 14).Replace(".dll", "")),
                                                                                                                          false,
                                                                                                                          true));
                                }
                                catch
                                {
                                    throw new BaseException("创建'用户非关系数据库策略对象'失败,可能存在的原因:未将'用户非关系数据库策略程序集'添加到bin目录中;'用户非关系数据库策略程序集'文件名不符合'HNLiving.NOSQLStrategy.{策略名称}.dll'格式");
                                }
                            }
                        }
                    }
                }
                return _iusernosqlstrategy;
            }
        }
    }
}
