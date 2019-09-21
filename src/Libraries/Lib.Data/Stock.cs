using Lib.Core;
using Lib.Core.Stock;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
        public static ResultEntity PickStock(string code, DataTable dtHis, StockPickEntity condition)
        {
            ResultEntity result = new ResultEntity();

            // 没有数据的不选
            if(dtHis.Rows.Count <= 0)
            {
                return result;
            }

            bool bPick = true;
            
            try
            {
                DataTable dtData = null;
                // 使用前复权数据
                if (condition.UseFormerComplexRights)
                {
                    dtData = StockHelper.GetFormerComplexRights(dtHis, true);
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

                #region 公式
                if (bPick && condition.FormulaEnable && !string.IsNullOrWhiteSpace(condition.Formula))
                {
                    if (dtData.Rows.Count > 0)
                    {
                        // 均线数据
                        DataTable dtAvg = StockHelper.GetAverageLine(dtData).Select("", "date desc").CopyToDataTable();

                        // 初始化公式计算
                        StockFormulaCalc sfc = new StockFormulaCalc(code, dtData, dtAvg);

                        // 公式计算结果
                        bPick = Convert.ToBoolean(sfc.Compute(condition.Formula));
                    }
                    else // 没有数据直接不选
                    {
                        bPick = false;
                    }
                    #region //
                    //// 将整个公式分成多行执行（移除空行）
                    //string[] formulas = condition.Formula.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);

                    //// 公式中的局部变量
                    //Hashtable htVars = new Hashtable();
                    //for (int i = 0; i < formulas.Length; i++)
                    //{
                    //    // 单行公式
                    //    string formula = formulas[i];

                    //    // 赋值参数
                    //    int index = formula.IndexOf(":=");
                    //    if(index > 0)
                    //    {
                    //        // 表达式
                    //        string expressions = formula.Substring(index + 2, formula.Length - 2).Trim();

                    //        // 表达式计算结果
                    //        object expValue = null;

                    //        string[] expPart = expressions.Split(new string[] { " AND ", " OR ", " & ", " || " }, StringSplitOptions.RemoveEmptyEntries);
                    //        if(expPart.Length > 0)  // 条件表达式
                    //        {
                    //            for (int j = 0; j < expPart.Length; j++)
                    //            {

                    //            }
                    //        }
                    //        else // 计算表达式
                    //        {
                    //            //expressions.inde
                    //        }

                    //        // 记录
                    //        htVars[formula.Substring(0, index).Trim()] = expValue;
                    //    }
                    //}

                    //for (int i = 0; i < formulas.Length; i++)
                    //{

                    //}
                    #endregion
                }
                #endregion

                #region 正则表达式
                //if (bPick && condition.RegexEnable && !string.IsNullOrWhiteSpace(condition.RegExp))
                //{
                //    //// 模板
                //    //string pattern = @"\[.*?\]";    // 匹配中括号里的内容（非贪婪算法）
                //    //string patt = @"MA\(.*?\)";     // 匹配移动平均线

                //    //// 匹配
                //    //MatchCollection mc = Regex.Matches(condition.RegExp, pattern);

                //    //// 循环处理匹配到的字符串
                //    //foreach(Match item in mc)
                //    //{
                //    //    string str = item.Value;


                //    //}
                //}
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
        /// 执行表达式
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        //private static object ExecExpression(DataTable data, string expression)
        //{
        //    object result = null;
        //    List<string[]> lstFunc = StockFunction.ParseExpression(expression);
        //    List<object> lstExecRet = new List<object>();
        //    StockFunction sf = new StockFunction(data);

        //    if (lstFunc != null)
        //    {
        //        for(int i = 0; i < lstFunc.Count; i++)
        //        {
        //            object execResult = sf.ExecFunction(lstFunc[i][0], lstFunc[i][1]);
        //            lstExecRet.Add(execResult);
        //        }
        //    }

        //    return result;
        //}

        

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
