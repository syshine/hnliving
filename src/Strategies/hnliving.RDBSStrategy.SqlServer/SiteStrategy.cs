using System;
using System.Text;
using System.Data;
using System.Data.Common;

using Lib.Core;

namespace hnliving.RDBSStrategy.SqlServer
{
    /// <summary>
    /// SqlServer策略之商城分部类
    /// </summary>
    public partial class RDBSStrategy : IRDBSStrategy
    {
        #region 筛选词

        /// <summary>
        /// 获得筛选词列表
        /// </summary>
        /// <returns></returns>
        public IDataReader GetFilterWordList()
        {
            string commandText = string.Format("SELECT {1} FROM [{0}filterwords]",
                                                RDBSHelper.RDBSTablePre,
                                                TableFields.FILTERWORDS);
            return RDBSHelper.ExecuteReader(CommandType.Text, commandText);
        }

        /// <summary>
        /// 添加筛选词
        /// </summary>
        public void AddFilterWord(FilterWordInfo filterWordInfo)
        {
            DbParameter[] parms = { 
                                    GenerateInParam("@match", SqlDbType.NVarChar, 250, filterWordInfo.Match),
                                    GenerateInParam("@replace", SqlDbType.NVarChar, 250, filterWordInfo.Replace)
                                   };
            string commandText = string.Format("INSERT INTO [{0}filterwords]([match],[replace]) VALUES(@match,@replace)",
                                                RDBSHelper.RDBSTablePre);
            RDBSHelper.ExecuteNonQuery(CommandType.Text, commandText, parms);
        }

        /// <summary>
        /// 更新筛选词
        /// </summary>
        public void UpdateFilterWord(FilterWordInfo filterWordInfo)
        {
            DbParameter[] parms = { 
                                    GenerateInParam("@match", SqlDbType.NVarChar, 250, filterWordInfo.Match),
                                    GenerateInParam("@replace", SqlDbType.NVarChar, 250, filterWordInfo.Replace),
                                    GenerateInParam("@id", SqlDbType.Int, 4, filterWordInfo.Id)
                                   };
            string commandText = string.Format("UPDATE [{0}filterwords] SET [match]=@match,[replace]=@replace WHERE [id]=@id",
                                                RDBSHelper.RDBSTablePre);
            RDBSHelper.ExecuteNonQuery(CommandType.Text, commandText, parms);
        }

        /// <summary>
        /// 删除筛选词
        /// </summary>
        /// <param name="idList">id列表</param>
        public void DeleteFilterWordById(string idList)
        {
            string commandText = string.Format("DELETE FROM [{0}filterwords] WHERE [id] IN ({1}) ",
                                                RDBSHelper.RDBSTablePre,
                                                idList);
            RDBSHelper.ExecuteNonQuery(CommandType.Text, commandText);
        }

        #endregion
    }
}