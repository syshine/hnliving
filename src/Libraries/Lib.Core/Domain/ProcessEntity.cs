using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lib.Core
{
    [Serializable]
    public class ProcessEntity
    {
        private int _totalCount;    // 总数量
        private int _doneCount;     // 已完成数量
        private int _present;       // 百分比
        private string _text;       // 文本
        private DateTime _startTime;// 已执行时间
        private string _execTime;   // 已执行时间

        /// <summary>
        /// 格式
        ///  包含{2}则文本包含已完成数量、总数量和百分比，否则只显示百分比
        /// </summary>
        private string _format;

        #region 构造函数
        public ProcessEntity()
        {
            StartTime = DateTime.Now;
            _execTime = "";
            _format = "{0}%({1}/{2})";
            TotalCount = int.MaxValue;
            DoneCount = 0;
        }

        public ProcessEntity(int totalCount)
        {
            StartTime = DateTime.Now;
            _execTime = "";
            _format = "{0}%({1}/{2})";
            TotalCount = totalCount;
            DoneCount = 0;
        }

        public ProcessEntity(int totalCount, int doneCount)
        {
            StartTime = DateTime.Now;
            _execTime = "";
            _format = "{0}%({1}/{2})";
            TotalCount = totalCount;
            DoneCount = doneCount;
        }
        public ProcessEntity(int totalCount, int doneCount, string format)
        {
            StartTime = DateTime.Now;
            _format = format;
            TotalCount = totalCount;
            DoneCount = doneCount;
        }

        public ProcessEntity(int totalCount, int doneCount, DateTime startTime)
        {
            StartTime = startTime;
            _format = "{0}%({1}/{2})";
            TotalCount = totalCount;
            DoneCount = doneCount;
        }

        public ProcessEntity(int totalCount, int doneCount, DateTime startTime, string format)
        {
            StartTime = startTime;
            _format = format;
            TotalCount = totalCount;
            DoneCount = doneCount;
        }
        #endregion

        public int TotalCount
        {
            get
            {
                return _totalCount;
            }

            set
            {
                _totalCount = value;
            }
        }

        public int DoneCount
        {
            get
            {
                return _doneCount;
            }

            set
            {
                _doneCount = value;
                Present = (int)(((_doneCount * 1.0) / _totalCount) * 100);
            }
        }

        public int Present
        {
            get
            {
                return _present;
            }

            set
            {
                if (value < 0)
                    _present = 0;
                else if (value > 100)
                    _present = 100;
                else
                    _present = value;

                // 包含{3}则文本包含已完成数量、总数量、百分比和执行时间
                // 包含{2}则文本包含已完成数量、总数量和百分比，否则只显示百分比
                if (_format.Contains("{3}"))
                    _text = string.Format(_format, _present, _doneCount, _totalCount, _execTime);
                else if (_format.Contains("{2}"))
                    _text = string.Format(_format, _present, _doneCount, _totalCount);
                else
                    _text = string.Format(_format, _present);
            }
        }

        public string Text
        {
            get
            {
                return _text;
            }

            set
            {
                _text = value;
            }
        }

        public string Format
        {
            get
            {
                return _format;
            }

            set
            {
                _format = value;

                // 包含{3}则文本包含已完成数量、总数量、百分比和执行时间
                // 包含{2}则文本包含已完成数量、总数量和百分比，否则只显示百分比
                if (_format.Contains("{3}"))
                    _text = string.Format(_format, _present, _doneCount, _totalCount, _execTime);
                else if (_format.Contains("{2}"))
                    _text = string.Format(_format, _present, _doneCount, _totalCount);
                else
                    _text = string.Format(_format, _present);
            }
        }

        public DateTime StartTime
        {
            get
            {
                return _startTime;
            }

            set
            {
                _startTime = value;

                // 执行时间
                TimeSpan ts = DateTime.Now - _startTime;
                _execTime = "";
                if (ts.Days > 0)
                    _execTime += ts.Days + "天";
                if (ts.Hours > 0)
                    _execTime += ts.Hours + "小时";
                if (ts.Minutes > 0)
                    _execTime += ts.Minutes + "分";
                if (ts.Seconds > 0)
                    _execTime += ts.Seconds + "秒";
                _execTime += ts.Milliseconds + "毫秒";
            }
        }

        public string ExecTime
        {
            get
            {
                return _execTime;
            }

            set
            {
                _execTime = value;
            }
        }
    }
}
