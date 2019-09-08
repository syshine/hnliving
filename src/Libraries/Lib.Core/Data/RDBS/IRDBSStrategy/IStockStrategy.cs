using System;
using System.Collections.Generic;
using System.Data;

namespace Lib.Core
{
    public partial interface IRDBSStrategy
    {
        /// <summary>
        /// 保存基础信息
        /// </summary>
        /// <param name="data"></param>
        /// <param name="split">分隔符</param>
        /// <returns></returns>
        int SaveStockBaseInfo(string data, string split = "&");

        /// <summary>
        /// 获取所有的股票
        /// </summary>
        /// <returns></returns>
        DataSet GetAllStock();

        /// <summary>
        /// 获取所有的股票
        /// </summary>
        /// <param name="lstCode">股票代码编号</param>
        /// <returns></returns>
        DataSet GetPartStock(List<string> lstCode);

        /// <summary>
        /// 保存股票历史信息
        /// </summary>
        /// <param name="data">保存的内容</param>
        /// <param name="timeout">超时时间</param>
        /// <returns></returns>
        int SaveStockHis(string data, int timeout = 180);

        /// <summary>
        /// 获取股票基本信息
        /// </summary>
        /// <param name="code">股票代码编号</param>
        /// <returns></returns>
        DataSet GetStockInfo(string code);

        /// <summary>
        /// 获取股票历史信息
        /// </summary>
        /// <param name="code">股票代码编号</param>
        /// <param name="type">沪市1,深市2</param>
        /// <returns></returns>
        DataTable GetStockInfoHis(string code, string type);
    }
}
