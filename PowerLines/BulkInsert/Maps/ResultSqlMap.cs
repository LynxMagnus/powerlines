using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PowerLines.BulkInsert.Maps
{
    public class ResultSqlMap
    {
        public static List<string> ColumnMapping()
        {
            return new List<string>
            {
                "ResultId,ResultId",
                "Division,Division",
                "Date,Date",
                "HomeTeam,HomeTeam",
                "AwayTeam,AwayTeam",
                "FullTimeHomeGoals,FullTimeHomeGoals",
                "FullTimeAwayGoals,FullTimeAwayGoals",
                "FullTimeResult,FullTimeResult",
                "HalfTimeHomeGoals,HalfTimeHomeGoals",
                "HalfTimeAwayGoals,HalfTimeAwayGoals",
                "HalfTimeResult,HalfTimeResult",
                "HomeOddsAverage,HomeOddsAverage",
                "DrawOddsAverage,DrawOddsAverage",
                "AwayOddsAverage,AwayOddsAverage"
            };
        }
    }
}