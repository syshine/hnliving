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
        /// <returns></returns>
        public static DataTable GetAverageLine(DataTable dtData, List<ushort> lstDays)
        {
            // 计时
            DateTime startTime = DateTime.Now;

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
                if (!decimal.TryParse(drsTemp[i]["TCLOSE"].ToString(), out value))
                {
                    string msg = string.Format("编码{0}计算均线时，日期{1}的数据可能有误，收盘价：{2}",
                        drsTemp[i]["s_code"].ToString(), drsTemp[i]["HDATE"].ToString(), drsTemp[i]["TCLOSE"].ToString());
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

            // 打印耗时
            DateTime endTime = DateTime.Now;
            double timespan = endTime.Subtract(startTime).TotalMilliseconds / 1000.0;
            System.Diagnostics.Debug.WriteLine(string.Format("计算均线耗时：{0}秒", timespan));

            return dtLine;
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
                    // 计算系数因子(因子=前一日数据中收盘价/今日数据中昨日收盘价)
                    decimal coef = Math.Round(LCLOSE / TCLOSE, 4); // 保留4位小数，四舍五入
                    //decimal coef = (int)((LCLOSE / TCLOSE) * 10000) / 10000.0m; // 保留4位小数，不进行四舍五入
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