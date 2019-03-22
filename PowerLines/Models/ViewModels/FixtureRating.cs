using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PowerLines.Models.ViewModels
{
    public class FixtureRating
    {
        public Fixture Fixture { get; set; }

        public int HomeSuperiority { get; set; }

        public int AwaySuperiority { get; set; }

        public int MatchRating
        {
            get
            {
                return HomeSuperiority - AwaySuperiority;
            }
        }

        

        public decimal HomePercentAverage
        {
            get
            {
                return Fixture.HomeOddsAverage == 0 ? 0 : decimal.Divide(1, Fixture.HomeOddsAverage);
            }
        }

        public decimal DrawPercentAverage
        {
            get
            {
                return Fixture.DrawOddsAverage == 0 ? 0 : decimal.Divide(1, Fixture.DrawOddsAverage);
            }
        }

        public decimal AwayPercentAverage
        {
            get
            {
                return Fixture.AwayOddsAverage == 0 ? 0 : decimal.Divide(1, Fixture.AwayOddsAverage);
            }
        }

        public decimal HomePercent { get; set; }

        public decimal DrawPercent { get; set; }

        public decimal AwayPercent { get; set; }

        private decimal threshold = 0.55M;

        private decimal lowerThreshold = 0.5M;

        private decimal thresholdBTTS = 0.59M;

        private decimal lowerThresholdBTTS = 0.54M;

        private decimal thresholdTwoGoals = 0.70M;

        private decimal lowerThresholdTwoGoals = 0.65M;

        private decimal thresholdThreeGoals = 0.56M;

        private decimal lowerThresholdThreeGoals = 0.51M;

        public decimal HomePercentWeighted
        {
            get
            {
                return (HomePercent * 0.9M) + HomePercentAverage;
            }
        }

        public decimal DrawPercentWeighted
        {
            get
            {
                return (DrawPercent * 0.9M) + DrawPercentAverage;
            }
        }

        public decimal AwayPercentWeighted
        {
            get
            {
                return (AwayPercent * 0.9M) + AwayPercentAverage;
            }
        }

        [Display(Name = "1")]
        [DisplayFormat(DataFormatString = "{0:n2}", ApplyFormatInEditMode = true)]
        public decimal HomeOdds
        {
            get
            {
                if (HomePercentAverage == 0)
                {
                    return HomePercent == 0 ? 0 : Math.Round(decimal.Divide(1, HomePercent),2);
                }

                return Math.Round(decimal.Divide(1, (decimal.Divide(HomePercentWeighted, (HomePercentWeighted + DrawPercentWeighted + AwayPercentWeighted)))),2);
            }
        }

        [Display(Name = "X")]
        [DisplayFormat(DataFormatString = "{0:n2}", ApplyFormatInEditMode = true)]
        public decimal DrawOdds
        {
            get
            {
                if (DrawPercentAverage == 0)
                {
                    return DrawPercent == 0 ? 0 : Math.Round(decimal.Divide(1, DrawPercent),2);
                }

                return Math.Round(decimal.Divide(1, (decimal.Divide(DrawPercentWeighted, (HomePercentWeighted + DrawPercentWeighted + AwayPercentWeighted)))),2);
            }
        }

        [Display(Name = "2")]
        [DisplayFormat(DataFormatString = "{0:n2}", ApplyFormatInEditMode = true)]
        public decimal AwayOdds
        {
            get
            {
                if (AwayPercentAverage == 0)
                {
                    return AwayPercent == 0 ? 0 : Math.Round(decimal.Divide(1, AwayPercent),2);
                }

                return Math.Round(decimal.Divide(1, (decimal.Divide(AwayPercentWeighted, (HomePercentWeighted + DrawPercentWeighted + AwayPercentWeighted)))),2);
            }
        }

        [DisplayFormat(DataFormatString = "{0:n2}", ApplyFormatInEditMode = true)]
        public decimal HomeValueOdds
        {
            get
            {
                return HomeOdds == 0 ? 0 : decimal.Divide(1, (decimal.Divide(1, HomeOdds)) - 0.05M);
            }
        }

        [DisplayFormat(DataFormatString = "{0:n2}", ApplyFormatInEditMode = true)]
        public decimal DrawValueOdds
        {
            get
            {
                return DrawOdds == 0 ? 0 : decimal.Divide(1, (decimal.Divide(1, DrawOdds)) - 0.01M);
            }
        }

        [DisplayFormat(DataFormatString = "{0:n2}", ApplyFormatInEditMode = true)]
        public decimal AwayValueOdds
        {
            get
            {
                return AwayOdds == 0 ? 0 : decimal.Divide(1, (decimal.Divide(1, AwayOdds)) - 0.05M);
            }
        }

        public string Predicted
        {
            get
            {
                if (HomeOdds > 0 && HomeOdds < DrawOdds && HomeOdds < AwayOdds)
                {
                    return "H";
                }
                if (DrawOdds > 0 && DrawOdds < HomeOdds && DrawOdds < AwayOdds)
                {
                    return "D";
                }
                if (AwayOdds > 0 && AwayOdds < HomeOdds && AwayOdds < DrawOdds)
                {
                    return "A";
                }
                return "X";
            }
        }

        public string PredictedBase
        {
            get
            {
                if (Fixture.HomeOddsAverage > 0 && Fixture.HomeOddsAverage < Fixture.DrawOddsAverage && Fixture.HomeOddsAverage < Fixture.AwayOddsAverage)
                {
                    return "H";
                }
                if (Fixture.DrawOddsAverage > 0 && Fixture.DrawOddsAverage < Fixture.HomeOddsAverage && Fixture.DrawOddsAverage < Fixture.AwayOddsAverage)
                {
                    return "D";
                }
                if (Fixture.AwayOddsAverage > 0 && Fixture.AwayOddsAverage < Fixture.HomeOddsAverage && Fixture.AwayOddsAverage < Fixture.DrawOddsAverage)
                {
                    return "A";
                }
                return "X";
            }
        }

        public string Recommended
        {
            get
            {
                if (!Valid)
                {
                    return "No Bet";
                }

                switch (Predicted)
                {
                    case "H":
                        return decimal.Divide(1, HomeOdds) > threshold ? "H" : "No Bet";
                    case "D":
                        return decimal.Divide(1, DrawOdds) > threshold ? "D" : "No Bet";
                    case "A":
                        return decimal.Divide(1, AwayOdds) > threshold ? "A" : "No Bet";
                    default:
                        return "No Bet";
                }
            }
        }

        public string RecommendedBase
        {
            get
            {
                if (!Valid)
                {
                    return "No Bet";
                }

                switch (PredictedBase)
                {
                    case "H":
                        return HomePercentAverage > threshold ? "H" : "No Bet";
                    case "D":
                        return DrawPercentAverage > threshold ? "D" : "No Bet";
                    case "A":
                        return AwayPercentAverage > threshold ? "A" : "No Bet";
                    default:
                        return "No Bet";
                }
            }
        }

        public string LowerRecommended
        {
            get
            {
                if (!Valid)
                {
                    return "No Bet";
                }

                switch (Predicted)
                {
                    case "H":
                        return decimal.Divide(1, HomeOdds) > lowerThreshold && decimal.Divide(1, HomeOdds) < threshold ? "H" : "No Bet";
                    case "D":
                        return decimal.Divide(1, DrawOdds) > lowerThreshold && decimal.Divide(1, DrawOdds) < threshold ? "D" : "No Bet";
                    case "A":
                        return decimal.Divide(1, AwayOdds) > lowerThreshold && decimal.Divide(1, AwayOdds) < threshold ? "A" : "No Bet";
                    default:
                        return "No Bet";
                }
            }
        }

        public string LowerRecommendedBase
        {
            get
            {
                if (!Valid)
                {
                    return "No Bet";
                }

                switch (PredictedBase)
                {
                    case "H":
                        return HomePercentAverage > lowerThreshold && HomePercentAverage < threshold ? "H" : "No Bet";
                    case "D":
                        return DrawPercentAverage > lowerThreshold && DrawPercentAverage < threshold ? "D" : "No Bet";
                    case "A":
                        return AwayPercentAverage > lowerThreshold && AwayPercentAverage < threshold ? "A" : "No Bet";
                    default:
                        return "No Bet";
                }
            }
        }

        public string Actual { get; set; }

        public string ActualBTTS { get; set; }

        public string ActualTwoGoals { get; set; }

        public string ActualThreeGoals { get; set; }

        public bool Valid { get; set; }

        public bool Correct
        {
            get
            {
                return Predicted == Actual ? true : false;
            }
        }

        public bool CorrectBase
        {
            get
            {
                return PredictedBase == Actual ? true : false;
            }
        }

        public bool CorrectBTTS
        {
            get
            {
                return PredictedBothTeamsScore == ActualBTTS ? true : false;
            }
        }

        public bool CorrectTwoGoals
        {
            get
            {
                return PredictedTwoGoals == ActualTwoGoals ? true : false;
            }
        }

        public bool CorrectThreeGoals
        {
            get
            {
                return PredictedThreeGoals == ActualThreeGoals ? true : false;
            }
        }

        public decimal BothTeamsScoreYesPercent { get; set; }

        public decimal BothTeamsScoreNoPercent { get; set; }

        [Display(Name ="Yes")]
        public decimal BothTeamsScoreYesOdds
        {
            get
            {
                return BothTeamsScoreYesPercent == 0 ? 0 : Math.Round(decimal.Divide(1, BothTeamsScoreYesPercent), 2);
            }
        }

        [Display(Name = "No")]
        public decimal BothTeamsScoreNoOdds
        {
            get
            {
                return BothTeamsScoreNoPercent == 0 ? 0 : Math.Round(decimal.Divide(1, BothTeamsScoreNoPercent), 2);
            }
        }

        public string PredictedBothTeamsScore
        {
            get
            {
                return BothTeamsScoreYesOdds > 0 && BothTeamsScoreYesOdds < BothTeamsScoreNoOdds ? "Yes" : "No";                
            }
        }

        public string RecommendedBothTeamsScore
        {
            get
            {
                if (!Valid)
                {
                    return "No Bet";
                }

                switch (PredictedBothTeamsScore)
                {
                    case "Yes":
                        return BothTeamsScoreYesPercent > thresholdBTTS ? "Yes" : "No Bet";
                    case "No":
                        return BothTeamsScoreNoPercent > thresholdBTTS ? "No" : "No Bet";                    
                    default:
                        return "No Bet";
                }
            }
        }

        public string LowerRecommendedBothTeamsScore
        {
            get
            {
                if (!Valid)
                {
                    return "No Bet";
                }

                switch (PredictedBothTeamsScore)
                {
                    case "Yes":
                        return BothTeamsScoreYesPercent > lowerThresholdBTTS ? "Yes" : "No Bet";
                    case "No":
                        return BothTeamsScoreNoPercent > lowerThresholdBTTS ? "No" : "No Bet";
                    default:
                        return "No Bet";
                }
            }
        }

        public decimal BothTeamsScoreYesValueOdds
        {
            get
            {
                return BothTeamsScoreYesOdds == 0 ? 0 : decimal.Divide(1, (decimal.Divide(1, BothTeamsScoreYesOdds)) - 0.05M);
            }
        }

        public decimal BothTeamsScoreNoValueOdds
        {
            get
            {
                return BothTeamsScoreNoOdds == 0 ? 0 : decimal.Divide(1, (decimal.Divide(1, BothTeamsScoreNoOdds)) - 0.05M);
            }
        }

        public decimal TwoGoalsYesPercent { get; set; }

        public decimal TwoGoalsNoPercent { get; set; }

        [Display(Name = "Yes")]
        public decimal TwoGoalsYesOdds
        {
            get
            {
                return TwoGoalsYesPercent == 0 ? 0 : Math.Round(decimal.Divide(1, TwoGoalsYesPercent), 2);
            }
        }

        [Display(Name = "No")]
        public decimal TwoGoalsNoOdds
        {
            get
            {
                return TwoGoalsNoPercent == 0 ? 0 : Math.Round(decimal.Divide(1, TwoGoalsNoPercent), 2);
            }
        }

        public string PredictedTwoGoals
        {
            get
            {
                return TwoGoalsYesOdds > 0 && TwoGoalsYesOdds < TwoGoalsNoOdds ? "Yes" : "No";
            }
        }

        public string RecommendedTwoGoals
        {
            get
            {
                if (!Valid)
                {
                    return "No Bet";
                }

                switch (PredictedTwoGoals)
                {
                    case "Yes":
                        return TwoGoalsYesPercent > thresholdTwoGoals ? "Yes" : "No Bet";
                    case "No":
                        return TwoGoalsNoPercent > thresholdTwoGoals ? "No" : "No Bet";
                    default:
                        return "No Bet";
                }
            }
        }

        public string LowerRecommendedTwoGoals
        {
            get
            {
                if (!Valid)
                {
                    return "No Bet";
                }

                switch (PredictedTwoGoals)
                {
                    case "Yes":
                        return TwoGoalsYesPercent > lowerThresholdTwoGoals ? "Yes" : "No Bet";
                    case "No":
                        return TwoGoalsNoPercent > lowerThresholdTwoGoals ? "No" : "No Bet";
                    default:
                        return "No Bet";
                }
            }
        }

        public decimal TwoGoalsYesValueOdds
        {
            get
            {
                return TwoGoalsYesOdds == 0 ? 0 : decimal.Divide(1, (decimal.Divide(1, TwoGoalsYesOdds)) - 0.05M);
            }
        }

        public decimal TwoGoalsNoValueOdds
        {
            get
            {
                return TwoGoalsNoOdds == 0 ? 0 : decimal.Divide(1, (decimal.Divide(1, TwoGoalsNoOdds)) - 0.05M);
            }
        }

        public decimal ThreeGoalsYesPercent { get; set; }

        public decimal ThreeGoalsNoPercent { get; set; }

        [Display(Name = "Yes")]
        public decimal ThreeGoalsYesOdds
        {
            get
            {
                return ThreeGoalsYesPercent == 0 ? 0 : Math.Round(decimal.Divide(1, ThreeGoalsYesPercent), 2);
            }
        }

        [Display(Name = "No")]
        public decimal ThreeGoalsNoOdds
        {
            get
            {
                return ThreeGoalsNoPercent == 0 ? 0 : Math.Round(decimal.Divide(1, ThreeGoalsNoPercent), 2);
            }
        }

        public string PredictedThreeGoals
        {
            get
            {
                return ThreeGoalsYesOdds > 0 && ThreeGoalsYesOdds < ThreeGoalsNoOdds ? "Yes" : "No";
            }
        }

        public string RecommendedThreeGoals
        {
            get
            {
                if (!Valid)
                {
                    return "No Bet";
                }

                switch (PredictedThreeGoals)
                {
                    case "Yes":
                        return ThreeGoalsYesPercent > thresholdThreeGoals ? "Yes" : "No Bet";
                    case "No":
                        return ThreeGoalsNoPercent > thresholdThreeGoals ? "No" : "No Bet";
                    default:
                        return "No Bet";
                }
            }
        }

        public string LowerRecommendedThreeGoals
        {
            get
            {
                if (!Valid)
                {
                    return "No Bet";
                }

                switch (PredictedThreeGoals)
                {
                    case "Yes":
                        return ThreeGoalsYesPercent > lowerThresholdThreeGoals ? "Yes" : "No Bet";
                    case "No":
                        return ThreeGoalsNoPercent > lowerThresholdThreeGoals ? "No" : "No Bet";
                    default:
                        return "No Bet";
                }
            }
        }

        public decimal ThreeGoalsYesValueOdds
        {
            get
            {
                return ThreeGoalsYesOdds == 0 ? 0 : decimal.Divide(1, (decimal.Divide(1, ThreeGoalsYesOdds)) - 0.05M);
            }
        }

        public decimal ThreeGoalsNoValueOdds
        {
            get
            {
                return ThreeGoalsNoOdds == 0 ? 0 : decimal.Divide(1, (decimal.Divide(1, ThreeGoalsNoOdds)) - 0.05M);
            }
        }

        public FixtureRating(Fixture fixture)
        {
            Fixture = fixture;
        }
    }
}