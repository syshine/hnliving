using System;

namespace Lib.Core.Domain.Ltr
{
    public class Pl5Entity
    {
        int _issue;//期号
        DateTime _date; // 日期
        string _numb;//号码

        public int Issue
        {
            get
            {
                return _issue;
            }

            set
            {
                _issue = value;
            }
        }

        public DateTime Date
        {
            get
            {
                return _date;
            }

            set
            {
                _date = value;
            }
        }

        public string Numb
        {
            get
            {
                return _numb;
            }

            set
            {
                _numb = value;
            }
        }
    }



    /// <summary>
    /// 显示Pl5模型类
    /// </summary>
    public class ShowLtrPl5Model
    {
        /// <summary>
        /// 期号
        /// </summary>
        public int Issue { get; set; }

        /// <summary>
        /// 合数
        /// </summary>
        public int Sum { get; set; }

        /// <summary>
        /// 号码
        /// </summary>
        public string Numb { get; set; }

        /// <summary>
        /// 日期
        /// </summary>
        public DateTime Date { get; set; }
    }
}
