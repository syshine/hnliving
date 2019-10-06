using Lib.Core;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Timers;
using System.Web;

namespace hnliving.web
{
    public class SiteMonitor
    {
        private static object _lockerLogs = new object();//锁对象
        private static Timer _timerLog = null;

        public static void MonitorLogs(bool enable = true)
        {
            if (enable)
            {
                if (_timerLog == null)
                {
                    _timerLog = new System.Timers.Timer();
                    _timerLog.Enabled = true;
                    _timerLog.Interval = 60000; //执行间隔时间,单位为毫秒; 这里实际间隔为60分钟
                    _timerLog.Elapsed += new System.Timers.ElapsedEventHandler(TimerEvent);
                    _timerLog.Start();
                }
            }
            else
            {
                // 清除定时器
                _timerLog.Stop();
                _timerLog = null;
            }
        }

        private static void TimerEvent(object source, ElapsedEventArgs e)
        {
            Timer src = (Timer)source;

            if (src == _timerLog) // 删除日志事件
            {
                DeleteLogs();
            }
        }

        /// <summary>
        /// 删除日志
        /// </summary>
        private static void DeleteLogs()
        {
            lock (_lockerLogs)
            {
                string path = IOHelper.GetMapPath("/App_Data/txtlogs/");

                DirectoryInfo dirLogs = new DirectoryInfo(path);
                FileInfo[] files = dirLogs.GetFiles();

                DateTime time0 = DateTime.Now.AddDays(MngConfig.SiteConfig.LogSaveDays * -1);
                for (int i = 0; i < files.Length; i++)
                {
                    DateTime time = files[i].CreationTimeUtc;
                    // 文件创建时间小于删除时间则删除
                    if (DateTime.Compare(time, time0) < 0)
                    {
                        File.Delete(files[i].FullName);
                    }
                }
            }
        }
    }
}