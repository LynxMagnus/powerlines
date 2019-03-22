using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PowerLines.BulkInsert.Maps
{
    public static class DatasetSqlMap
    {
        public static List<string> ColumnMapping()
        {
            return new List<string>
            {
                "ResultId,ResultId",
                "Division,Division",
                "Date,Date",
                "HomeSuperiority,HomeSuperiority",
                "AwaySuperiority,AwaySuperiority",
                "Actual,Actual",
                "HomeTeam,HomeTeam",
                "AwayTeam,AwayTeam",
                "HomeGoals,HomeGoals",
                "AwayGoals,AwayGoals"
            };
        }
    }
}