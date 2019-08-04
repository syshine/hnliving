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

        #region UEditor富文本编辑器

        IDataReader GetUEditorById(int ueid);

        string GetUEditorContentById(int ueid);

        IDataReader GetUEditorContentByType(int type);


        IDataReader GetUEditorList(int uid, int typeid = -1);

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        int UEditorAdd(UEditorEntity entity);

        int SaveUEditorContent(string content, int type);


        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        int UEditorUpdate(UEditorEntity uee);

        int UpdateUEditorContent(int ueid, string content);


        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="uid"></param>
        /// <param name="ueid"></param>
        /// <returns></returns>
        int DeleteUEditorById(int uid, int ueid);

        #region 分类

        DataTable GetUEditorSort(int uid);

        int UEditorAddSort(int uid, string name);

        int UEditorDelSort(int sid);

        /// <summary>
        /// 删除分类
        /// </summary>
        /// <param name="uid"></param>
        /// <param name="sid"></param>
        /// <returns></returns>
        int DeleteSortUEditorById(int uid, int sid);

        /// <summary>
        /// 根据ID获取名称
        /// </summary>
        /// <param name="typeid"></param>
        /// <returns></returns>
        string GetName(int typeid);
        #endregion
        #endregion
    }
}
