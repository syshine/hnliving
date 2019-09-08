using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lib.Core
{
    public class StockEntity
    {
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

    public class StockPickEntity
    {
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
    }
}
