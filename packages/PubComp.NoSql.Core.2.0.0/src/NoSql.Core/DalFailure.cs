using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PubComp.NoSql.Core
{
    public class DalFailure : Failure
    {
        public IEntity Entity
        {
            private set;
            get;
        }

        public DalOperation Operation
        {
            private set;
            get;
        }

        public DalFailure(
            String message = null, IEntity entity = null, DalOperation operation = DalOperation.Undefined, Exception innerException = null)
            : base(message, innerException)
        {
            this.Entity = entity;
            this.Operation = operation;
        }
    }

    public enum DalOperation
    {
        Undefined,
        Get,
        Add,
        Update,
        Delete
    }
}
