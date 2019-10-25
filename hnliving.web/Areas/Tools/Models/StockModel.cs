using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace hnliving.web.Areas.Tools.Models
{
    public class StockModel
    {
    }

    public class StockFormulaInfo
    {
        private int _fid = -1;//fid
        private int _uid = -1;//uid
        private string _groupName = "";//分组名称
        private string _fname = "";//标题
        private DateTime _create_time;//创建时间
        private DateTime _update_time;//更新时间

        public int Fid
        {
            get
            {
                return _fid;
            }

            set
            {
                _fid = value;
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

        public string GroupName
        {
            get
            {
                return _groupName;
            }

            set
            {
                _groupName = value;
            }
        }

        public string Fname
        {
            get
            {
                return _fname;
            }

            set
            {
                _fname = value;
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
    }

    public class StockFormulaListModel
    {
        /// <summary>
        /// 分页对象
        /// </summary>
        public PageModel PageModel { get; set; }

        /// <summary>
        /// 分组列表
        /// </summary>
        public List<KeyValuePair<string, string>> GroupList { get; set; }

        /// <summary>
        /// 内容列表
        /// </summary>
        public List<StockFormulaInfo> StockFormulaList { get; set; }
    }
}