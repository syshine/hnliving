using System;

namespace Lib.Core
{
    public class UnselectException : Exception
    {
        /// <summary>
        /// 不选的原因
        /// </summary>
        private string _reason;

        public string Reason
        {
            get
            {
                return _reason;
            }

            set
            {
                _reason = value;
            }
        }

        //无参数构造函数
        public UnselectException() : base()
        {

        }

        //带参数构造函数
        public UnselectException(string message, string reason = null) : base(message)
        {
            _reason = reason;
        }
    }
}
