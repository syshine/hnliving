using Lib.Core;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lib.Data
{
    public partial class UEditorUtils
    {
        public static UEditorEntity GetById(int ueid)
        {
            UEditorEntity uee = new UEditorEntity();
            IDataReader reader = Lib.Core.MngData.RDBS.GetUEditorById(ueid);
            while (reader.Read())
            {
                uee.Id = int.Parse(reader["id"].ToString());
                uee.Uid = int.Parse(reader["uid"].ToString());
                uee.Typeid = int.Parse(reader["typeid"].ToString());
                uee.Title = reader["title"].ToString();
                if (reader["create_time"] != null)
                    uee.Create_time = DateTime.Parse(reader["create_time"].ToString());
                if (reader["update_time"] != null)
                    uee.Update_time = DateTime.Parse(reader["update_time"].ToString());
                uee.Ue_content = reader["ue_content"].ToString();
                uee.Remark = reader["ue_content"].ToString();
                uee.Del_flag = int.Parse(reader["del_flag"].ToString());
                uee.Tag = reader["tag"].ToString();
            }
            reader.Close();

            return uee;
        }
        public static string GetContentById(int id)
        {
            return Lib.Core.MngData.RDBS.GetUEditorContentById(id);
        }

        public static List<UEditorEntity> GetContentByType(int typeid)
        {
            List<UEditorEntity> ueeList = new List<UEditorEntity>();
            IDataReader reader = Lib.Core.MngData.RDBS.GetUEditorContentByType(typeid);
            while (reader.Read())
            {
                UEditorEntity uee = new UEditorEntity();
                uee.Ue_content = reader["ue_content"].ToString();
                ueeList.Add(uee);
            }
            reader.Close();
            return ueeList;
        }

        public static List<UEditorEntity> GetList(int uid, int typeid = -1)
        {
            List<UEditorEntity> ueeList = new List<UEditorEntity>();
            IDataReader reader = Lib.Core.MngData.RDBS.GetUEditorList(uid, typeid);
            while (reader.Read())
            {
                UEditorEntity uee = new UEditorEntity();
                uee.Id = int.Parse(reader["id"].ToString());
                uee.Uid = int.Parse(reader["uid"].ToString());
                uee.Typeid = int.Parse(reader["typeid"].ToString());
                uee.Title = reader["title"].ToString();
                if (reader["create_time"] != null)
                    uee.Create_time = DateTime.Parse(reader["create_time"].ToString());
                if (reader["update_time"] != null)
                    uee.Update_time = DateTime.Parse(reader["update_time"].ToString());
                uee.Ue_content = reader["ue_content"].ToString();
                uee.Remark = reader["ue_content"].ToString();
                uee.Del_flag = int.Parse(reader["del_flag"].ToString());
                uee.Tag = reader["tag"].ToString();
                ueeList.Add(uee);
            }
            reader.Close();
            return ueeList;
        }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public static int Add(UEditorEntity entity)
        {
            return Lib.Core.MngData.RDBS.UEditorAdd(entity);
        }

        public static int Add(string content, int type)
        {
            return Lib.Core.MngData.RDBS.SaveUEditorContent(content, type);
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public static int Update(UEditorEntity uee)
        {
            return Lib.Core.MngData.RDBS.UEditorUpdate(uee);
        }

        public static int Update(int id, string content)
        {
            return Lib.Core.MngData.RDBS.UpdateUEditorContent(id, content);
        }
        
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="uid">用户ID</param>
        /// <param name="ueid"></param>
        /// <returns></returns>
        public static int DeleteById(int uid, int ueid)
        {
            return Lib.Core.MngData.RDBS.DeleteUEditorById(uid, ueid);
        }

        #region 分类
        /// <summary>
        /// 获取分类
        /// </summary>
        /// <param name="uid"></param>
        /// <returns></returns>
        public static DataTable GetSort(int uid)
        {
            //DataTable dtSort = new DataTable();
            //dtSort.Columns.Add("id");
            //dtSort.Columns.Add("name");

            //IDataReader reader = Lib.Core.MngData.RDBS.GetUEditorSort(uid);
            //while (reader.Read())
            //{
            //    DataRow dr = dtSort.NewRow();
            //    dr["id"] = reader["sid"].ToString();
            //    dr["name"] = reader["sname"].ToString();
            //    dtSort.Rows.Add(dr);
            //}
            //reader.Close();


            DataTable dtSort = Lib.Core.MngData.RDBS.GetUEditorSort(uid);
            return dtSort;
        }

        /// <summary>
        /// 新增分类
        /// </summary>
        /// <param name="uid"></param>
        /// <param name="title"></param>
        /// <returns></returns>
        public static int AddSort(int uid, string title)
        {
            return Lib.Core.MngData.RDBS.UEditorAddSort(uid, title);
        }

        /// <summary>
        /// 删除分类
        /// </summary>
        /// <param name="uid"></param>
        /// <param name="sid"></param>
        /// <returns></returns>
        public static int DeleteSortById(int uid, int sid)
        {
            return Lib.Core.MngData.RDBS.DeleteSortUEditorById(uid, sid);
        }
        #endregion
    }
}
