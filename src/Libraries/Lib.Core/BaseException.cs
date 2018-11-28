using System;
using System.Runtime.Serialization;

namespace Lib.Core
{
    /// <summary>
    /// HNLiving“Ï≥£¿‡
    /// </summary>
    [Serializable]
    public class BaseException : ApplicationException
    {
        public BaseException() { }

        public BaseException(string message) : base(message) { }

        public BaseException(string message, Exception inner) : base(message, inner) { }

        protected BaseException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
}
