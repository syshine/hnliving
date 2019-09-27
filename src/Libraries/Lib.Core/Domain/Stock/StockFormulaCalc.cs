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
        /// 数移动平均值 EMA（Exponential Moving Average
        /// </summary>
        private DataTable dtEMA = null;

        /// <summary>
        /// 指数平滑移动平均线（Moving Average Convergence and Divergence，简称MACD）
        /// </summary>
        private DataTable _dtMACD = null;

        public DataTable DtMACD
        {
            get
            {
                if (_dtMACD == null)
                    _dtMACD = StockHelper.GetMACD(dtData);

                return _dtMACD;
            }

            //set
            //{
            //    _dtMACD = value;
            //}
        }

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
        /// 构造函数
        /// </summary>
        /// <param name="data"></param>
        public StockFormulaCalc(string code, DataTable data, DataTable avg, DataTable macd)
        {
            stockCode = code;
            dtData = data;
            dtAverageLine = avg;
            _dtMACD = macd;
        }

        /// <summary>
        /// 执行函数
        /// </summary>
        /// <param name="functionName"></param>
        /// <param name="Params"></param>
        /// <returns></returns>
        public override object ExecFunction(string functionName, string Params)
        {
            object result;

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
                    if(Convert.ToInt32(result) < 0)
                    {
                        throw new FormulaException("未找到该条件成立的时候", indexFormula, funcFormula);
                    }
                    break;

                // 统计满足条件的周期数
                case "COUNT":
                    {
                        JudgeParamCount(2, lstParams.Count, funcFormula);
                        result = COUNT(lstParams[0], Convert.ToInt32(lstParams[1]));
                    }
                    break;

                // 上穿
                case "CROSS":
                    {
                        JudgeParamCount(2, lstParams.Count, funcFormula);
                        result = CROSS(lstParams[0], lstParams[1]);
                    }
                    break;

                // 指数移动平均值
                case "EMA":
                    {
                        JudgeParamCount(2, lstParams.Count, funcFormula);
                        ushort M = Convert.ToUInt16(Calc(lstParams[1]));
                        result = EMA(lstParams[0], M);
                    }
                    break;

                // 是否存在
                case "EXIST":
                    {
                        JudgeParamCount(2, lstParams.Count, funcFormula);
                        result = EXIST(lstParams[0], Convert.ToInt32(lstParams[1]));
                    }
                    break;

                // 求最高值
                case "HHV":
                    {
                        JudgeParamCount(2, lstParams.Count, funcFormula);
                        result = HHV(lstParams[0], Convert.ToInt32(lstParams[1]));
                    }
                    break;

                // 求上一高点到当前的周期数
                case "HHVBARS":
                    {
                        JudgeParamCount(2, lstParams.Count, funcFormula);
                        result = HHVBARS(lstParams[0], Convert.ToInt32(lstParams[1]));
                    }
                    break;

                // 求最低值
                case "LLV":
                    {
                        JudgeParamCount(2, lstParams.Count, funcFormula);
                        result = LLV(lstParams[0], Convert.ToInt32(lstParams[1]));
                    }
                    break;

                // 求上一低点到当前的周期数
                case "LLVBARS":
                    {
                        JudgeParamCount(2, lstParams.Count, funcFormula);
                        result = LLVBARS(lstParams[0], Convert.ToInt32(lstParams[1]));
                    }
                    break;

                // 移动平均线
                case "MA":
                    {
                        JudgeParamCount(2, lstParams.Count, funcFormula);
                        ushort M = Convert.ToUInt16(Calc(lstParams[1]));
                        result = MA(lstParams[0], M);
                    }
                    break;

                // 引用若干周期前的数据
                case "REF":
                    {
                        JudgeParamCount(2, lstParams.Count, funcFormula);
                        int A = Convert.ToInt32(Calc(lstParams[1]));
                        result = REF(lstParams[0], A);
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
        /// 统计连续满足条件的周期数
        /// 用法:BARSLASTCOUNT(X),统计连续满足条件的周期数
        /// 例如:BARSLASTCOUNT(C>O),表示统计连续收阳的周期数
        /// </summary>
        /// <param name="X"></param>
        /// <returns></returns>
        private int BARSLASTCOUNT(string X)
        {
            int result = statistics("BARSLASTCOUNT", X, -1);

            return result;
        }

        /// <summary>
        /// 上一次条件成立到当前的周期数
        /// 用法:BARSLAST(X),上一次X不为0到现在的天数
        /// 例如:BARSLAST(CLOSE/REF(CLOSE,1)>=1.1),表示上一个涨停板到当前的周期数
        /// </summary>
        /// <param name="X"></param>
        /// <returns></returns>
        private int BARSLAST(string X)
        {
            int result = -100000;

            List<string[]> lstX = ParseExpression(X);
            if (lstX.Count > 0 && lstX[0][0] != "")
            {
                // 要执行的函数名
                string functionName = lstX[0][0].ToUpper();
                string functionParams = lstX[0][1];

                // 第几次符合条件(从0开始算起)
                ushort times = 0;

                if (lstX[0][0].ToUpper() == "REF")
                {
                    // 解析REF参数 例：REF(CROSS(MA(C,5),MA(C,10)),1)
                    List<string> paramsRef = ParseParams(lstX[0][1]);
                    if (paramsRef.Count != 2)
                    {
                        throw new FormulaException("REF参数个数错误", indexFormula);
                    }

                    // 前几次?
                    times = Convert.ToUInt16(paramsRef[1]);

                    // 解析REF内部表达式 例：CROSS(MA(C,5),MA(C,10))
                    List<string[]> expRef = ParseExpression(paramsRef[0]);
                    if (expRef.Count > 0 && expRef[0][0] != "")
                    {
                        functionName = expRef[0][0];
                        functionParams = expRef[0][1];
                    }
                    else
                    {
                        throw new FormulaException("REF参数错误", indexFormula);
                    }
                }

                switch (functionName)
                {
                    case "CROSS":
                        {
                            List<string> paramsCROSS = ParseParams(functionParams);
                            result = CROSS(paramsCROSS[0], paramsCROSS[1], "BARSLAST", times);
                            if (result < 0)
                                result = -100000;
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
        private bool CROSS(string A, string B)
        {
            if (CROSS(A, B, "default") == 1)
                return true;
            else
                return false;
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
                string nameA = "";      // A函数名
                string nameB = "";      // B函数名
                string paramsA = "";    // A参数
                string paramsB = "";    // B参数
                int indexStart = 0;     // 开始循环的位置

                #region REF 例：CROSS(REF(MA(C,5),6),REF(MA(C,10),6)) 表示6天前5日线金叉10日线
                // 例：CROSS(REF(MA(C,5),6),REF(MA(C,10),6)) 表示6天前5日线金叉10日线
                if (lstA[0][0].Trim().ToUpper() == "REF" && lstB[0][0].Trim().ToUpper() == "REF")
                {
                    // 参数 例："MA(C,5),6"则表示6天前的5日均线
                    List<string> lstRefParamA = ParseParams(lstA[0][1].Trim());
                    List<string> lstRefParamB = ParseParams(lstB[0][1].Trim());

                    if (lstRefParamA.Count == 2 && lstRefParamB.Count == 2)
                    {
                        // 解析除几天前
                        int refA = int.Parse(lstRefParamA[1].Trim());
                        int refB = int.Parse(lstRefParamB[1].Trim());

                        // 取最小的周期日作为参考日
                        int refDay = Math.Min(refA, refB);

                        indexStart = refDay; // 开始循环的位置

                        // 解析ref里的表达式 例：MA(C,5)
                        List<string[]> lstRefA = ParseExpression(lstRefParamA[0]);
                        List<string[]> lstRefB = ParseExpression(lstRefParamB[0]);

                        if (lstRefA.Count > 0 && lstRefB.Count > 0)
                        {
                            nameA = lstRefA[0][0].Trim().ToUpper();
                            nameB = lstRefB[0][0].Trim().ToUpper();
                            paramsA = lstRefA[0][1];
                            paramsB = lstRefB[0][1];
                        }
                    }
                    else
                    {
                        throw new FormulaException("REF函数参数的个数是2", indexFormula);
                    }
                }
                #endregion
                else
                {
                    nameA = lstA[0][0].Trim().ToUpper();
                    nameB = lstB[0][0].Trim().ToUpper();
                    paramsA = lstA[0][1];
                    paramsB = lstB[0][1];
                }

                #region MA
                if (nameA == "MA" && nameB == "MA")
                {
                    // 参数 例：lstA[0][1]="C,5"则表示收盘价的5日均线
                    List<string> lstParamsA = ParseParams(paramsA);
                    List<string> lstParamsB = ParseParams(paramsB);

                    JudgeParamCount(2, lstParamsA.Count);
                    JudgeParamCount(2, lstParamsB.Count);

                    #region 均线字段名称
                    // 均线字段名称
                    string preA; // 字段前缀
                    string preB; // 字段前缀
                    switch (lstParamsA[0].Trim())
                    {
                        case "O":
                        case "OPEN":
                            preA = "open_day";
                            break;

                        case "H":
                        case "HIGH":
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
                    switch (lstParamsB[0].Trim())
                    {
                        case "O":
                        case "OPEN":
                            preB = "open_day";
                            break;

                        case "H":
                        case "HIGH":
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
                    string fieldA = preA + lstParamsA[1].Trim();
                    string fieldB = preB + lstParamsB[1].Trim();
                    #endregion

                    for (int i = indexStart; i < dtAverageLine.Rows.Count - 1; i++)
                    {
                        decimal valueA1 = Convert.ToDecimal(dtAverageLine.Rows[i][fieldA]);
                        decimal valueB1 = Convert.ToDecimal(dtAverageLine.Rows[i][fieldB]);
                        decimal valueA0 = Convert.ToDecimal(dtAverageLine.Rows[i + 1][fieldA]);
                        decimal valueB0 = Convert.ToDecimal(dtAverageLine.Rows[i + 1][fieldB]);

                        // 这一天A大于B
                        if (valueA1 > valueB1 && valueA1 > 0 && valueB1 > 0)
                        {
                            bool bCross = false;
                            //前一天A小于B
                            if (valueA0 <= valueB0 && valueA0 > 0 && valueB0 > 0)
                            {
                                bCross = true;
                            }
                            //else if (valueA0 == valueB0) // 如果是等于，则还需要看再往前一天
                            //{
                            //    if (i + 2 < dtAverageLine.Rows.Count)
                            //    {
                            //        decimal vA = Convert.ToDecimal(dtAverageLine.Rows[i + 2][fieldA]);
                            //        decimal vB = Convert.ToDecimal(dtAverageLine.Rows[i + 2][fieldB]);
                            //        if (valueA0 < valueB0)
                            //            bCross = true;
                            //    }
                            //}

                            // A上穿B
                            if (bCross)
                            {
                                if (count++ >= times)
                                {
                                    // 记录序号并跳出循环
                                    index = i;
                                    break;
                                }
                            }
                        }

                        if (type == "default")
                        {
                            if (index >= 0)
                                return 1;
                            else
                                return 0;
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
            else
            {
                if(type == "default")
                {
                    result = 0;
                }
            }

            return result;
        }

        /// <summary>
        /// 移动平均线
        /// </summary>
        /// <param name="X"></param>
        /// <param name="M"></param>
        /// <param name="before"></param>
        /// <returns></returns>
        private decimal MA(string X, ushort M, int before = 0)
        {
            if(before >= dtAverageLine.Rows.Count)
            {
                throw new UnselectException("MA：没有足够的数据");
            }

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
                case "HIGH":
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

        /// <summary>
        /// 引用若干周期前的数据
        /// 用法:REF(X,A),引用A周期前的X值
        /// 例如:REF(CLOSE,1),表示上一周期的收盘价,在日线上表示昨收价
        /// </summary>
        /// <param name="X"></param>
        /// <param name="A"></param>
        /// <returns></returns>
        private decimal REF(string X, int A)
        {
            if(A < 0)
            {
                throw new FormulaException("REF函数参数A不能为负数", indexFormula);
            }

            decimal result = -1;

            //string nameX = "";      // X函数名
            //string paramsX = "";    // X参数
            //int indexStart = 0;     // 开始循环的位置

            #region MA
            // 解析ref里的表达式 例：MA(C,5)
            List<string[]> lstRefX = ParseExpression(X);

            if (lstRefX.Count > 0)
            {
                string nameX = lstRefX[0][0].Trim().ToUpper();  // X函数名
                string paramsX = lstRefX[0][1];                 // X参数

                if (nameX == "MA")
                {
                    // 参数 例：lstRefX[0][1]="C,5"则表示收盘价的5日均线
                    List<string> lstParamsX = ParseParams(paramsX);

                    JudgeParamCount(2, lstParamsX.Count);

                    result = MA(lstParamsX[0], Convert.ToUInt16(lstParamsX[1]), A);
                    return result;
                }
                else
                {
                    throw new FormulaException("没有"+ nameX+"函数", indexFormula);
                }
            }
            #endregion

            switch (X.Trim().ToUpper())
            {
                // 收盘价
                case "C":
                case "CLOSE":
                    result = Convert.ToDecimal(dtData.Rows[A]["TCLOSE"]);
                    break;

                // 开盘价
                case "O":
                case "OPEN":
                    result = Convert.ToDecimal(dtData.Rows[A]["TOPEN"]);
                    break;

                // 最高价
                case "H":
                case "HIGH":
                    result = Convert.ToDecimal(dtData.Rows[A]["HIGH"]);
                    break;

                // 最低价
                case "L":
                case "LOW":
                    result = Convert.ToDecimal(dtData.Rows[A]["LOW"]);
                    break;

                // 成交量
                case "V":
                case "VAL":
                    result = Convert.ToDecimal(dtData.Rows[A]["VOTURNOVER"]);
                    break;

                default:
                    break;
            }

            return result;
        }

        /// <summary>
        /// 求上一高点到当前的周期数
        /// 用法:HHVBARS(X,N),求N周期内X最高值到当前周期数,N=0表示从第一个有效值开始统计
        /// 例如:HHVBARS(HIGH,0),求得历史新高到到当前的周期数
        /// </summary>
        /// <param name="X"></param>
        /// <param name="N"></param>
        /// <returns></returns>
        private int HHVBARS(string X, int N)
        {
            if (N >= dtData.Rows.Count)
            {
                throw new UnselectException("HHVBARS：没有足够的数据");
            }

            int result = -99999;
            string field = "";

            switch (X.Trim().ToUpper())
            {
                // 收盘价
                case "C":
                case "CLOSE":
                    field = "TCLOSE";
                    break;

                // 开盘价
                case "O":
                case "OPEN":
                    field = "TOPEN";
                    break;

                // 最高价
                case "H":
                case "HIGH":
                    field = "HIGH";
                    break;

                // 最低价
                case "L":
                case "LOW":
                    field = "LOW";
                    break;

                // 成交量
                case "V":
                case "VAL":
                    field = "VOTURNOVER";
                    break;

                default:
                    return result;
            }

            // 获取N个周期的数据，然后用linq求最大值
            IEnumerable<DataRow> idr = null;
            if (N > 0)
            {
                idr = dtData.AsEnumerable().Take(N);
            }
            else
            {
                idr = dtData.AsEnumerable();
            }
            decimal maxValue = idr.Max(r => decimal.Parse(r.Field<string>(field)));
            
            // 查找最大值所在行的序号
            DataTable dtTemp = idr.CopyToDataTable();
            string filter = string.Format("{0} = '{1}'", field, maxValue);
            DataRow[] drs = dtTemp.Select(filter, "fdate desc");
            result = dtTemp.Rows.IndexOf(drs[0]);

            return result;
        }

        /// <summary>
        /// 求最高值
        /// 用法:HHV(X,N),求N周期内X最高值,N=0则从第一个有效值开始
        /// 例如:HHV(HIGH,22),表示求22日最高价
        /// </summary>
        /// <param name="X"></param>
        /// <param name="N"></param>
        /// <returns></returns>
        private decimal HHV(string X, int N)
        {
            if (N >= dtData.Rows.Count)
            {
                throw new UnselectException("HHV：没有足够的数据");
            }

            decimal result = 0;
            string field = "";

            switch (X.Trim().ToUpper())
            {
                // 收盘价
                case "C":
                case "CLOSE":
                    field = "TCLOSE";
                    break;

                // 开盘价
                case "O":
                case "OPEN":
                    field = "TOPEN";
                    break;

                // 最高价
                case "H":
                case "HIGH":
                    field = "HIGH";
                    break;

                // 最低价
                case "L":
                case "LOW":
                    field = "LOW";
                    break;

                // 成交量
                case "V":
                case "VAL":
                    field = "VOTURNOVER";
                    break;

                default:
                    return result;
            }

            // 获取N个周期的数据，然后用linq求最大值
            if(N > 0)
            {
                result = dtData.AsEnumerable().Take(N).Max(r => decimal.Parse(r.Field<string>(field)));
            }
            else
            {
                result = dtData.AsEnumerable().Max(r => decimal.Parse(r.Field<string>(field)));
            }

            return result;
        }


        /// <summary>
        /// 求上一低点到当前的周期数
        /// 用法:LLVBARS(X,N),求N周期内X最低值到当前周期数,N=0表示从第一个有效值开始统计
        /// 例如:LLVBARS(HIGH,22),求得22日最低点到当前的周期数
        /// </summary>
        /// <param name="X"></param>
        /// <param name="N"></param>
        /// <returns></returns>
        private int LLVBARS(string X, int N)
        {
            if (N >= dtData.Rows.Count)
            {
                throw new UnselectException("LLVBARS：没有足够的数据");
            }

            int result = -99999;
            string field = "";

            switch (X.Trim().ToUpper())
            {
                // 收盘价
                case "C":
                case "CLOSE":
                    field = "TCLOSE";
                    break;

                // 开盘价
                case "O":
                case "OPEN":
                    field = "TOPEN";
                    break;

                // 最高价
                case "H":
                case "HIGH":
                    field = "HIGH";
                    break;

                // 最低价
                case "L":
                case "LOW":
                    field = "LOW";
                    break;

                // 成交量
                case "V":
                case "VAL":
                    field = "VOTURNOVER";
                    break;

                default:
                    return result;
            }

            // 获取N个周期的数据，然后用linq求最小值
            IEnumerable<DataRow> idr = null;
            if (N > 0)
            {
                idr = dtData.AsEnumerable().Take(N);
            }
            else
            {
                idr = dtData.AsEnumerable();
            }
            decimal minValue = idr.Min(r => decimal.Parse(r.Field<string>(field)));

            // 查找最小值所在行的序号
            DataTable dtTemp = idr.CopyToDataTable();
            string filter = string.Format("{0} = '{1}'", field, minValue);
            DataRow[] drs = dtTemp.Select(filter, "fdate desc");
            result = dtTemp.Rows.IndexOf(drs[0]);

            return result;
        }

        /// <summary>
        /// 求最低值
        /// 用法:LLV(X,N),求N周期内X最低值,N=0则从第一个有效值开始
        /// 例如:LLV(LOW,0),表示求历史最低价
        /// </summary>
        /// <param name="X"></param>
        /// <param name="N"></param>
        /// <returns></returns>
        private decimal LLV(string X, int N)
        {
            if (N >= dtData.Rows.Count)
            {
                throw new UnselectException("LLV：没有足够的数据");
            }

            decimal result = 0;
            string field = "";

            switch (X.Trim().ToUpper())
            {
                // 收盘价
                case "C":
                case "CLOSE":
                    field = "TCLOSE";
                    break;

                // 开盘价
                case "O":
                case "OPEN":
                    field = "TOPEN";
                    break;

                // 最高价
                case "H":
                case "HIGH":
                    field = "HIGH";
                    break;

                // 最低价
                case "L":
                case "LOW":
                    field = "LOW";
                    break;

                // 成交量
                case "V":
                case "VAL":
                    field = "VOTURNOVER";
                    break;

                default:
                    return result;
            }

            // 获取N个周期的数据，然后用linq求最低值
            if (N > 0)
            {
                result = dtData.AsEnumerable().Take(N).Min(r => decimal.Parse(r.Field<string>(field)));
            }
            else
            {
                result = dtData.AsEnumerable().Min(r => decimal.Parse(r.Field<string>(field)));
            }

            return result;
        }


        /// <summary>
        /// 统计满足条件的周期数
        /// 用法:COUNT(X,N),统计N周期中满足X条件的周期数,若N=0则从第一个有效值开始
        /// 例如:COUNT(CLOSE>OPEN,20),表示统计20周期内收阳的周期数
        /// </summary>
        /// <param name="X"></param>
        /// <param name="N"></param>
        /// <returns></returns>
        private int COUNT(string X, int N)
        {
            return statistics("COUNT", X, N);
        }

        /// <summary>
        /// 指数移动平均值
        /// EMA（Exponential Moving Average）
        /// </summary>
        /// <param name="X"></param>
        /// <param name="M"></param>
        /// <param name="before"></param>
        /// <returns></returns>
        private decimal EMA(string X, ushort M, int before = 0)
        {
            decimal result = -1;
            string colName = "";
            
            switch (X.ToUpper())
            {
                // 收盘价
                case "C":
                case "CLOSE":
                    colName = "EMA" + M.ToString();
                    break;

                // 开盘价
                case "O":
                case "OPEN":
                    colName = "open_EMA" + M.ToString();
                    break;

                // 最高价
                case "H":
                case "HIGH":
                    colName = "high_EMA" + M.ToString();
                    break;

                // 最低价
                case "L":
                case "LOW":
                    colName = "low_EMA" + M.ToString();
                    break;

                // 成交量
                case "V":
                case "VAL":
                    colName = "val_EMA" + M.ToString();
                    break;

                default:
                    break;
            }

            // 没有数据就获取
            if(dtEMA == null)
            {
                dtEMA = StockHelper.GetEMA(dtData, new List<ushort>() { M });
            }

            if (before >= dtEMA.Rows.Count)
            {
                throw new UnselectException("EMA：没有足够的数据");
            }

            // 没有当前的列就再计算添加
            if (!dtEMA.Columns.Contains(colName))
            {
                // 添加列
                dtEMA.Columns.Add(colName, Type.GetType("System.Decimal"));

                // 添加数据进EMA总表
                DataTable dtAdd = StockHelper.GetEMA(dtData, new List<ushort>() { M });
                for (int i = 0; i < dtEMA.Rows.Count; i++)
                {
                    if(dtEMA.Rows[i]["date"] == dtAdd.Rows[i]["date"])
                    {
                        dtEMA.Rows[i][colName] = dtAdd.Rows[i][colName];
                    }
                }
            }

            // 获取结果
            result = Convert.ToDecimal(dtEMA.Rows[before][colName]);

            return result;
        }

        /// <summary>
        /// 是否存在
        /// 用法:EXIST(CLOSE>OPEN,10),表示前10日内存在着阳线
        /// </summary>
        /// <param name="X"></param>
        /// <param name="N"></param>
        /// <returns></returns>
        private bool EXIST(string X, int N)
        {
            if (statistics("EXIST", X, N) > 0)
                return true;
            else
                return false;
        }

        #endregion

        #region 辅助股票函数
        private int statistics(string type, string X, int N)
        {
            int result = 0;
            int count = 0;      // 符合总数
            int series = 0;     // 当前连续符合次数

            int loopCnt = N > 0 ? N : dtData.Rows.Count; // 循环次数

            // 循环处理N个周期
            for (int i = 0; i < loopCnt; i++)
            {
                // 捕获异常，不影响其他周期
                try
                {
                    // 每个周期的数据
                    DataTable dtDataTemp = dtData.AsEnumerable().Skip(i).Take(N - i).CopyToDataTable();
                    DataTable dtAverageLineTemp = dtAverageLine.AsEnumerable().Skip(i).Take(N - i).CopyToDataTable();

                    // 计算
                    StockFormulaCalc sfc = new StockFormulaCalc(stockCode, dtDataTemp, dtAverageLineTemp);
                    bool bRet = Convert.ToBoolean(sfc.Compute(X));

                    if (bRet)
                    {
                        count++;
                        series++;
                    }
                    else
                    {
                        if (type == "BARSLASTCOUNT")
                        {
                            return series;
                        }
                    }

                    if (type == "EXIST")
                    {
                        return count;
                    }
                }
                catch (Exception)
                {
                    //throw;
                }

            }

            switch (type)
            {
                case "COUNT":
                    result = count;
                    break;

                case "BARSLASTCOUNT":
                    result = series;
                    break;

                default:
                    break;
            }


            return result;
        }

        #endregion
    }
}
