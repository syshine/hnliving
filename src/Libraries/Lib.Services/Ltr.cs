using Lib.Core;
using LibLtr;
using LibLtr.Entity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Lib.Services
{
    public partial class Ltr
    {
        static LtrUitls lu = new LtrUitls(new Data.Ltr());

        #region Qxc
        /// <summary>
        /// 获得Qxc数据
        /// </summary>
        /// <param name="issue">期号</param>
        /// <param name="count">数量</param>
        /// <returns></returns>
        public List<ShowLtrQxcModel> GetQxcData(int count, int issue = -1)
        {
            EntityDataRepertoryType edrt = new EntityDataRepertoryType();
            if (count <= 0)
            {
                // 获取全部
                edrt.SetTypeAll();
            }
            else
            {
                edrt.SetTypeCnt(count, issue);
            }
            DataTable dt = lu.LaQxc.GetData(edrt);

            List<ShowLtrQxcModel> lstEntity = new List<ShowLtrQxcModel>();
            if (dt != null)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    ShowLtrQxcModel entity = new ShowLtrQxcModel();
                    entity.Issue = int.Parse(dr["issue"].ToString());
                    entity.Sum = int.Parse(dr["sum"].ToString());
                    entity.Numb = dr["numbers"].ToString();
                    entity.Date = DateTime.Parse(dr["ltr_date"].ToString());
                    lstEntity.Add(entity);
                }
            }

            return lstEntity;
        }

        /// <summary>
        /// 获得Qxc合数
        /// </summary>
        /// <param name="issue">期号</param>
        /// <returns></returns>
        public EntitySumResult GetQxcSum(int issue = -1)
        {
            EntitySumResult esr = lu.LaQxc.GetSumNumberInfo(issue);

            return esr;
        }

        /// <summary>
        /// 获得Qxc高频数据
        /// </summary>
        /// <param name="count">数量</param>
        /// <param name="issue">期号</param>
        /// <param name="filter">数据筛选条件</param>
        /// <param name="filter_ret">结果筛选条件</param>
        /// <returns></returns>
        public ResultEntity GetQxcHighFrequency(int count = 100, int issue = -1, string filter = "", string filter_ret = "")
        {
            ResultEntity result = new ResultEntity();
            try
            {
                EntityDataRepertoryType edrt = new EntityDataRepertoryType();
                if (count <= 0)
                {
                    // 获取全部
                    edrt.SetTypeAll();
                }
                else
                {
                    edrt.SetTypeCnt(count, issue);
                }
                DataTable dt = lu.LaQxc.GetData(edrt); // 获取数据
                //dt.Columns["issue"].ColumnName = "期号";
                //dt.Columns["numbers_4"].ColumnName = "号码";
                //dt.Columns["ltr_date"].ColumnName = "日期";
                //dt.Columns["thousands"].ColumnName = "千";
                //dt.Columns["hundreds"].ColumnName = "百";
                //dt.Columns["tens"].ColumnName = "十";
                //dt.Columns["units"].ColumnName = "个";
                //dt.Columns["relation_size"].ColumnName = "大小";
                //dt.Columns["relation_parity"].ColumnName = "单双";
                //dt.Columns["day_of_week"].ColumnName = "星期几";
                DataTable dtTemp = dt.Select(filter, "").CopyToDataTable(); // 筛选数据

                // 计算高频数据
                DataTable dtResult = lu.LaQxc.GetHighFrequencyLocation(dtTemp);//GetHighFrequencyLocation(dtTemp);

                // 筛选结果数据
                if(string.IsNullOrWhiteSpace(filter_ret))
                {
                    filter_ret = "号码数量 > 3 and 回报率 > 2";
                }
                DataRow[] drs = dtResult.Select(filter_ret, "回报率 desc");
                if (drs.Length <= 0)
                {
                    result.SetSuccess();
                }
                else
                {
                    dtResult = drs.CopyToDataTable();
                    result.SetSuccess(dtResult);
                }
            }
            catch (Exception ex)
            {
                result.SetError(ex.Message);
            }

            return result;
        }

        ///// <summary>
        ///// 获取高频定位
        ///// </summary>
        ///// <param name="dt"></param>
        ///// <returns></returns>
        //public DataTable GetHighFrequencyLocation(DataTable dt)
        //{
        //    DataTable dtResult = new DataTable();
        //    dtResult.Columns.Add("numbs1");
        //    dtResult.Columns.Add("numbs2");
        //    dtResult.Columns.Add("numbs3");
        //    dtResult.Columns.Add("号码");
        //    dtResult.Columns.Add("号码数量", typeof(int));
        //    dtResult.Columns.Add("出现次数", typeof(int));
        //    dtResult.Columns.Add("概率", typeof(decimal));
        //    dtResult.Columns.Add("回报率", typeof(decimal));

        //    int avgCnt = (int)Math.Ceiling(dt.Rows.Count / 100.0);


        //    // 计数
        //    //int[,,] arrCnt2 = new int[6, 10, 10];
        //    int[,,,] arrCnt3 = new int[4, 10, 10, 10];
        //    int[][][] arrCnt2 = new int[6][][];
        //    for (int i = 0; i < arrCnt2.Length; i++)
        //    {
        //        arrCnt2[i] = new int[10][];
        //        for (int j = 0; j < arrCnt2[i].Length; j++)
        //        {
        //            arrCnt2[i][j] = new int[10];
        //        }
        //    }

        //    for (int i = 0; i < dt.Rows.Count; i++)
        //    {
        //        DataRow row = dt.Rows[i];

        //        int thousands = Convert.ToInt32(row["thousands"]);
        //        int hundreds = Convert.ToInt32(row["hundreds"]);
        //        int tens = Convert.ToInt32(row["tens"]);
        //        int units = Convert.ToInt32(row["units"]);

        //        #region 计数
        //        // 两位
        //        arrCnt2[0][thousands][hundreds]++;   // 千百 ??XX
        //        arrCnt2[1][thousands][tens]++;       // 千十 ?X?X
        //        arrCnt2[2][thousands][units]++;      // 千个 ?XX?
        //        arrCnt2[3][hundreds][tens]++;        // 百十 X??X
        //        arrCnt2[4][hundreds][units]++;       // 百个 X?X?
        //        arrCnt2[5][tens][units]++;           // 十个 XX??

        //        // 三位
        //        arrCnt3[0, thousands, hundreds, tens]++;   // 千百十 ???X
        //        arrCnt3[1, thousands, hundreds, units]++;  // 千百个 ??X?
        //        arrCnt3[2, thousands, tens, units]++;      // 千十个 ?X??
        //        arrCnt3[3, hundreds, tens, units]++;       // 百十个 X???
        //        #endregion
        //    }

        //    string[] fields2 = new string[6] { "{0}.{1}.X.X", "{0}.X.{1}.X", "{0}.X.X.{1}", "X.{0}.{1}.X", "X.{0}.X.{1}", "X.X.{0}.{1}" };
            
        //    for (int i = 0; i < 6; i++)
        //    {
        //        #region 1个数字
        //        // 1个数字
        //        for (int n1 = 0; n1 < 10; n1++)
        //        {
        //            int[] arr = arrCnt2[i][n1];

        //            // 排序后倒序
        //            int[] arrSort = arr.Distinct().ToArray();
        //            Array.Sort(arrSort);
        //            Array.Reverse(arrSort);

        //            // 出现次数第2多
        //            string numbs2_2 = "";
        //            int cnt2_2 = 0;

        //            // 出现次数第3多
        //            string numbs2_3 = "";
        //            int cnt2_3 = 0;

        //            for (int m1 = 0; m1 < 10; m1++)
        //            {
        //                int count = arrCnt2[i][n1][m1];
        //                #region// 1个的样本容量太小，不添加
        //                //if (count >= arrSort[0] && count >= avgCnt)
        //                //{
        //                //    DataRow drNew = dtResult.NewRow();

        //                //    drNew["location"] = string.Format(fields2[i], n1, m1);
        //                //    drNew["numbs1"] = n1;
        //                //    drNew["numbs2"] = m1;
        //                //    drNew["numbs_count"] = 1;
        //                //    drNew["cnt"] = count;
        //                //    drNew["frequency"] = Math.Round(1.0m * count / dt.Rows.Count, 4);
        //                //    drNew["rate"] = Math.Round(100.0m * count / dt.Rows.Count, 4);

        //                //    dtResult.Rows.Add(drNew);
        //                //}
        //                #endregion

        //                if (arrSort.Length > 2)
        //                {
        //                    if (count >= arrSort[2] && count >= avgCnt)
        //                    {
        //                        numbs2_3 += m1;
        //                        cnt2_3 += count;
        //                    }
        //                }
        //                if (arrSort.Length > 1)
        //                {
        //                    if (arrCnt2[i][n1][m1] >= arrSort[1] && arrCnt2[i][n1][m1] >= avgCnt)
        //                    {
        //                        numbs2_2 += m1;
        //                        cnt2_2 += count;
        //                    }
        //                }
        //            }
        //            if (numbs2_2 != "")
        //            {
        //                DataRow drNew = dtResult.NewRow();

        //                drNew["numbs1"] = n1;
        //                drNew["numbs2"] = numbs2_2;
        //                drNew["号码"] = string.Format(fields2[i], n1, numbs2_2);
        //                drNew["号码数量"] = numbs2_2.Length;
        //                drNew["出现次数"] = cnt2_2;
        //                drNew["概率"] = Math.Round(1.0m * cnt2_2 / dt.Rows.Count, 2);
        //                drNew["回报率"] = Math.Round(100.0m * cnt2_2 / (dt.Rows.Count * numbs2_2.Length), 2);

        //                dtResult.Rows.Add(drNew);
        //            }
        //            if (numbs2_3 != "")
        //            {
        //                DataRow drNew = dtResult.NewRow();

        //                drNew["numbs1"] = n1;
        //                drNew["numbs2"] = numbs2_3;
        //                drNew["号码"] = string.Format(fields2[i], n1, numbs2_3);
        //                drNew["号码数量"] = numbs2_3.Length;
        //                drNew["出现次数"] = cnt2_3;
        //                drNew["概率"] = Math.Round(1.0m * cnt2_3 / dt.Rows.Count, 2);
        //                drNew["回报率"] = Math.Round(100.0m * cnt2_3 / (dt.Rows.Count * numbs2_3.Length), 2);

        //                dtResult.Rows.Add(drNew);
        //            }
        //        }
        //        #endregion

        //        #region 2个数字
        //        // 2个数字
        //        for (int n1 = 0; n1 < 10; n1++)
        //        {
        //            for (int n2 = n1 + 1; n2 < 10; n2++)
        //            {
        //                int[] arr = new int[10];
        //                for (int j = 0; j < 10; j++)
        //                {
        //                    arr[j] = arrCnt2[0][n1][j] + arrCnt2[0][n2][j];
        //                }

        //                // 排序后倒序
        //                int[] arrSort = arr.Distinct().ToArray();
        //                Array.Sort(arrSort);
        //                Array.Reverse(arrSort);

        //                string numbs1 = n1.ToString() + n2.ToString();

        //                // 出现次数第2多
        //                string numbs2_2 = "";
        //                int cnt2_2 = 0;

        //                // 出现次数第3多
        //                string numbs2_3 = "";
        //                int cnt2_3 = 0;

        //                for (int m1 = 0; m1 < 10; m1++)
        //                {
        //                    int count = arrCnt2[i][n1][m1] + arrCnt2[i][n2][m1];
        //                    if (count >= arrSort[0] && count >= avgCnt * 2)
        //                    {
        //                        DataRow drNew = dtResult.NewRow();

        //                        drNew["numbs1"] = numbs1;
        //                        drNew["numbs2"] = m1;
        //                        drNew["号码"] = string.Format(fields2[i], drNew["numbs1"], m1);
        //                        drNew["号码数量"] = numbs1.Length;
        //                        drNew["出现次数"] = count;
        //                        drNew["概率"] = Math.Round(1.0m * count / dt.Rows.Count, 2);
        //                        drNew["回报率"] = Math.Round(100.0m * count / (dt.Rows.Count * numbs1.Length), 2);

        //                        dtResult.Rows.Add(drNew);
        //                    }

        //                    if (arrSort.Length > 2)
        //                    {
        //                        if (count >= arrSort[2] && count >= avgCnt * 2)
        //                        {
        //                            numbs2_3 += m1.ToString();
        //                            cnt2_3 += count;
        //                        }
        //                    }
        //                    if (arrSort.Length > 1)
        //                    {
        //                        if (count >= arrSort[1] && count >= avgCnt * 2)
        //                        {
        //                            numbs2_2 += m1.ToString();
        //                            cnt2_2 += count;
        //                        }
        //                    }
        //                }
        //                if (numbs2_2 != "" && numbs2_2.Length != 1) // 避免重复添加
        //                {
        //                    DataRow drNew = dtResult.NewRow();
                            
        //                    drNew["numbs1"] = numbs1;
        //                    drNew["numbs2"] = numbs2_2;
        //                    drNew["号码"] = string.Format(fields2[i], drNew["numbs1"], numbs2_2);
        //                    drNew["号码数量"] = numbs1.Length * numbs2_2.Length;
        //                    drNew["出现次数"] = cnt2_2;
        //                    drNew["概率"] = Math.Round(1.0m * cnt2_2 / dt.Rows.Count, 2);
        //                    drNew["回报率"] = Math.Round(100.0m * cnt2_2 / (dt.Rows.Count * numbs1.Length * numbs2_2.Length), 2);

        //                    dtResult.Rows.Add(drNew);
        //                }
        //                if (numbs2_3 != "" && numbs2_3.Length != 1 && numbs2_3 != numbs2_2) // 避免重复添加
        //                {
        //                    DataRow drNew = dtResult.NewRow();

        //                    drNew["numbs1"] = numbs1;
        //                    drNew["numbs2"] = numbs2_3;
        //                    drNew["号码"] = string.Format(fields2[i], drNew["numbs1"], numbs2_3);
        //                    drNew["号码数量"] = numbs1.Length * numbs2_3.Length;
        //                    drNew["出现次数"] = cnt2_3;
        //                    drNew["概率"] = Math.Round(1.0m * cnt2_3 / dt.Rows.Count, 2);
        //                    drNew["回报率"] = Math.Round(100.0m * cnt2_3 / (dt.Rows.Count * numbs1.Length * numbs2_3.Length), 2);

        //                    dtResult.Rows.Add(drNew);
        //                }
        //            }
        //        }
        //        #endregion

        //        #region 3个数字
        //        // 3个数字
        //        for (int n1 = 0; n1 < 10; n1++)
        //        {
        //            for (int n2 = n1 + 1; n2 < 10; n2++)
        //            {
        //                for (int n3 = n2 + 1; n3 < 10; n3++)
        //                {
        //                    int[] arr = new int[10];
        //                    for (int j = 0; j < 10; j++)
        //                    {
        //                        arr[j] = arrCnt2[i][n1][j] + arrCnt2[i][n2][j] + arrCnt2[i][n3][j];
        //                    }

        //                    // 排序后倒序
        //                    int[] arrSort = arr.Distinct().ToArray();
        //                    Array.Sort(arrSort);
        //                    Array.Reverse(arrSort);

        //                    string numbs1 = n1.ToString() + n2.ToString() + n3.ToString();

        //                    // 出现次数第2多
        //                    string numbs2_2 = "";
        //                    int cnt2_2 = 0;

        //                    // 出现次数第3多
        //                    string numbs2_3 = "";
        //                    int cnt2_3 = 0;

        //                    for (int m1 = 0; m1 < 10; m1++)
        //                    {
        //                        int count = arrCnt2[i][n1][m1] + arrCnt2[i][n2][m1] + arrCnt2[i][n3][m1];
        //                        if (count >= arrSort[0] && count >= avgCnt * 2)
        //                        {
        //                            DataRow drNew = dtResult.NewRow();

        //                            drNew["numbs1"] = numbs1;
        //                            drNew["numbs2"] = m1;
        //                            drNew["号码"] = string.Format(fields2[i], drNew["numbs1"], m1);
        //                            drNew["号码数量"] = numbs1.Length;
        //                            drNew["出现次数"] = count;
        //                            drNew["概率"] = Math.Round(1.0m * count / dt.Rows.Count, 2);
        //                            drNew["回报率"] = Math.Round(100.0m * count / (dt.Rows.Count * numbs1.Length), 2);

        //                            dtResult.Rows.Add(drNew);
        //                        }

        //                        if (arrSort.Length > 2)
        //                        {
        //                            if (count >= arrSort[2] && count >= avgCnt * 2)
        //                            {
        //                                numbs2_3 += m1.ToString();
        //                                cnt2_3 += count;
        //                            }
        //                        }
        //                        if (arrSort.Length > 1)
        //                        {
        //                            if (count >= arrSort[1] && count >= avgCnt * 2)
        //                            {
        //                                numbs2_2 += m1.ToString();
        //                                cnt2_2 += count;
        //                            }
        //                        }
        //                    }
        //                    if (numbs2_2 != "" && numbs2_2.Length != 1) // 避免重复添加
        //                    {
        //                        DataRow drNew = dtResult.NewRow();
                                
        //                        drNew["numbs1"] = numbs1;
        //                        drNew["numbs2"] = numbs2_2;
        //                        drNew["号码"] = string.Format(fields2[i], drNew["numbs1"], numbs2_2);
        //                        drNew["号码数量"] = numbs1.Length * numbs2_2.Length;
        //                        drNew["出现次数"] = cnt2_2;
        //                        drNew["概率"] = Math.Round(1.0m * cnt2_2 / dt.Rows.Count, 2);
        //                        drNew["回报率"] = Math.Round(100.0m * cnt2_2 / (dt.Rows.Count * numbs1.Length * numbs2_2.Length), 2);

        //                        dtResult.Rows.Add(drNew);
        //                    }
        //                    if (numbs2_3 != "" && numbs2_3.Length != 1 && numbs2_3 != numbs2_2) // 避免重复添加
        //                    {
        //                        DataRow drNew = dtResult.NewRow();

        //                        drNew["numbs1"] = numbs1;
        //                        drNew["numbs2"] = numbs2_3;
        //                        drNew["号码"] = string.Format(fields2[i], drNew["numbs1"], numbs2_3);
        //                        drNew["号码数量"] = numbs1.Length * numbs2_3.Length;
        //                        drNew["出现次数"] = cnt2_3;
        //                        drNew["概率"] = Math.Round(1.0m * cnt2_3 / dt.Rows.Count, 2);
        //                        drNew["回报率"] = Math.Round(100.0m * cnt2_3 / (dt.Rows.Count * numbs1.Length * numbs2_3.Length), 2);

        //                        dtResult.Rows.Add(drNew);
        //                    }
        //                }
        //            }
        //        }
        //        #endregion

        //        #region 4个数字
        //        // 4个数字
        //        for (int n1 = 0; n1 < 10; n1++)
        //        {
        //            for (int n2 = n1 + 1; n2 < 10; n2++)
        //            {
        //                for (int n3 = n2 + 1; n3 < 10; n3++)
        //                {
        //                    for (int n4 = n3 + 1; n4< 10; n4++)
        //                    {
        //                        int[] arr = new int[10];
        //                        for (int j = 0; j < 10; j++)
        //                        {
        //                            arr[j] = arrCnt2[i][n1][j] + arrCnt2[i][n2][j] + arrCnt2[i][n3][j] + arrCnt2[i][n4][j];
        //                        }

        //                        // 排序后倒序
        //                        int[] arrSort = arr.Distinct().ToArray();
        //                        Array.Sort(arrSort);
        //                        Array.Reverse(arrSort);
                                

        //                        string numbs1 = n1.ToString() + n2.ToString() + n3.ToString() + n4.ToString();

        //                        // 出现次数第2多
        //                        string numbs2_2 = "";
        //                        int cnt2_2 = 0;

        //                        // 出现次数第3多
        //                        string numbs2_3 = "";
        //                        int cnt2_3 = 0;

        //                        for (int m1 = 0; m1 < 10; m1++)
        //                        {
        //                            int count = arrCnt2[i][n1][m1] + arrCnt2[i][n2][m1] + arrCnt2[i][n3][m1] + arrCnt2[i][n4][m1];
        //                            if (count >= arrSort[0] && count >= avgCnt * 2)
        //                            {
        //                                DataRow drNew = dtResult.NewRow();

        //                                drNew["numbs1"] = numbs1;
        //                                drNew["numbs2"] = m1;
        //                                drNew["号码"] = string.Format(fields2[i], drNew["numbs1"], m1);
        //                                drNew["号码数量"] = numbs1.Length;
        //                                drNew["出现次数"] = count;
        //                                drNew["概率"] = Math.Round(1.0m * count / dt.Rows.Count, 2);
        //                                drNew["回报率"] = Math.Round(100.0m * count / (dt.Rows.Count * numbs1.Length), 2);

        //                                dtResult.Rows.Add(drNew);
        //                            }

        //                            if (arrSort.Length > 2)
        //                            {
        //                                if (count >= arrSort[2] && count >= avgCnt * 2)
        //                                {
        //                                    numbs2_3 += m1.ToString();
        //                                    cnt2_3 += count;
        //                                }
        //                            }
        //                            if (arrSort.Length > 1)
        //                            {
        //                                if (count >= arrSort[1] && count >= avgCnt * 2)
        //                                {
        //                                    numbs2_2 += m1.ToString();
        //                                    cnt2_2 += count;
        //                                }
        //                            }
        //                        }
        //                        if (numbs2_2 != "" && numbs2_2.Length != 1) // 避免重复添加
        //                        {
        //                            DataRow drNew = dtResult.NewRow();

        //                            drNew["numbs1"] = numbs1;
        //                            drNew["numbs2"] = numbs2_2;
        //                            drNew["号码"] = string.Format(fields2[i], drNew["numbs1"], numbs2_2);
        //                            drNew["号码数量"] = numbs1.Length * numbs2_2.Length;
        //                            drNew["出现次数"] = cnt2_2;
        //                            drNew["概率"] = Math.Round(1.0m * cnt2_2 / dt.Rows.Count, 4);
        //                            drNew["回报率"] = Math.Round(100.0m * cnt2_2 / (dt.Rows.Count * numbs1.Length * numbs2_2.Length), 4);

        //                            dtResult.Rows.Add(drNew);
        //                        }
        //                        if (numbs2_3 != "" && numbs2_3.Length != 1 && numbs2_3 != numbs2_2) // 避免重复添加
        //                        {
        //                            DataRow drNew = dtResult.NewRow();

        //                            drNew["numbs1"] = numbs1;
        //                            drNew["numbs2"] = numbs2_3;
        //                            drNew["号码"] = string.Format(fields2[i], drNew["numbs1"], numbs2_3);
        //                            drNew["号码数量"] = numbs1.Length * numbs2_3.Length;
        //                            drNew["出现次数"] = cnt2_3;
        //                            drNew["概率"] = Math.Round(1.0m * cnt2_3 / dt.Rows.Count, 2);
        //                            drNew["回报率"] = Math.Round(100.0m * cnt2_3 / (dt.Rows.Count * numbs1.Length * numbs2_3.Length), 2);

        //                            dtResult.Rows.Add(drNew);
        //                        } 
        //                    }
        //                }
        //            }
        //        }
        //        #endregion

        //        #region 5个数字
        //        // 5个数字
        //        for (int n1 = 0; n1 < 10; n1++)
        //        {
        //            for (int n2 = n1 + 1; n2 < 10; n2++)
        //            {
        //                for (int n3 = n2 + 1; n3 < 10; n3++)
        //                {
        //                    for (int n4 = n3 + 1; n4 < 10; n4++)
        //                    {
        //                        for (int n5 = n4 + 1; n5 < 10; n5++)
        //                        {
        //                            int[] arr = new int[10];
        //                            for (int j = 0; j < 10; j++)
        //                            {
        //                                arr[j] = arrCnt2[i][n1][j] + arrCnt2[i][n2][j] + arrCnt2[i][n3][j] + arrCnt2[i][n4][j] + arrCnt2[i][n5][j];
        //                            }

        //                            // 排序后倒序
        //                            int[] arrSort = arr.Distinct().ToArray();
        //                            Array.Sort(arrSort);
        //                            Array.Reverse(arrSort);
                                    
        //                            string numbs1 = n1.ToString() + n2.ToString() + n3.ToString() + n4.ToString() + n5.ToString();

        //                            // 出现次数第2多
        //                            string numbs2_2 = "";
        //                            int cnt2_2 = 0;

        //                            // 出现次数第3多
        //                            string numbs2_3 = "";
        //                            int cnt2_3 = 0;

        //                            for (int m1 = 0; m1 < 10; m1++)
        //                            {
        //                                int count = arrCnt2[i][n1][m1] + arrCnt2[i][n2][m1] + arrCnt2[i][n3][m1] + arrCnt2[i][n4][m1] + arrCnt2[i][n5][m1];
        //                                if (count >= arrSort[0] && count >= avgCnt * 2)
        //                                {
        //                                    DataRow drNew = dtResult.NewRow();

        //                                    drNew["numbs1"] = numbs1;
        //                                    drNew["numbs2"] = m1;
        //                                    drNew["号码"] = string.Format(fields2[i], drNew["numbs1"], m1);
        //                                    drNew["号码数量"] = numbs1.Length;
        //                                    drNew["出现次数"] = count;
        //                                    drNew["概率"] = Math.Round(1.0m * count / dt.Rows.Count, 2);
        //                                    drNew["回报率"] = Math.Round(100.0m * count / (dt.Rows.Count * numbs1.Length), 2);

        //                                    dtResult.Rows.Add(drNew);
        //                                }

        //                                if (arrSort.Length > 2)
        //                                {
        //                                    if (count >= arrSort[2] && count >= avgCnt * 2)
        //                                    {
        //                                        numbs2_3 += m1.ToString();
        //                                        cnt2_3 += count;
        //                                    }
        //                                }
        //                                if (arrSort.Length > 1)
        //                                {
        //                                    if (count >= arrSort[1] && count >= avgCnt * 2)
        //                                    {
        //                                        numbs2_2 += m1.ToString();
        //                                        cnt2_2 += count;
        //                                    }
        //                                }
        //                            }
        //                            if (numbs2_2 != "" && numbs2_2.Length != 1) // 避免重复添加
        //                            {
        //                                DataRow drNew = dtResult.NewRow();

        //                                drNew["numbs1"] = numbs1;
        //                                drNew["numbs2"] = numbs2_2;
        //                                drNew["号码"] = string.Format(fields2[i], drNew["numbs1"], numbs2_2);
        //                                drNew["号码数量"] = numbs1.Length * numbs2_2.Length;
        //                                drNew["出现次数"] = cnt2_2;
        //                                drNew["概率"] = Math.Round(1.0m * cnt2_2 / dt.Rows.Count, 2);
        //                                drNew["回报率"] = Math.Round(100.0m * cnt2_2 / (dt.Rows.Count * numbs1.Length * numbs2_2.Length), 2);

        //                                dtResult.Rows.Add(drNew);
        //                            }
        //                            if (numbs2_3 != "" && numbs2_3.Length != 1 && numbs2_3 != numbs2_2) // 避免重复添加
        //                            {
        //                                DataRow drNew = dtResult.NewRow();

        //                                drNew["numbs1"] = numbs1;
        //                                drNew["numbs2"] = numbs2_3;
        //                                drNew["号码"] = string.Format(fields2[i], drNew["numbs1"], numbs2_3);
        //                                drNew["号码数量"] = numbs1.Length * numbs2_3.Length;
        //                                drNew["出现次数"] = cnt2_3;
        //                                drNew["概率"] = Math.Round(1.0m * cnt2_3 / dt.Rows.Count, 2);
        //                                drNew["回报率"] = Math.Round(100.0m * cnt2_3 / (dt.Rows.Count * numbs1.Length * numbs2_3.Length), 2);

        //                                dtResult.Rows.Add(drNew);
        //                            } 
        //                        }
        //                    }
        //                }
        //            }
        //        }
        //        #endregion
        //    }

        //    return dtResult.Select("", "回报率 desc").CopyToDataTable();
        //}

        /// <summary>
        /// 新增Qxc数据
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public static int AddQxcNumb(QxcEntity entity)
        {
            int bRet = Lib.Data.Ltr.AddQxcNumb(entity);
            if (bRet == 1)
            {
                UpdateQxcData();
            }
            return bRet;
        }

        /// <summary>
        /// 刷新Qxc数据
        /// </summary>
        /// <returns></returns>
        public static bool UpdateQxcData()
        {
            return lu.UpdateQxcData();
        }


        /// <summary>
        /// 获取Qxc最新一期数据
        /// </summary>
        /// <returns></returns>
        public static QxcEntity GetCurrentQxc()
        {
            QxcEntity entity = new QxcEntity();
            entity.Issue = lu.LaQxc.Issue;
            DateTime dt = new DateTime();
            if (!DateTime.TryParse(lu.LaQxc.GetLtyDate(), out dt))
                dt = DateTime.Today;
            entity.Date = dt;
            entity.Numb = "";

            return entity;
        }

        /// <summary>
        /// 获取Qxc日
        /// </summary>
        /// <returns></returns>
        public static DayOfWeek[] GetQxcDayOfWeek()
        {
            Define df = new Define();
            df.Lt = Define.LtyType.SEVEN_STAR;
            return df.EnumLtrDayOfWeek;
        }
        #endregion

        #region Pl5
        /// <summary>
        /// 获得Pl5数据
        /// </summary>
        /// <param name="issue">期号</param>
        /// <param name="count">数量</param>
        /// <returns></returns>
        public List<ShowLtrPl5Model> GetPl5Data(int count, int issue = -1)
        {
            EntityDataRepertoryType edrt = new EntityDataRepertoryType();
            if (count <= 0)
            {
                // 获取全部
                edrt.SetTypeAll();
            }
            else
            {
                edrt.SetTypeCnt(count, issue);
            }
            DataTable dt = lu.LaPl5.GetData(edrt);

            List<ShowLtrPl5Model> lstEntity = new List<ShowLtrPl5Model>();
            if (dt != null)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    ShowLtrPl5Model entity = new ShowLtrPl5Model();
                    entity.Issue = int.Parse(dr["issue"].ToString());
                    entity.Sum = int.Parse(dr["sum"].ToString());
                    entity.Numb = dr["numbers"].ToString();
                    entity.Date = DateTime.Parse(dr["ltr_date"].ToString());
                    lstEntity.Add(entity);
                }
            }

            return lstEntity;
        }

        /// <summary>
        /// 获得Pl5合数
        /// </summary>
        /// <param name="issue">期号</param>
        /// <returns></returns>
        public EntitySumResult GetPl5Sum(int issue)
        {
            EntitySumResult esr = lu.LaPl5.GetSumNumberInfo(issue);

            return esr;
        }

        /// <summary>
        /// 获得Pl5高频数据
        /// </summary>
        /// <param name="count">数量</param>
        /// <param name="issue">期号</param>
        /// <param name="filter">数据筛选条件</param>
        /// <param name="filter_ret">结果筛选条件</param>
        /// <returns></returns>
        public ResultEntity GetPl5HighFrequency(int count = 100, int issue = -1, string filter = "", string filter_ret = "")
        {
            ResultEntity result = new ResultEntity();
            try
            {
                EntityDataRepertoryType edrt = new EntityDataRepertoryType();
                if (count <= 0)
                {
                    // 获取全部
                    edrt.SetTypeAll();
                }
                else
                {
                    edrt.SetTypeCnt(count, issue);
                }
                DataTable dt = lu.LaPl5.GetData(edrt); // 获取数据
                //dt.Columns["issue"].ColumnName = "期号";
                //dt.Columns["numbers_4"].ColumnName = "号码";
                //dt.Columns["ltr_date"].ColumnName = "日期";
                //dt.Columns["thousands"].ColumnName = "千";
                //dt.Columns["hundreds"].ColumnName = "百";
                //dt.Columns["tens"].ColumnName = "十";
                //dt.Columns["units"].ColumnName = "个";
                //dt.Columns["relation_size"].ColumnName = "大小";
                //dt.Columns["relation_parity"].ColumnName = "单双";
                //dt.Columns["day_of_week"].ColumnName = "星期几";
                DataTable dtTemp = dt.Select(filter, "").CopyToDataTable(); // 筛选数据

                // 计算高频数据
                DataTable dtResult = lu.LaPl5.GetHighFrequencyLocation(dtTemp);//GetHighFrequencyLocation(dtTemp);

                // 筛选结果数据
                if (string.IsNullOrWhiteSpace(filter_ret))
                {
                    filter_ret = "号码数量 > 3 and 回报率 > 2";
                }
                DataRow[] drs = dtResult.Select(filter_ret, "回报率 desc");
                if (drs.Length <= 0)
                {
                    result.SetSuccess();
                }
                else
                {
                    dtResult = drs.CopyToDataTable();
                    result.SetSuccess(dtResult);
                }
            }
            catch (Exception ex)
            {
                result.SetError(ex.Message);
            }

            return result;
        }

        /// <summary>
        /// 新增Pl5数据
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public static int AddPl5Numb(Pl5Entity entity)
        {
            int bRet = Lib.Data.Ltr.AddPl5Numb(entity);
            if(bRet == 1)
            {
                UpdatePl5Data();
            }
            return bRet;
        }


        /// <summary>
        /// 刷新Pl5数据
        /// </summary>
        /// <returns></returns>
        public static bool UpdatePl5Data()
        {
            return lu.UpdatePl5Data();
        }

        /// <summary>
        /// 获取Pl5最新一期数据
        /// </summary>
        /// <returns></returns>
        public static Pl5Entity GetCurrentPl5()
        {
            Pl5Entity entity = new Pl5Entity();
            entity.Issue = lu.LaPl5.Issue;
            DateTime dt = new DateTime();
            if (!DateTime.TryParse(lu.LaPl5.GetLtyDate(), out dt))
                dt = DateTime.Today;
            entity.Date = dt;
            entity.Numb = "";

            return entity;
        }

        /// <summary>
        /// 获取Pl5日
        /// </summary>
        /// <returns></returns>
        public static DayOfWeek[] GetPl5DayOfWeek()
        {
            Define df = new Define();
            df.Lt = Define.LtyType.PERMUTATION_FIVE;
            return df.EnumLtrDayOfWeek;
        }
        #endregion
    }
}
