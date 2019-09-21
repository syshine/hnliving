using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lib.Core
{
    public class FormulaException : Exception
    {
        private int _errorRow;
        private string _errorFormula;

        public int ErrorRow
        {
            get
            {
                return _errorRow;
            }

            set
            {
                _errorRow = value;
            }
        }

        public string ErrorFormula
        {
            get
            {
                return _errorFormula;
            }

            set
            {
                _errorFormula = value;
            }
        }

        //无参数构造函数
        public FormulaException() : base()
        {
    
        }

        //带参数构造函数
        public FormulaException(string message, int row, string formula = null) : base(message)
        {
            _errorRow = row;
            _errorFormula = formula;
        }
    }
}
