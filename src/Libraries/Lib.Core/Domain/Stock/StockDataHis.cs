using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lib.Core
{
    public class StockDataHis
    {
        /// <summary>
        /// 属性名称大小写必须跟数据库的字段名称一模一样(包括大小写)
        /// </summary>
        private int _hID;
        private string _sCODE;      // 股票代码
        private string _sNAME;      // 股票名称
        private DateTime _hDATE;    // 日期
        private string _tCLOSE;     // 收盘价
        private string _hIGH;       // 最高价
        private string _lOW;        // 最低价
        private string _tOPEN;      // 开盘价
        private string _lCLOSE;     // 前收盘
        private string _cHG;        // 涨跌额
        private string _pCHG;       // 涨跌幅
        private string _tURNOVER;   // 换手率
        private string _vOTURNOVER; // 成交量
        private string _vATURNOVER; // 成交金额
        private string _tCAP;       // 总市值
        private string _mCAP;       // 流通市值

        public StockDataHis(string text)
        {
            string[] values = text.Split(',');
            _hDATE = DateTime.Parse(values[0]);
            _sCODE = values[1].Trim('\'');
            _sNAME = values[2];
            _tCLOSE = values[3];
            _hIGH = values[4];
            _lOW = values[5];
            _tOPEN = values[6];
            _lCLOSE = values[7];
            _cHG = values[8];
            _pCHG = values[9];
            _tURNOVER = values[10];

            // 将科学计数法转成真实数字再保存
            double temp = 0;
            if(double.TryParse(values[11], out temp))
                _vOTURNOVER = temp.ToString();
            else
                _vOTURNOVER = values[11];

            if (double.TryParse(values[12], out temp))
                _vATURNOVER = temp.ToString();
            else
                _vATURNOVER = values[12];

            if (double.TryParse(values[13], out temp))
                _tCAP = temp.ToString();
            else
                _tCAP = values[13];

            if (double.TryParse(values[14], out temp))
                _mCAP = temp.ToString();
            else
                _mCAP = values[14];
        }

        public int HID
        {
            get
            {
                return _hID;
            }

            set
            {
                _hID = value;
            }
        }

        public string SCODE
        {
            get
            {
                return _sCODE;
            }

            set
            {
                _sCODE = value;
            }
        }

        public string SNAME
        {
            get
            {
                return _sNAME;
            }

            set
            {
                _sNAME = value;
            }
        }

        public DateTime HDATE
        {
            get
            {
                return _hDATE;
            }

            set
            {
                _hDATE = value;
            }
        }

        public string TCLOSE
        {
            get
            {
                return _tCLOSE;
            }

            set
            {
                _tCLOSE = value;
            }
        }

        public string HIGH
        {
            get
            {
                return _hIGH;
            }

            set
            {
                _hIGH = value;
            }
        }

        public string LOW
        {
            get
            {
                return _lOW;
            }

            set
            {
                _lOW = value;
            }
        }

        public string TOPEN
        {
            get
            {
                return _tOPEN;
            }

            set
            {
                _tOPEN = value;
            }
        }

        public string LCLOSE
        {
            get
            {
                return _lCLOSE;
            }

            set
            {
                _lCLOSE = value;
            }
        }

        public string CHG
        {
            get
            {
                return _cHG;
            }

            set
            {
                _cHG = value;
            }
        }

        public string PCHG
        {
            get
            {
                return _pCHG;
            }

            set
            {
                _pCHG = value;
            }
        }

        public string TURNOVER
        {
            get
            {
                return _tURNOVER;
            }

            set
            {
                _tURNOVER = value;
            }
        }

        public string VOTURNOVER
        {
            get
            {
                return _vOTURNOVER;
            }

            set
            {
                _vOTURNOVER = value;
            }
        }

        public string VATURNOVER
        {
            get
            {
                return _vATURNOVER;
            }

            set
            {
                _vATURNOVER = value;
            }
        }

        public string TCAP
        {
            get
            {
                return _tCAP;
            }

            set
            {
                _tCAP = value;
            }
        }

        public string MCAP
        {
            get
            {
                return _mCAP;
            }

            set
            {
                _mCAP = value;
            }
        }
    }
}
