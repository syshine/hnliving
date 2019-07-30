using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PubComp.NoSql.Core
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = true)]
    public class KnownDataTypesAttribute : Attribute
    {
        public KnownDataTypesAttribute(params Type[] types)
        {
            this.Types = types;
        }

        public Type[] Types
        {
            private set;
            get;
        }
    }
}
