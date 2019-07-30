using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PubComp.NoSql.Core
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true, Inherited = true)]
    public class NavigationAttribute : Attribute
    {
        public NavigationAttribute(string idPropertyName, Type entitySetItemType)
        {
            this.IdPropertyName = idPropertyName;
            this.EntitySetItemType = entitySetItemType;
        }

        public string IdPropertyName
        {
            private set;
            get;
        }

        public Type EntitySetItemType
        {
            private set;
            get;
        }
    }
}
