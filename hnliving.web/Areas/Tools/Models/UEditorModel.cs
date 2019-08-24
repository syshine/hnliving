using System;
using System.Collections.Generic;

namespace hnliving.web.Areas.Tools.Models
{
    public class UEditorModel
    {
    }

    public class UEditorInfo
    {
        private int _id = -1;//id
        private int _uid = -1;//uid
        private string _typename = "";//类型名称
        private string _title = "";//标题
        private DateTime _create_time;//创建时间
        private DateTime _update_time;//更新时间
        private string _tag = "";//标签

        public int Id
        {
            get
            {
                return _id;
            }

            set
            {
                _id = value;
            }
        }

        public int Uid
        {
            get
            {
                return _uid;
            }

            set
            {
                _uid = value;
            }
        }

        public string Typename
        {
            get
            {
                return _typename;
            }

            set
            {
                _typename = value;
            }
        }

        public string Title
        {
            get
            {
                return _title;
            }

            set
            {
                _title = value;
            }
        }

        public DateTime Create_time
        {
            get
            {
                return _create_time;
            }

            set
            {
                _create_time = value;
            }
        }

        public DateTime Update_time
        {
            get
            {
                return _update_time;
            }

            set
            {
                _update_time = value;
            }
        }

        public string Tag
        {
            get
            {
                return _tag;
            }

            set
            {
                _tag = value;
            }
        }
    }

    public class UEditorListModel
    {
        /// <summary>
        /// 分页对象
        /// </summary>
        public PageModel PageModel { get; set; }

        /// <summary>
        /// 类型列表
        /// </summary>
        public List<KeyValuePair<string, string>> TypeList { get; set; }

        /// <summary>
        /// 内容列表
        /// </summary>
        public List<UEditorInfo> UEditorList { get; set; }
    }
}