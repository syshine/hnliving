using System;

namespace Lib.Core
{
    public class UEditorEntity
    {
        private int _id=-1;//id
        private int _uid=-1;//uid
        private int _typeid = 0;//类型
        private string _title = "";//标题
        private DateTime _create_time;//创建时间
        private DateTime _update_time;//更新时间
        private string _ue_content = "";//内容
        private string _remark = "";//备注
        private int _del_flag=-1;//删除标志
        private string _tag="";//标签

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

        public int Typeid
        {
            get
            {
                return _typeid;
            }

            set
            {
                _typeid = value;
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

        public string Ue_content
        {
            get
            {
                return _ue_content;
            }

            set
            {
                _ue_content = value;
            }
        }

        public string Remark
        {
            get
            {
                return _remark;
            }

            set
            {
                _remark = value;
            }
        }

        public int Del_flag
        {
            get
            {
                return _del_flag;
            }

            set
            {
                _del_flag = value;
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
}