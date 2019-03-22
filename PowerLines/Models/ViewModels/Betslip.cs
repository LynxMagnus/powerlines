using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PowerLines.Models.ViewModels
{
    public class Betslip
    {
        public Fixture Fixture { get; set; }

        public string Predicted { get; set; }

        [Display(Name = "Market Odds")]
        public decimal MarketOdds { get; set; }
                
        public decimal Odds { get; set; }

        public string Description
        {
            get
            {
                switch (Predicted)
                {
                    case "H":
                        return Fixture.HomeTeam;
                    case "D":
                        return "Draw";
                    case "A":
                        return Fixture.AwayTeam;
                    default:
                        return "";
                }
            }
        }

        public Betslip()
        {
        }

        public Betslip(Fixture fixture, string predicted, decimal odds)
        {
            Fixture = fixture;
            Predicted = predicted;
            Odds = odds;
        }
    }
}