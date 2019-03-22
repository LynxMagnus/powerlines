using PowerLines.DAL;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using FastMember;
using System.Web;

namespace PowerLines.BulkInsert
{
    public class SqlBulkInsert<T> : IBulkInsert<T>, IDisposable where T : class
    {
        PowerLinesContext db;
        string connStr;

        public SqlBulkInsert(PowerLinesContext context)
        {
            db = context;
            connStr = ConfigurationManager.ConnectionStrings["PowerLinesContext"].ConnectionString;
        }

        public object ObjectReader { get; private set; }

        public void Insert(List<T> obj, string table, List<string> columnMappings = null)
        {
            using (var bcp = new SqlBulkCopy(connStr))
            {
                using (var reader = FastMember.ObjectReader.Create(obj))
                {
                    if(columnMappings != null)
                    {
                        foreach(var mapping in columnMappings)
                        {
                            var split = mapping.Split(new[] { ',' });
                            bcp.ColumnMappings.Add(split.First(), split.Last());
                        }
                    }

                    bcp.DestinationTableName = table;
                    bcp.BatchSize = 10000;
                    bcp.BulkCopyTimeout = 60;
                    bcp.WriteToServer(reader);
                }
            }
        }

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if(!disposed)
            {
                if(disposing)
                {
                    db.Dispose();
                }

                disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
        }
    }
}