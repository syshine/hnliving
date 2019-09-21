using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lib.Core
{
    public partial class StockFormulaCalc : FormulaCalc
    {
        private string stockCode = "";

        /// <summary>
        /// 历史数据
        /// </summary>
        private DataTable dtData = null;

        /// <summary>
        /// 移动平均线数据
        /// </summary>
        private DataTable dtAverageLine = null;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="data"></param>
        public StockFormulaCalc(string code, DataTable data, DataTable avg)
        {
            stockCode = code;
            dtData = data;
            dtAverageLine = avg;
        }


        /// <summary>
        /// 执行函数
        /// </summary>
        /// <param name="functionName"></param>
        /// <param name="Params"></param>
        /// <returns></returns>
        public override decimal ExecFunction(string functionName, string Params)
        {
            decimal result;

            // 函数公式
            string funcFormula = GetFuncFormula(functionName, Params);

            // 解析参数
            List<string> lstParams = ParseParams(Params);
            for (int i = 0; i < lstParams.Count; i++)
            {
                if (string.IsNullOrWhiteSpace(lstParams[i]))
                {
                    string msg = string.Format("第{0}个参数为空", i + 1);
                    throw new FormulaException(msg, indexFormula, funcFormula);
                }
            }

            switch (functionName.ToUpper())
            {
                // 上一次条件成立到当前的周期数
                case "BARSLAST":
                    JudgeParamCount(1, lstParams.Count, funcFormula);
                    result = BARSLAST(lstParams[0]);
                    break;

                // 上穿
                case "CROSS":
                    {
                        JudgeParamCount(2, lstParams.Count, funcFormula);
                        result = CROSS(lstParams[0], lstParams[1]);
                    }
                    break;

                // 移动平均线
                case "MA":
                    {
                        JudgeParamCount(2, lstParams.Count, funcFormula);
                        result = MA(lstParams[0], Convert.ToUInt16(lstParams[1]));
                    }
                    break;

                default:
                    return base.ExecFunction(functionName, Params);
                    //break;
            }

            // 记录过程（有计算过程才记录）
            log(string.Format("{0}({1}) = {2}", functionName, Params, result));

            return result;
        }

        /// <summary>
        /// 获取值 (定义的值、转换成decimal类型)
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        protected override decimal GetValue(string value)
        {
            decimal result;
            switch (value)
            {
                case "C":
                case "CLOSE":
                    result = Convert.ToDecimal(dtData.Rows[0]["TCLOSE"]);
                    break;

                case "H":
                case "HIGH":
                    result = Convert.ToDecimal(dtData.Rows[0]["HIGH"]);
                    break;

                case "L":
                case "LOW":
                    result = Convert.ToDecimal(dtData.Rows[0]["LOW"]);
                    break;

                case "O":
                case "OPEN":
                    result = Convert.ToDecimal(dtData.Rows[0]["TOPEN"]);
                    break;

                case "V":
                case "VOL":
                    result = Convert.ToDecimal(dtData.Rows[0]["VOTURNOVER"]);
                    break;

                default:
                    return base.GetValue(value);
            }

            return result;
        }

        #region 股票函数
        /// <summary>
        /// 上一次条件成立到当前的周期数
        /// 用法:BARSLAST(X),上一次X不为0到现在的天数
        /// 例如:BARSLAST(CLOSE/REF(CLOSE,1)>=1.1),表示上一个涨停板到当前的周期数
        /// </summary>
        /// <param name="X"></param>
        /// <returns></returns>
        private int BARSLAST(string X)
        {
            int result = -1;

            List<string[]> lstX = ParseExpression(X);
            if (lstX.Count > 0 && lstX[0][0] != "")
            {
                // 要执行的函数名
                string functionName = lstX[0][0].ToUpper();

                // 第几次符合条件
                ushort times = 1;

                if (lstX[0][0].ToUpper() == "REF")
                {
                    // 解析REF 例：REF(CROSS(MA(C,5),MA(C,10)),1)
                    List<string[]> paramsRef = ParseExpression(lstX[0][1]);
                    if (paramsRef.Count > 0 && paramsRef[0][0] != "")
                    {
                        functionName = paramsRef[0][0];
                        times = ushort.Parse(paramsRef[0][1]);
                    }

                }

                switch (functionName)
                {
                    case "CROSS":
                        {
                            List<string> paramsCROSS = ParseParams(lstX[0][1]);
                            result = CROSS(paramsCROSS[0], paramsCROSS[1], "CROSS", times);
                        }
                        break;

                    case "":
                        {
                        }
                        break;

                    default:
                        break;
                }
            }

            return result;
        }

        /// <summary>
        /// 两条线交叉
        /// 用法:CROSS(A,B),表示当A从下方向上传过B时返回1,否则返回0
        /// 例如:CROSS(MA(CLOSE,5),MA(CLOSE,10)),表示5日均线上穿10日均线
        /// </summary>
        /// <param name="A"></param>
        /// <param name="B"></param>
        /// <returns></returns>
        private int CROSS(string A, string B)
        {
            return CROSS(A, B, "default");
        }

        /// <summary>
        /// 两条线交叉
        /// 用法:CROSS(A,B),表示当A从下方向上传过B时返回1,否则返回0
        /// 例如:CROSS(MA(CLOSE,5),MA(CLOSE,10)),表示5日均线上穿10日均线
        /// </summary>
        /// <param name="A"></param>
        /// <param name="B"></param>
        /// <param name="type">default:CROSS;BARSLAST:BARSLAST(CROSS)</param>
        /// <param name="times">default:几天前(相当于REF(CROSS));BARSLAST:第几次上穿;从0开始！</param>
        /// <returns></returns>
        private int CROSS(string A, string B, string type, ushort times = 0)
        {
            int result = -1;
            int index = -1;
            int count = 0;  // 上穿次数

            List<string[]> lstA = ParseExpression(A);
            List<string[]> lstB = ParseExpression(B);

            if (lstA.Count > 0 && lstB.Count > 0)
            {
                string nameA = ""; // A函数名
                string nameB = ""; // B函数名
                int indexStart = dtAverageLine.Rows.Count - 1; // 开始循环的位置

                #region REF 例：CROSS(REF(MA(C,5),6),REF(MA(C,10),6)) 表示6天前5日线金叉10日线
                // 例：CROSS(REF(MA(C,5),6),REF(MA(C,10),6)) 表示6天前5日线金叉10日线
                if (lstA[0][0].Trim().ToUpper() == "REF" && lstB[0][0].Trim().ToUpper() == "REF")
                {
                    List<string[]> lstRefA = ParseExpression(lstA[0][1].Trim());
                    List<string[]> lstRefB = ParseExpression(lstB[0][1].Trim());

                    if (lstRefA.Count > 0 && lstRefB.Count > 0)
                    {
                        nameA = lstRefA[0][0].Trim().ToUpper();
                        nameB = lstRefB[0][0].Trim().ToUpper();

                        // 参数 例：lstRefA[0][1]="MA(C,5),6"则表示6天前的5日均线
                        List<string> paramsRefA = ParseParams(lstRefA[0][1]);
                        List<string> paramsRefB = ParseParams(lstRefB[0][1]);

                        int refA = int.Parse(paramsRefA[1].Trim());
                        int refB = int.Parse(paramsRefB[1].Trim());

                        // 取最小的周期日作为参考日
                        int refDay = Math.Min(refA, refB);

                        indexStart = dtAverageLine.Rows.Count - 1 - refDay; // 开始循环的位置
                    }
                }
                #endregion
                else
                {
                    nameA = lstA[0][0].Trim().ToUpper();
                    nameB = lstB[0][0].Trim().ToUpper();
                }

                #region MA
                if (lstA[0][0].Trim().ToUpper() == "MA" && lstB[0][0].Trim().ToUpper() == "MA")
                {
                    // 参数 例：lstA[0][1]="C,5"则表示收盘价的5日均线
                    List<string> paramsA = ParseParams(lstA[0][1]);
                    List<string> paramsB = ParseParams(lstB[0][1]);

                    #region 均线字段名称
                    // 均线字段名称
                    string preA; // 字段前缀
                    string preB; // 字段前缀
                    switch (paramsA[0].Trim())
                    {
                        case "O":
                        case "OPEN":
                            preA = "open_day";
                            break;

                        case "H":
                        case "HIGN":
                            preA = "high_day";
                            break;

                        case "L":
                        case "LOW":
                            preA = "low_day";
                            break;

                        case "V":
                        case "VAL":
                            preA = "val_day";
                            break;

                        case "C":
                        case "CLOSE":
                        default:
                            preA = "day";
                            break;
                    }
                    switch (paramsB[0].Trim())
                    {
                        case "O":
                        case "OPEN":
                            preB = "open_day";
                            break;

                        case "H":
                        case "HIGN":
                            preB = "high_day";
                            break;

                        case "L":
                        case "LOW":
                            preB = "low_day";
                            break;

                        case "V":
                        case "VAL":
                            preB = "val_day";
                            break;

                        case "C":
                        case "CLOSE":
                        default:
                            preB = "day";
                            break;
                    }
                    string fieldA = preA + paramsA[1].Trim();
                    string fieldB = preB + paramsB[1].Trim();
                    #endregion

                    for (int i = indexStart; i > 0; i--)
                    {
                        decimal valueA1 = Convert.ToDecimal(dtAverageLine.Rows[i][fieldA]);
                        decimal valueB1 = Convert.ToDecimal(dtAverageLine.Rows[i][fieldB]);
                        decimal valueA0 = Convert.ToDecimal(dtAverageLine.Rows[i - 1][fieldA]);
                        decimal valueB0 = Convert.ToDecimal(dtAverageLine.Rows[i - 1][fieldB]);

                        // 这一天A大于B
                        if (valueA1 > valueB1)
                        {
                            bool bCross = false;
                            //前一天A小于B
                            if (valueA0 < valueB0)
                            {
                                bCross = true;
                            }
                            else if (valueA0 == valueB0) // 如果是等于，则还需要看再往前一天
                            {
                                if (i - 2 >= 0)
                                {
                                    decimal vA = Convert.ToDecimal(dtAverageLine.Rows[i - 2][fieldA]);
                                    decimal vB = Convert.ToDecimal(dtAverageLine.Rows[i - 2][fieldB]);
                                    if (valueA0 < valueB0)
                                        bCross = true;
                                }
                            }

                            // A上穿B
                            if (bCross)
                            {
                                if (count++ >= times)
                                {
                                    // 记录序号并跳出循环
                                    index = dtAverageLine.Rows.Count - 1 - i;
                                    break;
                                }
                            }
                        }

                        if (type == "default")
                        {
                            if (index >= 0)
                                return 1;
                        }
                    }
                }
                #endregion

            }

            if (index >= 0)
            {
                if (type == "BARSLAST")
                {
                    result = index;
                }
                else
                    result = 1;
            }

            return result;
        }

        private decimal MA(string X, ushort M, ushort before = 0)
        {
            decimal result = -1;

            // 几日均线
            List<ushort> lstDays = new List<ushort>() { M };

            switch (X.ToUpper())
            {
                // 收盘价
                case "C":
                case "CLOSE":
                    result = Convert.ToDecimal(dtAverageLine.Rows[before]["day" + M.ToString()]);
                    break;

                // 开盘价
                case "O":
                case "OPEN":
                    result = Convert.ToDecimal(dtAverageLine.Rows[before]["open_day" + M.ToString()]);
                    break;

                // 最高价
                case "H":
                case "HIGN":
                    result = Convert.ToDecimal(dtAverageLine.Rows[before]["high_day" + M.ToString()]);
                    break;

                // 最低价
                case "L":
                case "LOW":
                    result = Convert.ToDecimal(dtAverageLine.Rows[before]["low_day" + M.ToString()]);
                    break;

                // 成交量
                case "V":
                case "VAL":
                    result = Convert.ToDecimal(dtAverageLine.Rows[before]["val_day" + M.ToString()]);
                    break;

                default:
                    break;
            }

            return result;
        }
        #endregion
    }
}
