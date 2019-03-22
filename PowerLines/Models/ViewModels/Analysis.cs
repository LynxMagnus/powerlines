using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PowerLines.Models.ViewModels
{
    public class Analysis
    {
        public LeagueAnalysis OverallAnalysis { get; set; }

        public List<LeagueAnalysis> LeagueAnalysis { get; set; }

        public Analysis(List<FixtureRating> fixtureRatings)
        {
            OverallAnalysis = new LeagueAnalysis(fixtureRatings);
            LeagueAnalysis = new List<ViewModels.LeagueAnalysis>();

            foreach (var division in fixtureRatings.Select(x => x.Fixture.Division).Distinct())
            {
                LeagueAnalysis.Add(new LeagueAnalysis(fixtureRatings.Where(x => x.Fixture.Division == division).ToList()));
            }
        }
    }

    public class LeagueAnalysis
    {
        private List<FixtureRating> fixtureRatings { get; set; }

        public string Division
        {
            get
            {
                return fixtureRatings.FirstOrDefault()?.Fixture?.Division;
            }
        }

        [Display(Name = "Division")]
        public string DivisionName
        {
            get
            {
                return fixtureRatings.FirstOrDefault()?.Fixture?.DivisionName;
            }
        }

        public int Fixtures
        {
            get
            {
                return fixtureRatings.Count();
            }
        }

        [Display(Name = "Str Recommended")]
        public int Recommended
        {
            get
            {
                return fixtureRatings.Where(x => x.Recommended != "No Bet").Count();
            }
        }

        [Display(Name = "Base")]
        public int RecommendedBase
        {
            get
            {
                return fixtureRatings.Where(x => x.RecommendedBase != "No Bet").Count();
            }
        }

        [Display(Name = "Str Correct")]
        public int Correct
        {
            get
            {
                return fixtureRatings.Where(x => x.Recommended != "No Bet" && x.Correct).Count();
            }
        }

        [Display(Name = "Base")]
        public int CorrectBase
        {
            get
            {
                return fixtureRatings.Where(x => x.RecommendedBase != "No Bet" && x.CorrectBase).Count();
            }
        }

        [Display(Name = "Str Accuracy")]
        public decimal Accuracy
        {
            get
            {
                return Recommended == 0 ? 0 : Math.Round(decimal.Divide(Correct, Recommended) * 100, 2);
            }
        }

        [Display(Name = "Base")]
        public decimal AccuracyBase
        {
            get
            {
                return RecommendedBase == 0 ? 0 : Math.Round(decimal.Divide(CorrectBase, RecommendedBase) * 100, 2);
            }
        }

        [Display(Name = "Recommended")]
        public int LowerRecommended
        {
            get
            {
                return fixtureRatings.Where(x => x.LowerRecommended != "No Bet").Count();
            }
        }

        [Display(Name = "Base")]
        public int LowerRecommendedBase
        {
            get
            {
                return fixtureRatings.Where(x => x.LowerRecommendedBase != "No Bet").Count();
            }
        }

        [Display(Name = "Rec Correct")]
        public int LowerCorrect
        {
            get
            {
                return fixtureRatings.Where(x => x.LowerRecommended != "No Bet" && x.Correct).Count();
            }
        }

        [Display(Name = "Base")]
        public int LowerCorrectBase
        {
            get
            {
                return fixtureRatings.Where(x => x.LowerRecommendedBase != "No Bet" && x.CorrectBase).Count();
            }
        }

        [Display(Name = "Rec Accuracy")]
        public decimal LowerAccuracy
        {
            get
            {
                return LowerRecommended == 0 ? 0 : Math.Round(decimal.Divide(LowerCorrect, LowerRecommended) * 100, 2);
            }
        }

        [Display(Name = "Base")]
        public decimal LowerAccuracyBase
        {
            get
            {
                return LowerRecommendedBase == 0 ? 0 : Math.Round(decimal.Divide(LowerCorrectBase, LowerRecommendedBase) * 100, 2);
            }
        }

        [Display(Name = "Gen Correct")]
        public int GeneralCorrect
        {
            get
            {
                return fixtureRatings.Where(x => x.Correct).Count();
            }
        }

        [Display(Name = "Base")]
        public int GeneralCorrectBase
        {
            get
            {
                return fixtureRatings.Where(x => x.CorrectBase).Count();
            }
        }

        [Display(Name = "Gen Accuracy")]
        public decimal GeneralAccuracy
        {
            get
            {
                return Fixtures == 0 ? 0 : Math.Round(decimal.Divide(GeneralCorrect, Fixtures) * 100, 2);
            }
        }

        [Display(Name = "Base")]
        public decimal GeneralAccuracyBase
        {
            get
            {
                return Fixtures == 0 ? 0 : Math.Round(decimal.Divide(GeneralCorrectBase, Fixtures) * 100, 2);
            }
        }

        [Display(Name = "Home")]
        public decimal HomeAccuracy
        {
            get
            {
                return fixtureRatings.Where(x => x.Predicted == "H").Count() == 0 ? 0 : Math.Round(decimal.Divide(fixtureRatings.Where(x => x.Predicted == "H" && x.Correct).Count(), fixtureRatings.Where(x => x.Predicted == "H").Count()) * 100, 2);
            }
        }

        [Display(Name = "Base")]
        public decimal HomeAccuracyBase
        {
            get
            {
                return fixtureRatings.Where(x => x.PredictedBase == "H").Count() == 0 ? 0 : Math.Round(decimal.Divide(fixtureRatings.Where(x => x.PredictedBase == "H" && x.CorrectBase).Count(), fixtureRatings.Where(x => x.PredictedBase == "H").Count()) * 100, 2);
            }
        }

        [Display(Name = "Draw")]
        public decimal DrawAccuracy
        {
            get
            {
                return fixtureRatings.Where(x => x.Predicted == "D").Count() == 0 ? 0 : Math.Round(decimal.Divide(fixtureRatings.Where(x => x.Predicted == "D" && x.Correct).Count(), fixtureRatings.Where(x => x.Predicted == "D").Count()) * 100, 2);
            }
        }

        [Display(Name = "Base")]
        public decimal DrawAccuracyBase
        {
            get
            {
                return fixtureRatings.Where(x => x.PredictedBase == "D").Count() == 0 ? 0 : Math.Round(decimal.Divide(fixtureRatings.Where(x => x.PredictedBase == "D" && x.CorrectBase).Count(), fixtureRatings.Where(x => x.PredictedBase == "D").Count()) * 100, 2);
            }
        }

        [Display(Name = "Away")]
        public decimal AwayAccuracy
        {
            get
            {
                return fixtureRatings.Where(x => x.Predicted == "A").Count() == 0 ? 0 : Math.Round(decimal.Divide(fixtureRatings.Where(x => x.Predicted == "A" && x.Correct).Count(), fixtureRatings.Where(x => x.Predicted == "A").Count()) * 100, 2);
            }
        }

        [Display(Name = "Base")]
        public decimal AwayAccuracyBase
        {
            get
            {
                return fixtureRatings.Where(x => x.PredictedBase == "A").Count() == 0 ? 0 : Math.Round(decimal.Divide(fixtureRatings.Where(x => x.PredictedBase == "A" && x.CorrectBase).Count(), fixtureRatings.Where(x => x.PredictedBase == "A").Count()) * 100, 2);
            }
        }

        [Display(Name = "BTTS Str Recommended")]
        public int RecommendedBTTS
        {
            get
            {
                return fixtureRatings.Where(x => x.RecommendedBothTeamsScore != "No Bet").Count();
            }
        }

        [Display(Name = "BTTS Str Correct")]
        public int CorrectBTTS
        {
            get
            {
                return fixtureRatings.Where(x => x.RecommendedBothTeamsScore != "No Bet" && x.CorrectBTTS).Count();
            }
        }


        [Display(Name = "BTTS Str Accuracy")]
        public decimal AccuracyBTTS
        {
            get
            {
                return RecommendedBTTS == 0 ? 0 : Math.Round(decimal.Divide(CorrectBTTS, RecommendedBTTS) * 100, 2);
            }
        }

        [Display(Name = "BTTS Recommended")]
        public int LowerRecommendedBTTS
        {
            get
            {
                return fixtureRatings.Where(x => x.LowerRecommendedBothTeamsScore != "No Bet").Count();
            }
        }


        [Display(Name = "BTTS Rec Correct")]
        public int LowerCorrectBTTS
        {
            get
            {
                return fixtureRatings.Where(x => x.LowerRecommendedBothTeamsScore != "No Bet" && x.CorrectBTTS).Count();
            }
        }

        [Display(Name = "BTTS Rec Accuracy")]
        public decimal LowerAccuracyBTTS
        {
            get
            {
                return LowerRecommendedBTTS == 0 ? 0 : Math.Round(decimal.Divide(LowerCorrectBTTS, LowerRecommendedBTTS) * 100, 2);
            }
        }

        [Display(Name = "BTTS Gen Correct")]
        public int GeneralCorrectBTTS
        {
            get
            {
                return fixtureRatings.Where(x => x.CorrectBTTS).Count();
            }
        }

        [Display(Name = "BTTS Gen Accuracy")]
        public decimal GeneralAccuracyBTTS
        {
            get
            {
                return Fixtures == 0 ? 0 : Math.Round(decimal.Divide(GeneralCorrectBTTS, Fixtures) * 100, 2);
            }
        }

        [Display(Name = "Over 1.5 Str Recommended")]
        public int RecommendedTwoGoals
        {
            get
            {
                return fixtureRatings.Where(x => x.RecommendedTwoGoals != "No Bet").Count();
            }
        }

        [Display(Name = "Over 1.5 Str Correct")]
        public int CorrectTwoGoals
        {
            get
            {
                return fixtureRatings.Where(x => x.RecommendedTwoGoals != "No Bet" && x.CorrectTwoGoals).Count();
            }
        }


        [Display(Name = "Over 1.5 Str Accuracy")]
        public decimal AccuracyTwoGoals
        {
            get
            {
                return RecommendedTwoGoals == 0 ? 0 : Math.Round(decimal.Divide(CorrectTwoGoals, RecommendedTwoGoals) * 100, 2);
            }
        }

        [Display(Name = "Over 1.5 Recommended")]
        public int LowerRecommendedTwoGoals
        {
            get
            {
                return fixtureRatings.Where(x => x.LowerRecommendedTwoGoals != "No Bet").Count();
            }
        }


        [Display(Name = "Over 1.5 Rec Correct")]
        public int LowerCorrectTwoGoals
        {
            get
            {
                return fixtureRatings.Where(x => x.LowerRecommendedTwoGoals != "No Bet" && x.CorrectTwoGoals).Count();
            }
        }

        [Display(Name = "Over 1.5 Rec Accuracy")]
        public decimal LowerAccuracyTwoGoals
        {
            get
            {
                return LowerRecommendedTwoGoals == 0 ? 0 : Math.Round(decimal.Divide(LowerCorrectTwoGoals, LowerRecommendedTwoGoals) * 100, 2);
            }
        }

        [Display(Name = "Over 1.5 Gen Correct")]
        public int GeneralCorrectTwoGoals
        {
            get
            {
                return fixtureRatings.Where(x => x.CorrectTwoGoals).Count();
            }
        }

        [Display(Name = "Over 1.5 Gen Accuracy")]
        public decimal GeneralAccuracyTwoGoals
        {
            get
            {
                return Fixtures == 0 ? 0 : Math.Round(decimal.Divide(GeneralCorrectTwoGoals, Fixtures) * 100, 2);
            }
        }

        [Display(Name = "Over 2.5 Str Recommended")]
        public int RecommendedThreeGoals
        {
            get
            {
                return fixtureRatings.Where(x => x.RecommendedThreeGoals != "No Bet").Count();
            }
        }

        [Display(Name = "Over 2.5 Str Correct")]
        public int CorrectThreeGoals
        {
            get
            {
                return fixtureRatings.Where(x => x.RecommendedThreeGoals != "No Bet" && x.CorrectThreeGoals).Count();
            }
        }


        [Display(Name = "Over 2.5 Str Accuracy")]
        public decimal AccuracyThreeGoals
        {
            get
            {
                return RecommendedThreeGoals == 0 ? 0 : Math.Round(decimal.Divide(CorrectThreeGoals, RecommendedThreeGoals) * 100, 2);
            }
        }

        [Display(Name = "Over 2.5 Recommended")]
        public int LowerRecommendedThreeGoals
        {
            get
            {
                return fixtureRatings.Where(x => x.LowerRecommendedThreeGoals != "No Bet").Count();
            }
        }


        [Display(Name = "Over 2.5 Rec Correct")]
        public int LowerCorrectThreeGoals
        {
            get
            {
                return fixtureRatings.Where(x => x.LowerRecommendedThreeGoals != "No Bet" && x.CorrectThreeGoals).Count();
            }
        }

        [Display(Name = "Over 2.5 Rec Accuracy")]
        public decimal LowerAccuracyThreeGoals
        {
            get
            {
                return LowerRecommendedThreeGoals == 0 ? 0 : Math.Round(decimal.Divide(LowerCorrectThreeGoals, LowerRecommendedThreeGoals) * 100, 2);
            }
        }

        [Display(Name = "Over 2.5 Gen Correct")]
        public int GeneralCorrectThreeGoals
        {
            get
            {
                return fixtureRatings.Where(x => x.CorrectThreeGoals).Count();
            }
        }

        [Display(Name = "Over 2.5 Gen Accuracy")]
        public decimal GeneralAccuracyThreeGoals
        {
            get
            {
                return Fixtures == 0 ? 0 : Math.Round(decimal.Divide(GeneralCorrectThreeGoals, Fixtures) * 100, 2);
            }
        }

        public LeagueAnalysis(List<FixtureRating> fixtureRatings)
        {
            this.fixtureRatings = fixtureRatings;
        }
    }
}