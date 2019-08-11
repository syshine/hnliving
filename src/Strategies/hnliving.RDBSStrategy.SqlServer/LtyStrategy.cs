using System.Data;

using Lib.Core;
using System.Data.Common;
using Lib.Core.Domain.Ltr;

namespace hnliving.RDBSStrategy.SqlServer
{
    /// <summary>
    /// SqlServer策略之Ltr分部类
    /// </summary>
    public partial class RDBSStrategy : IRDBSStrategy
    {
        #region Qxc
        /// <summary>
        /// 获取Qxc数据
        /// </summary>
        /// <returns></returns>
        public DataTable GetQxcData()
        {
            string commandText = string.Format("SELECT issue, numbers, ltr_date FROM [{0}ltr_qxc]",
                                                RDBSHelper.RDBSTablePre);

            return RDBSHelper.ExecuteDataset(CommandType.Text, commandText).Tables[0];
        }

        /// <summary>
        /// 新增Qxc数据
        /// </summary>
        /// <param name="entity">Qxc实体数据</param>
        /// <returns></returns>
        public int AddQxcNumb(QxcEntity entity)
        {
            DbParameter[] parms = {
                                       GenerateInParam("@issue",SqlDbType.Int,4, entity.Issue),
                                       GenerateInParam("@date",SqlDbType.DateTime,8, entity.Date.ToString("yyyy-MM-dd")),
                                       GenerateInParam("@numb",SqlDbType.NVarChar,entity.Numb.Length, entity.Numb),
                                   };
            return TypeHelper.ObjectToInt(RDBSHelper.ExecuteScalar(CommandType.StoredProcedure,
                                                                   string.Format("{0}addqxcnumb", RDBSHelper.RDBSTablePre),
                                                                   parms), -1);
        }
        #endregion

        #region Pl5
        /// <summary>
        /// 获取Pl5数据
        /// </summary>
        /// <returns></returns>
        public DataTable GetPl5Data()
        {
            string commandText = string.Format("SELECT issue, numbers, ltr_date FROM [{0}ltr_pl5]",
                                                RDBSHelper.RDBSTablePre);

            return RDBSHelper.ExecuteDataset(CommandType.Text, commandText).Tables[0];
        }

        /// <summary>
        /// 新增Pl5数据
        /// </summary>
        /// <param name="entity">Pl5实体数据</param>
        /// <returns></returns>
        public int AddPl5Numb(Pl5Entity entity)
        {
            DbParameter[] parms = {
                                       GenerateInParam("@issue",SqlDbType.Int,4, entity.Issue),
                                       GenerateInParam("@date",SqlDbType.DateTime,8, entity.Date.ToString("yyyy-MM-dd")),
                                       GenerateInParam("@numb",SqlDbType.NVarChar,entity.Numb.Length, entity.Numb),
                                   };
            return TypeHelper.ObjectToInt(RDBSHelper.ExecuteScalar(CommandType.StoredProcedure,
                                                                   string.Format("{0}addpl5numb", RDBSHelper.RDBSTablePre),
                                                                   parms), -1);
        }
        #endregion
    }
}
