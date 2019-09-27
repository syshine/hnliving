using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lib.Core.Stock
{
    public class StockFunction
    {
        /// <summary>
        /// 历史数据
        /// </summary>
        private DataTable dtData = null;

        /// <summary>
        /// 移动平均线数据
        /// </summary>
        private DataTable dtAverageLine = null;

        public StockFunction(DataTable data)
        {
            dtData = data;
            dtAverageLine = StockHelper.GetAverageLine(dtData);
        }

        public object ExecFunction(string functionName, string Params)
        {
            object result = null;
            
            switch(functionName.ToUpper())
            {
                case "BARSLAST":
                    result = BARSLAST(Params);
                    break;

                case "CROSS":
                    {
                        List<string> lstParams = ParseParams(Params);
                        result = CROSS(lstParams[0], lstParams[1]);
                    }
                    break;

                default:
                    break;
            }

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
            int result = -1;

            List<string[]> lstX = ParseExpression(X);
            if(lstX.Count > 0 && lstX[0][0] != "")
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

                switch(functionName)
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

            if(lstA.Count > 0 && lstB.Count > 0)
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

                    // 均线字段名称
                    string fieldA = paramsA[0].Trim() + paramsA[1].Trim();
                    string fieldB = paramsB[0].Trim() + paramsB[1].Trim();

                    for(int i = indexStart; i > 0; i--)
                    {
                        decimal valueA1 = Convert.ToDecimal(dtAverageLine.Rows[i][fieldA]);
                        decimal valueB1 = Convert.ToDecimal(dtAverageLine.Rows[i][fieldB]);
                        decimal valueA0 = Convert.ToDecimal(dtAverageLine.Rows[i-1][fieldA]);
                        decimal valueB0 = Convert.ToDecimal(dtAverageLine.Rows[i-1][fieldB]);

                        // 这一天A大于B
                        if (valueA1 > valueB1)
                        {
                            bool bCross = false;
                            //前一天A小于B
                            if ( valueA0 < valueB0)
                            {
                                bCross = true;
                            }
                            else if (valueA0 == valueB0) // 如果是等于，则还需要看再往前一天
                            {
                                if(i-2>=0)
                                {
                                    decimal vA = Convert.ToDecimal(dtAverageLine.Rows[i - 2][fieldA]);
                                    decimal vB = Convert.ToDecimal(dtAverageLine.Rows[i - 2][fieldB]);
                                    if (valueA0 < valueB0)
                                        bCross = true;
                                }
                            }

                            // A上穿B
                            if(bCross)
                            {
                                if(count++ >= times)
                                {
                                    // 记录序号并跳出循环
                                    index = dtAverageLine.Rows.Count - 1 - i;
                                    break;
                                }
                            }
                        }

                        if(type == "default")
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
                if(type == "BARSLAST")
                {
                    result = index;
                }
                else
                    result = 1;
            }

            return result;
        }

        private int MA(string X, ushort M, ushort before = 0)
        {
            int result = -1;

            // 几日均线
            List<ushort> lstDays = new List<ushort>() { M };

            switch (X.ToUpper())
            {
                // 收盘价
                case "C":
                case "CLOSE":
                    {
                        decimal value = Convert.ToDecimal(dtAverageLine.Rows[before]["DAY" + M.ToString()]);
                    }
                    break;

                // 开盘价
                case "O":
                case "OPEN":
                    {
                        //DataTable dtAver = StockHelper.GetAverageLine(dtData, lstDays, "TOPEN");
                    }
                    break;

                default:
                    break;
            }

            return result;
        }

        /// <summary>
        /// 解析表达式
        /// </summary>
        /// <param name="expression"></param>
        /// <param name="left_char"></param>
        /// <param name="right_char"></param>
        /// <returns></returns>
        public static List<string[]> ParseExpression(string expression, char left_char = '(', char right_char = ')')
        {
            List<string[]> lstResult = new List<string[]>();
            string left = string.Format("{0}", left_char);
            string right = string.Format("{0}", right_char);

            // 左右符号数量
            int leftCount = 0;
            int rightCount = 0;

            // 第一个对应左右符号的序号
            int leftIndex = -1;
            int rightIndex = -1;
            for (int i = 0; i < expression.Length; i++)
            {
                string s = expression.Substring(i, 1);
                if (s == left)
                {
                    // 第一个左字符的序号
                    if (++leftCount == 1)
                        leftIndex = i;
                }
                else if (s == right)
                {
                    rightCount++;

                    // 如果先出现右字符，则表示表达式错误，直接返回
                    if (rightCount > leftCount)
                    {
                        lstResult.Clear();
                        return lstResult;
                    }

                    // 跟第一个左字符对应的右字符序号
                    if (rightCount == leftCount)
                    {
                        rightIndex = i;

                        string[] result = new string[2] { "", "" };

                        // 匹配函数名
                        string temp = expression.Substring(0, leftIndex - 1).Trim();
                        char[] arrChar = temp.ToCharArray();
                        for (int j = arrChar.Length - 1; j > 0; j--)
                        {
                            if (!char.IsLetterOrDigit(arrChar[j]))
                            {
                                result[0] = temp.Substring(j + 1, temp.Length - j - 1).Trim();
                                break;
                            }
                        }

                        // 函数内的参数，如果result[0]为空，则表示这是计算式子
                        result[1] = expression.Substring(leftIndex + 1, rightIndex - leftIndex - 1).Trim();

                        // 添加结果
                        lstResult.Add(result);

                        // 继续查找
                        leftCount = 0;
                        rightCount = 0;
                        leftIndex = -1;
                        rightIndex = -1;
                    }
                }
            }

            return lstResult;
        }

        /// <summary>
        /// 解析参数
        /// </summary>
        /// <param name="strParams"></param>
        /// <param name="separator"></param>
        /// <returns></returns>
        public static List<string> ParseParams(string strParams, char separator = ',')
        {
            List<string> lstResult = new List<string>();
            char left = '(';
            char right = ')';

            // 左右符号数量
            int leftCount = 0;
            int rightCount = 0;

            // 第一个对应左右符号的序号
            int leftIndex = -1;
            int rightIndex = -1;

            // 参数从哪个序号开始
            int indexParam = 0;


            char[] charParams = strParams.ToCharArray();
            for (int i = 0; i < charParams.Length; i++)
            {
                char s = charParams[i];
                if (s == left)
                {
                    // 第一个左字符的序号
                    if (++leftCount == 1)
                        leftIndex = i;
                }
                else if (s == right)
                {
                    rightCount++;

                    // 如果先出现右字符，则表示表达式错误，直接返回
                    if (rightCount > leftCount)
                    {
                        lstResult.Clear();
                        return lstResult;
                    }

                    // 跟第一个左字符对应的右字符序号
                    if (rightCount == leftCount)
                    {
                        rightIndex = i;

                        // 继续查找
                        leftCount = 0;
                        rightCount = 0;
                        leftIndex = -1;
                        rightIndex = -1;
                    }
                }
                else if (s == separator)
                {
                    // separator在括号外
                    if (rightCount == leftCount)
                    {
                        string result = strParams.Substring(indexParam, i - indexParam).Trim();
                        // 添加结果
                        lstResult.Add(result);

                        // 记录下个参数起始位置
                        indexParam = i + 1;
                    }
                }
            }

            return lstResult;
            //return strParams.Split(new string[] { separator }, StringSplitOptions.RemoveEmptyEntries).ToList();
        }

        /// <summary>
        /// 计算
        /// </summary>
        /// <param name="expression">表达式</param>
        /// <returns></returns>
        public static decimal Calc(string expression)
        {
            // 
            string tempExp = expression;

            // 先找出最外层括号并计算
            List<string[]> lst = ParseExpression(expression);
            List<decimal> lstCalc = new List<decimal>();
            for (int i = 0; i < lst.Count; i++)
            {
                // 如果括号左边没有函数名，则为计算
                if (string.IsNullOrWhiteSpace(lst[i][0]))
                {
                    // 递归计算
                    lstCalc.Add(Calc(lst[i][1]));

                    // 用占位符替换原表达式，到后面统一计算
                    tempExp = tempExp.Replace("(" + lst[i][1] + ")", "{" + i + "}");
                }
                else // 是函数
                {
                    throw new Exception("表达式有误");
                }
            }

            // 将括号里的计算结果替换到表达式里
            //decimal[] arrCalc = lstCalc.ToArray();
            //string[] arrCalc = lstCalc.ConvertAll(s => s.ToString());
            string[] arrCalc = Array.ConvertAll(lstCalc.ToArray(), s => s.ToString());
            tempExp = string.Format(tempExp, arrCalc);

            #region 四则运算参数准备
            // 四则运算
            char[] arr = tempExp.ToCharArray();
            int index = 0;
            List<decimal> lstValue = new List<decimal>();   // 数值
            List<char> lstOperator = new List<char>();      // 操作符(+ - * /)
            for (int i = 0; i < arr.Length; i++)
            {
                switch (arr[i])
                {
                    case '+':
                    case '-':
                    case '*':
                    case '/':
                        {
                            string strValue = tempExp.Substring(index, i - index).Trim();
                            lstValue.Add(decimal.Parse(strValue));
                            lstOperator.Add(arr[i]);
                            index = i + 1;
                        }
                        break;

                    default:
                        break;
                }
            }

            // 添加最后一个数值
            string v = tempExp.Substring(index, tempExp.Length - index).Trim();
            lstValue.Add(decimal.Parse(v));
            #endregion

            #region 先乘除
            // 乘除
            List<decimal> lstValueNew = new List<decimal>();   // 进行乘除后的数值
            List<char> lstOperatorNew = new List<char>();      // 进行乘除后的操作符(+ -)
            decimal tempValue = 0;
            for (int i = 0; i < lstOperator.Count; i++)
            {
                if (lstOperator[i] == '*') // 乘
                {
                    // 被乘数
                    decimal coefficient;

                    // 如果上一个符号为乘或除，则使用记录的系数当被乘数
                    if (i > 0 && (lstOperator[i - 1] == '*' || lstOperator[i - 1] == '/'))
                        coefficient = tempValue;
                    else
                        coefficient = lstValue[i];

                    tempValue = coefficient * lstValue[i + 1];
                }
                else if (lstOperator[i] == '/') // 除
                {
                    // 被除数
                    decimal coefficient;

                    // 如果上一个符号为乘或除，则使用记录的系数当被除数
                    if (i > 0 && (lstOperator[i - 1] == '*' || lstOperator[i - 1] == '/'))
                        coefficient = tempValue;
                    else
                        coefficient = lstValue[i];

                    tempValue = coefficient / lstValue[i + 1];
                }
                else // 加减
                {
                    // 如果上一个符号为乘或除，则添加数值为之前记录的数值
                    if (i > 0 && (lstOperator[i - 1] == '*' || lstOperator[i - 1] == '/'))
                    {
                        lstValueNew.Add(tempValue);
                        tempValue = 0;
                    }
                    else
                    {
                        lstValueNew.Add(lstValue[i]);
                    }

                    lstOperatorNew.Add(lstOperator[i]);
                }
            }

            // 如果最后一个运算符号为乘除，则最后一个值为之前记录的数值
            if (lstOperator.Last() == '*' || lstOperator.Last() == '/')
            {
                lstValueNew.Add(tempValue);
            }
            else // 如果最后一个运算符号为加减，则最后一个值为原来最后一个数值
            {

                lstValueNew.Add(lstValue.Last());
            }
            #endregion

            #region 后加减
            // 加减
            decimal result = lstValueNew[0];
            for (int i = 0; i < lstOperatorNew.Count; i++)
            {
                if (lstOperatorNew[i] == '+') // 加
                {
                    result += lstValueNew[i + 1];
                }
                else if (lstOperatorNew[i] == '-') // 减
                {
                    result -= lstValueNew[i + 1];
                }
            }
            #endregion

            return result;
        }

    }
}
