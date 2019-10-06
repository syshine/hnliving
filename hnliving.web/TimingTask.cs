using Lib.Core;
using Lib.Services;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Timers;
using System.Web;
using System.Xml;

namespace hnliving.web
{
    public class TimingTask
    {
        private static object _lockerQxc = new object();//锁对象
        private static object _lockerPl5 = new object();//锁对象

        private static QxcEntity _qxc = null;
        private static Pl5Entity _pl5 = null;

        private static bool _bMonitorQxc = false;
        private static bool _bMonitorPl5 = false;

        private static Timer _timerLtr = null;

        public static QxcEntity Qxc
        {
            get
            {
                if(_qxc == null)
                    _qxc = Ltr.GetCurrentQxc();

                return _qxc;
            }
        }

        public static Pl5Entity Pl5
        {
            get
            {
                if (_pl5 == null)
                    _pl5 = Ltr.GetCurrentPl5();

                return _pl5;
            }
        }

        public static void MonitorQxc(bool enable = true)
        {
            if (enable)
            {
                _bMonitorQxc = true;

                if (_timerLtr == null)
                {
                    _timerLtr = new System.Timers.Timer();
                    _timerLtr.Enabled = true;
                    _timerLtr.Interval = 300000; //执行间隔时间,单位为毫秒; 这里实际间隔为5分钟
                    _timerLtr.Elapsed += new System.Timers.ElapsedEventHandler(DownloadLtr);
                    _timerLtr.Start();
                }
            }
            else
            {
                _bMonitorQxc = false;

                // 如果都关闭了，那么清除定时器
                if(_bMonitorPl5 == false && _timerLtr != null)
                {
                    _timerLtr.Stop();
                    _timerLtr = null;
                }
            }
        }
        public static void MonitorPl5(bool enable = true)
        {
            if (enable)
            {
                _bMonitorPl5 = true;

                if (_timerLtr == null)
                {
                    _timerLtr = new System.Timers.Timer();
                    _timerLtr.Enabled = true;
                    _timerLtr.Interval = 300000; //执行间隔时间,单位为毫秒; 这里实际间隔为5分钟
                    _timerLtr.Elapsed += new System.Timers.ElapsedEventHandler(DownloadLtr);
                    _timerLtr.Start();
                }
            }
            else
            {
                _bMonitorPl5 = false;

                // 如果都关闭了，那么清除定时器
                if (_bMonitorQxc == false && _timerLtr != null)
                {
                    _timerLtr.Stop();
                    _timerLtr = null;
                }
            }
        }

        private static void DownloadLtr(object source, ElapsedEventArgs e)
        {
            // 下载Qxc
            if(_bMonitorQxc)
            {
                DownloadQxc();
            }

            // 下载Pl5
            if (_bMonitorPl5)
            {
                DownloadPl5();
            }
        }

        private static void DownloadQxc()
        {
            lock (_lockerQxc)
            {
                // 获取当前期号数据
                _qxc = Ltr.GetCurrentQxc();

                bool bAcquire = false; // 是否获取数据
                if (DateTime.Now.Date > Qxc.Date) // 日期已经超过开奖日
                    bAcquire = true;
                else if (DateTime.Now.Date == Qxc.Date)
                {
                    // 8点半开奖
                    if (DateTime.Now.Hour > 20)
                        bAcquire = true;
                    else if (DateTime.Now.Hour == 20 && DateTime.Now.Minute >= 30)
                        bAcquire = true;
                }

                if (bAcquire)
                {
                    try
                    {
                        bool bOK = false;
                        int cnt = 5; // 获取数据量
                        do
                        {
                            string uri = "http://datachart.500.com/qxc/history/inc/history.php?limit=" + cnt;
                            string pageHtml = WebHelper.GetRequestData(uri, "get", "", Encoding.GetEncoding("gb2312"));
                            int start = pageHtml.IndexOf("class=\"chart\"");
                            string strReg = pageHtml.Substring(start);
                            // 模板
                            string pattern = @"<table(\s|\S)+?</table>";    // 匹配<table>的内容（非贪婪算法）

                            // 匹配
                            MatchCollection mc = Regex.Matches(strReg, pattern);

                            // 解析数据
                            DataTable dt = ParsingLtrData(mc[0].Value, 7);

                            // 如果数据不够，则多获取20条
                            int minIssue = Convert.ToInt32(dt.Compute("min(issue)", ""));
                            if (minIssue > Qxc.Issue)
                            {
                                cnt += 20;
                            }
                            else
                            {
                                // 选出新的数据
                                DataRow[] drs = dt.Select("issue >= " + Qxc.Issue, "issue asc");

                                // 保存到数据库
                                for (int i = 0; i < drs.Length; i++)
                                {
                                    QxcEntity entity = new QxcEntity()
                                    {
                                        Issue = Convert.ToInt32(drs[i]["issue"]),
                                        Date = Convert.ToDateTime(drs[i]["ltr_date"]),
                                        Numb = drs[i]["numbers"].ToString()
                                    };

                                    if (Lib.Services.Ltr.AddQxcNumb(entity) != 1)
                                    {
                                        throw new Exception("新增Qxc失败！");
                                    }
                                }
                            }
                        } while (!bOK);
                    }
                    catch (Exception ex)
                    {
                        Logs.Write(ex.Message);
                    }
                }
            }
        }

        private static void DownloadPl5()
        {
            lock (_lockerPl5)
            {
                // 获取当前期号数据
                _pl5 = Ltr.GetCurrentPl5();

                bool bAcquire = false; // 是否获取数据
                if (DateTime.Now.Date > Pl5.Date) // 日期已经超过开奖日
                    bAcquire = true;
                else if (DateTime.Now.Date == Pl5.Date)
                {
                    // 8点半开奖
                    if (DateTime.Now.Hour > 20)
                        bAcquire = true;
                    else if (DateTime.Now.Hour == 20 && DateTime.Now.Minute >= 30)
                        bAcquire = true;
                }

                if (bAcquire)
                {
                    try
                    {
                        bool bOK = false;
                        int cnt = 5; // 获取数据量
                        do
                        {
                            string uri = "http://datachart.500.com/plw/history/inc/history.php?limit=" + cnt;
                            string pageHtml = WebHelper.GetRequestData(uri, "get", "", Encoding.GetEncoding("gb2312"));
                            int start = pageHtml.IndexOf("class=\"chart\"");
                            string strReg = pageHtml.Substring(start);
                            // 模板
                            string pattern = @"<table(\s|\S)+?</table>";    // 匹配<table>的内容（非贪婪算法）

                            // 匹配
                            MatchCollection mc = Regex.Matches(strReg, pattern);

                            // 解析数据
                            DataTable dt = ParsingLtrData(mc[0].Value, 5);

                            // 如果数据不够，则多获取20条
                            int minIssue = Convert.ToInt32(dt.Compute("min(issue)", ""));
                            if (minIssue > Pl5.Issue)
                            {
                                cnt += 20;
                            }
                            else
                            {
                                // 选出新的数据
                                DataRow[] drs = dt.Select("issue >= " + Pl5.Issue, "issue asc");

                                // 保存到数据库
                                for (int i = 0; i < drs.Length; i++)
                                {
                                    Pl5Entity entity = new Pl5Entity()
                                    {
                                        Issue = Convert.ToInt32(drs[i]["issue"]),
                                        Date = Convert.ToDateTime(drs[i]["ltr_date"]),
                                        Numb = drs[i]["numbers"].ToString()
                                    };

                                    if (Lib.Services.Ltr.AddPl5Numb(entity) != 1)
                                    {
                                        throw new Exception("新增Pl5失败！");
                                    }
                                }
                            }
                        } while (!bOK);
                    }
                    catch (Exception ex)
                    {
                        Logs.Write(ex.Message);
                    }
                }
            }
        }

        /// <summary>
        /// 解析数据
        /// </summary>
        /// <param name="strTable">XML表格数据</param>
        /// <param name="numbLength">数字长度</param>
        /// <returns></returns>
        public static DataTable ParsingLtrData(string strTable, int numbLength)
        {
            DataTable dtResult = new DataTable();
            dtResult.Columns.Add("issue", typeof(int));
            dtResult.Columns.Add("numbers");
            dtResult.Columns.Add("ltr_date", typeof(DateTime));

            try
            {
                //初始化一个xml实例
                XmlDocument xml = new XmlDocument();
                xml.LoadXml(strTable);

                //指定一个节点
                //XmlNode root = xml.SelectSingleNode("/body/table");

                //获取同名同级节点集合
                //XmlNodeList nodelist = xml.SelectNodes("/table");

                // 数据格式 
                //<tr class="t_tr1">
                //    <!--<td>2</td>-->
                //    <td class="t_tr1">19112</td>
                //    <td class="cfont2">1 4 1 7 8 9 2<span class="cBlue"> </span></td>
                //    <td>32</td>
                //    <td class="t_tr1">11716896</td>
                //    <td class="t_tr1">2019-09-24</td>
                //</tr>
                XmlNodeList trNodelist = xml.SelectNodes("//tr");
                for(int i = 1; i < trNodelist.Count; i++)
                {
                    // 解析数据
                    XmlNodeList tdNodelist = trNodelist[i].ChildNodes;//.SelectNodes("//td");
                    int issue = int.Parse(tdNodelist[1].InnerText.Trim());
                    string numbers = tdNodelist[2].InnerText.Replace(" ", "");
                    DateTime ltr_date = DateTime.Parse(tdNodelist[5].InnerText.Trim());

                    // 号码个数
                    if(numbers.Length != numbLength)
                    {
                        throw new Exception("数据错误！");
                    }

                    // 如果不全是数字，则错误
                    int temp;
                    if (!int.TryParse(numbers, out temp))
                    {
                        throw new Exception("数据错误！");
                    }

                    // 添加到结果集
                    DataRow drNew = dtResult.NewRow();
                    drNew["issue"] = issue;
                    drNew["numbers"] = numbers;
                    drNew["ltr_date"] = ltr_date.ToString("yyyy-MM-dd");
                    dtResult.Rows.Add(drNew);
                }
            }
            catch (Exception ex)
            {
                Logs.Write(ex.Message);
                return null;
            }

            return dtResult;
        }
    }
}