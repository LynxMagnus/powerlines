using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PowerLines.Models
{
    public class Tracker
    {        
        public int TrackerId { get; set; }

        public string UserId { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime Date { get; set; }

        public string Division { get; set; }

        public string HomeTeam { get; set; }

        public string AwayTeam { get; set; }

        [Display(Name = "Balance")]
        public decimal Bank { get; set; }

        public decimal Reducer { get; set; }

        public string Predicted { get; set; }

        [Display(Name = "Market Odds")]
        public decimal MarketOdds { get; set; }
                
        [DisplayFormat(DataFormatString = "{0:n2}")]
        public decimal Odds { get; set; }

        public string Description
        {
            get
            {
                switch (Predicted)
                {
                    case "H":
                        return HomeTeam;
                    case "D":
                        return "Draw";
                    case "A":
                        return AwayTeam;
                    default:
                        return "";
                }
            }
        }

        [Display(Name = "Market %")]
        [DisplayFormat(DataFormatString = "{0:n2}")]
        public decimal MarketPercent
        {
            get
            {
                return MarketOdds == 0 ? 0 : decimal.Divide(1, MarketOdds);
            }
        }

        [Display(Name = "PL %")]
        [DisplayFormat(DataFormatString = "{0:n2}")]
        public decimal Percent
        {
            get
            {
                return Odds == 0 ? 0 : decimal.Divide(1, Odds);
            }
        }
        
        [DisplayFormat(DataFormatString = "{0:n2}")]
        public decimal Margin
        {
            get
            {
                return Percent - MarketPercent;
            }
        }

        public bool ValidMargin
        {
            get
            {
                decimal validMargin = Predicted == "D" ? 0.01M : 0.05M;

                return Margin >= validMargin ? true : false;
            }
        }

        [Display(Name = "Stake")]
        [DisplayFormat(DataFormatString = "{0:n2}")]
        public decimal StakePercent
        {
            get
            {
                return MarketOdds == 0 || Odds == 0 || Reducer == 0 ? 0 :
                    ((MarketOdds * (decimal.Divide(1, Odds)) - 1) / (MarketOdds - 1)) * (Reducer / 100);
            }
        }

        [Display(Name = "Stake")]
        [DisplayFormat(DataFormatString = "{0:n2}")]
        public decimal Stake
        {
            get
            {
                return StakePercent == 0 ? 0 : Math.Round(Bank * StakePercent, 2);
            }
        }

        public decimal Potential
        {
            get
            {
                return Stake == 0 ? 0 : Math.Round(Stake * MarketOdds, 2);
            }
        }

        public decimal Profit
        {
            get
            {
                return Potential == 0 ? 0 : Potential - Stake;
            }
        }

        [Display(Name = "ROI")]
        [DisplayFormat(DataFormatString = "{0:n2}")]
        public decimal ReturnOnInvestment
        {
            get
            {
                return Profit == 0 ? 0 : decimal.Divide(Profit, Stake) * 100;
            }
        }

        public decimal Remaining
        {
            get
            {
                return Stake == 0 ? Bank : Bank - Stake;
            }
        }

        public string Result { get; set; }

        [Display(Name = "Profit/Loss")]
        public decimal ProfitLoss
        {
            get
            {
                return Result == null ? 0 : Result == Predicted ? Profit : Stake * -1;
            }
        }

        public bool IsComplete
        {
            get
            {
                return Result != null;
            }
        }

        public bool IsWin
        {
            get
            {
                return IsComplete && Result == Predicted;
            }
        }

        public bool IsToday
        {
            get
            {
                return Date.Date == DateTime.Now.Date; 
            }
        }

        public Tracker()
        {
        }

        public Tracker(Fixture fixture, string outcome, decimal marketOdds, decimal odds, decimal balance, decimal reducer, string userId)
        {
            Date = fixture.Date;
            Division = fixture.Division;
            HomeTeam = fixture.HomeTeam;
            AwayTeam = fixture.AwayTeam;
            Predicted = outcome;
            MarketOdds = marketOdds;
            Odds = odds;
            Bank = balance;
            Reducer = reducer;
            UserId = userId;
        }

        public Tracker(string outcome, decimal marketOdds, decimal odds, decimal balance, decimal reducer)
        {
            Predicted = outcome;
            MarketOdds = marketOdds;
            Odds = odds;
            Bank = balance;
            Reducer = reducer;
        }
    }
}