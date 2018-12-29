using System;
using System.Data;
using System.Collections.Generic;

namespace Lib.Core
{
    /// <summary>
    /// 关系数据库策略之分部接口
    /// </summary>
    public partial interface IRDBSStrategy
    {
        #region 筛选词

        /// <summary>
        /// 获得筛选词列表
        /// </summary>
        /// <returns></returns>
        IDataReader GetFilterWordList();

        /// <summary>
        /// 添加筛选词
        /// </summary>
        void AddFilterWord(FilterWordInfo filterWordInfo);

        /// <summary>
        /// 更新筛选词
        /// </summary>
        void UpdateFilterWord(FilterWordInfo filterWordInfo);

        /// <summary>
        /// 删除筛选词
        /// </summary>
        /// <param name="idList">id列表</param>
        void DeleteFilterWordById(string idList);

        #endregion
    }
}
