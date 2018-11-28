using System;
using System.IO;

namespace Lib.Core
{
    /// <summary>
    /// HNLiving日志管理类
    /// </summary>
    public class MngLog
    {
        private static ILogStrategy _ilogstrategy = null;//日志策略

        static MngLog()
        {
            try
            {
                string[] fileNameList = Directory.GetFiles(System.Web.HttpRuntime.BinDirectory, "HNLiving.LogStrategy.*.dll", SearchOption.TopDirectoryOnly);
                _ilogstrategy = (ILogStrategy)Activator.CreateInstance(Type.GetType(string.Format("HNLiving.LogStrategy.{0}.LogStrategy, HNLiving.LogStrategy.{0}", fileNameList[0].Substring(fileNameList[0].IndexOf("LogStrategy.") + 12).Replace(".dll", "")),
                                                                                    false,
                                                                                    true));
            }
            catch
            {
                throw new BaseException("创建'日志策略对象'失败,可能存在的原因:未将'日志策略程序集'添加到bin目录中;'日志策略程序集'文件名不符合'HNLiving.LogStrategy.{策略名称}.dll'格式");
            }
        }

        /// <summary>
        /// 日志策略实例
        /// </summary>
        public static ILogStrategy Instance
        {
            get { return _ilogstrategy; }
        }
    }
}
