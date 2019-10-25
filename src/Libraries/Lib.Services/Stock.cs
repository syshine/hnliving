using Lib.Core;
using Newtonsoft.Json;
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
        private static object _lockerStockPick = new object();//锁对象,选股

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
        /// <param name="lstCode"></param>
        /// <param name="guid"></param>
        /// <returns></returns>

        public static string SaveStockHis(List<string> lstCode = null, string guid = "")
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

                // 保存总股票数和线程数量到memcache或redis
                InitProcessData(guid, 1, dsStock.Tables[0].Rows.Count, dtStartSave);
                // 保存总股票数和线程数量到memcache或redis
                //MemCachedHelper mch = null;
                //if (MngConfig.SiteConfig.EnableMemcache && !string.IsNullOrWhiteSpace(guid))
                //{
                //    mch = new MemCachedHelper();
                //    mch.Set_date(guid + "_thread_cnt", 1, 120 * 60000);
                //    mch.Set_date(guid + "_total_cnt", dsStock.Tables[0].Rows.Count, 120 * 60000);
                //    mch.Set_date(guid + "_exec_time", dtStartSave, 120 * 60000);
                //}

                #region 直接保存
                //foreach (DataRow dr in dsStock.Tables[0].Rows)
                for (int index = 0; index < dsStock.Tables[0].Rows.Count; index++)
                {
                    DataRow dr = dsStock.Tables[0].Rows[index];

                    // 进度
                    int progress = (int)(((index + 1.0) / dsStock.Tables[0].Rows.Count) * 100);
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
                            // 保存进度到memcache或redis
                            SetProcessData(guid + "_td_pk_1", index + 1);
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

                        // 保存进度到memcache或redis
                        SetProcessData(guid + "_td_pk_1", index + 1);
                        //// 保存进度到memcache
                        //if (MngConfig.SiteConfig.EnableMemcache && !string.IsNullOrWhiteSpace(guid))
                        //{
                        //    mch.Set_date(guid.ToString() + "_td_pk_1", index + 1, 3 * 60000);
                        //}

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

            #region // 读取多线程结果
            //for (int i = 0; i < nThread; i++)
            //{
            //    foreach (Task<List<string>> t in lstTask)
            //    {
            //        lstMsg.AddRange(t.Result);
            //    }
            //}
            #endregion


            // 计算总耗时
            TimeSpan ts = DateTime.Now - dtStartSave;
            string time = string.Format("总耗时：{0}天{1}小时{2}分{3}秒\r\n", ts.Days, ts.Hours, ts.Minutes, ts.Seconds);


            return time + string.Format("处理{0}条数据。\r\n", cntData) + string.Join("\r\n", lstMsg);
        }

        #region //
        //private static List<string> SaveHisMethod(DataTable dtStock, string nameThread)
        //{
        //    List<string> lstMsg = new List<string>();
        //    DateTime dtStartSave = DateTime.Now;

        //    foreach (DataRow dr in dtStock.Rows)
        //    {
        //        // 进度
        //        //double progress = ((dtStock.Rows.IndexOf(dr) + 1.0) / dtStock.Rows.Count) * 100;
        //        int progress = Convert.ToInt32(((dtStock.Rows.IndexOf(dr) + 1.0) / dtStock.Rows.Count) * 100);
        //        // 为了不影响其他股票数据的保存，所以try写在循环里面
        //        try
        //        {
        //            // 股票代码
        //            string code = dr["s_code"].ToString();

        //            // 开始时间
        //            DateTime dtStart = new DateTime(1991, 1, 2);
        //            if (dr["last_date"] != null)
        //            {
        //                if (!DateTime.TryParse(dr["last_date"].ToString(), out dtStart))
        //                {
        //                    dtStart = new DateTime(1991, 1, 2);
        //                }
        //                else
        //                {
        //                    dtStart = dtStart.AddDays(1);

        //                    // 如果大于今天或者还在今天4点以前则不下载
        //                    int n = dtStart.Date.CompareTo(DateTime.Now.Date);
        //                    if (n > 0 || (n == 0 && DateTime.Now.Hour < 16))
        //                    {
        //                        // Log记录
        //                        string errorMsg = "股票代码{0}已经是最新，不用下载。进度：{1}%！";
        //                        Logs.Write(string.Format(errorMsg, dr["s_code"].ToString(), progress));
        //                        continue;
        //                    }
        //                }

        //            }

        //            // 类型（沪市或深市）
        //            int type = int.Parse(dr["s_type"].ToString());
        //            type -= 1; // 因为stock表里保存的沪市是1，深市是2，而接口中要求的沪市是0，深市是1，所以减一

        //            // 获取数据
        //            string content = GetStockHis(type.ToString(), code, dtStart, DateTime.Now);
        //            string data = content.Substring(content.IndexOf("\r\n") + 2).Replace("\r\n", "&"); // 去掉列名，将回车缓存&分隔

        //            // 保存进数据库
        //            int count = Lib.Core.MngData.RDBS.SaveStockHis(data);

        //            if (count >= 0)
        //            {
        //                // Log记录
        //                string errorMsg = "股票代码{0}保存成功！本次保存{1}条数据。进度：{2}%！";
        //                Logs.Write(string.Format(errorMsg, dr["s_code"].ToString(), count.ToString(), progress));
        //            }
        //            else
        //            {
        //                // Log记录
        //                string errorMsg = "股票代码{0}保存失败！第{1}条数据。进度：{2}%！";
        //                errorMsg = string.Format(errorMsg, dr["s_code"].ToString(), Math.Abs(count).ToString(), progress);
        //                lstMsg.Add(errorMsg);
        //                Logs.Write(errorMsg);
        //            }

        //        }
        //        catch (Exception ex)
        //        {
        //            string errorMsg = "股票代码{0}保存出错！进度：{1}%！错误消息：{2}";
        //            errorMsg = string.Format(errorMsg, dr["s_code"].ToString(), progress, ex.Message);
        //            lstMsg.Add(errorMsg);
        //            Logs.Write(errorMsg);
        //        }
        //    }


        //    // 计算总耗时
        //    TimeSpan ts = DateTime.Now - dtStartSave;
        //    string time = string.Format("保存股票历史线程{4}耗时：{0}天{1}小时{2}分{3}秒\r\n", ts.Days, ts.Hours, ts.Minutes, ts.Seconds, nameThread);
        //    Logs.Write(time);

        //    return lstMsg;
        //}
        #endregion


        public static ResultEntity GetDetail(string stockCode)
        {
            ResultEntity result = new ResultEntity();

            try
            {
                // 获取基础信息
                DataSet dsStock = Lib.Core.MngData.RDBS.GetStockInfo(stockCode);

                if (dsStock.Tables[0].Rows.Count > 0)
                {
                    DataRow dr = dsStock.Tables[0].Rows[0];

                    // 获取历史数据
                    DataTable dtStockHis = Lib.Core.MngData.RDBS.GetStockInfoHis(stockCode, dr["s_type"].ToString());

                    // K线数据
                    StockKLineData stockData = new StockKLineData();

                    // 交易所简称
                    string exchange_name = dr["s_type"].ToString() == "1" ? StockDefine.EXCHANGE_SH : StockDefine.EXCHANGE_SZ;

                    // 股票名称简写(需改)
                    string short_name = dr["s_name"].ToString();// "LDZB";

                    stockData.INFO.Add(exchange_name);
                    stockData.INFO.Add(dr["s_code"].ToString());
                    stockData.INFO.Add(dr["s_name"].ToString());
                    stockData.INFO.Add(dr["s_code"].ToString() + "," + short_name);
                    stockData.INFO.Add(3);
                    stockData.INFO.Add(2);
                    stockData.INFO.Add(2);
                    stockData.INFO.Add(19.60);
                    stockData.INFO.Add(21.56);
                    stockData.INFO.Add(17.64);

                    // 日线数据
                    DataRow[] drsTemp = dtStockHis.Select("", "HDATE asc");
                    foreach (DataRow row in drsTemp)
                    {
                        List<object> lst = new List<object>();
                        lst.Add(int.Parse(row["fdate"].ToString()));
                        lst.Add(double.Parse(row["TOPEN"].ToString()));
                        lst.Add(double.Parse(row["HIGH"].ToString()));
                        lst.Add(double.Parse(row["LOW"].ToString()));
                        lst.Add(double.Parse(row["TCLOSE"].ToString()));
                        lst.Add(long.Parse(row["VOTURNOVER"].ToString()));
                        lst.Add(long.Parse(row["VATURNOVER"].ToString()));
                        stockData.DAY.Add(lst);
                    }

                    // 分钟线数据

                    // 设置结果
                    result.SetSuccess(stockData);
                }
                else
                {
                    result.Message = "没有代码为" + stockCode + "的股票";
                }
            }
            catch (Exception ex)
            {
                result.SetError(ex.Message);
            }

            return result;
        }

        /// <summary>
        /// 获取均线数据
        /// </summary>
        /// <param name="stockCode"></param>
        /// <returns></returns>
        public static ResultEntity GetAverageLine(string stockCode)
        {
            ResultEntity result = new ResultEntity();

            try
            {
                // 获取基础信息
                DataSet dsStock = Lib.Core.MngData.RDBS.GetStockInfo(stockCode);

                if (dsStock.Tables[0].Rows.Count > 0)
                {
                    DataRow dr = dsStock.Tables[0].Rows[0];

                    // 获取历史数据
                    DataTable dtStockHis = Lib.Core.MngData.RDBS.GetStockInfoHis(stockCode, dr["s_type"].ToString());

                    // 获取均线数据
                    DataTable stockData = StockHelper.GetAverageLine(dtStockHis);

                    // 转换结果
                    result.SetSuccess(stockData);
                }
                else
                {
                    result.Message = "没有代码为" + stockCode + "的股票";
                }
            }
            catch(Exception ex)
            {
                result.SetError(ex.Message);
            }

            return result;
        }

        /// <summary>
        /// 获取MACD
        /// 指数平滑移动平均线（Moving Average Convergence and Divergence，简称MACD）
        /// </summary>
        /// <param name="stockCode"></param>
        /// <returns></returns>
        public static ResultEntity GetMACD(string stockCode)
        {
            ResultEntity result = new ResultEntity();

            try
            {
                // 获取基础信息
                DataSet dsStock = Lib.Core.MngData.RDBS.GetStockInfo(stockCode);

                if (dsStock.Tables[0].Rows.Count > 0)
                {
                    DataRow dr = dsStock.Tables[0].Rows[0];

                    // 获取历史数据
                    DataTable dtStockHis = Lib.Core.MngData.RDBS.GetStockInfoHis(stockCode, dr["s_type"].ToString());

                    // 获取MACD数据
                    DataTable stockData = StockHelper.GetMACD(dtStockHis);

                    // 转换结果
                    result.SetSuccess(stockData);
                }
                else
                {
                    result.Message = "没有代码为" + stockCode + "的股票";
                }
            }
            catch(Exception ex)
            {
                result.SetError(ex.Message);
            }

            return result;
        }

        /// <summary>
        /// 获取前复权数据
        /// </summary>
        /// <param name="stockCode"></param>
        /// <returns></returns>
        public static ResultEntity GetFormerComplexRights(string stockCode)
        {
            ResultEntity result = new ResultEntity();

            try
            {
                // 获取基础信息
                DataSet dsStock = Lib.Core.MngData.RDBS.GetStockInfo(stockCode);

                if (dsStock.Tables[0].Rows.Count > 0)
                {
                    DataRow dr = dsStock.Tables[0].Rows[0];

                    // 获取历史数据
                    DataTable dtStockHis = Lib.Core.MngData.RDBS.GetStockInfoHis(stockCode, dr["s_type"].ToString());

                    // 获取前复权数据
                    DataTable stockData = StockHelper.GetFormerComplexRights(dtStockHis);

                    // 转换结果
                    result.SetSuccess(stockData);
                }
                else
                {
                    result.Message = "没有代码为" + stockCode + "的股票";
                }
            }
            catch (Exception ex)
            {
                result.SetError(ex.Message);
            }

            return result;
        }

        public static int AddFormula(StockPickEntity spe)
        {
            return Lib.Core.MngData.RDBS.AddFormula(spe);
        }

        public static int UpdateFormula(StockPickEntity spe)
        {
            return Lib.Core.MngData.RDBS.UpdateFormula(spe);
        }

        public static StockPickEntity GetFormulaById(int fid)
        {
            StockPickEntity spe = new StockPickEntity();
            IDataReader reader = Lib.Core.MngData.RDBS.GetFormulaById(fid);
            while (reader.Read())
            {
                spe = ConvertToStockPickEntity(reader);
                //spe.Fid = Convert.ToInt32(reader["fid"]);
                //spe.Uid = Convert.ToInt32(reader["uid"]);
                //spe.Groupid = Convert.ToInt32(reader["Groupid"]);
                //spe.Name = reader["Name"].ToString();
                //if (reader["create_time"] != null)
                //    spe.CreateTime = DateTime.Parse(reader["create_time"].ToString());
                //if (reader["update_time"] != null)
                //    spe.UpdateTime = DateTime.Parse(reader["update_time"].ToString());
                //spe.UseFormerComplexRights = Convert.ToBoolean(reader["UseFormerComplexRights"]);
                //spe.PCHGEnable = Convert.ToBoolean(reader["PCHGEnable"]);
                //spe.Days = Convert.ToInt32(reader["Days"]);
                //spe.IsRise = Convert.ToBoolean(reader["IsRise"]);
                //spe.Operator = reader["Operator"].ToString();
                //spe.PCHG = Convert.ToDecimal(reader["PCHG"]);
                //spe.PriceEnable = Convert.ToBoolean(reader["PriceEnable"]);
                //spe.PriceLow = Convert.ToDecimal(reader["PriceLow"]);
                //spe.PriceHigh = Convert.ToDecimal(reader["PriceHigh"]);
                //spe.MCAPEnable = Convert.ToBoolean(reader["MCAPEnable"]);
                //spe.MCAPLow = Convert.ToDecimal(reader["MCAPLow"]);
                //spe.MCAPHigh = Convert.ToDecimal(reader["MCAPHigh"]);
                //spe.FormulaEnable = Convert.ToBoolean(reader["FormulaEnable"]);
                //spe.PreAvgLines = reader["PreAvgLines"].ToString();
                //spe.PreDays = Convert.ToInt32(reader["PreDays"]);
                //spe.Formula = reader["Formula"].ToString();
            }
            reader.Close();

            return spe;
        }

        public static StockPickEntity ConvertToStockPickEntity(IDataReader reader)
        {
            StockPickEntity spe = new StockPickEntity();
            spe.Fid = Convert.ToInt32(reader["fid"]);
            spe.Uid = Convert.ToInt32(reader["uid"]);
            spe.Groupid = Convert.ToInt32(reader["Groupid"]);
            spe.Fname = reader["fname"].ToString();
            if (reader["create_time"] != null)
                spe.CreateTime = DateTime.Parse(reader["create_time"].ToString());
            if (reader["update_time"] != null)
                spe.UpdateTime = DateTime.Parse(reader["update_time"].ToString());
            spe.UseFormerComplexRights = Convert.ToBoolean(reader["UseFormerComplexRights"]);
            spe.PCHGEnable = Convert.ToBoolean(reader["PCHGEnable"]);
            spe.Days = Convert.ToInt32(reader["Days"]);
            spe.IsRise = Convert.ToBoolean(reader["IsRise"]);
            spe.Operator = reader["Operator"].ToString();
            spe.PCHG = Convert.ToDecimal(reader["PCHG"]);
            spe.PriceEnable = Convert.ToBoolean(reader["PriceEnable"]);
            spe.PriceLow = Convert.ToDecimal(reader["PriceLow"]);
            spe.PriceHigh = Convert.ToDecimal(reader["PriceHigh"]);
            spe.MCAPEnable = Convert.ToBoolean(reader["MCAPEnable"]);
            spe.MCAPLow = Convert.ToDecimal(reader["MCAPLow"]);
            spe.MCAPHigh = Convert.ToDecimal(reader["MCAPHigh"]);
            spe.FormulaEnable = Convert.ToBoolean(reader["FormulaEnable"]);
            spe.PreAvgLines = reader["PreAvgLines"].ToString();
            spe.PreDays = Convert.ToInt32(reader["PreDays"]);
            spe.Formula = reader["Formula"].ToString();
            return spe;
        }

        /// <summary>
        /// 根据用户ID和分组获取列表
        /// </summary>
        /// <param name="uid"></param>
        /// <param name="groupid"></param>
        /// <returns></returns>
        public static List<StockPickEntity> GetFormulaList(PageEntity pager, int uid = -1, int groupid = -1)
        {
            //return Lib.Data.UEditorUtils.GetList(uid);

            List<StockPickEntity> speList = new List<StockPickEntity>();
            IDataReader reader = Lib.Core.MngData.RDBS.GetFormulaList(pager, uid, groupid);
            while (reader.Read())
            {
                StockPickEntity spe = ConvertToStockPickEntity(reader);
                speList.Add(spe);
                //StockPickEntity spe = new StockPickEntity();
                //spe.Fid = int.Parse(reader["Fid"].ToString());
                //spe.Uid = int.Parse(reader["Uid"].ToString());
                //spe.Groupid = int.Parse(reader["Groupid"].ToString());
                //spe.Name = reader["Name"].ToString();
                //if (reader["create_time"] != null)
                //    spe.CreateTime = DateTime.Parse(reader["create_time"].ToString());
                //if (reader["update_time"] != null)
                //    spe.UpdateTime = DateTime.Parse(reader["update_time"].ToString());
                //spe.UseFormerComplexRights = Convert.ToBoolean(reader["UseFormerComplexRights"]);
                //spe.PCHGEnable = Convert.ToBoolean(reader["PCHGEnable"]);
                //spe.Days = Convert.ToInt32(reader["Days"]);
                //spe.IsRise = Convert.ToBoolean(reader["IsRise"]);
                //spe.Operator = reader["Operator"].ToString();
                //spe.PCHG = Convert.ToDecimal(reader["PCHG"]);
                //spe.PriceEnable = Convert.ToBoolean(reader["PriceEnable"]);
                //spe.PriceLow = Convert.ToDecimal(reader["PriceLow"]);
                //spe.PriceHigh = Convert.ToDecimal(reader["PriceHigh"]);
                //spe.MCAPEnable = Convert.ToBoolean(reader["MCAPEnable"]);
                //spe.MCAPLow = Convert.ToDecimal(reader["MCAPLow"]);
                //spe.MCAPHigh = Convert.ToDecimal(reader["MCAPHigh"]);
                //spe.FormulaEnable = Convert.ToBoolean(reader["FormulaEnable"]);
                //spe.PreAvgLines = reader["PreAvgLines"].ToString();
                //spe.PreDays = Convert.ToInt32(reader["PreDays"]);
                //spe.Formula = reader["Formula"].ToString();
            }
            reader.Close();
            return speList;
        }

        public static int DeleteFormulaById(int uid, int fid)
        {
            return Lib.Core.MngData.RDBS.DeleteFormulaById(uid, fid);
        }

        #region 分类

        /// <summary>
        /// 根据用户ID获取公式分组列表
        /// </summary>
        /// <param name="uid"></param>
        /// <returns></returns>
        public static DataTable GetFormulaGroup(int uid)
        {
            return MngData.RDBS.GetFormulaGroupList(uid); ;
        }

        public static int AddFormulaGroup(int uid, string name)
        {
            return MngData.RDBS.AddFormulaGroup(uid, name);
        }

        public static int DeleteFormulaGroupById(int uid, int gid)
        {
            return MngData.RDBS.DeleteFormulaGroupById(uid, gid);
        }

        /// <summary>
        /// 根据ID获取公式分组名称
        /// </summary>
        /// <param name="groupid"></param>
        /// <returns></returns>
        public static string GetFormulaGroupName(int groupid)
        {
            string name = MngData.RDBS.GetFormulaGroupName(groupid);
            return name == null ? "" : name;
        }
        #endregion

        public static ResultEntity Pick(StockPickEntity pickEntity, string guid = "")
        {
            ResultEntity result = new ResultEntity();
            DateTime dtStart = DateTime.Now;

            // 设置5个线程取执行保存
            const int THREAD_COUNT = 5;//5
            List<Task> lstTask = new List<Task>();

            try
            {
                #region //测试
                //DateTime start = DateTime.Now;
                //int times = 1000;//次数
                //int rows = 610;
                //for (int i = 0; i < times; i++)
                //{
                //    // 获取数据 (去掉停牌的数据：[PCHG] <> 'None')
                //    string sql111 = @"select top {1} * from [{0}stock_his_data1] where [SCODE] = '600000' AND [PCHG] <> 'None' order by [HDATE] desc";

                //    string commandText111 = string.Format(sql111, RDBSHelper.RDBSTablePre, rows);

                //    DateTime start111 = DateTime.Now;
                //    DataTable dtHis111 = RDBSHelper.ExecuteDataset(CommandType.Text, commandText111).Tables[0];
                //    // 计算总耗时
                //    TimeSpan dt111 = DateTime.Now - start111;
                //    string time111 = string.Format("选股线程{0}耗时：{1}小时{2}分{3}秒{4}毫秒\r\n", "168", dt111.Hours, dt111.Minutes, dt111.Seconds, dt111.Milliseconds);
                //    System.Diagnostics.Debug.WriteLine(time111);
                //}
                //// 计算总耗时
                //TimeSpan ts000 = DateTime.Now - start;
                //string time000 = string.Format("调用查询{0}次查询{4}行耗时：{1}小时{2}分{3}秒\r\n", times, ts000.Hours, ts000.Minutes, ts000.Seconds, rows);

                //result.SetError(time000);
                //return result;
                #endregion

                #region 获取基础信息
                // 获取基础信息
                string sql = @"select s_type, s_code, s_name, s_price from [{0}stock] order by s_code";

                string commandText = string.Format(sql, RDBSHelper.RDBSTablePre);

                DataTable dtStock = RDBSHelper.ExecuteDataset(CommandType.Text, commandText).Tables[0];
                #endregion

                #region // 测试
                //result = PickStockMethod(dtStock, pickEntity, "135");
                //return result;
                #endregion

                #region 多线程处理
                // 每个线程执行的个股数量
                int cnt = dtStock.Rows.Count / THREAD_COUNT + 1;

                // 设置多个线程取执行保存
                for (int i = 0; i < THREAD_COUNT; i++)
                {
                    // 选取从cnt * i开始的cnt个结果并转换成数据集
                    DataTable dtStockPart = dtStock.AsEnumerable().Skip(cnt * i).Take(cnt).CopyToDataTable();

                    // 开始任务
                    //Task task =  new Task(() => SaveHisMethod(dtStockPart));
                    //Task task = Task.Factory.StartNew<string[]>(() => PickStockMethod(dtStockPart, (i + 1).ToString()));
                    string threadName = guid.ToString() + "_td_pk_" + (i + 1).ToString();
                    Task<ResultEntity> task = Task<ResultEntity>.Factory.StartNew(() => PickStockMethod(dtStockPart, pickEntity, threadName));
                    lstTask.Add(task);
                }

                // 保存总股票数和线程数量到memcache或redis
                InitProcessData(guid, THREAD_COUNT, dtStock.Rows.Count, dtStart);
                //// 保存总股票数和线程数量到memcache
                //if (MngConfig.SiteConfig.EnableMemcache && !string.IsNullOrWhiteSpace(guid))
                //{
                //    MemCachedHelper mch = new MemCachedHelper();
                //    mch.Set_date(guid+"_thread_cnt", THREAD_COUNT, 60 * 60000);
                //    mch.Set_date(guid+"_total_cnt", dtStock.Rows.Count, 60 * 60000);
                //    mch.Set_date(guid + "_exec_time", dtStart, 60 * 60000);
                //}

                // 等待任务结束
                Task.WaitAll(lstTask.ToArray());
                #endregion
                
                #region  处理多线程执行结果
                List<StockShow> lstStock = new List<StockShow>();
                StringBuilder sb = new StringBuilder();
                // 将多线程的结果合并
                foreach (Task<ResultEntity> t in lstTask)
                {
                    ResultEntity resultPart = t.Result;
                    if (resultPart.IsSuccess)
                    {
                        lstStock.AddRange((List<StockShow>)resultPart.Data);
                        sb.Append(resultPart.Message);
                    }
                }
                #endregion

                #region 模拟数据
                //List<StockCodeName> lstStock = new List<StockCodeName>();
                //StockCodeName[] stocks = new StockCodeName[10];
                //stocks[0] = new StockCodeName("000001", "东方答一");
                //stocks[1] = new StockCodeName("000002", "东方答二");
                //stocks[2] = new StockCodeName("000003", "东方答三");
                //stocks[3] = new StockCodeName("000004", "东方答四");
                //stocks[4] = new StockCodeName("000005", "东方答五");
                //stocks[5] = new StockCodeName("000006", "东方答六");
                //stocks[6] = new StockCodeName("000007", "东方答七");
                //stocks[7] = new StockCodeName("000008", "东方答八");
                //stocks[8] = new StockCodeName("000009", "东方答九");
                //stocks[9] = new StockCodeName("000010", "东方答十");
                //lstStock.AddRange(stocks);
                #endregion

                // 转换结果
                result.SetSuccess(lstStock, sb.ToString());
            }
            catch (Exception ex)
            {
                result.SetError(ex.Message);
            }


            //// 保存总股票数和线程数量到memcache
            //if (MngConfig.SiteConfig.EnableMemcache && !string.IsNullOrWhiteSpace(guid))
            //{
            //    MemCachedHelper mch = new MemCachedHelper();
            //    mch.Set_date(guid.ToString() + "_finish", true, 2 * 60000);
            //}
            return result;
        }

        /// <summary>
        /// 挑选股票方法
        /// </summary>
        /// <param name="dtStock"></param>
        /// <param name="condition"></param>
        /// <param name="nameThread"></param>
        /// <returns></returns>
        private static ResultEntity PickStockMethod(DataTable dtStock, StockPickEntity condition, string nameThread)
        {
            ResultEntity result = new ResultEntity();

            MemCachedHelper mch = new MemCachedHelper();

            List<StockShow> lstStock = new List<StockShow>();
            List<string> lstMsg = new List<string>();
            DateTime dtStartPick = DateTime.Now;

            //foreach (DataRow dr in dtStock.Rows)
            for(int i = 0; i < dtStock.Rows.Count; i++)
            {
                // 进度
                int progress = (int)(((i + 1.0) / dtStock.Rows.Count) * 100);
                // 为了不影响其他股票的筛选，所以try写在循环里面
                try
                {
                    // 股票代码
                    string code = dtStock.Rows[i]["s_code"].ToString();
                    // 股票名称
                    string name = dtStock.Rows[i]["s_name"].ToString();
                    
                    // 类型（沪市或深市）
                    int type = int.Parse(dtStock.Rows[i]["s_type"].ToString());
                    type -= 1; // 因为stock表里保存的沪市是1，深市是2，而接口中要求的沪市是0，深市是1，所以减一

                    // 获取的记录条数
                    int cntRecord = 1;
                    cntRecord = condition.Days;
                    string cntSql = "top " + cntRecord;
                    if(condition.FormulaEnable)
                    {
                        if (condition.PreDays >= 0)
                        {
                            string[] arrAvgLines = condition.PreAvgLines.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

                            int days = 0;
                            if (arrAvgLines.Length > 0 )
                            {
                                // 字符串数组转数字数组
                                int[] iAvgLines = Array.ConvertAll(arrAvgLines, int.Parse);

                                days = iAvgLines.Max() + condition.PreDays;
                            }

                            cntRecord = Math.Max(condition.Days, days);

                            cntSql = "top " + cntRecord;
                        }
                        else
                        {
                            cntSql = "";
                        }
                    }

                    #region 获取数据
                    // 获取数据 (去掉停牌的数据：[PCHG] <> 'None')(此处[SCODE] = '{2}'必须加引号，否则执行时间为500ms以上)
                    string sql = @"select {3} *, CONVERT(varchar(8),HDATE, 112) fdate from [{0}stock_his_data{1}] where [SCODE] = '{2}' AND [PCHG] <> 'None' order by [HDATE] desc";

                    string commandText = string.Format(sql, RDBSHelper.RDBSTablePre, dtStock.Rows[i]["s_type"].ToString(), code, cntSql);

                    //DateTime start = DateTime.Now;
                    DataTable dtHis = RDBSHelper.ExecuteDataset(CommandType.Text, commandText).Tables[0];
                    //// 计算总耗时
                    //TimeSpan tsdt = DateTime.Now - start;
                    //string time111 = string.Format("选股线程{0}耗时：{1}小时{2}分{3}秒{4}毫秒\r\n", nameThread, tsdt.Hours, tsdt.Minutes, tsdt.Seconds, tsdt.Milliseconds);
                    //System.Diagnostics.Debug.WriteLine(time111);
                    #endregion

                    // 筛选
                    ResultEntity pickRusult = Lib.Data.Stock.PickStock(code, dtHis, condition);
                    if(pickRusult.IsSuccess)
                    {
                        StockShow stock = new StockShow(code, name, Convert.ToInt32(dtStock.Rows[i]["s_type"]),
                            Convert.ToDecimal(dtStock.Rows[i]["s_price"]), dtHis.Rows[0]["PCHG"].ToString());
                        
                        lstStock.Add(stock);
                    }
                    else
                    {
                        // 添加错误信息
                        if(!string.IsNullOrWhiteSpace(pickRusult.Message))
                            lstMsg.Add(pickRusult.Message);
                    }

                }
                catch (Exception ex)
                {
                    string errorMsg = "线程{0}股票代码{1}选股出错！进度：{2}%！错误消息：{3}！StackTrace:{4}";
                    errorMsg = string.Format(errorMsg, nameThread, dtStock.Rows[i]["s_code"].ToString(), progress, ex.Message, ex.StackTrace);
                    lstMsg.Add(errorMsg);
                    Logs.Write(errorMsg);
                }
                finally
                {
                    //  保存进度到memcache或redis
                    SetProcessData(nameThread, i + 1);
                    ////  保存进度到memcache
                    //if (MngConfig.SiteConfig.EnableMemcache && nameThread.IndexOf('_') != 0)
                    //{
                    //    mch.Set_date(nameThread, i + 1, 3 * 60000);
                    //}
                }
            }


            // 计算总耗时
            TimeSpan ts = DateTime.Now - dtStartPick;
            string time = string.Format("选股线程{0}用时：{1}分{2}秒\r\n", nameThread, ts.Minutes, ts.Seconds);
            Logs.Write(time);

            // 设置结果
            string strMessage = string.Join("\r\n", lstMsg) + time;
            result.SetSuccess(lstStock, strMessage);
            return result;
        }

        /// <summary>
        /// 初始化进度数据
        /// </summary>
        /// <param name="thread_cnt"></param>
        /// <param name="total_cnt"></param>
        /// <param name="exec_time"></param>
        /// <param name="guid"></param>
        /// <returns></returns>
        private static bool InitProcessData(string guid, int thread_cnt, int total_cnt, DateTime exec_time)
        {
            // 是否执行
            bool bExec = false;

            try
            {
                // 保存总股票数和线程数量到memcache或redis
                #region memcache
                if (MngConfig.SiteConfig.EnableMemcache && !string.IsNullOrWhiteSpace(guid))
                {
                    MemCachedHelper mch = new MemCachedHelper();
                    mch.Set_date(guid + "_thread_cnt", thread_cnt, 60 * 60000);
                    mch.Set_date(guid + "_total_cnt", total_cnt, 60 * 60000);
                    mch.Set_date(guid + "_exec_time", exec_time, 60 * 60000);
                    bExec = true;
                }
                #endregion
                #region redis
                else if (MngConfig.SiteConfig.EnableRedis && !string.IsNullOrWhiteSpace(guid))
                {
                    RedisHelper.SetString(guid + "_thread_cnt", thread_cnt.ToString());
                    RedisHelper.SetString(guid + "_total_cnt", total_cnt.ToString());
                    RedisHelper.Set(guid + "_exec_time", exec_time);
                    bExec = true;
                }
                #endregion
            }
            catch (Exception)
            {
                bExec = false;
            }

            return bExec;
        }

        /// <summary>
        /// 设置进度数据
        /// </summary>
        /// <param name="key"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        private static bool SetProcessData(string key, object data)
        {
            // 是否执行
            bool bExec = false;

            try
            {
                //  保存进度到memcache或redis
                #region memcache
                if (MngConfig.SiteConfig.EnableMemcache && key.IndexOf('_') != 0)
                {
                    MemCachedHelper mch = new MemCachedHelper();
                    mch.Set_date(key, data, 3 * 60000);
                }
                #endregion
                #region redis
                else if (MngConfig.SiteConfig.EnableRedis && key.IndexOf('_') != 0)
                {
                    RedisHelper.Set(key, data);
                }
                #endregion
            }
            catch (Exception ex)
            {
                bExec = false;
            }

            return bExec;
        }
        
    }
}
