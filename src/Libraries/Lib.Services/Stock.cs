using Lib.Core;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Lib.Services
{
    public class Stock
    {
        private static object _lockerStockBase = new object();//锁对象，保存个股基本信息
        private static object _lockerStockHis = new object();//锁对象,保存个股历史

        public static string GetHistoryFromWeb(string stockCode, DateTime start, DateTime end, string type = "cn")
        {
            string url = @"http://q.stock.sohu.com/hisHq?code={3}_{0}&start={1}&end={2}";
            url = string.Format(url, stockCode, start.ToString("yyyyMMdd"), end.ToString("yyyyMMdd"), type);

            try
            {
                HttpClient client = new HttpClient();
                Task<string> ret = client.GetStringAsync(url);
                
                string result = ret.Result;

                return result;

                //if (type == "zs") et.Code = "{0}_{1}".F("Index", et.Code);//加前缀区分
                //et.Insert();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return e.Message;
            }
            //WebClient client = new WebClient();
            ////client.Timeout = 1000 * 120;
            //var text = client.GetHtml(url);
            //var doc = new HtmlDocument();
            //doc.LoadHtml(text);
            //var value = doc.DocumentNode.InnerText;
            //var et = new StockHisText()
            //{
            //    Code = stockCode,
            //    Start = start,
            //    End = end,
            //    HisText = value
            //};
        }


        public static string GetStockBaseInfo(int page = 1, int count = int.MaxValue)
        {
            string url = @"http://nufm.dfcfw.com/EM_Finance2014NumericApplication/JS.aspx/JS.aspx?type=ct&st=(FFRank)&sr=1&p="
                        + page+"&ps="+ count + @"&js=var%20mozselQI={pages:(pc),data:[(x)]}&token=894050c76af8597a853f5b408b759f5d&cmd=C._AB&sty=DCFFITAM&rt=49461817";
            
            try
            {
                HttpClient client = new HttpClient();
                Task<string> ret = client.GetStringAsync(url);
                // 返回的格式：
                // var mozselQI={pages:1,data:["1,603530,神马电力,13.78,100.00,1,9.98,74.34,4,61.17,76.18,4,-,输配电气,BK04571,2019-08-12 15:00:00","1,603115,海星股份,16.13,100.00,2,10.03,95.30,1,-,95.30,1,-,材料行业,BK05371,2019-08-12 15:00:00"]}
                string result = ret.Result.Substring(ret.Result.IndexOf('=') + 1);
                return result;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return e.Message;
            }
        }

        public static int SaveStockBaseInfo(int page = 1, int count = int.MaxValue)
        {
            lock (_lockerStockBase)
            {
                string result = GetStockBaseInfo();

                // 返回的格式：
                // var mozselQI={pages:1,data:["1,603530,神马电力,13.78,100.00,1,9.98,74.34,4,61.17,76.18,4,-,输配电气,BK04571,2019-08-12 15:00:00","1,603115,海星股份,16.13,100.00,2,10.03,95.30,1,-,95.30,1,-,材料行业,BK05371,2019-08-12 15:00:00"]}

                JObject json = JObject.Parse(result);
                JArray ja = (JArray)json["data"];

                string split = "&"; // 分隔符
                Lib.Core.MngData.RDBS.SaveStockBaseInfo(string.Join(split, ja), split);
            }

            return 1;
        }

        /// <summary>
        /// 获取个股历史数据
        /// </summary>
        /// <param name="type">0为沪市，1为深市</param>
        /// <param name="stockCode">股票代码</param>
        /// <param name="start">开始时间</param>
        /// <param name="end">结束时间</param>
        /// <returns></returns>
        public static string GetStockHis(string type, string stockCode, DateTime start, DateTime end)
        {
            string url = @"http://quotes.money.163.com/service/chddata.html?code={0}{1}&start={2}&end={3}&fields=TCLOSE;HIGH;LOW;TOPEN;LCLOSE;CHG;PCHG;TURNOVER;VOTURNOVER;VATURNOVER;TCAP;MCAP";
            url = string.Format(url, type, stockCode, start.ToString("yyyyMMdd"), end.ToString("yyyyMMdd"));

            try
            {
                HttpClient client = new HttpClient();
                Task<Stream> ret = client.GetStreamAsync(url);

                // 获取结果流并转换成字符串
                Stream stream = ret.Result;
                StreamReader reader = new StreamReader(stream, Encoding.Default);
                string result = reader.ReadToEnd();
                return result;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return e.Message;
            }
        }

        /// <summary>
        /// 保存个股历史数据
        /// </summary>
        /// <returns></returns>
        public static string SaveStockHis(List<string> lstCode = null)
        {
            List<string> lstMsg = new List<string>();
            int cntData = 0;
            DateTime dtStartSave = DateTime.Now;

            //// 设置10个线程取执行保存
            //int nThread = 10;
            //List<Task> lstTask = new List<Task>();

            lock (_lockerStockBase)
            {
                // Tables[0]是股票代码及所处市场（沪深），Tables[1]是沪市的最后更新时间，Tables[2]是深市的最后更新时间
                DataSet dsStock = null;
                if(lstCode == null )    // 空则保存全部
                    dsStock = Lib.Core.MngData.RDBS.GetAllStock();
                else // 否则只保存列表里的编号
                    dsStock = Lib.Core.MngData.RDBS.GetPartStock(lstCode);

                if (dsStock.Tables.Count != 3)
                {
                    return "获取股票基本信息失败";
                }
                //DataTable dtStock = dsStock.Tables[0];

                #region // 不能用多线程处理，会造成死锁
                //// 每个线程执行的个股数量
                //int cnt = dtStock.Rows.Count / nThread + 1;

                //// 设置多个线程取执行保存
                //for (int i = 0; i < nThread; i++)
                //{
                //    // 选取从cnt * i开始的cnt个结果并转换成数据集
                //    DataTable dtStockPart = dtStock.AsEnumerable().Skip(cnt * i).Take(cnt).CopyToDataTable();

                //    // 开始任务
                //    //Task task =  new Task(() => SaveHisMethod(dtStockPart));
                //    //Task task = Task.Factory.StartNew<string[]>(() => SaveHisMethod(dtStockPart, (i + 1).ToString()));
                //    Task<List<string>> task = Task<List<string>>.Factory.StartNew(() => SaveHisMethod(dtStockPart, (i + 1).ToString()));
                //    lstTask.Add(task);
                //}

                //// 等待任务结束
                //Task.WaitAll(lstTask.ToArray());
                #endregion

                #region 直接保存
                foreach (DataRow dr in dsStock.Tables[0].Rows)
                {
                    // 进度
                    //double progress = ((dtStock.Rows.IndexOf(dr) + 1.0) / dtStock.Rows.Count) * 100;
                    int progress = Convert.ToInt32(((dsStock.Tables[0].Rows.IndexOf(dr) + 1.0) / dsStock.Tables[0].Rows.Count) * 100);
                    // 为了不影响其他股票数据的保存，所以try写在循环里面
                    try
                    {
                        // 股票代码
                        string code = dr["s_code"].ToString();

                        // 开始时间
                        DateTime dtStart = new DateTime(1991, 1, 2);
                        string strStart = "";
                        string filter = "scode = " + code;
                        if (dr["s_type"].ToString() == "1") // 沪市
                        {
                            DataRow[] drs = dsStock.Tables[1].Select(filter);
                            if (drs.Length > 0)
                                strStart = drs[0]["last_date"].ToString();
                        }
                        else if (dr["s_type"].ToString() == "2") // 深市
                        {
                            DataRow[] drs = dsStock.Tables[2].Select(filter);
                            if (drs.Length > 0)
                                strStart = drs[0]["last_date"].ToString();
                        }
                        else
                        {
                            // Log记录
                            string str = "股票代码{0}类型错误，未进行处理。进度：{1}%！";
                            Logs.Write(string.Format(str, dr["s_code"].ToString(), progress));
                            continue;
                        }

                        if (strStart != "")
                        {
                            if (!DateTime.TryParse(strStart, out dtStart))
                            {
                                dtStart = new DateTime(1991, 1, 2);
                            }
                            else
                            {
                                dtStart = dtStart.AddDays(1);
                                
                                // 如果大于今天或者还在今天4点以前则不下载
                                int n = dtStart.Date.CompareTo(DateTime.Now.Date);
                                if (n > 0 || (n == 0 && DateTime.Now.Hour < 16))
                                {
                                    // Log记录
                                    string msg = "股票代码{0}已经是最新，不用下载。进度：{1}%！";
                                    Logs.Write(string.Format(msg, dr["s_code"].ToString(), progress));
                                    continue;
                                }
                            }

                        }

                        // 类型（沪市或深市）
                        int type = int.Parse(dr["s_type"].ToString());
                        type -= 1; // 因为stock表里保存的沪市是1，深市是2，而接口中要求的沪市是0，深市是1，所以减一

                        // 获取数据
                        string content = GetStockHis(type.ToString(), code, dtStart, DateTime.Now);
                        string data = content.Substring(content.IndexOf("\r\n") + 2).Replace("\r\n", "&").TrimEnd(new char[] {' ', '&'}); // 去掉列名，将回车缓存&分隔

                        #region BulkCopye批量插入数据库
                        // 转成数据集
                        List<StockDataHis> lstEntity = new List<StockDataHis>();
                        string[] arrData = data.Split(new char[] { '&' }, StringSplitOptions.RemoveEmptyEntries);

                        if(arrData.Length == 0)
                        {
                            // Log记录
                            string msg = "股票代码{0}已经是最新不用保存！进度：{1}%！";
                            Logs.Write(string.Format(msg, dr["s_code"].ToString(), progress));
                            continue;
                        }

                        // 添加列表数据
                        for(int i = 0; i < arrData.Length; i++)
                        {
                            lstEntity.Add(new StockDataHis(arrData[i]));
                        }

                        // 批量保存
                        string tableName = string.Format("{0}stock_his_data{1}", RDBSHelper.RDBSTablePre, dr["s_type"].ToString());
                        SqlBulkCopyHelper.BulkInsert(tableName, lstEntity);
                        cntData += arrData.Length;
                        // Log记录
                        string errorMsg = "股票代码{0}保存成功！本次保存{1}条数据。进度：{2}%！";
                        Logs.Write(string.Format(errorMsg, dr["s_code"].ToString(), arrData.Length.ToString(), progress));
                        
                        #endregion

                        #region // 存储过程保存方式(速度太慢，已弃用)
                        //// 保存进数据库
                        //int timeout = 180 + content.Length / 5000;//超时时间，180秒为基础，5000个字加一秒
                        //int count = Lib.Core.MngData.RDBS.SaveStockHis(data, timeout);

                        //if (count >= 0)
                        //{
                        //    cntData += count;
                        //    // Log记录
                        //    string errorMsg = "股票代码{0}保存成功！本次保存{1}条数据。进度：{2}%！";
                        //    Logs.Write(string.Format(errorMsg, dr["s_code"].ToString(), count.ToString(), progress));
                        //}
                        //else
                        //{
                        //    // Log记录
                        //    string errorMsg = "股票代码{0}保存失败！第{1}条数据。进度：{2}%！";
                        //    errorMsg = string.Format(errorMsg, dr["s_code"].ToString(), Math.Abs(count).ToString(), progress);
                        //    Logs.Write(errorMsg);
                        //}
                        #endregion

                    }
                    catch (Exception ex)
                    {
                        string errorMsg = "股票代码{0}保存出错！进度：{1}%！错误消息：{2}";
                        errorMsg = string.Format(errorMsg, dr["s_code"].ToString(), progress, ex.Message);
                        Logs.Write(errorMsg);
                        lstMsg.Add(errorMsg);
                    }
                }
                #endregion
            }

            //for (int i = 0; i < nThread; i++)
            //{
            //    foreach (Task<List<string>> t in lstTask)
            //    {
            //        lstMsg.AddRange(t.Result);
            //    }
            //}
            

            // 计算总耗时
            TimeSpan ts = DateTime.Now - dtStartSave;
            string time = string.Format("总耗时：{0}天{1}小时{2}分{3}秒\r\n", ts.Days, ts.Hours, ts.Minutes, ts.Seconds);


            return time + string.Format("处理{0}条数据。\r\n", cntData) + string.Join("\r\n", lstMsg);
        }

        private static List<string> SaveHisMethod(DataTable dtStock, string nameThread)
        {
            List<string> lstMsg = new List<string>();
            DateTime dtStartSave = DateTime.Now;

            foreach (DataRow dr in dtStock.Rows)
            {
                // 进度
                //double progress = ((dtStock.Rows.IndexOf(dr) + 1.0) / dtStock.Rows.Count) * 100;
                int progress = Convert.ToInt32(((dtStock.Rows.IndexOf(dr) + 1.0) / dtStock.Rows.Count) * 100);
                // 为了不影响其他股票数据的保存，所以try写在循环里面
                try
                {
                    // 股票代码
                    string code = dr["s_code"].ToString();

                    // 开始时间
                    DateTime dtStart = new DateTime(1991, 1, 2);
                    if (dr["last_date"] != null)
                    {
                        if (!DateTime.TryParse(dr["last_date"].ToString(), out dtStart))
                        {
                            dtStart = new DateTime(1991, 1, 2);
                        }
                        else
                        {
                            dtStart = dtStart.AddDays(1);

                            // 如果大于今天或者还在今天4点以前则不下载
                            int n = dtStart.Date.CompareTo(DateTime.Now.Date);
                            if (n > 0 || (n == 0 && DateTime.Now.Hour < 16))
                            {
                                // Log记录
                                string errorMsg = "股票代码{0}已经是最新，不用下载。进度：{1}%！";
                                Logs.Write(string.Format(errorMsg, dr["s_code"].ToString(), progress));
                                continue;
                            }
                        }

                    }

                    // 类型（沪市或深市）
                    int type = int.Parse(dr["s_type"].ToString());
                    type -= 1; // 因为stock表里保存的沪市是1，深市是2，而接口中要求的沪市是0，深市是1，所以减一

                    // 获取数据
                    string content = GetStockHis(type.ToString(), code, dtStart, DateTime.Now);
                    string data = content.Substring(content.IndexOf("\r\n") + 2).Replace("\r\n", "&"); // 去掉列名，将回车缓存&分隔

                    // 保存进数据库
                    int count = Lib.Core.MngData.RDBS.SaveStockHis(data);

                    if (count >= 0)
                    {
                        // Log记录
                        string errorMsg = "股票代码{0}保存成功！本次保存{1}条数据。进度：{2}%！";
                        Logs.Write(string.Format(errorMsg, dr["s_code"].ToString(), count.ToString(), progress));
                    }
                    else
                    {
                        // Log记录
                        string errorMsg = "股票代码{0}保存失败！第{1}条数据。进度：{2}%！";
                        errorMsg = string.Format(errorMsg, dr["s_code"].ToString(), Math.Abs(count).ToString(), progress);
                        lstMsg.Add(errorMsg);
                        Logs.Write(errorMsg);
                    }

                }
                catch (Exception ex)
                {
                    string errorMsg = "股票代码{0}保存出错！进度：{1}%！错误消息：{2}";
                    errorMsg = string.Format(errorMsg, dr["s_code"].ToString(), progress, ex.Message);
                    lstMsg.Add(errorMsg);
                    Logs.Write(errorMsg);
                }
            }


            // 计算总耗时
            TimeSpan ts = DateTime.Now - dtStartSave;
            string time = string.Format("保存股票历史线程{4}耗时：{0}天{1}小时{2}分{3}秒\r\n", ts.Days, ts.Hours, ts.Minutes, ts.Seconds, nameThread);
            Logs.Write(time);

            return lstMsg;
        }
    }
}
