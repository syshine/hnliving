using System;
using System.Runtime.Serialization;

namespace Lib.Core
{
    /// <summary>
    /// hnliving���ݿ��쳣
    /// </summary>
    [Serializable]
    public class DbException : BaseException
    {
        public DbException() : base() { }

        public DbException(string message) : base(message) { }

        public DbException(string message, Exception inner) : base(message, inner) { }

        public DbException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
}
