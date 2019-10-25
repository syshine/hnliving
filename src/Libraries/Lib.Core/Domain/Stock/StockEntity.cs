using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lib.Core
{
    [Serializable]
    public class StockEntity
    {
        /// <summary>
        /// 股票代码
        /// </summary>
        private string _code;

        /// <summary>
        /// 股票名称
        /// </summary>
        private string _name;

        /// <summary>
        /// 股票市场（1为沪市，2为深市）
        /// </summary>
        private int _type;

        /// <summary>
        /// 股票价格
        /// </summary>
        private decimal _price;

        public StockEntity()
        {
        }

        public StockEntity(string _code, string _name)
        {
            this._code = _code;
            this._name = _name;
        }

        public StockEntity(string _code, string _name, int _type, decimal _price)
        {
            this._code = _code;
            this._name = _name;
            this._type = _type;
            this._price = _price;
        }

        public string Code
        {
            get
            {
                return _code;
            }

            set
            {
                _code = value;
            }
        }

        public string Name
        {
            get
            {
                return _name;
            }

            set
            {
                _name = value;
            }
        }

        public int Type
        {
            get
            {
                return _type;
            }

            set
            {
                _type = value;
            }
        }

        public decimal Price
        {
            get
            {
                return _price;
            }

            set
            {
                _price = value;
            }
        }
    }

    /// <summary>
    /// 简单的代码名称类
    /// </summary>
    [Serializable]
    public class StockCodeName
    {
        /// <summary>
        /// 股票代码
        /// </summary>
        private string _code;

        /// <summary>
        /// 股票名称
        /// </summary>
        private string _name;

        public StockCodeName()
        {
        }

        public StockCodeName(string _code, string _name)
        {
            this._code = _code;
            this._name = _name;
        }

        public string Code
        {
            get
            {
                return _code;
            }

            set
            {
                _code = value;
            }
        }

        public string Name
        {
            get
            {
                return _name;
            }

            set
            {
                _name = value;
            }
        }
    }

    [Serializable]
    public class StockShow
    {
        /// <summary>
        /// 股票代码
        /// </summary>
        private string _code;

        /// <summary>
        /// 股票名称
        /// </summary>
        private string _name;

        /// <summary>
        /// 股票市场（1为沪市，2为深市）
        /// </summary>
        private int _type = 1;

        /// <summary>
        /// 价格
        /// </summary>
        private decimal _price = 0m;

        /// <summary>
        /// 涨跌幅
        /// </summary>
        private string _pCHG;

        /// <summary>
        /// 查看股票地址
        /// </summary>
        private string _url = "";

        public StockShow()
        {
        }

        public StockShow(string _code, string _name)
        {
            this._code = _code;
            this._name = _name;
        }

        public StockShow(string _code, string _name, int _type, decimal _price, string _pCHG, string _url = "")
        {
            this._code = _code;
            this._name = _name;
            this._type = _type;
            this._price = _price;

            // 涨跌幅
            decimal pchg = 0m;
            if (decimal.TryParse(_pCHG, out pchg))
            {
                this._pCHG = (pchg / 100).ToString("P");
            }
            else
            {
                this._pCHG = _pCHG;
            }

            // url地址
            if(string.IsNullOrWhiteSpace(_url))
            {
                if(_type == 1) // 沪市
                {
                    this._url = string.Format(@"http://quote.eastmoney.com/{0}{1}.html", "sh", _code); // 东方财富
                    //this._url = string.Format(@"https://finance.sina.com.cn/realstock/company/{0}{1}/nc.shtml", "sh", _code); // 新浪财经
                }
                else // 深市
                {
                    this._url = string.Format(@"http://quote.eastmoney.com/{0}{1}.html", "sz", _code);
                    //this._url = string.Format(@"https://finance.sina.com.cn/realstock/company/{0}{1}/nc.shtml", "sz", _code); // 新浪财经
                }
            }
            else
                this._url = _url;
        }

        public string Code
        {
            get
            {
                return _code;
            }

            set
            {
                _code = value;
            }
        }

        public string Name
        {
            get
            {
                return _name;
            }

            set
            {
                _name = value;
            }
        }

        public int Type
        {
            get
            {
                return _type;
            }

            set
            {
                _type = value;
            }
        }

        public decimal Price
        {
            get
            {
                return _price;
            }

            set
            {
                _price = value;
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

        public string Url
        {
            get
            {
                return _url;
            }

            set
            {
                _url = value;
            }
        }
    }

    public class StockPickEntity
    {
        /// <summary>
        /// 公式ID
        /// </summary>
        private int _fid;

        /// <summary>
        /// 用户ID
        /// </summary>
        private int _uid;

        /// <summary>
        /// 分组ID
        /// </summary>
        private int _groupid;

        /// <summary>
        /// 名称
        /// </summary>
        private string _fname;

        /// <summary>
        /// 创建时间
        /// </summary>
        private DateTime _createTime;

        /// <summary>
        /// 更新时间
        /// </summary>
        private DateTime _updateTime;

        /// <summary>
        /// 使用前复权数据
        /// </summary>
        private bool _useFormerComplexRights;

        /// <summary>
        /// 使用涨跌幅
        /// </summary>
        private bool _pCHGEnable;

        /// <summary>
        /// 连续N日
        /// </summary>
        private int _days;

        /// <summary>
        /// 是否涨
        /// </summary>
        private bool _isRise;

        /// <summary>
        /// 操作符
        /// </summary>
        private string _operator;

        /// <summary>
        /// 涨跌幅
        /// </summary>
        private decimal _pCHG;

        /// <summary>
        /// 使用价格区间
        /// </summary>
        private bool _priceEnable;

        /// <summary>
        /// 最低价
        /// </summary>
        private decimal _priceLow;

        /// <summary>
        /// 最高价
        /// </summary>
        private decimal _priceHigh;

        /// <summary>
        /// 使用价格区间
        /// </summary>
        private bool _mCAPEnable;

        /// <summary>
        /// 最低流通市值(亿)
        /// </summary>
        private decimal _mCAPLow;
        
        /// <summary>
        /// 最高流通市值(亿)
        /// </summary>
        private decimal _mCAPHigh;

        /// <summary>
        /// 使用公式
        /// </summary>
        private bool _formulaEnable;

        /// <summary>
        /// 预处理哪几天均线
        /// </summary>
        private string _preAvgLines;

        /// <summary>
        /// 预处理均线天数
        /// </summary>
        private int _preDays;

        /// <summary>
        /// 公式
        /// </summary>
        private string _formula;


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

        public int Groupid
        {
            get
            {
                return _groupid;
            }

            set
            {
                _groupid = value;
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

        public DateTime CreateTime
        {
            get
            {
                return _createTime;
            }

            set
            {
                _createTime = value;
            }
        }

        public DateTime UpdateTime
        {
            get
            {
                return _updateTime;
            }

            set
            {
                _updateTime = value;
            }
        }

        public bool UseFormerComplexRights
        {
            get
            {
                return _useFormerComplexRights;
            }

            set
            {
                _useFormerComplexRights = value;
            }
        }

        public bool PCHGEnable
        {
            get
            {
                return _pCHGEnable;
            }

            set
            {
                _pCHGEnable = value;
            }
        }

        public int Days
        {
            get
            {
                return _days;
            }

            set
            {
                _days = value;
            }
        }

        public bool IsRise
        {
            get
            {
                return _isRise;
            }

            set
            {
                _isRise = value;
            }
        }

        public string Operator
        {
            get
            {
                return _operator;
            }

            set
            {
                _operator = value;
            }
        }

        public decimal PCHG
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

        public bool PriceEnable
        {
            get
            {
                return _priceEnable;
            }

            set
            {
                _priceEnable = value;
            }
        }

        public decimal PriceLow
        {
            get
            {
                return _priceLow;
            }

            set
            {
                _priceLow = value;
            }
        }

        public decimal PriceHigh
        {
            get
            {
                return _priceHigh;
            }

            set
            {
                _priceHigh = value;
            }
        }

        public bool MCAPEnable
        {
            get
            {
                return _mCAPEnable;
            }

            set
            {
                _mCAPEnable = value;
            }
        }

        public decimal MCAPLow
        {
            get
            {
                return _mCAPLow;
            }

            set
            {
                _mCAPLow = value;
            }
        }

        public decimal MCAPHigh
        {
            get
            {
                return _mCAPHigh;
            }

            set
            {
                _mCAPHigh = value;
            }
        }

        public bool FormulaEnable
        {
            get
            {
                return _formulaEnable;
            }

            set
            {
                _formulaEnable = value;
            }
        }

        public string PreAvgLines
        {
            get
            {
                return _preAvgLines;
            }

            set
            {
                _preAvgLines = value;
            }
        }

        public int PreDays
        {
            get
            {
                return _preDays;
            }

            set
            {
                _preDays = value;
            }
        }

        public string Formula
        {
            get
            {
                return _formula;
            }

            set
            {
                _formula = value;
            }
        }
    }
}
