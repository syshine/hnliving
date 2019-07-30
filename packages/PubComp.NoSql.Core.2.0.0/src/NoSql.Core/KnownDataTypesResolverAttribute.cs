using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace PubComp.NoSql.Core
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = true)]
    public class KnownDataTypesResolverAttribute : Attribute
    {
        public KnownDataTypesResolverAttribute(params Type[] typesToUseForSearchingAssemblies)
        {
            this.TypesToUseForSearchingAssemblies = typesToUseForSearchingAssemblies;
        }

        public Type[] TypesToUseForSearchingAssemblies
        {
            private set;
            get;
        }
    }
}
