using System.Data;

using Lib.Core;
using System.Data.Common;
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

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="spe"></param>
        /// <returns></returns>
        public int AddFormula(StockPickEntity spe)
        {
            DbParameter[] parms = {
                                    GenerateInParam("@uid", SqlDbType.Int, 4, spe.Uid),
                                    GenerateInParam("@groupid", SqlDbType.Int, 4, spe.Groupid),
                                    GenerateInParam("@fname", SqlDbType.NVarChar, spe.Fname.Length, spe.Fname),
                                    GenerateInParam("@UseFormerComplexRights", SqlDbType.Int, 4, spe.UseFormerComplexRights),
                                    GenerateInParam("@PCHGEnable", SqlDbType.Int, 4, spe.PCHGEnable),
                                    GenerateInParam("@Days", SqlDbType.Int, 4, spe.Days),
                                    GenerateInParam("@IsRise", SqlDbType.Int, 4, spe.IsRise),
                                    GenerateInParam("@Operator", SqlDbType.NVarChar, spe.Operator.Length, spe.Operator),
                                    GenerateInParam("@PCHG", SqlDbType.Decimal, 4, spe.PCHG),
                                    GenerateInParam("@PriceEnable", SqlDbType.Int, 4, spe.PriceEnable),
                                    GenerateInParam("@PriceLow", SqlDbType.Decimal, 4, spe.PriceLow),
                                    GenerateInParam("@PriceHigh", SqlDbType.Decimal, 4, spe.PriceHigh),
                                    GenerateInParam("@MCAPEnable", SqlDbType.Int, 4, spe.MCAPEnable),
                                    GenerateInParam("@MCAPLow", SqlDbType.Decimal, 4, spe.MCAPLow),
                                    GenerateInParam("@MCAPHigh", SqlDbType.Decimal, 4, spe.MCAPHigh),
                                    GenerateInParam("@FormulaEnable", SqlDbType.Int, 4, spe.FormulaEnable),
                                    GenerateInParam("@PreAvgLines", SqlDbType.NVarChar, spe.PreAvgLines.Length, spe.PreAvgLines),
                                    GenerateInParam("@PreDays", SqlDbType.Int, 4, spe.PreDays),
                                    GenerateInParam("@Formula", SqlDbType.NVarChar, spe.Formula.Length, spe.Formula)
                                    };
            string commandText = string.Format(@"INSERT INTO [{0}stock_formula] (uid,groupid,fname,create_time,update_time,UseFormerComplexRights,PCHGEnable,Days,IsRise,Operator,PCHG,
                                                PriceEnable,PriceLow,PriceHigh,MCAPEnable,MCAPLow,MCAPHigh,FormulaEnable,PreAvgLines,PreDays,Formula)
                                                VALUES(@uid,@groupid,@fname,GETDATE(),GETDATE(),@UseFormerComplexRights,@PCHGEnable,@Days,@IsRise,@Operator,@PCHG,
                                                @PriceEnable,@PriceLow,@PriceHigh,@MCAPEnable,@MCAPLow,@MCAPHigh,@FormulaEnable,@PreAvgLines,@PreDays,@Formula)",
                                                RDBSHelper.RDBSTablePre);
            return RDBSHelper.ExecuteNonQuery(CommandType.Text, commandText, parms);
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="spe"></param>
        /// <returns></returns>
        public int UpdateFormula(StockPickEntity spe)
        {
            DbParameter[] parms = {
                                    GenerateInParam("@fid", SqlDbType.Int, 4, spe.Fid),
                                    GenerateInParam("@uid", SqlDbType.Int, 4, spe.Uid),
                                    GenerateInParam("@groupid", SqlDbType.Int, 4, spe.Groupid),
                                    GenerateInParam("@fname", SqlDbType.NVarChar, spe.Fname.Length, spe.Fname),
                                    GenerateInParam("@UseFormerComplexRights", SqlDbType.Int, 4, spe.UseFormerComplexRights),
                                    GenerateInParam("@PCHGEnable", SqlDbType.Int, 4, spe.PCHGEnable),
                                    GenerateInParam("@Days", SqlDbType.Int, 4, spe.Days),
                                    GenerateInParam("@IsRise", SqlDbType.Int, 4, spe.IsRise),
                                    GenerateInParam("@Operator", SqlDbType.NVarChar, spe.Operator.Length, spe.Operator),
                                    GenerateInParam("@PCHG", SqlDbType.Decimal, 4, spe.PCHG),
                                    GenerateInParam("@PriceEnable", SqlDbType.Int, 4, spe.PriceEnable),
                                    GenerateInParam("@PriceLow", SqlDbType.Decimal, 4, spe.PriceLow),
                                    GenerateInParam("@PriceHigh", SqlDbType.Decimal, 4, spe.PriceHigh),
                                    GenerateInParam("@MCAPEnable", SqlDbType.Int, 4, spe.MCAPEnable),
                                    GenerateInParam("@MCAPLow", SqlDbType.Decimal, 4, spe.MCAPLow),
                                    GenerateInParam("@MCAPHigh", SqlDbType.Decimal, 4, spe.MCAPHigh),
                                    GenerateInParam("@FormulaEnable", SqlDbType.Int, 4, spe.FormulaEnable),
                                    GenerateInParam("@PreAvgLines", SqlDbType.NVarChar, spe.PreAvgLines.Length, spe.PreAvgLines),
                                    GenerateInParam("@PreDays", SqlDbType.Int, 4, spe.PreDays),
                                    GenerateInParam("@Formula", SqlDbType.NVarChar, spe.Formula.Length, spe.Formula)
                                    };

            string commandText = string.Format(@"UPDATE [{0}stock_formula] SET 
                                                groupid=@groupid,
                                                fname=@fname,
                                                update_time=GETDATE(),
                                                UseFormerComplexRights=@UseFormerComplexRights,
                                                PCHGEnable=@PCHGEnable,
                                                Days=@Days,
                                                IsRise=@IsRise,
                                                Operator=@Operator,
                                                PCHG=@PCHG,
                                                PriceEnable=@PriceEnable,
                                                PriceLow=@PriceLow,
                                                PriceHigh=@PriceHigh,
                                                MCAPEnable=@MCAPEnable,
                                                MCAPLow=@MCAPLow,
                                                MCAPHigh=@MCAPHigh,
                                                FormulaEnable=@FormulaEnable,
                                                PreAvgLines=@PreAvgLines,
                                                PreDays=@PreDays,
                                                Formula=@Formula
                                                WHERE fid=@fid and uid=@uid",
                                                RDBSHelper.RDBSTablePre);
            return RDBSHelper.ExecuteNonQuery(CommandType.Text, commandText, parms);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="uid"></param>
        /// <param name="fid"></param>
        /// <returns></returns>
        public int DeleteFormulaById(int uid, int fid)
        {
            DbParameter[] parms = {
                                    GenerateInParam("@uid", SqlDbType.Int, 4, uid),
                                    GenerateInParam("@fid", SqlDbType.Int, 4, fid)
                                    };
            string commandText = string.Format("UPDATE [{0}stock_formula] SET del_flag = 1 WHERE uid=@uid AND fid=@fid",
                                                RDBSHelper.RDBSTablePre);
            return RDBSHelper.ExecuteNonQuery(CommandType.Text, commandText, parms);
        }

        /// <summary>
        /// 获取公式列表
        /// </summary>
        /// <param name="pager"></param>
        /// <param name="uid"></param>
        /// <param name="groupid"></param>
        /// <returns></returns>
        public IDataReader GetFormulaList(PageEntity pager, int uid, int groupid = -1)
        {
            string commandText = "";

            if (groupid >= 0)
            {
                commandText = string.Format("SELECT top 100 percent * FROM [{0}stock_formula] WHERE del_flag != 1 AND uid={1} AND groupid ={2} order by update_time desc",
                                                    RDBSHelper.RDBSTablePre,
                                                    uid,
                                                    groupid);
            }
            else
            {
                commandText = string.Format("SELECT top 100 percent * FROM [{0}stock_formula] WHERE del_flag != 1 AND uid={1} order by update_time desc",
                                                    RDBSHelper.RDBSTablePre,
                                                    uid);
            }

            if (pager.Totalcount >= 0)
            {
                pager.Totalcount = RDBSHelper.GetPageCount2008(commandText);
            }

            string sql = "";
            if (pager.Pagesize > 0)
                sql = RDBSHelper.GetPageSql2008(commandText, pager.Pagesize, pager.Pageindex);
            else
                sql = commandText;

            return RDBSHelper.ExecuteReader(CommandType.Text, sql);

        }

        /// <summary>
        /// 通过ID获取公式
        /// </summary>
        /// <param name="fid"></param>
        /// <returns></returns>
        public IDataReader GetFormulaById(int fid)
        {
            string commandText = string.Format("SELECT * FROM [{0}stock_formula] WHERE del_flag != 1 AND fid={1}",
                                                RDBSHelper.RDBSTablePre,
                                                fid);


            return RDBSHelper.ExecuteReader(CommandType.Text, commandText);
        }

        #region 分组
        /// <summary>
        /// 获取分组列表
        /// </summary>
        /// <param name="uid">用户id</param>
        /// <returns></returns>
        public DataTable GetFormulaGroupList(int uid)
        {
            string commandText = string.Format("SELECT uid, gid, gname FROM [{0}stock_formula_group] WHERE del_flag != 1 AND uid in(-1,{1}) order by uid,gid",
                                                RDBSHelper.RDBSTablePre,
                                                uid);

            return RDBSHelper.ExecuteDataset(CommandType.Text, commandText).Tables[0];
        }

        /// <summary>
        /// 新增公式分组
        /// </summary>
        /// <param name="uid"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public int AddFormulaGroup(int uid, string name)
        {
            DbParameter[] parms = {
                                    GenerateInParam("@uid", SqlDbType.Int, 4, uid),
                                    GenerateInParam("@gname", SqlDbType.NVarChar, name.Length, name)
                                    };
            string commandText = string.Format("INSERT INTO [{0}stock_formula_group] (uid,gname) VALUES(@uid,@gname)",
                                                RDBSHelper.RDBSTablePre);
            return RDBSHelper.ExecuteNonQuery(CommandType.Text, commandText, parms);
        }

        ///// <summary>
        ///// 删除公式分组
        ///// </summary>
        ///// <param name="gid"></param>
        ///// <returns></returns>
        //public int DelFormulaGroup(int gid)
        //{
        //    DbParameter[] parms = {
        //                            GenerateInParam("@gid", SqlDbType.Int, 4, sid)
        //                            };
        //    string commandText = string.Format("UPDATE [{0}stock_group] SET del_flag=1 WHERE gid=@gid",
        //                                        RDBSHelper.RDBSTablePre);
        //    return RDBSHelper.ExecuteNonQuery(CommandType.Text, commandText, parms);
        //}

        /// <summary>
        /// 删除公式分组
        /// </summary>
        /// <param name="uid"></param>
        /// <param name="gid"></param>
        /// <returns></returns>
        public int DeleteFormulaGroupById(int uid, int gid)
        {
            DbParameter[] parms = {
                                    GenerateInParam("@uid", SqlDbType.Int, 4, uid),
                                    GenerateInParam("@gid", SqlDbType.Int, 4, gid)
                                    };
            string commandText = string.Format("UPDATE [{0}stock_formula_group] SET del_flag = 1 WHERE uid=@uid AND gid=@gid",
                                                RDBSHelper.RDBSTablePre);
            return RDBSHelper.ExecuteNonQuery(CommandType.Text, commandText, parms);
        }



        /// <summary>
        /// 根据ID获取名称
        /// </summary>
        /// <param name="gid"></param>
        /// <returns></returns>
        public string GetFormulaGroupName(int gid)
        {
            DbParameter[] parms = {
                                    GenerateInParam("@gid", SqlDbType.Int, 4, gid)
                                    };
            string commandText = string.Format("SELECT gname FROM [{0}stock_formula_group] WHERE del_flag != 1 AND gid =@gid",
                                                RDBSHelper.RDBSTablePre);

            object obj = RDBSHelper.ExecuteScalar(CommandType.Text, commandText, parms);
            if (obj != null)
                return obj.ToString();
            else
                return null;
        }
        #endregion
    }
}
