using System.Data;

using Lib.Core;
using System.Data.Common;
using Lib.Core.Domain.Ltr;
using System.Collections.Generic;

namespace hnliving.RDBSStrategy.SqlServer
{
    /// <summary>
    /// SqlServer策略之Stock分部类
    /// </summary>
    public partial class RDBSStrategy : IRDBSStrategy
    {
        /// <summary>
        /// 保存基础信息
        /// </summary>
        /// <param name="data"></param>
        /// <param name="split">分隔符</param>
        /// <returns></returns>
        public int SaveStockBaseInfo(string data, string split = "&")
        {
            DbParameter[] parms = {
                                       GenerateInParam("@data",SqlDbType.NVarChar,data.Length, data),
                                       GenerateInParam("@split",SqlDbType.NVarChar,data.Length, split)
                                   };
            return TypeHelper.ObjectToInt(RDBSHelper.ExecuteScalar(CommandType.StoredProcedure,
                                                                   string.Format("{0}savestockbaseinfo", RDBSHelper.RDBSTablePre),
                                                                   parms), -1);
        }



        /// <summary>
        /// 获取所有的股票
        /// </summary>
        /// <returns></returns>
        public DataSet GetAllStock()
        {
            string sql = @"select s_code, s_type from [{0}stock] order by s_code;
                            select scode, max(hdate) last_date from [{0}stock_his_data1] group by scode;
                            select scode, max(hdate) last_date from [{0}stock_his_data2] group by scode;";

            string commandText = string.Format(sql, RDBSHelper.RDBSTablePre);

            return RDBSHelper.ExecuteDataset(CommandType.Text, commandText);
        }

        /// <summary>
        /// 获取部分的股票
        /// </summary>
        /// <param name="lstCode">股票代码编号</param>
        /// <returns></returns>
        public DataSet GetPartStock(List<string> lstCode)
        {
            string sql = @"select s_code, s_type from [{0}stock] where s_code in ({1}) order by s_code;
                            select scode, max(hdate) last_date from [{0}stock_his_data1] where scode in ({1}) group by scode;
                            select scode, max(hdate) last_date from [{0}stock_his_data2] where scode in ({1}) group by scode;";

            string commandText = string.Format(sql, RDBSHelper.RDBSTablePre, string.Join(",", lstCode));

            return RDBSHelper.ExecuteDataset(CommandType.Text, commandText);
        }

        /// <summary>
        /// 保存股票历史信息
        /// </summary>
        /// <param name="data">保存的内容</param>
        /// <param name="timeout">超时时间</param>
        /// <returns></returns>
        public int SaveStockHis(string data, int timeout = 180)
        {
            DbParameter[] parms = {
                                       GenerateInParam("@data",SqlDbType.NVarChar,data.Length, data)
                                   };
            return TypeHelper.ObjectToInt(RDBSHelper.ExecuteScalar(CommandType.StoredProcedure,
                                                                   string.Format("{0}savestockdatahis", RDBSHelper.RDBSTablePre),
                                                                   timeout,
                                                                   parms), -1);
        }


        /// <summary>
        /// 获取股票信息
        /// </summary>
        /// <param name="code">股票代码编号</param>
        /// <returns></returns>
        public DataSet GetStockInfo(string code)
        {
            string sql = @"select * from [{0}stock] where s_code = '{1}'";

            string commandText = string.Format(sql, RDBSHelper.RDBSTablePre, code);

            return RDBSHelper.ExecuteDataset(CommandType.Text, commandText);
        }
        /// <summary>
        /// 获取股票历史信息
        /// </summary>
        /// <param name="code">股票代码编号</param>
        /// <param name="type">沪市1,深市2</param>
        /// <returns></returns>
        public DataTable GetStockInfoHis(string code, string type)
        {
            // 排除停牌的数据CHG <> 'None'
            string sql = @"select *, CONVERT(varchar(8),HDATE, 112) fdate from [{0}stock_his_data{1}] where scode = '{2}' and CHG <> 'None' order by hdate asc";

            string commandText = string.Format(sql, RDBSHelper.RDBSTablePre, type, code);

            return RDBSHelper.ExecuteDataset(CommandType.Text, commandText).Tables[0];
        }
    }
}
