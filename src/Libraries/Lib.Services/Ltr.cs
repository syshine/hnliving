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
        /// <returns></returns>
        public DataTable GetQxcHighFrequency(int count = 100)
        {
            EntityDataRepertoryType edrt = new EntityDataRepertoryType();
            if (count <= 0)
            {
                // 获取全部
                edrt.SetTypeAll();
            }
            else
            {
                edrt.SetTypeCnt(count);
            }
            DataTable dt = lu.LaQxc.GetData(edrt);
            DataTable dtTemp = dt.Select("issue % 10 =4", "").CopyToDataTable();
            DataTable dtResult = GetHighFrequencyLocation(dtTemp);//lu.LaQxc.GetHighFrequencyLocation(dt);

            return dtResult;
        }

        /// <summary>
        /// 获取高频定位
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public DataTable GetHighFrequencyLocation(DataTable dt)
        {
            DataTable dtResult = new DataTable();
            dtResult.Columns.Add("location");
            dtResult.Columns.Add("numbs1");
            dtResult.Columns.Add("numbs2");
            dtResult.Columns.Add("numbs3");
            dtResult.Columns.Add("numbs_count", typeof(int));
            dtResult.Columns.Add("cnt", typeof(int));
            dtResult.Columns.Add("frequency", typeof(decimal));
            dtResult.Columns.Add("rate", typeof(decimal));

            int avgCnt = (int)Math.Ceiling(dt.Rows.Count / 100.0);


            // 计数
            //int[,,] arrCnt2 = new int[6, 10, 10];
            int[,,,] arrCnt3 = new int[4, 10, 10, 10];
            int[][][] arrCnt2 = new int[6][][];
            for (int i = 0; i < arrCnt2.Length; i++)
            {
                arrCnt2[i] = new int[10][];
                for (int j = 0; j < arrCnt2[i].Length; j++)
                {
                    arrCnt2[i][j] = new int[10];
                }
            }

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                DataRow row = dt.Rows[i];
                //("numbers_4");                  // 开奖号码(前4位)
                //("thousands", typeof(int));     // 千位
                //("hundreds", typeof(int));      // 百位
                //("tens", typeof(int));          // 十位
                //("units", typeof(int));         // 个位
                //("sum", typeof(int));           // 合数
                //("relation_size");              // 大小关系
                //("relation_parity");            // 奇偶关系
                //("day_of_week");                // 开j时间(周几)
                //("sort_num");                   // 排序号码

                int thousands = Convert.ToInt32(row["thousands"]);
                int hundreds = Convert.ToInt32(row["hundreds"]);
                int tens = Convert.ToInt32(row["tens"]);
                int units = Convert.ToInt32(row["units"]);

                #region 计数
                // 两位
                arrCnt2[0][thousands][hundreds]++;   // 千百 ??XX
                arrCnt2[1][thousands][tens]++;       // 千十 ?X?X
                arrCnt2[2][thousands][units]++;      // 千个 ?XX?
                arrCnt2[3][hundreds][tens]++;        // 百十 X??X
                arrCnt2[4][hundreds][units]++;       // 百个 X?X?
                arrCnt2[5][tens][units]++;           // 十个 XX??

                // 三位
                arrCnt3[0, thousands, hundreds, tens]++;   // 千百十 ???X
                arrCnt3[1, thousands, hundreds, units]++;  // 千百个 ??X?
                arrCnt3[2, thousands, tens, units]++;      // 千十个 ?X??
                arrCnt3[3, hundreds, tens, units]++;       // 百十个 X???
                #endregion
            }

            #region //把计数合放进DataTable以供计算
            //DataTable dtCnt = new DataTable();
            //dtCnt.Columns.Add("index", typeof(int));
            //dtCnt.Columns.Add("location1", typeof(int));
            //dtCnt.Columns.Add("location2", typeof(int));
            //dtCnt.Columns.Add("cnt", typeof(int));
            //for (int i = 0; i < 6; i++)
            //{
            //    for (int j = 0; j < 10; j++)
            //    {
            //        for (int k = 0; k < 10; k++)
            //        {
            //            DataRow drNew = dtCnt.NewRow();

            //            drNew["index"] = i;
            //            drNew["location1"] = j;
            //            drNew["location2"] = k;
            //            drNew["cnt"] = arrCnt2[i][j][k];

            //            dtCnt.Rows.Add(drNew);
            //        }
            //    }
            //}
            #endregion

            string[] fields2 = new string[6] { "{0}.{1}.X.X", "{0}.X.{1}.X", "{0}.X.X.{1}", "X.{0}.{1}.X", "X.{0}.X.{1}", "X.X.{0}.{1}" };
            //string[,] fields2 = new string[6, 2] { { "thousands", "hundreds" }, { "thousands", "tens" }, { "thousands", "units" }, { "hundreds", "tens" }, { "hundreds", "units" }, { "tens", "units" } };

            for (int i = 0; i < 6; i++)
            {
                //int[] arrSum = new int[10];
                //for (int j = 0; j < 10; j++)
                //{
                //    string filter = string.Format("index={0} and location1={1}", i, j);
                //    arrSum[j] = Convert.ToInt32(dtCnt.Compute("sum(cnt)", filter));
                //}

                #region 1个数字
                // 1个数字
                for (int n1 = 0; n1 < 10; n1++)
                {
                    //int count = arrSum[n1];
                    int[] arr = arrCnt2[i][n1];

                    // 排序后倒序
                    int[] arrSort = arr.Distinct().ToArray();
                    Array.Sort(arrSort);
                    Array.Reverse(arrSort);

                    //// 取最多的N次(例：如果有1、3、4、5、6次的，只取大于等于5的)，加MIN(1)是防止次数全部都一样
                    //int ind = Math.Min(1, arrSort.Length - 1);

                    string numbs2_2 = "";
                    int cnt2_2 = 0;
                    string numbs2_3 = "";
                    int cnt2_3 = 0;
                    for (int m1 = 0; m1 < 10; m1++)
                    {
                        int count = arrCnt2[i][n1][m1];
                        #region// 1个的样本容量太小，不添加
                        //if (count >= arrSort[0] && count >= avgCnt)
                        //{
                        //    DataRow drNew = dtResult.NewRow();

                        //    drNew["location"] = string.Format(fields2[i], n1, m1);
                        //    drNew["numbs1"] = n1;
                        //    drNew["numbs2"] = m1;
                        //    drNew["numbs_count"] = 1;
                        //    drNew["cnt"] = count;
                        //    drNew["frequency"] = Math.Round(1.0m * count / dt.Rows.Count, 4);
                        //    drNew["rate"] = Math.Round(100.0m * count / dt.Rows.Count, 4);

                        //    dtResult.Rows.Add(drNew);
                        //}
                        #endregion

                        if (arrSort.Length > 2)
                        {
                            if (count >= arrSort[2] && count >= avgCnt)
                            {
                                numbs2_3 += m1;
                                cnt2_3 += count;
                            }
                        }
                        if (arrSort.Length > 1)
                        {
                            if (arrCnt2[i][n1][m1] >= arrSort[1] && arrCnt2[i][n1][m1] >= avgCnt)
                            {
                                numbs2_2 += m1;
                                cnt2_2 += count;
                            }
                        }
                    }
                    if (numbs2_2 != "")
                    {
                        DataRow drNew = dtResult.NewRow();

                        drNew["location"] = string.Format(fields2[i], n1, numbs2_2);
                        drNew["numbs1"] = n1;
                        drNew["numbs2"] = numbs2_2;
                        drNew["numbs_count"] = numbs2_2.Length;
                        drNew["cnt"] = cnt2_2;
                        drNew["frequency"] = Math.Round(1.0m * cnt2_2 / dt.Rows.Count, 4);
                        drNew["rate"] = Math.Round(100.0m * cnt2_2 / (dt.Rows.Count * numbs2_2.Length), 4);

                        dtResult.Rows.Add(drNew);
                    }
                    if (numbs2_3 != "")
                    {
                        DataRow drNew = dtResult.NewRow();

                        drNew["location"] = string.Format(fields2[i], n1, numbs2_3);
                        drNew["numbs1"] = n1;
                        drNew["numbs2"] = numbs2_3;
                        drNew["numbs_count"] = numbs2_3.Length;
                        drNew["cnt"] = cnt2_3;
                        drNew["frequency"] = Math.Round(1.0m * cnt2_3 / dt.Rows.Count, 4);
                        drNew["rate"] = Math.Round(100.0m * cnt2_3 / (dt.Rows.Count * numbs2_3.Length), 4);

                        dtResult.Rows.Add(drNew);
                    }
                }
                #endregion

                #region 2个数字
                // 2个数字
                for (int n1 = 0; n1 < 10; n1++)
                {
                    for (int n2 = n1 + 1; n2 < 10; n2++)
                    {
                        //string filter = string.Format("{0}={2} and {1}={3}", fields2[i, 0], fields2[i, 1], n1, n2);
                        //int count = Convert.ToInt32(dt.Compute("count(1)", filter));

                        //int count = arrSum[n1] + arrSum[n2];
                        int[] arr = new int[10];
                        for (int j = 0; j < 10; j++)
                        {
                            arr[j] = arrCnt2[0][n1][j] + arrCnt2[0][n2][j];
                        }

                        // 排序后倒序
                        int[] arrSort = arr.Distinct().ToArray();
                        Array.Sort(arrSort);
                        Array.Reverse(arrSort);

                        //if (count > avgCnt * 2)
                        //{
                        //    DataRow drNew = dtResult.NewRow();

                        //    drNew["numbs"] = n1.ToString() + n2.ToString();
                        //    drNew["frequency"] = Math.Round(1.0m * count / dt.Rows.Count, 4);
                        //    drNew["rate"] = Math.Round(20.0m * count / dt.Rows.Count, 4);

                        //    dtResult.Rows.Add(drNew);
                        //}

                        string numbs1 = n1.ToString() + n2.ToString();
                        string numbs2_2 = "";
                        int cnt2_2 = 0;
                        string numbs2_3 = "";
                        int cnt2_3 = 0;
                        for (int m1 = 0; m1 < 10; m1++)
                        {
                            int count = arrCnt2[i][n1][m1] + arrCnt2[i][n2][m1];
                            if (count >= arrSort[0] && count >= avgCnt * 2)
                            {
                                DataRow drNew = dtResult.NewRow();

                                drNew["numbs1"] = numbs1;
                                drNew["numbs2"] = m1;
                                drNew["location"] = string.Format(fields2[i], drNew["numbs1"], m1);
                                drNew["numbs_count"] = numbs1.Length;
                                drNew["cnt"] = count;
                                drNew["frequency"] = Math.Round(1.0m * count / dt.Rows.Count, 4);
                                drNew["rate"] = Math.Round(100.0m * count / (dt.Rows.Count * numbs1.Length), 4);

                                dtResult.Rows.Add(drNew);
                            }

                            if (arrSort.Length > 2)
                            {
                                if (count >= arrSort[2] && count >= avgCnt * 2)
                                {
                                    numbs2_3 += m1.ToString();
                                    cnt2_3 += count;
                                }
                            }
                            if (arrSort.Length > 1)
                            {
                                if (count >= arrSort[1] && count >= avgCnt * 2)
                                {
                                    numbs2_2 += m1.ToString();
                                    cnt2_2 += count;
                                }
                            }
                        }
                        if (numbs2_2 != "" && numbs2_2.Length != 1) // 避免重复添加
                        {
                            DataRow drNew = dtResult.NewRow();
                            
                            drNew["numbs1"] = numbs1;
                            drNew["numbs2"] = numbs2_2;
                            drNew["location"] = string.Format(fields2[i], drNew["numbs1"], numbs2_2);
                            drNew["numbs_count"] = numbs1.Length * numbs2_2.Length;
                            drNew["cnt"] = cnt2_2;
                            drNew["frequency"] = Math.Round(1.0m * cnt2_2 / dt.Rows.Count, 4);
                            drNew["rate"] = Math.Round(100.0m * cnt2_2 / (dt.Rows.Count * numbs1.Length * numbs2_2.Length), 4);

                            dtResult.Rows.Add(drNew);
                        }
                        if (numbs2_3 != "" && numbs2_3.Length != 1 && numbs2_3 != numbs2_2) // 避免重复添加
                        {
                            DataRow drNew = dtResult.NewRow();

                            drNew["numbs1"] = numbs1;
                            drNew["numbs2"] = numbs2_3;
                            drNew["location"] = string.Format(fields2[i], drNew["numbs1"], numbs2_3);
                            drNew["numbs_count"] = numbs1.Length * numbs2_3.Length;
                            drNew["cnt"] = cnt2_3;
                            drNew["frequency"] = Math.Round(1.0m * cnt2_3 / dt.Rows.Count, 4);
                            drNew["rate"] = Math.Round(100.0m * cnt2_3 / (dt.Rows.Count * numbs1.Length * numbs2_3.Length), 4);

                            dtResult.Rows.Add(drNew);
                        }
                    }
                }
                #endregion

                #region 3个数字
                // 3个数字
                for (int n1 = 0; n1 < 10; n1++)
                {
                    for (int n2 = n1 + 1; n2 < 10; n2++)
                    {
                        for (int n3 = n2 + 1; n3 < 10; n3++)
                        {
                            int[] arr = new int[10];
                            for (int j = 0; j < 10; j++)
                            {
                                arr[j] = arrCnt2[i][n1][j] + arrCnt2[i][n2][j] + arrCnt2[i][n3][j];
                            }

                            // 排序后倒序
                            int[] arrSort = arr.Distinct().ToArray();
                            Array.Sort(arrSort);
                            Array.Reverse(arrSort);

                            //if (count > avgCnt * 2)
                            //{
                            //    DataRow drNew = dtResult.NewRow();

                            //    drNew["numbs"] = n1.ToString() + n2.ToString();
                            //    drNew["frequency"] = Math.Round(1.0m * count / dt.Rows.Count, 4);
                            //    drNew["rate"] = Math.Round(20.0m * count / dt.Rows.Count, 4);

                            //    dtResult.Rows.Add(drNew);
                            //}


                            string numbs1 = n1.ToString() + n2.ToString() + n3.ToString();
                            string numbs2_2 = "";
                            int cnt2_2 = 0;
                            string numbs2_3 = "";
                            int cnt2_3 = 0;
                            for (int m1 = 0; m1 < 10; m1++)
                            {
                                int count = arrCnt2[i][n1][m1] + arrCnt2[i][n2][m1] + arrCnt2[i][n3][m1];
                                if (count >= arrSort[0] && count >= avgCnt * 2)
                                {
                                    DataRow drNew = dtResult.NewRow();

                                    drNew["numbs1"] = numbs1;
                                    drNew["numbs2"] = m1;
                                    drNew["location"] = string.Format(fields2[i], drNew["numbs1"], m1);
                                    drNew["numbs_count"] = numbs1.Length;
                                    drNew["cnt"] = count;
                                    drNew["frequency"] = Math.Round(1.0m * count / dt.Rows.Count, 4);
                                    drNew["rate"] = Math.Round(100.0m * count / (dt.Rows.Count * numbs1.Length), 4);

                                    dtResult.Rows.Add(drNew);
                                }

                                if (arrSort.Length > 2)
                                {
                                    if (count >= arrSort[2] && count >= avgCnt * 2)
                                    {
                                        numbs2_3 += m1.ToString();
                                        cnt2_3 += count;
                                    }
                                }
                                if (arrSort.Length > 1)
                                {
                                    if (count >= arrSort[1] && count >= avgCnt * 2)
                                    {
                                        numbs2_2 += m1.ToString();
                                        cnt2_2 += count;
                                    }
                                }
                            }
                            if (numbs2_2 != "" && numbs2_2.Length != 1) // 避免重复添加
                            {
                                DataRow drNew = dtResult.NewRow();
                                
                                drNew["numbs1"] = numbs1;
                                drNew["numbs2"] = numbs2_2;
                                drNew["location"] = string.Format(fields2[i], drNew["numbs1"], numbs2_2);
                                drNew["numbs_count"] = numbs1.Length * numbs2_2.Length;
                                drNew["cnt"] = cnt2_2;
                                drNew["frequency"] = Math.Round(1.0m * cnt2_2 / dt.Rows.Count, 4);
                                drNew["rate"] = Math.Round(100.0m * cnt2_2 / (dt.Rows.Count * numbs1.Length * numbs2_2.Length), 4);

                                dtResult.Rows.Add(drNew);
                            }
                            if (numbs2_3 != "" && numbs2_3.Length != 1 && numbs2_3 != numbs2_2) // 避免重复添加
                            {
                                DataRow drNew = dtResult.NewRow();

                                drNew["numbs1"] = numbs1;
                                drNew["numbs2"] = numbs2_3;
                                drNew["location"] = string.Format(fields2[i], drNew["numbs1"], numbs2_3);
                                drNew["numbs_count"] = numbs1.Length * numbs2_3.Length;
                                drNew["cnt"] = cnt2_3;
                                drNew["frequency"] = Math.Round(1.0m * cnt2_3 / dt.Rows.Count, 4);
                                drNew["rate"] = Math.Round(100.0m * cnt2_3 / (dt.Rows.Count * numbs1.Length * numbs2_3.Length), 4);

                                dtResult.Rows.Add(drNew);
                            }
                        }
                    }
                }
                #endregion

                #region 4个数字
                // 4个数字
                for (int n1 = 0; n1 < 10; n1++)
                {
                    for (int n2 = n1 + 1; n2 < 10; n2++)
                    {
                        for (int n3 = n2 + 1; n3 < 10; n3++)
                        {
                            for (int n4 = n3 + 1; n4< 10; n4++)
                            {
                                int[] arr = new int[10];
                                for (int j = 0; j < 10; j++)
                                {
                                    arr[j] = arrCnt2[i][n1][j] + arrCnt2[i][n2][j] + arrCnt2[i][n3][j] + arrCnt2[i][n4][j];
                                }

                                // 排序后倒序
                                int[] arrSort = arr.Distinct().ToArray();
                                Array.Sort(arrSort);
                                Array.Reverse(arrSort);

                                //if (count > avgCnt * 2)
                                //{
                                //    DataRow drNew = dtResult.NewRow();

                                //    drNew["numbs"] = n1.ToString() + n2.ToString();
                                //    drNew["frequency"] = Math.Round(1.0m * count / dt.Rows.Count, 4);
                                //    drNew["rate"] = Math.Round(20.0m * count / dt.Rows.Count, 4);

                                //    dtResult.Rows.Add(drNew);
                                //}


                                string numbs1 = n1.ToString() + n2.ToString() + n3.ToString() + n4.ToString();
                                string numbs2_2 = "";
                                int cnt2_2 = 0;
                                string numbs2_3 = "";
                                int cnt2_3 = 0;
                                for (int m1 = 0; m1 < 10; m1++)
                                {
                                    int count = arrCnt2[i][n1][m1] + arrCnt2[i][n2][m1] + arrCnt2[i][n3][m1] + arrCnt2[i][n4][m1];
                                    if (count >= arrSort[0] && count >= avgCnt * 2)
                                    {
                                        DataRow drNew = dtResult.NewRow();

                                        drNew["numbs1"] = numbs1;
                                        drNew["numbs2"] = m1;
                                        drNew["location"] = string.Format(fields2[i], drNew["numbs1"], m1);
                                        drNew["numbs_count"] = numbs1.Length;
                                        drNew["cnt"] = count;
                                        drNew["frequency"] = Math.Round(1.0m * count / dt.Rows.Count, 4);
                                        drNew["rate"] = Math.Round(100.0m * count / (dt.Rows.Count * numbs1.Length), 4);

                                        dtResult.Rows.Add(drNew);
                                    }

                                    if (arrSort.Length > 2)
                                    {
                                        if (count >= arrSort[2] && count >= avgCnt * 2)
                                        {
                                            numbs2_3 += m1.ToString();
                                            cnt2_3 += count;
                                        }
                                    }
                                    if (arrSort.Length > 1)
                                    {
                                        if (count >= arrSort[1] && count >= avgCnt * 2)
                                        {
                                            numbs2_2 += m1.ToString();
                                            cnt2_2 += count;
                                        }
                                    }
                                }
                                if (numbs2_2 != "" && numbs2_2.Length != 1) // 避免重复添加
                                {
                                    DataRow drNew = dtResult.NewRow();

                                    drNew["numbs1"] = numbs1;
                                    drNew["numbs2"] = numbs2_2;
                                    drNew["location"] = string.Format(fields2[i], drNew["numbs1"], numbs2_2);
                                    drNew["numbs_count"] = numbs1.Length * numbs2_2.Length;
                                    drNew["cnt"] = cnt2_2;
                                    drNew["frequency"] = Math.Round(1.0m * cnt2_2 / dt.Rows.Count, 4);
                                    drNew["rate"] = Math.Round(100.0m * cnt2_2 / (dt.Rows.Count * numbs1.Length * numbs2_2.Length), 4);

                                    dtResult.Rows.Add(drNew);
                                }
                                if (numbs2_3 != "" && numbs2_3.Length != 1 && numbs2_3 != numbs2_2) // 避免重复添加
                                {
                                    DataRow drNew = dtResult.NewRow();

                                    drNew["numbs1"] = numbs1;
                                    drNew["numbs2"] = numbs2_3;
                                    drNew["location"] = string.Format(fields2[i], drNew["numbs1"], numbs2_3);
                                    drNew["numbs_count"] = numbs1.Length * numbs2_3.Length;
                                    drNew["cnt"] = cnt2_3;
                                    drNew["frequency"] = Math.Round(1.0m * cnt2_3 / dt.Rows.Count, 4);
                                    drNew["rate"] = Math.Round(100.0m * cnt2_3 / (dt.Rows.Count * numbs1.Length * numbs2_3.Length), 4);

                                    dtResult.Rows.Add(drNew);
                                } 
                            }
                        }
                    }
                }
                #endregion

                #region 5个数字
                // 5个数字
                for (int n1 = 0; n1 < 10; n1++)
                {
                    for (int n2 = n1 + 1; n2 < 10; n2++)
                    {
                        for (int n3 = n2 + 1; n3 < 10; n3++)
                        {
                            for (int n4 = n3 + 1; n4 < 10; n4++)
                            {
                                for (int n5 = n4 + 1; n5 < 10; n5++)
                                {
                                    int[] arr = new int[10];
                                    for (int j = 0; j < 10; j++)
                                    {
                                        arr[j] = arrCnt2[i][n1][j] + arrCnt2[i][n2][j] + arrCnt2[i][n3][j] + arrCnt2[i][n4][j] + arrCnt2[i][n5][j];
                                    }

                                    // 排序后倒序
                                    int[] arrSort = arr.Distinct().ToArray();
                                    Array.Sort(arrSort);
                                    Array.Reverse(arrSort);

                                    //if (count > avgCnt * 2)
                                    //{
                                    //    DataRow drNew = dtResult.NewRow();

                                    //    drNew["numbs"] = n1.ToString() + n2.ToString();
                                    //    drNew["frequency"] = Math.Round(1.0m * count / dt.Rows.Count, 4);
                                    //    drNew["rate"] = Math.Round(20.0m * count / dt.Rows.Count, 4);

                                    //    dtResult.Rows.Add(drNew);
                                    //}


                                    string numbs1 = n1.ToString() + n2.ToString() + n3.ToString() + n4.ToString() + n5.ToString();
                                    string numbs2_2 = "";
                                    int cnt2_2 = 0;
                                    string numbs2_3 = "";
                                    int cnt2_3 = 0;
                                    for (int m1 = 0; m1 < 10; m1++)
                                    {
                                        int count = arrCnt2[i][n1][m1] + arrCnt2[i][n2][m1] + arrCnt2[i][n3][m1] + arrCnt2[i][n4][m1] + arrCnt2[i][n5][m1];
                                        if (count >= arrSort[0] && count >= avgCnt * 2)
                                        {
                                            DataRow drNew = dtResult.NewRow();

                                            drNew["numbs1"] = numbs1;
                                            drNew["numbs2"] = m1;
                                            drNew["location"] = string.Format(fields2[i], drNew["numbs1"], m1);
                                            drNew["numbs_count"] = numbs1.Length;
                                            drNew["cnt"] = count;
                                            drNew["frequency"] = Math.Round(1.0m * count / dt.Rows.Count, 4);
                                            drNew["rate"] = Math.Round(100.0m * count / (dt.Rows.Count * numbs1.Length), 4);

                                            dtResult.Rows.Add(drNew);
                                        }

                                        if (arrSort.Length > 2)
                                        {
                                            if (count >= arrSort[2] && count >= avgCnt * 2)
                                            {
                                                numbs2_3 += m1.ToString();
                                                cnt2_3 += count;
                                            }
                                        }
                                        if (arrSort.Length > 1)
                                        {
                                            if (count >= arrSort[1] && count >= avgCnt * 2)
                                            {
                                                numbs2_2 += m1.ToString();
                                                cnt2_2 += count;
                                            }
                                        }
                                    }
                                    if (numbs2_2 != "" && numbs2_2.Length != 1) // 避免重复添加
                                    {
                                        DataRow drNew = dtResult.NewRow();

                                        drNew["numbs1"] = numbs1;
                                        drNew["numbs2"] = numbs2_2;
                                        drNew["location"] = string.Format(fields2[i], drNew["numbs1"], numbs2_2);
                                        drNew["numbs_count"] = numbs1.Length * numbs2_2.Length;
                                        drNew["cnt"] = cnt2_2;
                                        drNew["frequency"] = Math.Round(1.0m * cnt2_2 / dt.Rows.Count, 4);
                                        drNew["rate"] = Math.Round(100.0m * cnt2_2 / (dt.Rows.Count * numbs1.Length * numbs2_2.Length), 4);

                                        dtResult.Rows.Add(drNew);
                                    }
                                    if (numbs2_3 != "" && numbs2_3.Length != 1 && numbs2_3 != numbs2_2) // 避免重复添加
                                    {
                                        DataRow drNew = dtResult.NewRow();

                                        drNew["numbs1"] = numbs1;
                                        drNew["numbs2"] = numbs2_3;
                                        drNew["location"] = string.Format(fields2[i], drNew["numbs1"], numbs2_3);
                                        drNew["numbs_count"] = numbs1.Length * numbs2_3.Length;
                                        drNew["cnt"] = cnt2_3;
                                        drNew["frequency"] = Math.Round(1.0m * cnt2_3 / dt.Rows.Count, 4);
                                        drNew["rate"] = Math.Round(100.0m * cnt2_3 / (dt.Rows.Count * numbs1.Length * numbs2_3.Length), 4);

                                        dtResult.Rows.Add(drNew);
                                    } 
                                }
                            }
                        }
                    }
                }
                #endregion
            }

            return dtResult.Select("", "rate desc").CopyToDataTable();
        }

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
        /// <returns></returns>
        public DataTable GetPl5HighFrequency(int count = 100)
        {
            EntityDataRepertoryType edrt = new EntityDataRepertoryType();
            if (count <= 0)
            {
                // 获取全部
                edrt.SetTypeAll();
            }
            else
            {
                edrt.SetTypeCnt(count);
            }
            DataTable dt = lu.LaPl5.GetData(edrt);
            DataTable dtTemp = dt.Select("day_of_week ='星期一'", "").CopyToDataTable();
            DataTable dtResult = GetHighFrequencyLocation(dtTemp);//lu.LaPl5.GetHighFrequencyLocation(dt);
            dtResult = dtResult.Select("numbs_count > 3 and rate > 2", "rate desc").CopyToDataTable();
            return dtResult;
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
