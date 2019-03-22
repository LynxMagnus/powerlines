using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace PowerLines.Models
{
    public class Result
    {
        public int ResultId { get; set; }

        public string Division { get; set; }
        
        public DateTime Date { get; set; }

        public string HomeTeam { get; set; }

        public string AwayTeam { get; set; }

        public int FullTimeHomeGoals { get; set; }

        public int FullTimeAwayGoals { get; set; }

        public string FullTimeResult { get; set; }

        public int HalfTimeHomeGoals { get; set; }

        public int HalfTimeAwayGoals { get; set; }

        public string HalfTimeResult { get; set; }

        public decimal HomeOddsAverage { get; set; }

        public decimal DrawOddsAverage { get; set; }

        public decimal AwayOddsAverage { get; set; }
    }
}