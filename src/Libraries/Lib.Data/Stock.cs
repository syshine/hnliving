using Lib.Core;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lib.Data
{
    /// <summary>
    /// 用户数据访问类
    /// </summary>
    public partial class Stock
    {
        #region 算法
        /// <summary>
        /// 选股
        /// </summary>
        /// <returns></returns>
        public static ResultEntity PickStock(DataTable dtHis, StockPickEntity condition)
        {
            ResultEntity result = new ResultEntity();
            bool bPick = true;
            
            try
            {
                DataTable dtData = null;
                // 使用前复权数据
                if (condition.UseFormerComplexRights)
                {
                    dtData = StockHelper.GetFormerComplexRights(dtHis);
                }
                else
                {
                    dtData = dtHis;
                }

                #region 连续上涨(下跌)
                if (condition.PCHGEnable)
                {
                    // 数据量不够时直接不选
                    if (dtHis.Rows.Count < condition.Days)
                    {
                        bPick = false;
                    }
                    else
                    {
                        for (int i = 0; i < condition.Days; i++)
                        {
                            // 涨跌幅
                            decimal pchg = decimal.Parse(dtHis.Rows[i]["PCHG"].ToString());

                            if (condition.IsRise) // 上涨
                            {
                                if (condition.Operator == "<=") // 不超过
                                {
                                    if (pchg < 0 || pchg > condition.PCHG)
                                    {
                                        bPick = false;
                                        break;
                                    }
                                }
                                else if (condition.Operator == ">") // 超过
                                {
                                    if (pchg < 0 || pchg <= condition.PCHG)
                                    {
                                        bPick = false;
                                        break;
                                    }
                                }
                                else
                                {
                                    bPick = false;
                                    break;
                                }
                            }
                            else // 下跌
                            {

                                if (condition.Operator == "<=") // 不超过
                                {
                                    if (pchg > 0 || pchg < condition.PCHG)
                                    {
                                        bPick = false;
                                        break;
                                    }
                                }
                                else if (condition.Operator == ">") // 超过
                                {
                                    if (pchg > 0 || pchg >= condition.PCHG)
                                    {
                                        bPick = false;
                                        break;
                                    }
                                }
                                else
                                {
                                    bPick = false;
                                    break;
                                }
                            }
                        }
                    }
                }
                #endregion

                #region 价格区间
                if(bPick && condition.PriceEnable)
                {
                    if (dtHis.Rows.Count > 0)
                    {
                        // 收盘价
                        decimal TCLOSE = decimal.Parse(dtHis.Rows[0]["TCLOSE"].ToString());
                        if(TCLOSE < condition.PriceLow || TCLOSE > condition.PriceHigh)
                            bPick = false;
                    }
                    else
                    {
                        bPick = false;
                    }
                }
                #endregion

                #region 流通市值区间
                if (bPick && condition.MCAPEnable)
                {
                    if (dtHis.Rows.Count > 0)
                    {
                        // 流通市值
                        decimal MCAP = decimal.Parse(dtHis.Rows[0]["MCAP"].ToString());
                        if (MCAP < condition.MCAPLow || MCAP > condition.MCAPHigh)
                            bPick = false;
                    }
                    else
                    {
                        bPick = false;
                    }
                }
                #endregion
            }
            catch (Exception ex)
            {
                bPick = false;
                result.SetError(ex.Message);
            }

            // 如果选中，设置结果
            if (bPick)
                result.SetSuccess();

            return result;
        }

        /// <summary>
        /// 获取前复权数据
        /// </summary>
        /// <param name="dtHis"></param>
        /// <returns></returns>
        //private static DataTable GetFormerComplexRights(DataTable dtHis)
        //{
        //    // 复制数据集
        //    DataTable dtData = dtHis.Copy();

        //    // 系数
        //    decimal coefficient = 1.0m;

        //    // 前复权需要从后面逆推(i = dtHis.Rows.Count - 2是因为第一行不需要考虑前复权,减少判断量)
        //    for (int i = dtHis.Rows.Count - 2; i < dtHis.Rows.Count; i--)
        //    {
        //        // 今收盘价
        //        decimal TCLOSE = Convert.ToDecimal(dtHis.Rows[i]["TCLOSE"]);

        //        // 明天数据的前一日收盘价
        //        decimal LTCLOSE = Convert.ToDecimal(dtHis.Rows[i]["LTCLOSE"]);

        //        // 如果这条数据的收盘价不等于后一天数据的前一日收盘价，则明天为除权日，今天之前的数据需要复权
        //        if (TCLOSE != LTCLOSE)
        //        {
        //            // 计算系数因子(因子=前一日数据中收盘价/今日数据中昨日收盘价)
        //            coefficient *= (LTCLOSE / TCLOSE); // 该因子是累计的(历史有多次除权)
        //        }

        //        // 因子不为1才转换，减少数据处理
        //        if (coefficient != 1)
        //        {
        //            decimal temp = 0;

        //            // 收盘价
        //            temp = Convert.ToDecimal(dtData.Rows[i]["TCLOSE"]);
        //            dtData.Rows[i]["TCLOSE"] = temp * coefficient;

        //            // 最高价
        //            temp = Convert.ToDecimal(dtData.Rows[i]["HIGH"]);
        //            dtData.Rows[i]["HIGH"] = temp * coefficient;

        //            // 最低价
        //            temp = Convert.ToDecimal(dtData.Rows[i]["LOW"]);
        //            dtData.Rows[i]["LOW"] = temp * coefficient;

        //            // 开盘价
        //            temp = Convert.ToDecimal(dtData.Rows[i]["TOPEN"]);
        //            dtData.Rows[i]["TOPEN"] = temp * coefficient;

        //            // 昨收盘价(是否需要处理？)
        //            temp = Convert.ToDecimal(dtData.Rows[i]["LTCLOSE"]);
        //            dtData.Rows[i]["LTCLOSE"] = temp * coefficient;

        //            // 成交量暂不处理(是否需要处理？)
        //        }
        //    }
            
        //    return dtData;
        //}

        #endregion
    }
}
