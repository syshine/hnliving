using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PubComp.NoSql.Core
{
    public class DalNullIdFailure : DalFailure
    {
        public DalNullIdFailure(
            String message = null, IEntity entity = null, DalOperation operation = DalOperation.Undefined, Exception innerException = null)
            : base(message, entity, operation, innerException)
        {
        }
    }
}
