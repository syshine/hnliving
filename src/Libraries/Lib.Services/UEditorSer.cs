using Lib.Core;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lib.Services
{
    public partial class UEditorSer
    {
        public static UEditorEntity GetById(int ueid)
        {
            //return Lib.Data.UEditorUtils.GetById(ueid);

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
            //if (id > 0)
            //    return Lib.Data.UEditorUtils.GetContentById(id);

            if (id > 0)
                return Lib.Core.MngData.RDBS.GetUEditorContentById(id);
            else
                return null;
        }

        public static List<UEditorEntity> GetContentByType(int typeid)
        {
            //return Lib.Data.UEditorUtils.GetContentByType(typeid);

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

        /// <summary>
        /// 根据用户ID和类型获取列表
        /// </summary>
        /// <param name="uid"></param>
        /// <param name="typeid"></param>
        /// <returns></returns>
        public static List<UEditorEntity> GetList(int uid = -1, int typeid = -1)
        {
            //return Lib.Data.UEditorUtils.GetList(uid);
            
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

        public static int Add(UEditorEntity uee)
        {
            //return Lib.Data.UEditorUtils.Add(uee);
            return Lib.Core.MngData.RDBS.UEditorAdd(uee);
        }

        public static int Add(string content, int type)
        {
            //return Lib.Data.UEditorUtils.Add(content, type);
            return Lib.Core.MngData.RDBS.SaveUEditorContent(content, type);
        }

        public static int Update(UEditorEntity uee)
        {
            //return Lib.Data.UEditorUtils.Update(uee);
            return Lib.Core.MngData.RDBS.UEditorUpdate(uee);
        }

        public static int Update(int ueid,string content)
        {
            //return Lib.Data.UEditorUtils.Update(ueid, content);
            return Lib.Core.MngData.RDBS.UpdateUEditorContent(ueid, content);
        }

        public static int DeleteById(int uid, int ueid)
        {
            //return Lib.Data.UEditorUtils.DeleteById(uid, ueid);
            return Lib.Core.MngData.RDBS.DeleteUEditorById(uid, ueid);
        }

        #region 分类

        /// <summary>
        /// 根据用户ID获取分类列表
        /// </summary>
        /// <param name="uid"></param>
        /// <returns></returns>
        public static DataTable GetSort(int uid)
        {
            //return Lib.Data.UEditorUtils.GetSort(uid);
            DataTable dtSort = Lib.Core.MngData.RDBS.GetUEditorSort(uid);
            return dtSort;
        }

        public static int AddSort(int uid, string title)
        {
            //return Lib.Data.UEditorUtils.AddSort(uid, title);
            return Lib.Core.MngData.RDBS.UEditorAddSort(uid, title);
        }

        public static int DeleteSortById(int uid, int sid)
        {
            //return Lib.Data.UEditorUtils.DeleteSortById(uid, sid);
            return Lib.Core.MngData.RDBS.DeleteSortUEditorById(uid, sid);
        }

        /// <summary>
        /// 根据ID获取名称
        /// </summary>
        /// <param name="typeid"></param>
        /// <returns></returns>
        public static string GetName(int typeid)
        {
            string name = Lib.Core.MngData.RDBS.GetName(typeid);
            return name == null ? "" : name;
        }
        #endregion
    }
}
