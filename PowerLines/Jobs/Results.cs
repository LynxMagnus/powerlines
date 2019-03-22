using PowerLines.BulkInsert;
using PowerLines.DAL;
using PowerLines.Inbound;
using PowerLines.Models;
using PowerLines.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PowerLines.Jobs
{
    public static class Results
    {
        public static void Update()
        {
            using (var db = new PowerLinesContext())
            {
                var resultService = new ResultService(db, new ResultReader(), new RatingService(db, new SqlBulkInsert<Dataset>(db)), new SqlBulkInsert<Result>(db));
                resultService.Upload(true);                
            }
        }
    }
}