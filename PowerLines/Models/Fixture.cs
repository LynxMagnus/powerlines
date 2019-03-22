using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace PowerLines.Models
{
    public class Fixture
    {
        public int FixtureId { get; set; }

        public string Division { get; set; }

        [Display(Name = "Division")]
        public string DivisionName
        {
            get
            {
                switch (Division)
                {
                    case "E0":
                        return "England - Premier League";
                    case "E1":
                        return "England - Championship";
                    case "E2":
                        return "England - League 1";
                    case "E3":
                        return "England - League 2";
                    case "EC":
                        return "England - Conference";
                    case "B1":
                        return "Belgium - Pro League";
                    case "D1":
                        return "Germany - Bundesliga";
                    case "D2":
                        return "Germany - 2. Bundesliga";
                    case "F1":
                        return "France - Ligue 1";
                    case "F2":
                        return "France - Ligue 2";
                    case "G1":
                        return "Greece - Super League";
                    case "I1":
                        return "Italy - Serie A";
                    case "I2":
                        return "Italy - Serie B";
                    case "N1":
                        return "Netherlands - Eredivisie";
                    case "P1":
                        return "Portugal - Primeira Liga";
                    case "SC0":
                        return "Scotland - Premiership";
                    case "SC1":
                        return "Scotland - Championship";
                    case "SC2":
                        return "Scotland - League 1";
                    case "SC3":
                        return "Scotland - League 2";
                    case "SP1":
                        return "Spain - La Liga";
                    case "SP2":
                        return "Spain - Segunda Division";
                    case "T1":
                        return "Turkey - Süper Lig";
                    default:
                        return "Unknown";
                }
            }
        }

        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime Date { get; set; }

        [Display(Name = "Home")]
        public string HomeTeam { get; set; }

        [Display(Name = "Away")]
        public string AwayTeam { get; set; }

        [NotMapped]
        public string Actual { get; set; }

        [NotMapped]
        public string ActualBTTS { get; set; }

        [NotMapped]
        public string ActualTwoGoals { get; set; }

        [NotMapped]
        public string ActualThreeGoals { get; set; }

        public decimal HomeOddsAverage { get; set; }

        public decimal DrawOddsAverage { get; set; }

        public decimal AwayOddsAverage { get; set; }        
    }
}