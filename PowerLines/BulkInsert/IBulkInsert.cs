using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerLines.BulkInsert
{
    public interface IBulkInsert<T> where T : class
    {
        void Insert(List<T> obj, string table, List<string> columnMappings = null);
    }
}
