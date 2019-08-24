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

        #region UEditor富文本编辑器
        /// <summary>
        /// 通过ID获取记录
        /// </summary>
        /// <param name="ueid">ueditor id</param>
        /// <returns></returns>
        public IDataReader GetUEditorById(int ueid)
        {
            string commandText = string.Format("SELECT * FROM [{0}ueditor] WHERE del_flag != 1 AND id={1}",
                                                RDBSHelper.RDBSTablePre,
                                                ueid);


            return RDBSHelper.ExecuteReader(CommandType.Text, commandText);
        }

        public string GetUEditorContentById(int id)
        {
            string commandText = string.Format("SELECT ue_content FROM [{0}ueditor] WHERE del_flag is null AND id ={1}",
                                                RDBSHelper.RDBSTablePre,
                                                id);
            object obj = RDBSHelper.ExecuteScalar(CommandType.Text, commandText);
            if (obj != null)
                return obj.ToString();
            else
                return null;
        }

        public IDataReader GetUEditorContentByType(int type)
        {
            string commandText = string.Format("SELECT ue_content FROM [{0}ueditor] WHERE del_flag != 1 AND typeid ={1}",
                                                RDBSHelper.RDBSTablePre,
                                                type);
            return RDBSHelper.ExecuteReader(CommandType.Text, commandText);
        }

        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="pager">页码</param>
        /// <param name="uid">用户id</param>
        /// <param name="typeid">类型id</param>
        /// <returns></returns>
        public IDataReader GetUEditorList(PageEntity pager, int uid, int typeid = -1)
        {
            string commandText = "";

            if (typeid >= 0)
            {
                commandText = string.Format("SELECT * FROM [{0}ueditor] WHERE del_flag != 1 AND uid={1} AND typeid ={2} order by update_time desc",
                                                    RDBSHelper.RDBSTablePre,
                                                    uid,
                                                    typeid);
            }
            else
            {
                commandText = string.Format("SELECT * FROM [{0}ueditor] WHERE del_flag != 1 AND uid={1} order by update_time desc",
                                                    RDBSHelper.RDBSTablePre,
                                                    uid);
            }

            if(pager.Totalcount >= 0)
            {
                pager.Totalcount = RDBSHelper.GetPageCount(commandText);
            }

            string sql = "";
            if (pager.Pagesize > 0)
                sql = RDBSHelper.GetPageSql(commandText, pager.Pagesize, pager.Pageindex);
            else
                sql = commandText;

            return RDBSHelper.ExecuteReader(CommandType.Text, sql);

        }

        #region 分类
        /// <summary>
        /// 获取分类列表
        /// </summary>
        /// <param name="uid">用户id</param>
        /// <returns></returns>
        public DataTable GetUEditorSort(int uid)
        {
            string commandText = string.Format("SELECT uid, sid, sname FROM [{0}ueditor_sort] WHERE del_flag != 1 AND uid in(-1,{1}) order by uid,sid",
                                                RDBSHelper.RDBSTablePre,
                                                uid);
            
            return RDBSHelper.ExecuteDataset(CommandType.Text, commandText).Tables[0];
        }


        public int UEditorAddSort(int uid, string name)
        {
            DbParameter[] parms = {
                                    GenerateInParam("@uid", SqlDbType.Int, 4, uid),
                                    GenerateInParam("@sname", SqlDbType.NVarChar, name.Length, name)
                                    };
            string commandText = string.Format("INSERT INTO [{0}ueditor_sort] (uid,sname) VALUES(@uid,@sname)",
                                                RDBSHelper.RDBSTablePre);
            return RDBSHelper.ExecuteNonQuery(CommandType.Text, commandText, parms);
        }

        public int UEditorDelSort(int sid)
        {
            DbParameter[] parms = {
                                    GenerateInParam("@sid", SqlDbType.Int, 4, sid)
                                    };
            string commandText = string.Format("UPDATE [{0}ueditor_sort] SET del_flag=1 WHERE sid=@sid",
                                                RDBSHelper.RDBSTablePre);
            return RDBSHelper.ExecuteNonQuery(CommandType.Text, commandText, parms);
        }

        public int DeleteSortUEditorById(int uid, int sid)
        {
            DbParameter[] parms = {
                                    GenerateInParam("@uid", SqlDbType.Int, 4, uid),
                                    GenerateInParam("@sid", SqlDbType.Int, 4, sid)
                                    };
            string commandText = string.Format("UPDATE [{0}ueditor_sort] SET del_flag = 1 WHERE uid=@uid AND sid=@sid",
                                                RDBSHelper.RDBSTablePre);
            return RDBSHelper.ExecuteNonQuery(CommandType.Text, commandText, parms);
        }



        /// <summary>
        /// 根据ID获取名称
        /// </summary>
        /// <param name="typeid"></param>
        /// <returns></returns>
        public string GetName(int typeid)
        {
            DbParameter[] parms = {
                                    GenerateInParam("@sid", SqlDbType.Int, 4, typeid)
                                    };
            string commandText = string.Format("SELECT sname FROM [{0}ueditor_sort] WHERE del_flag != 1 AND sid =@sid",
                                                RDBSHelper.RDBSTablePre);

            object obj = RDBSHelper.ExecuteScalar(CommandType.Text, commandText, parms);
            if (obj != null)
                return obj.ToString();
            else
                return null;
        }
        #endregion

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public int UEditorAdd(UEditorEntity entity)
        {
            DbParameter[] parms = {
                                    GenerateInParam("@uid", SqlDbType.Int, 4, entity.Uid),
                                    GenerateInParam("@typeid", SqlDbType.Int, 4, entity.Typeid),
                                    GenerateInParam("@title", SqlDbType.NVarChar, entity.Title.Length, entity.Title),
                                    GenerateInParam("@ue_content", SqlDbType.NVarChar, entity.Ue_content.Length, entity.Ue_content),
                                    GenerateInParam("@tag", SqlDbType.NVarChar, entity.Tag.Length, entity.Tag),
                                    GenerateInParam("@remark", SqlDbType.NVarChar, entity.Remark.Length, entity.Remark)
                                    };
            string commandText = string.Format("INSERT INTO [{0}ueditor] (uid,typeid,title,create_time,update_time,ue_content,tag,remark) VALUES(@uid,@typeid,@title,GETDATE(),GETDATE(),@ue_content,@tag,@remark)",
                                                RDBSHelper.RDBSTablePre);
            return RDBSHelper.ExecuteNonQuery(CommandType.Text, commandText, parms);
        }

        public int SaveUEditorContent(string content, int type)
        {
            DbParameter[] parms = {
                                    GenerateInParam("@content", SqlDbType.NVarChar, content.Length, content),
                                    GenerateInParam("@type", SqlDbType.Int,4, type)
                                    };
            string commandText = string.Format("INSERT INTO [{0}ueditor] (ue_content,typeid) VALUES(@content,@typeid)",
                                                RDBSHelper.RDBSTablePre);
            return RDBSHelper.ExecuteNonQuery(CommandType.Text, commandText, parms);
        }


        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="uee"></param>
        /// <returns></returns>
        public int UEditorUpdate(UEditorEntity uee)
        {
            DbParameter[] parms = {
                                    GenerateInParam("@id", SqlDbType.Int, 4, uee.Id),
                                    GenerateInParam("@uid", SqlDbType.Int, 4, uee.Uid),
                                    GenerateInParam("@typeid", SqlDbType.Int, 4, uee.Typeid),
                                    GenerateInParam("@title", SqlDbType.NVarChar, uee.Title.Length, uee.Title),
                                    GenerateInParam("@ue_content", SqlDbType.NVarChar, uee.Ue_content.Length, uee.Ue_content),
                                    GenerateInParam("@tag", SqlDbType.NVarChar, uee.Tag.Length, uee.Tag),
                                    GenerateInParam("@remark", SqlDbType.NVarChar, uee.Remark.Length, uee.Remark)
                                    };
            string commandText = string.Format("UPDATE [{0}ueditor] SET typeid=@typeid,title=@title,update_time=GETDATE(),ue_content=@ue_content,tag=@tag,remark=@remark WHERE id=@id and uid=@uid",
                                                RDBSHelper.RDBSTablePre);
            return RDBSHelper.ExecuteNonQuery(CommandType.Text, commandText, parms);
        }

        public int UpdateUEditorContent(int ueid, string content)
        {
            DbParameter[] parms = {
                                    GenerateInParam("@id", SqlDbType.Int, 4, ueid),
                                    GenerateInParam("@content", SqlDbType.NVarChar, content.Length, content)
                                    };
            string commandText = string.Format("UPDATE [{0}ueditor] SET ue_content=@content WHERE id=@id",
                                                RDBSHelper.RDBSTablePre);
            return RDBSHelper.ExecuteNonQuery(CommandType.Text, commandText, parms);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="uid"></param>
        /// <param name="ueid"></param>
        /// <returns></returns>
        public int DeleteUEditorById(int uid, int ueid)
        {
            DbParameter[] parms = {
                                    GenerateInParam("@uid", SqlDbType.Int, 4, uid),
                                    GenerateInParam("@ueid", SqlDbType.Int, 4, ueid)
                                    };
            string commandText = string.Format("UPDATE [{0}ueditor] SET del_flag = 1 WHERE uid=@uid AND id=@ueid",
                                                RDBSHelper.RDBSTablePre);
            return RDBSHelper.ExecuteNonQuery(CommandType.Text, commandText, parms);
        }
        #endregion
    }
}