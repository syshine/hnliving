using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Lib.Core
{
    public class FormulaCalc
    {
        // 表达式中的局部变量
        protected Hashtable htVariate = new Hashtable();
        protected Hashtable htVarExp = new Hashtable();

        protected int indexFormula = 0;

        /// <summary>
        /// 计算多行表达式
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        public object Compute(string expression)
        {
            object result = "";

            // 将整个公式分成多行执行（移除空行）
            string[] formulas = expression.Split(new string[] { "\r\n", "\n" }, StringSplitOptions.None);

            // 结果行序号
            int nRetLine = -1;
            for (int i = formulas.Length - 1; i >= 0; i--)
            {
                if (!string.IsNullOrWhiteSpace(formulas[i]))
                {
                    if (formulas[i].IndexOf(":=") > 0)
                    {
                        continue;
                    }

                    // 取到后直接跳出循环
                    nRetLine = i;
                    break;
                }
            }

            // 循环执行(只执行到结果行)
            for (int i = 0; i <= nRetLine; i++)
            {
                indexFormula = i;

                // 空行则直接进行下一行
                if (string.IsNullOrWhiteSpace(formulas[i]))
                    continue;

                log(string.Format("\r\n---------------开始计算第{0}行---------------", (indexFormula + 1)));

                // 单行公式
                string formula = formulas[i];

                // 赋值参数
                int index = formula.IndexOf(":=");
                if (index > 0 || i == nRetLine) // i == nRetLine为结果行
                {
                    // 表达式
                    string expressions;
                    if (i != nRetLine)
                    {
                        expressions = formula.Substring(index + 2).Trim();
                        //expressions = formula.Substring(index + 2, formula.Length - 2 - index).Trim();
                    }
                    else
                    {
                        // :为输出, 没有也可以
                        int ind = formula.IndexOf(":");
                        expressions = formula.Substring(ind + 1).Trim();
                        //expressions = formula.Substring(ind + 1, formula.Length - 1 - ind).Trim();
                    }

                    // 替换表达式中的变量
                    foreach (string key in htVarExp.Keys)
                    {
                        //expressions = expressions.Replace(key, htVarExp[key].ToString());
                        Regex reg = new Regex(string.Format(@"\b{0}\b", key));
                        //Regex reg = new Regex(string.Format(@"\b{0}\b", key));
                        string strReplace = string.Format("({0})", htVarExp[key].ToString());
                        expressions = reg.Replace(expressions, strReplace);
                    }
                    foreach (string key in htVariate.Keys)
                    {
                        //expressions = expressions.Replace(key, htVariate[key].ToString());
                        Regex reg = new Regex(string.Format(@"\b{0}\b", key));
                        expressions = reg.Replace(expressions, htVariate[key].ToString());
                    }

                    if (i != nRetLine)
                    {
                        // 变量名
                        string varName = formula.Substring(0, index).Trim();

                        // 变量名前带@的表达式表示先不计算，等后面的表达式一起计算
                        if (varName.Substring(0, 1) == "@")
                        {
                            // 记录
                            htVarExp[varName.Substring(1)] = expressions;

                            log("@标记先不计算");
                        }
                        else
                        {
                            // 表达式计算结果
                            object expValue = Calc(expressions);

                            // 记录
                            htVariate[varName] = expValue;
                            log(string.Format("{0} = {1}", varName, expValue));
                        }
                    }
                    else
                    {
                        // 计算最终结果
                        result = Calc(expressions);
                        log(string.Format("最终结果：{0}", result));
                    }

                }
                else
                {
                    log("不是返回值不计算");
                }
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
        public List<string[]> ParseExpression(string expression, char left_char = '(', char right_char = ')')
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
                        string temp = expression.Substring(0, leftIndex).Trim();//
                        bool bHasSpace = (temp.Length == leftIndex) ? false : true; // 是否有空格
                        char[] arrChar = temp.ToCharArray();
                        int startIndex = 0;
                        for (int j = arrChar.Length - 1; j >= 0; j--)
                        {
                            // 取到下一个不是字母或数字的时候就是函数名
                            if (!char.IsLetterOrDigit(arrChar[j]))
                            {
                                startIndex = j + 1;
                                result[0] = temp.Substring(startIndex);
                                //result[0] = temp.Substring(startIndex, temp.Length - startIndex);
                                break;
                            }
                            else
                            {
                                // 函数名前面没有字符的情况
                                if (j == 0)
                                {
                                    result[0] = temp;
                                    break;
                                }
                            }
                        }

                        if (result[0] != "" && result[0].ToUpper() != "AND" && result[0].ToUpper() != "OR")
                        {
                            if (bHasSpace)
                                throw new FormulaException("函数名和括号之间不能有空格", indexFormula, expression.Substring(startIndex, leftIndex - startIndex));
                        }

                        // 函数内的参数，如果result[0]为空，则表示这是计算式子
                        result[1] = expression.Substring(leftIndex + 1, rightIndex - leftIndex - 1);

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
        public List<string> ParseParams(string strParams, char separator = ',')
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

            // 添加最后一个结果
            lstResult.Add(strParams.Substring(indexParam).Trim());

            return lstResult;
            //return strParams.Split(new string[] { separator }, StringSplitOptions.RemoveEmptyEntries).ToList();
        }

        /// <summary>
        /// 计算单行表达式
        /// </summary>
        /// <param name="expression">表达式</param>
        /// <returns></returns>
        public object Calc(string expression)
        {
            // 
            string tempExp = expression;

            // 先找出最外层括号并计算
            List<string[]> lst = ParseExpression(expression);
            List<object> lstCalc = new List<object>();
            for (int i = 0; i < lst.Count; i++)
            {
                // 如果括号左边没有函数名，则为计算
                if (string.IsNullOrWhiteSpace(lst[i][0])
                    || lst[i][0].ToUpper() == "AND"
                    || lst[i][0].ToUpper() == "OR")
                {
                    // 递归计算
                    lstCalc.Add(Calc(lst[i][1]));

                    // 用占位符替换原表达式，到后面统一计算
                    tempExp = tempExp.Replace("(" + lst[i][1] + ")", "{" + i + "}");
                }
                else // 是函数
                {
                    decimal val = ExecFunction(lst[i][0], lst[i][1]);


                    // 递归计算
                    lstCalc.Add(val);

                    // 用占位符替换原表达式，到后面统一计算
                    tempExp = tempExp.Replace(lst[i][0] + "(" + lst[i][1] + ")", "{" + i + "}");

                    //throw new Exception(lst[i][0]);
                }
            }

            // 将括号里的计算结果替换到表达式里
            string[] arrCalc = lstCalc.ConvertAll(s => s.ToString()).ToArray();
            //string[] arrCalc = Array.ConvertAll(lstCalc.ToArray(), s => s.ToString());
            tempExp = string.Format(tempExp, arrCalc);

            #region 逻辑运算参数准备
            // 逻辑运算
            char[] arrLogic = tempExp.ToCharArray();
            int index = 0;
            List<bool> lstLogicRet = new List<bool>();              // 逻辑结果(item)
            List<string> lstLogicOperator = new List<string>();     // 操作符(& && AND || OR)
            for (int i = 0; i < arrLogic.Length;)
            {
                // 逻辑操作符 (与都用 && 表示, 或都用 || 表示)
                string logicOperator = "";

                // 是否逻辑操作符
                bool bOperator = false;

                // 比较符号所占字符数
                int nOperator = 1;
                if (arrLogic[i] == '&') // &
                {
                    bOperator = true;
                    logicOperator = "&&";
                    if (i < arrLogic.Length - 1 && arrLogic[i + 1] == '&') // &&
                    {
                        nOperator = 2;
                    }
                }
                else if (arrLogic[i] == '|')  // ||
                {
                    if (i < arrLogic.Length - 1 && arrLogic[i + 1] == '|')
                    {
                        bOperator = true;
                        nOperator = 2;
                        logicOperator = "||";
                    }
                }
                else if (arrLogic[i] == 'o' || arrLogic[i] == 'O') // OR
                {
                    if (i < arrLogic.Length - 1)
                    {
                        if (arrLogic[i + 1] == 'r' || arrLogic[i + 1] == 'R')
                        {
                            bOperator = true;
                            nOperator = 2;
                            logicOperator = "||";
                        }
                    }
                }
                else if (arrLogic[i] == 'a' || arrLogic[i] == 'A') // AND
                {
                    if (i < arrLogic.Length - 2)
                    {
                        string str = tempExp.Substring(i, 3).ToUpper();
                        if (str == "AND")
                        {
                            bOperator = true;
                            nOperator = 3;
                            logicOperator = "&&";
                        }
                    }
                }

                if (bOperator)
                {
                    // 逻辑操作符
                    //string logicOperator = tempExp.Substring(i, nOperator);
                    lstLogicOperator.Add(logicOperator);

                    // 逻辑表达式
                    string strExp = tempExp.Substring(index, i - index);
                    bool bRet;
                    if (TryLogicArithmetic(strExp, out bRet))
                    {
                        lstLogicRet.Add(bRet);
                        i += nOperator;
                        index = i;
                    }
                    else
                    {
                        throw new FormulaException("不能进行逻辑运算", indexFormula, strExp);
                    }
                }
                else
                {
                    i++;
                }
            }

            // 逻辑运算添加最后一个结果
            if (lstLogicOperator.Count > 0)
            {
                string strExp = tempExp.Substring(index).Trim();
                //string strExp = tempExp.Substring(index, tempExp.Length - index).Trim();
                bool bRet;
                if (TryLogicArithmetic(strExp, out bRet))
                {
                    lstLogicRet.Add(bRet);
                }
                else
                {
                    throw new FormulaException("不能进行逻辑运算", indexFormula, strExp);
                }
            }
            #endregion

            #region 逻辑运算 (与运算比或运算优先级高)
            if (lstLogicOperator.Count > 0)
            {
                #region 先进行与运算
                // 与运算
                List<bool> lstLogicRetOr = new List<bool>();            // 进行与运算后的结果
                List<string> lstLogicOperatorOr = new List<string>();   // 进行与运算的操作符(或)
                bool tempLogicValue = false;
                for (int i = 0; i < lstLogicOperator.Count; i++)
                {
                    if (lstLogicOperator[i] == "&&") // 与
                    {
                        // 系数(布尔)
                        bool coefficient = lstLogicRet[0];

                        // 如果上一个符号为与运算，则使用记录的系数当被乘数
                        if (i > 0 && (lstLogicOperator[i - 1] == "&&"))
                            coefficient = tempLogicValue;
                        else
                            coefficient = lstLogicRet[i];

                        tempLogicValue = coefficient && lstLogicRet[i + 1];
                    }
                    else // 或
                    {
                        // 如果上一个符号为与运算，则添加数值为之前记录的数值
                        if (i > 0 && (lstLogicOperator[i - 1] == "&&"))
                        {
                            lstLogicRetOr.Add(tempLogicValue);
                            tempLogicValue = false;
                        }
                        else
                        {
                            lstLogicRetOr.Add(lstLogicRet[i]);
                        }

                        lstLogicOperatorOr.Add(lstLogicOperator[i]);
                    }
                }

                // 如果最后一个运算符号为与，则最后一个值为之前记录的逻辑结果
                if (lstLogicOperator.Count > 0 && lstLogicOperator.Last() == "&&")
                {
                    lstLogicRetOr.Add(tempLogicValue);
                }
                else // 如果最后一个运算符号为或，则最后一个值为原来最后一个逻辑结果
                {
                    lstLogicRetOr.Add(lstLogicRet.Last());
                }
                #endregion

                #region 再进行或运算
                // 或运算
                bool retLogic = lstLogicRetOr[0];
                for (int i = 0; i < lstLogicOperatorOr.Count; i++)
                {
                    retLogic = retLogic || lstLogicRetOr[i + 1];
                }

                // 记录过程（有计算过程才记录）
                if (lstLogicOperator.Count > 0 || lstLogicOperatorOr.Count > 0)
                {
                    log(tempExp.Trim() + " = " + retLogic);
                    //log(expression + " = " + result);
                }
                #endregion

                return retLogic;
            }
            #endregion
            else
            {
                #region //>=<运算参数准备
                //char[] arr = tempExp.ToCharArray();
                //string[] arrArithmeticExp = new string[] { "", "" };    // 四则运算表达式
                //string compareOperator = "";                            // 比较符(> >= = <= < <> !=)
                //for (int i = 0; i < arr.Length; i++)
                //{
                //    // != 不等于
                //    bool bNotEqual = (arr[i] == '!' && i < arr.Length - 1 && arr[i + 1] == '=') ? true : false;

                //    if (arr[i] == '>' || arr[i] == '=' || arr[i] == '<' || bNotEqual)
                //    {
                //        // 比较符号所占字符数
                //        int nOperator = 1;
                //        if (i < arr.Length - 1)
                //        {
                //            if (arr[i + 1] == '=')
                //                nOperator = 2;
                //            else if (arr[i] == '<' && arr[i + 1] == '>')
                //                nOperator = 2;
                //            else if (bNotEqual)
                //                nOperator = 2;
                //        }

                //        // 比较符号 (> >= = <= <)
                //        compareOperator = tempExp.Substring(i, nOperator);

                //        // 左边运算表达式
                //        arrArithmeticExp[0] = tempExp.Substring(0, i);

                //        // 右边运算表达式
                //        arrArithmeticExp[1] = tempExp.Substring(i + nOperator, arr.Length - i - nOperator);

                //        // 比较表达式不能有多个比较符号
                //        if (arrArithmeticExp[1].Contains("=")
                //            || arrArithmeticExp[1].Contains(">")
                //            || arrArithmeticExp[1].Contains("<"))
                //        {
                //            throw new Exception("比较表达式不能有多个比较符号");
                //        }
                //        break;
                //    }
                //}
                #endregion

                #region // 四则运算
                #region //四则运算参数准备
                //// 四则运算
                //char[] arr = tempExp.ToCharArray();
                //int index = 0;
                //List<decimal> lstValue = new List<decimal>();   // 数值
                //List<char> lstOperator = new List<char>();      // 操作符(+ - * /)
                //for (int i = 0; i < arr.Length; i++)
                //{
                //    switch (arr[i])
                //    {
                //        case '+':
                //        case '-':
                //        case '*':
                //        case '/':
                //            {
                //                string strValue = tempExp.Substring(index, i - index).Trim();
                //                lstValue.Add(decimal.Parse(strValue));
                //                lstOperator.Add(arr[i]);
                //                index = i + 1;
                //            }
                //            break;

                //        case '>':
                //        case '=':
                //        case '<':
                //            {
                //                string strValue = tempExp.Substring(index, i - index).Trim();
                //                lstValue.Add(decimal.Parse(strValue));
                //                lstOperator.Add(arr[i]);
                //                index = i + 1;
                //            }
                //            break;

                //        default:
                //            break;
                //    }
                //}

                //// 添加最后一个数值
                //string v = tempExp.Substring(index, tempExp.Length - index).Trim();
                //lstValue.Add(decimal.Parse(v));
                #endregion

                #region //先乘除
                //// 乘除
                //List<decimal> lstValueNew = new List<decimal>();   // 进行乘除后的数值
                //List<char> lstOperatorNew = new List<char>();      // 进行乘除后的操作符(+ -)
                //decimal tempValue = 0;
                //for (int i = 0; i < lstOperator.Count; i++)
                //{
                //    if(lstOperator[i] == '*') // 乘
                //    {
                //        // 被乘数
                //        decimal coefficient;

                //        // 如果上一个符号为乘或除，则使用记录的系数当被乘数
                //        if (i > 0 && (lstOperator[i - 1] == '*' || lstOperator[i - 1] == '/'))
                //            coefficient = tempValue;
                //        else
                //            coefficient = lstValue[i];

                //        tempValue = coefficient * lstValue[i+1];
                //    }
                //    else if ( lstOperator[i] == '/') // 除
                //    {
                //        // 被除数
                //        decimal coefficient;

                //        // 如果上一个符号为乘或除，则使用记录的系数当被除数
                //        if (i > 0 && (lstOperator[i - 1] == '*' || lstOperator[i - 1] == '/'))
                //            coefficient = tempValue;
                //        else
                //            coefficient = lstValue[i];

                //        tempValue = coefficient / lstValue[i + 1];
                //    }
                //    else // 加减
                //    {
                //        // 如果上一个符号为乘或除，则添加数值为之前记录的数值
                //        if (i > 0 && (lstOperator[i - 1] == '*' || lstOperator[i - 1] == '/'))
                //        {
                //            lstValueNew.Add(tempValue);
                //            tempValue = 0;
                //        }
                //        else
                //        {
                //            lstValueNew.Add(lstValue[i]);
                //        }

                //        lstOperatorNew.Add(lstOperator[i]);
                //    }
                //}

                //// 如果最后一个运算符号为乘除，则最后一个值为之前记录的数值
                //if (lstOperator.Count > 0 && (lstOperator.Last() == '*' || lstOperator.Last() == '/'))
                //{
                //    lstValueNew.Add(tempValue);
                //}
                //else // 如果最后一个运算符号为加减，则最后一个值为原来最后一个数值
                //{

                //    lstValueNew.Add(lstValue.Last());
                //}
                #endregion

                #region //后加减
                //// 加减
                //decimal result = lstValueNew[0];
                //for (int i = 0; i < lstOperatorNew.Count; i++)
                //{
                //    if (lstOperatorNew[i] == '+') // 加
                //    {
                //        result += lstValueNew[i + 1];
                //    }
                //    else if (lstOperatorNew[i] == '-') // 减
                //    {
                //        result -= lstValueNew[i + 1];
                //    }
                //}
                #endregion
                #endregion

                #region //>=<运算
                //// >=<运算
                //if (!string.IsNullOrWhiteSpace(compareOperator))
                //{
                //    bool result = false;
                //    decimal leftValue = Arithmetic(arrArithmeticExp[0]);    // 比较运算符左边的值
                //    decimal rightValue = Arithmetic(arrArithmeticExp[1]);   // 比较运算符右边的值
                //    switch (compareOperator)
                //    {
                //        case ">=":
                //            result = leftValue >= rightValue ? true : false;
                //            break;

                //        case ">":
                //            result = leftValue > rightValue ? true : false;
                //            break;

                //        case "<=":
                //            result = leftValue <= rightValue ? true : false;
                //            break;

                //        case "<":
                //            result = leftValue < rightValue ? true : false;
                //            break;

                //        case "=":
                //        case "==":
                //            result = leftValue == rightValue ? true : false;
                //            break;

                //        case "!=":
                //        case "<>":
                //            result = leftValue != rightValue ? true : false;
                //            break;

                //        default:
                //            break;
                //    }

                //    // 记录过程（有计算过程才记录）
                //    log(tempExp.Trim() + " = " + result);
                //    return result;
                //}
                #endregion
                #region 逻辑运算 (>=<运算)
                bool bRet;
                if (TryLogicArithmetic(tempExp, out bRet))
                {
                    return bRet;
                }
                #endregion
                #region 四则运算
                else
                {
                    decimal result = Arithmetic(tempExp);
                    return result;
                }
                #endregion
            }
        }

        /// <summary>
        /// 执行函数
        /// </summary>
        /// <param name="functionName"></param>
        /// <param name="Params"></param>
        /// <returns></returns>
        public virtual decimal ExecFunction(string functionName, string Params)
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
                // 最小值
                case "MIN":
                    {
                        JudgeParamCount(2, lstParams.Count, funcFormula);
                        decimal val1 = Convert.ToDecimal(Calc(lstParams[0]));
                        decimal val2 = Convert.ToDecimal(Calc(lstParams[1]));
                        result = Math.Min(val1, val2);
                    }
                    break;

                // 最大值
                case "MAX":
                    {
                        JudgeParamCount(2, lstParams.Count, funcFormula);
                        decimal val1 = Convert.ToDecimal(Calc(lstParams[0]));
                        decimal val2 = Convert.ToDecimal(Calc(lstParams[1]));
                        result = Math.Max(val1, val2);
                    }
                    break;

                // 绝对值
                case "ABS":
                    {
                        JudgeParamCount(1, lstParams.Count, funcFormula);
                        result = Math.Abs(Convert.ToDecimal(Calc(lstParams[0])));
                    }
                    break;

                // 幂函数
                case "POW":
                    {
                        JudgeParamCount(2, lstParams.Count, funcFormula);
                        double x = Convert.ToDouble(Calc(lstParams[0]));
                        double y = Convert.ToDouble(Calc(lstParams[1]));
                        result = Convert.ToDecimal(Math.Pow(x, y));
                    }
                    break;

                // 开方
                case "SQRT":
                    {
                        JudgeParamCount(1, lstParams.Count, funcFormula);
                        double v = Convert.ToDouble(Calc(lstParams[0]));
                        result = Convert.ToDecimal(Math.Sqrt(v));
                    }
                    break;

                // 上限 (返回大于或等于指定的十进制数的最小整数值)
                case "CEILING":
                    {
                        JudgeParamCount(1, lstParams.Count, funcFormula);
                        result = Math.Ceiling(Convert.ToDecimal(Calc(lstParams[0])));
                    }
                    break;

                // 下限 (返回小于或等于指定小数的最大整数)
                case "FLOOR":
                    {
                        JudgeParamCount(1, lstParams.Count, funcFormula);
                        result = Math.Floor(Convert.ToDecimal(Calc(lstParams[0])));
                    }
                    break;

                // 将小数值舍入到最接近的整数值 (四舍六入五凑偶)
                case "ROUND":
                    {
                        JudgeParamCount(1, lstParams.Count, funcFormula);
                        result = Math.Round(Convert.ToDecimal(Calc(lstParams[0])));
                    }
                    break;

                // 取整数部分
                case "TRUNCATE":
                    {
                        JudgeParamCount(1, lstParams.Count, funcFormula);
                        result = Math.Truncate(Convert.ToDecimal(Calc(lstParams[0])));
                    }
                    break;

                // 指定数字的自然对数（底为 e）
                case "LOG":
                    {
                        JudgeParamCount(1, lstParams.Count, funcFormula);
                        double v = Convert.ToDouble(Calc(lstParams[0]));
                        result = Convert.ToDecimal(Math.Log(v));
                    }
                    break;

                // 指定数字以 10 为底的对数
                case "LOG10":
                    {
                        JudgeParamCount(1, lstParams.Count, funcFormula);
                        double v = Convert.ToDouble(Calc(lstParams[0]));
                        result = Convert.ToDecimal(Math.Log10(v));
                    }
                    break;

                #region 三角函数

                // 正弦值
                case "SIN":
                    {
                        JudgeParamCount(1, lstParams.Count, funcFormula);
                        result = Convert.ToDecimal(Math.Sin(Convert.ToDouble(Calc(lstParams[0]))));
                    }
                    break;

                // 余弦值
                case "COS":
                    {
                        JudgeParamCount(1, lstParams.Count, funcFormula);
                        result = Convert.ToDecimal(Math.Cos(Convert.ToDouble(Calc(lstParams[0]))));
                    }
                    break;

                // 正切值
                case "TAN":
                    {
                        JudgeParamCount(1, lstParams.Count, funcFormula);
                        result = Convert.ToDecimal(Math.Tan(Convert.ToDouble(Calc(lstParams[0]))));
                    }
                    break;

                // 余切值
                case "COT":
                    {
                        JudgeParamCount(1, lstParams.Count, funcFormula);
                        result = Convert.ToDecimal(1 / Math.Tan(Convert.ToDouble(Calc(lstParams[0]))));
                    }
                    break;



                // 反正弦值
                case "ASIN":
                    {
                        JudgeParamCount(1, lstParams.Count, funcFormula);
                        result = Convert.ToDecimal(Math.Asin(Convert.ToDouble(Calc(lstParams[0]))));
                    }
                    break;

                // 反余弦值
                case "ACOS":
                    {
                        JudgeParamCount(1, lstParams.Count, funcFormula);
                        result = Convert.ToDecimal(Math.Acos(Convert.ToDouble(Calc(lstParams[0]))));
                    }
                    break;

                // 反正切值
                case "ATAN":
                    {
                        JudgeParamCount(1, lstParams.Count, funcFormula);
                        result = Convert.ToDecimal(Math.Atan(Convert.ToDouble(Calc(lstParams[0]))));
                    }
                    break;

                // 反余切值
                case "ACOT":
                    {
                        JudgeParamCount(1, lstParams.Count, funcFormula);
                        result = Convert.ToDecimal(Math.Atan(1 / Convert.ToDouble(Calc(lstParams[0]))));
                    }
                    break;
                #endregion

                #region 其他
                // 弧度转角度
                case "TOANGLE":
                    {
                        JudgeParamCount(1, lstParams.Count, funcFormula);
                        double temp = Convert.ToDouble(Calc(lstParams[0])) * 180 / Math.PI;
                        result = Convert.ToDecimal(temp);
                    }
                    break;

                // 角度转弧度
                case "TORADIAN":
                    {
                        JudgeParamCount(1, lstParams.Count, funcFormula);
                        double temp = Convert.ToDouble(Calc(lstParams[0])) * Math.PI / 180;
                        result = Convert.ToDecimal(temp);
                    }
                    break;
                #endregion

                default:
                    string formula = string.Format("{0}({1})", functionName, Params);
                    string msg = string.Format("没有{0}函数", functionName);
                    throw new FormulaException(msg, indexFormula, formula);
                    //break;
            }

            // 记录过程（有计算过程才记录）
            log(string.Format("{0}({1}) = {2}", functionName, Params, result));

            return result;
        }

        /// <summary>
        /// 判断参数个数是否正确
        /// </summary>
        /// <param name="countFunc">函数参数个数</param>
        /// <param name="countParam">输入的参数个数</param>
        /// <param name="formula">相关公式</param>
        protected void JudgeParamCount(int countFunc, int countParam, string formula = "")
        {
            if (countFunc != countParam)
            {
                string msg = string.Format("函数参数的个数是{0}，入参个数是{1}", countFunc, countParam);
                throw new FormulaException(msg, indexFormula, formula);
            }
        }

        /// <summary>
        /// 根据函数名和参数获取函数公式
        /// </summary>
        /// <param name="functionName"></param>
        /// <param name="Params"></param>
        /// <returns></returns>
        protected string GetFuncFormula(string functionName, string Params)
        {
            return string.Format("{0}({1})", functionName, Params);
        }

        /// <summary>
        /// 尝试逻辑运算
        /// 如果表达式没有运算，且不能转换成布尔类型，则返回不是逻辑运算
        /// </summary>
        /// <param name="expression">表达式</param>
        /// <param name="result">结果</param>
        /// <returns>是否逻辑运算</returns>
        public bool TryLogicArithmetic(string expression, out bool result)
        {
            string tempExp = expression;
            result = false;

            #region >=<运算参数准备
            char[] arr = tempExp.ToCharArray();
            string[] arrArithmeticExp = new string[] { "", "" };    // 四则运算表达式
            string compareOperator = "";                            // 比较符(> >= = <= < <> !=)
            for (int i = 0; i < arr.Length; i++)
            {
                // != 不等于
                bool bNotEqual = (arr[i] == '!' && i < arr.Length - 1 && arr[i + 1] == '=') ? true : false;

                if (arr[i] == '>' || arr[i] == '=' || arr[i] == '<' || bNotEqual)
                {
                    // 比较符号所占字符数
                    int nOperator = 1;
                    if (i < arr.Length - 1)
                    {
                        if (arr[i + 1] == '=')
                            nOperator = 2;
                        else if (arr[i] == '<' && arr[i + 1] == '>')
                            nOperator = 2;
                        else if (bNotEqual)
                            nOperator = 2;
                    }

                    // 比较符号 (> >= = <= <)
                    compareOperator = tempExp.Substring(i, nOperator);

                    // 左边运算表达式
                    arrArithmeticExp[0] = tempExp.Substring(0, i);

                    // 右边运算表达式
                    arrArithmeticExp[1] = tempExp.Substring(i + nOperator);
                    //arrArithmeticExp[1] = tempExp.Substring(i + nOperator, arr.Length - i - nOperator);

                    // 比较表达式不能有多个比较符号
                    if (arrArithmeticExp[1].Contains("=")
                        || arrArithmeticExp[1].Contains(">")
                        || arrArithmeticExp[1].Contains("<"))
                    {
                        throw new FormulaException("比较表达式不能有多个比较符号", indexFormula, expression);
                    }
                    break;
                }
            }
            #endregion

            #region >=<运算
            // >=<运算
            if (!string.IsNullOrWhiteSpace(compareOperator))
            {
                decimal leftValue = Arithmetic(arrArithmeticExp[0]);    // 比较运算符左边的值
                decimal rightValue = Arithmetic(arrArithmeticExp[1]);   // 比较运算符右边的值
                switch (compareOperator)
                {
                    case ">=":
                        result = leftValue >= rightValue ? true : false;
                        break;

                    case ">":
                        result = leftValue > rightValue ? true : false;
                        break;

                    case "<=":
                        result = leftValue <= rightValue ? true : false;
                        break;

                    case "<":
                        result = leftValue < rightValue ? true : false;
                        break;

                    case "=":
                    case "==":
                        result = leftValue == rightValue ? true : false;
                        break;

                    case "!=":
                    case "<>":
                        result = leftValue != rightValue ? true : false;
                        break;

                    default:
                        break;
                }

                // 记录过程（有计算过程才记录）
                log(tempExp.Trim() + " = " + result);
            }
            #endregion
            else
            {
                if(tempExp == "1")
                {
                    result = true;
                }
                else if (tempExp == "0")
                {
                    result = false;
                }
                else
                {
                    // 如果表达式没有运算，且不能转换成布尔类型，则返回不是逻辑运算
                    if (!bool.TryParse(tempExp, out result))
                        return false;
                }
            }

            return true;
        }

        /// <summary>
        /// 四则运算
        /// </summary>
        /// <param name="expression">表达式</param>
        /// <returns></returns>
        public decimal Arithmetic(string expression)
        {
            string tempExp = expression.Trim();

            try
            {
                #region 四则运算参数准备
                // 四则运算
                char[] arr = tempExp.ToCharArray();
                int index = 0;
                List<decimal> lstValue = new List<decimal>();   // 数值
                List<char> lstOperator = new List<char>();      // 操作符(+ - * /)
                for (int i = 1; i < arr.Length; i++) // i从1开始是为了兼容开头负数，而且也不会有表达式第一个字符就是运算符
                {
                    switch (arr[i])
                    {
                        case '+':
                        case '-':
                        case '*':
                        case '/':
                            {
                                string strValue = tempExp.Substring(index, i - index).Trim();
                                lstValue.Add(GetValue(strValue));
                                lstOperator.Add(arr[i]);
                                index = i + 1;
                            }
                            break;

                        default:
                            break;
                    }
                }

                // 添加最后一个数值
                string v = tempExp.Substring(index).Trim();
                //string v = tempExp.Substring(index, tempExp.Length - index).Trim();
                lstValue.Add(GetValue(v));
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
                if (lstOperator.Count > 0 && (lstOperator.Last() == '*' || lstOperator.Last() == '/'))
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

                // 记录过程（有计算过程才记录）
                if (lstOperator.Count > 0 || lstOperatorNew.Count > 0)
                {
                    log(tempExp.Trim() + " = " + result);
                    //log(expression + " = " + result);
                }

                return result;
            }
            catch (Exception ex)
            {
                throw new FormulaException(ex.Message, indexFormula, expression);
            }
        }

        /// <summary>
        /// 获取值 (定义的值、转换成decimal类型)
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        protected virtual decimal GetValue(string value)
        {
            decimal result;
            switch (value)
            {
                case "E":
                    result = Convert.ToDecimal(Math.E);
                    break;

                case "PI":
                    result = Convert.ToDecimal(Math.PI);
                    break;

                default:
                    result = decimal.Parse(value);
                    break;
            }

            return result;
        }

        public void log(string text)
        {
#if DEBUG
            MngLog.Instance.Write(text);
            //rtbProcess.AppendText(text + "\r\n");
#endif
        }
    }
}
