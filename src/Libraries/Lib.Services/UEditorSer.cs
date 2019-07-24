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
            return Lib.Data.UEditorUtils.GetById(ueid);
        }

        public static string GetContentById(int id)
        {
            if (id > 0)
                return Lib.Data.UEditorUtils.GetContentById(id);

            return null;
        }

        public static List<UEditorEntity> GetContentByType(int type)
        {
            return Lib.Data.UEditorUtils.GetContentByType(type);
        }

        /// <summary>
        /// 根据用户ID获取列表
        /// </summary>
        /// <param name="uid"></param>
        /// <returns></returns>
        public static List<UEditorEntity> GetList(int uid)
        {
            return Lib.Data.UEditorUtils.GetList(uid);
        }

        public static int Add(UEditorEntity uee)
        {
            return Lib.Data.UEditorUtils.Add(uee);
        }

        public static int Add(string content, int type)
        {
            return Lib.Data.UEditorUtils.Add(content, type);
        }

        public static int Update(UEditorEntity uee)
        {
            return Lib.Data.UEditorUtils.Update(uee);
        }

        public static int Update(int ueid,string content)
        {
            return Lib.Data.UEditorUtils.Update(ueid, content);
        }

        public static int DeleteById(int uid, int ueid)
        {
            return Lib.Data.UEditorUtils.DeleteById(uid, ueid);
        }

        #region 分类

        /// <summary>
        /// 根据用户ID获取分类列表
        /// </summary>
        /// <param name="uid"></param>
        /// <returns></returns>
        public static DataTable GetSort(int uid)
        {
            return Lib.Data.UEditorUtils.GetSort(uid);
        }

        public static int AddSort(int uid, string title)
        {
            return Lib.Data.UEditorUtils.AddSort(uid, title);
        }

        public static int DeleteSortById(int uid, int sid)
        {
            return Lib.Data.UEditorUtils.DeleteSortById(uid, sid);
        }
        #endregion
    }
}
