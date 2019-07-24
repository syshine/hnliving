using System;

namespace Lib.Core
{

    /// <summary>
    /// 配置信息类
    /// </summary>
    [Serializable]
    public class AccessConfigInfo : IConfigInfo
    {
        private string _area = "";
        private string _control = "";
        private string _action = "";
        private string _value = "";


        public string Area
        {
            get
            {
                return _area;
            }

            set
            {
                _area = value;
            }
        }

        public string Control
        {
            get
            {
                return _control;
            }

            set
            {
                _control = value;
            }
        }

        public string Action
        {
            get
            {
                return _action;
            }

            set
            {
                _action = value;
            }
        }

        public string Value
        {
            get
            {
                return _value;
            }

            set
            {
                _value = value;
            }
        }
    }

    //public class Tools : IConfigInfo
    //{
    //    private UEditor _ue = new UEditor();
    //}

    //public class UEditor : IConfigInfo
    //{
    //    private int _add = DefalutValue.defaultAccess;//新增
    //    private int _delete = DefalutValue.defaultAccess;//删除
    //    private int _list = DefalutValue.defaultAccess;//列表

    //    public int Add
    //    {
    //        get
    //        {
    //            return _add;
    //        }

    //        set
    //        {
    //            _add = value;
    //        }
    //    }

    //    public int Delete
    //    {
    //        get
    //        {
    //            return _delete;
    //        }

    //        set
    //        {
    //            _delete = value;
    //        }
    //    }

    //    public int List
    //    {
    //        get
    //        {
    //            return _list;
    //        }

    //        set
    //        {
    //            _list = value;
    //        }
    //    }
    //}

    public static class DefalutValue
    {
        public const string Access = "6";
    }
}
