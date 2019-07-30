using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PubComp.NoSql.Core
{
    public class Failure : ApplicationException
    {
        public Failure()
            : base()
        {
        }

        public Failure(String message)
            : base(message)
        {
        }

        public Failure(String message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
