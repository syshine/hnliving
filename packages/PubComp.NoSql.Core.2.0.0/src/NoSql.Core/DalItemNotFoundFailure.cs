using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PubComp.NoSql.Core
{
    public class DalItemNotFoundFailure : DalFailure
    {
        public DalItemNotFoundFailure(
            String message = null, IEntity entity = null, DalOperation operation = DalOperation.Undefined, Exception innerException = null)
            : base(message, entity, operation, innerException)
        {
        }
    }
}
