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

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="spe"></param>
        /// <returns></returns>
        int AddFormula(StockPickEntity spe);

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="spe"></param>
        /// <returns></returns>
        int UpdateFormula(StockPickEntity spe);

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="uid"></param>
        /// <param name="fid"></param>
        /// <returns></returns>
        int DeleteFormulaById(int uid, int fid);

        /// <summary>
        /// 获取公式列表
        /// </summary>
        /// <param name="pager"></param>
        /// <param name="uid"></param>
        /// <param name="groupid"></param>
        /// <returns></returns>
        IDataReader GetFormulaList(PageEntity pager, int uid, int groupid = -1);

        /// <summary>
        /// 通过ID获取公式
        /// </summary>
        /// <param name="fid"></param>
        /// <returns></returns>
        IDataReader GetFormulaById(int fid);

        #region 分组
        /// <summary>
        /// 获取分组列表
        /// </summary>
        /// <param name="uid">用户id</param>
        /// <returns></returns>
        DataTable GetFormulaGroupList(int uid);

        /// <summary>
        /// 新增公式分组
        /// </summary>
        /// <param name="uid"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        int AddFormulaGroup(int uid, string name);

        /// <summary>
        /// 删除公式分组
        /// </summary>
        /// <param name="uid"></param>
        /// <param name="gid"></param>
        /// <returns></returns>
        int DeleteFormulaGroupById(int uid, int gid);
        
        /// <summary>
        /// 根据ID获取名称
        /// </summary>
        /// <param name="gid"></param>
        /// <returns></returns>
        string GetFormulaGroupName(int gid);
        #endregion
    }
}
