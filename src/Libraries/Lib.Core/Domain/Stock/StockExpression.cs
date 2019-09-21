using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lib.Core
{
    public class StockExpression
    {
    }

    /// <summary>
    /// 嵌套的函数或表达式
    /// </summary>
    public class StockExpressionModel
    {
        /// <summary>
        /// 是否函数
        /// 否则为表达式
        /// </summary>
        private bool _isFunction;

        /// <summary>
        /// 函数名称或表达式(+-*/><等)
        /// </summary>
        private string _operate;

        /// <summary>
        /// 参数
        /// </summary>
        private List<StockExpressionEntity> _params;

        public bool IsFunction
        {
            get
            {
                return _isFunction;
            }

            set
            {
                _isFunction = value;
            }
        }

        public string Operate
        {
            get
            {
                return _operate;
            }

            set
            {
                _operate = value;
            }
        }

        public List<StockExpressionEntity> Params
        {
            get
            {
                return _params;
            }

            set
            {
                _params = value;
            }
        }
    }

    /// <summary>
    /// 最基本的函数或表达式
    /// </summary>
    public class StockExpressionEntity
    {
        /// <summary>
        /// 是否函数
        /// 否则为表达式
        /// </summary>
        private bool _isFunction;

        /// <summary>
        /// 函数名称或表达式(+-*/><等)
        /// </summary>
        private string _operate;

        /// <summary>
        /// 参数
        /// </summary>
        private List<string> _params;

        public bool IsFunction
        {
            get
            {
                return _isFunction;
            }

            set
            {
                _isFunction = value;
            }
        }

        public string Operate
        {
            get
            {
                return _operate;
            }

            set
            {
                _operate = value;
            }
        }

        public List<string> Params
        {
            get
            {
                return _params;
            }

            set
            {
                _params = value;
            }
        }
    }
}
