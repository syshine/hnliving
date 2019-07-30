using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PubComp.NoSql.Core
{
    public interface IFileSet
    {
        Type KeyType
        {
            get;
        }
    }

    public interface IFileSet<TKey> : IFileSet
    {
        void Store(Stream inputStream, string fileName, TKey id);

        void Retreive(Stream outputStream, TKey id);

        void Store(string inputFilePath, string fileName, TKey id);

        void Retreive(string outputFilePath, TKey id);

        void Delete(TKey id);
    }
}
