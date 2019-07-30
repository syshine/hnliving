using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PubComp.NoSql.Core
{
    public class InvalidOperationFailure : Failure
    {
        public InvalidOperationFailure(String message = null, Exception innerException = null)
            : base(message, innerException)
        {
        }
    }
}
