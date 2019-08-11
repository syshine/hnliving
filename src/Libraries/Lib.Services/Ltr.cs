using Lib.Core.Domain.Ltr;
using LibLtr;
using LibLtr.Entity;
using System;
using System.Collections.Generic;
using System.Data;

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
            edrt.SetTypeCnt(count, issue);
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
        /// 新增Qxc数据
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public static int AddQxcNumb(QxcEntity entity)
        {
            return Lib.Data.Ltr.AddQxcNumb(entity);
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
            edrt.SetTypeCnt(count, issue);
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
        /// 新增Pl5数据
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public static int AddPl5Numb(Pl5Entity entity)
        {
            return Lib.Data.Ltr.AddPl5Numb(entity);
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
