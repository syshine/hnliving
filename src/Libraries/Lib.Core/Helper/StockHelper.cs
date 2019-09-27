//using Microsoft.Analytics.Interfaces;
//using Microsoft.Analytics.Types.Sql;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;

namespace Lib.Core
{
    // datatable 列转list
    //var list =dt.AsEnumerable().Select<DataRow, int>(x => Convert.ToInt32(x["列名"])).ToList<int>();
    public class StockHelper
    {
        /// <summary>
        /// 获取均线
        /// 获取5, 10, 20, 30, 60, 610均线
        /// </summary>
        /// <param name="dtData"></param>
        /// <returns></returns>
        public static DataTable GetAverageLine(DataTable dtData)
        {
            List<ushort> days = new List<ushort>(new ushort[] { 5, 10, 20, 30, 60, 610 });

            return GetAverageLine(dtData, days);
        }


        /// <summary>
        /// 获取均线
        /// </summary>
        /// <param name="dtData"></param>
        /// <param name="lstDays">需要获取的哪几个N日均线</param>
        /// <param name="fieldName">计算哪个字段的均线</param>
        /// <returns></returns>
        public static DataTable GetAverageLine(DataTable dtData, List<ushort> lstDays, string fieldName = "TCLOSE")
        {
#if DEBUG
            // 计时
            DateTime startTime = DateTime.Now;
#endif

            // 去重后排序再处理
            List<ushort> days = lstDays.Distinct().ToList();
            days.Sort();

            // 构建数据集
            List<AverageLineValue> lstALV = new List<AverageLineValue>();   // 计算均线值的类
            List<string> lstColName = new List<string>();
            DataTable dtLine = new DataTable();
            dtLine.Columns.Add("date");
            foreach (ushort nDay in days)
            {
                // 添加计算类
                lstALV.Add(new AverageLineValue(nDay));

                // 添加数据集列
                string colName = "day" + nDay;
                lstColName.Add(colName);
                dtLine.Columns.Add(colName, Type.GetType("System.Decimal"));
            }

            // 升序处理
            DataRow[] drsTemp = dtData.Select("", "HDATE asc");
            for (int i = 0; i < drsTemp.Length; i++)
            {
                DataRow newRow = dtLine.NewRow();

                // 日期
                newRow["date"] = drsTemp[i]["fdate"].ToString();

                // 是否有脏数据
                decimal value = 0m;
                if (!decimal.TryParse(drsTemp[i][fieldName].ToString(), out value))
                {
                    string msg = string.Format("编码{0}计算均线时，日期{1}的数据可能有误，数据：{2}",
                        drsTemp[i]["s_code"].ToString(), drsTemp[i]["HDATE"].ToString(), drsTemp[i][fieldName].ToString());
                    MngLog.Instance.Write(msg);
                }

                // 计算数据
                for(int j = 0; j < lstColName.Count; j++)
                {
                    newRow[lstColName[j]] = lstALV[j].Update(value);
                }

                // 添加数据到数据集
                dtLine.Rows.Add(newRow);
            }

#if DEBUG
            // 打印耗时
            DateTime endTime = DateTime.Now;
            double timespan = endTime.Subtract(startTime).TotalMilliseconds / 1000.0;
            System.Diagnostics.Debug.WriteLine(string.Format("计算均线耗时：{0}秒", timespan));
#endif

            return dtLine;
        }

        /// <summary>
        /// 获取指数平均数
        /// 公式 : EMA (N) = (2 * value  + (N - 1) * EMA(N - 1)) / (N + 1)
        /// </summary>
        /// <param name="dtData"></param>
        /// <param name="lstDays">需要获取的哪几个N指数平均数</param>
        /// <param name="X"></param>
        /// <returns></returns>
        public static DataTable GetEMA(DataTable dtData, List<ushort> lstDays, string X = "CLOSE")
        {
            // 去重后排序再处理
            List<ushort> days = lstDays.Distinct().ToList();
            days.Sort();

            // 初始化数据集
            DataTable dtEMA = new DataTable();
            dtEMA.Columns.Add("date");
            foreach (ushort nDay in days)
            {
                // 添加数据集列
                string colName = "EMA" + nDay;
                dtEMA.Columns.Add(colName, Type.GetType("System.Decimal"));
            }

            // 添加数据
            if(dtData.Rows.Count > 0)
            {
                string field = GetField(X);

                // 添加第一个数据
                DataRow row0 = dtEMA.NewRow();
                row0["date"] = dtData.Rows[0]["fdate"].ToString();
                foreach (ushort nDay in days)
                {
                    string colName = "EMA" + nDay;
                    row0[colName] = dtData.Rows[0][field].ToString();
                }
                dtEMA.Rows.Add(row0);

                // 循环计算值
                for (int i = 1; i < dtData.Rows.Count; i++)
                {
                    DataRow newRow = dtEMA.NewRow();
                    newRow["date"] = dtData.Rows[0]["fdate"].ToString();

                    foreach (ushort nDay in days)
                    {
                        string colName = "EMA" + nDay;

                        // 计算EMA    公式 : EMA (N) = (2 * value  + (N - 1) * EMA(N - 1)) / (N + 1)
                        decimal preEMA = Convert.ToDecimal(dtEMA.Rows[i - 1][colName]); // 前一日EMA
                        decimal value = Convert.ToDecimal(dtData.Rows[i][field]);
                        decimal EMA = (2 * value + (nDay - 1) * preEMA) / (nDay + 1);

                        newRow[colName] = EMA;
                    }

                    dtEMA.Rows.Add(newRow);
                }
            }

            return dtEMA;
        }



        /// <summary>
        /// GetDIFF
        /// DIFF : EMA(CLOSE,SHORT) - EMA(CLOSE,LONG);
        /// DEA  : EMA(DIFF,M);
        /// MACD : 2*(DIFF-DEA), COLORSTICK;
        /// </summary>
        /// <param name="dtData"></param>
        /// <param name="shortDay"></param>
        /// <param name="longDay"></param>
        /// <param name="fieldName"></param>
        /// <returns></returns>
        public static DataTable GetDIFF(DataTable dtData, ushort shortDay = 12, ushort longDay = 26, string fieldName = "CLOSE")
        {
            DataTable dtEMA = GetEMA(dtData, new List<ushort>() { shortDay, longDay }, fieldName);
            string colNameShort = "EMA" + shortDay;
            string colNameLong = "EMA" + longDay;

            DataTable dtDIFF = new DataTable();
            dtDIFF.Columns.Add("date");
            dtDIFF.Columns.Add("DIFF", Type.GetType("System.Decimal"));

            // 遍历赋值
            for (int i = 0; i < dtEMA.Rows.Count; i++)
            {
                DataRow newRow = dtDIFF.NewRow();

                newRow["date"] = dtData.Rows[i]["fdate"].ToString();

                // 计算DIFF
                decimal emaShort = Convert.ToDecimal(dtEMA.Rows[i][colNameShort]);
                decimal emaLong = Convert.ToDecimal(dtEMA.Rows[i][colNameLong]);
                newRow["DIFF"] = emaShort - emaLong;

                // 添加新行
                dtDIFF.Rows.Add(newRow);
            }

            return dtDIFF;
        }

        /// <summary>
        /// GetDEA
        /// DIFF : EMA(CLOSE,SHORT) - EMA(CLOSE,LONG);
        /// DEA  : EMA(DIFF,M);
        /// MACD : 2*(DIFF-DEA), COLORSTICK;
        /// </summary>
        /// <param name="dtData"></param>
        /// <param name="M">DIFF的M日平滑移动平均值</param>
        /// <param name="shortDay"></param>
        /// <param name="longDay"></param>
        /// <param name="fieldName"></param>
        /// <returns></returns>
        public static DataTable GetDEA(DataTable dtDIFF, ushort M = 9)
        {
            // 获取DEA数据
            DataTable dtDEA = GetEMA(dtDIFF, new List<ushort> { M }, "DIFF");
            // 更改列名
            dtDEA.Columns["EMA" + M].ColumnName = "DEA";
            return dtDEA;
        }

        /// <summary>
        /// GetMACD
        /// DIFF : EMA(CLOSE,SHORT) - EMA(CLOSE,LONG);
        /// DEA  : EMA(DIFF,M);
        /// MACD : 2*(DIFF-DEA), COLORSTICK;
        /// </summary>
        /// <param name="dtData"></param>
        /// <param name="shortDay"></param>
        /// <param name="longDay"></param>
        /// <param name="fieldName"></param>
        /// <returns></returns>
        public static DataTable GetMACD(DataTable dtData, ushort M = 9, ushort shortDay = 12, ushort longDay = 26, string fieldName = "CLOSE")
        {
            DataTable dtDataDIFF = GetDIFF(dtData, shortDay, longDay, fieldName);
            DataTable dtDataDEA = GetDEA(dtDataDIFF, M);
            DataTable dtDataMACD = dtDataDIFF.Copy();
            dtDataMACD.Columns.Add("DEA", Type.GetType("System.Decimal"));
            dtDataMACD.Columns.Add("MACD", Type.GetType("System.Decimal"));

            // 遍历赋值
            for (int i = 0; i < dtDataMACD.Rows.Count; i++)
            {
                decimal dea = Convert.ToDecimal(dtDataDEA.Rows[i]["DEA"]);
                decimal diff = Convert.ToDecimal(dtDataDIFF.Rows[i]["DIFF"]);
                
                dtDataMACD.Rows[i]["DEA"] = Math.Round(dea, 2);
                dtDataMACD.Rows[i]["DIFF"] = Math.Round(diff, 2);
                dtDataMACD.Rows[i]["MACD"] = Math.Round(2 * (diff - dea), 2);
            }

            return dtDataMACD;
        }

        /// <summary>
        /// 获取前复权数据 (正序倒序数据)
        /// </summary>
        /// <param name="dtHis"></param>
        /// <param name="isDesc"></param>
        /// <returns></returns>
        public static DataTable GetFormerComplexRights(DataTable dtHis, bool isDesc)
        {
            if(!isDesc) // 正序数据
            {
                return GetFormerComplexRights(dtHis);
            }
            else // 倒序数据
            {
                DataTable dtTemp = dtHis.Select("", "fdate asc").CopyToDataTable();
                return GetFormerComplexRights(dtTemp).Select("", "fdate desc").CopyToDataTable();
            }
        }

        /// <summary>
        /// 获取前复权数据
        /// </summary>
        /// <param name="dtHis"></param>
        /// <returns></returns>
        public static DataTable GetFormerComplexRights(DataTable dtHis)
        {
            // 复制数据集
            DataTable dtData = dtHis.Copy();

            // 系数
            decimal coefficient = 1.0m;

            // 前复权需要从后面逆推(i = dtHis.Rows.Count - 2是因为第一行不需要考虑前复权,减少判断量)
            for (int i = dtHis.Rows.Count - 2; i > 0; i--)
            {
                // 今收盘价
                decimal TCLOSE = Convert.ToDecimal(dtHis.Rows[i]["TCLOSE"]);

                // 明天数据的前一日收盘价
                decimal LCLOSE = Convert.ToDecimal(dtHis.Rows[i+1]["LCLOSE"]);// Convert.ToDecimal(dtHis.Rows[i+1]["LCLOSE"]);

                // 如果这条数据的收盘价不等于后一天数据的前一日收盘价，则明天为除权日，今天之前的数据需要复权
                if (TCLOSE != LCLOSE)
                {
                    //LCLOSE = Convert.ToDecimal(dtData.Rows[i + 1]["LCLOSE"]);
                    // 计算系数因子(因子=前一日数据中收盘价/今日数据中昨日收盘价)
                    //decimal coef = Math.Round(LCLOSE / TCLOSE, 4); // 保留4位小数，四舍五入
                    decimal coef = (int)((LCLOSE / TCLOSE) * 10000) / 10000.0m; // 保留4位小数，不进行四舍五入
                    coefficient *= (int)(coef * 10000) / 10000.0m;              // 该因子是累计的(历史有多次除权)
                    //coefficient *= (LCLOSE / TCLOSE); // 该因子是累计的(历史有多次除权)
                }

                // 因子不为1才转换，减少数据处理
                if (coefficient != 1)
                {
                    decimal temp = 0;

                    // 收盘价
                    temp = Convert.ToDecimal(dtHis.Rows[i]["TCLOSE"]);
                    dtData.Rows[i]["TCLOSE"] = Math.Round(temp * coefficient, 2);   // 保留两位小数，四舍五入
                    //dtData.Rows[i]["TCLOSE"] = (int)(temp * coefficient * 100) / 100.0;   // 保留两位小数，不进行四舍五入

                    // 最高价
                    temp = Convert.ToDecimal(dtHis.Rows[i]["HIGH"]);
                    dtData.Rows[i]["HIGH"] = Math.Round(temp * coefficient, 2);         // 保留两位小数，四舍五入
                    //dtData.Rows[i]["HIGH"] = (int)(temp * coefficient * 100) / 100.0; // 保留两位小数，不进行四舍五入

                    // 最低价
                    temp = Convert.ToDecimal(dtHis.Rows[i]["LOW"]);
                    dtData.Rows[i]["LOW"] = Math.Round(temp * coefficient, 2);          // 保留两位小数，四舍五入
                    //dtData.Rows[i]["LOW"] = (int)(temp * coefficient * 100) / 100.0;  // 保留两位小数，不进行四舍五入

                    // 开盘价
                    temp = Convert.ToDecimal(dtHis.Rows[i]["TOPEN"]);
                    dtData.Rows[i]["TOPEN"] = Math.Round(temp * coefficient, 2);            // 保留两位小数，四舍五入
                    //dtData.Rows[i]["TOPEN"] = (int)(temp * coefficient * 100) / 100.0;    // 保留两位小数，不进行四舍五入

                    // 昨收盘价(是否需要处理？)
                    temp = Convert.ToDecimal(dtHis.Rows[i]["LCLOSE"]);
                    dtData.Rows[i]["LCLOSE"] = Math.Round(temp * coefficient, 2);           // 保留两位小数，四舍五入
                    //dtData.Rows[i]["LCLOSE"] = (int)(temp * coefficient * 100) / 100.0;   // 保留两位小数，不进行四舍五入

                    // 成交量暂不处理(是否需要处理？)
                }
            }

            return dtData;
        }

        /// <summary>
        /// 获取字段名
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string GetField(string key)
        {
            string result = key;
            switch (key.Trim().ToUpper())
            {
                // 收盘价
                case "C":
                case "CLOSE":
                    result = "TCLOSE";
                    break;

                // 开盘价
                case "O":
                case "OPEN":
                    result = "TOPEN";
                    break;

                // 最高价
                case "H":
                case "HIGH":
                    result = "HIGH";
                    break;

                // 最低价
                case "L":
                case "LOW":
                    result = "LOW";
                    break;

                // 成交量
                case "V":
                case "VAL":
                    result = "VOTURNOVER";
                    break;

                default:
                    break;
            }

            return result;
        }
    }

    public class AverageLineValue
    {
        ushort COUNT;  // 总共几位数
        List<decimal> lstData = new List<decimal>();
        int indLast = 0; // 最早一个数据的序号
        decimal sum = 0.0m;

        public AverageLineValue(ushort count)
        {
            COUNT = count;
        }

        public decimal Update(decimal value)
        {
            if(lstData.Count == COUNT)  // 数据已满，将最新的数据代替最早的一个
            {
                // 先求和,始终保持两位小数,防止误差
                sum = Math.Round(sum + value - lstData[indLast], 2, MidpointRounding.AwayFromZero);

                // 将最新的数据代替最早的一个
                lstData[indLast] = value;

                // 更新最早的数据序号
                indLast = (indLast + 1) % COUNT;

                // 保留两位小数返回平均值
                return Math.Round(sum / COUNT, 2, MidpointRounding.AwayFromZero);
            }
            else if (lstData.Count == COUNT - 1)    // 刚好数据填满
            {
                // 增加数据
                lstData.Add(value);
                indLast = 0;

                // 求和
                sum = lstData.Sum();

                // 保留两位小数返回平均值
                return Math.Round(sum / COUNT, 2, MidpointRounding.AwayFromZero);
            }
            else // 数据不够
            {
                // 增加数据
                lstData.Add(value);
                return 0;
            }
        }
    }
}