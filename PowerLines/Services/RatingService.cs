using PowerLines.BulkInsert;
using PowerLines.BulkInsert.Maps;
using PowerLines.DAL;
using PowerLines.Models;
using PowerLines.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;

namespace PowerLines.Services
{
    public class RatingService : IRatingService
    {
        static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        PowerLinesContext db;
        IBulkInsert<Dataset> bulkInsert;
        DateTime currentDate = DateTime.Now.Date;
        List<Result> results = new List<Result>();
        List<Fixture> fixtures = new List<Fixture>();
        List<Dataset> analysis = new List<Dataset>();
        bool test;
        int matchesToInclude = 6;
        int validThreshold = 15;

        public RatingService(PowerLinesContext context, IBulkInsert<Dataset> bulkInsert)
        {
            db = context;
            this.bulkInsert = bulkInsert;
        }

        public List<FixtureRating> Calculate(bool test = false, bool reset = false)
        {
            try
            {
                this.test = test;

                if (reset)
                {
                    DeleteAnalysis();
                    RunAnalysis();
                }

                List<FixtureRating> fixtureRatings = new List<FixtureRating>();

                if (!test)
                {
                    var current = DateTime.Now.Date;

                    fixtures = db.Fixtures.AsNoTracking().Where(x => x.Date >= current).OrderBy(x => x.Date).ThenBy(x => x.Division).ThenBy(x => x.HomeTeam).ToList();
                }
                else
                {
                    SetupTestFixtures();
                }

                analysis = db.Datasets.AsNoTracking().ToList();

                for (int i = 0; i < fixtures.Count; i++)
                {
                    FixtureRating fixtureRating = SetupFixtureRating(fixtures[i]);
                    fixtureRatings.Add(fixtureRating);
                }

                return fixtureRatings;
            }
            catch (Exception ex)
            {
                log.Error("Error calculating fixtures", ex);
                throw;
            }
        }

        public void RunAnalysis()
        {
            results = db.Results.AsNoTracking().ToList();

            var existingDatasets = new HashSet<int>(db.Datasets.AsNoTracking().Select(x => x.ResultId));

            var datasetsToCreate = results.Where(x => !existingDatasets.Contains(x.ResultId) && x.Date.Month != 7 && x.Date.Month != 8).ToList();

            if (datasetsToCreate.Count > 0)
            {
                CreateDatasets(datasetsToCreate);
            }
        }

        private void CreateDatasets(List<Result> datasetsToCreate)
        {
            List<Dataset> newDatasets = new List<Dataset>();

            for (int i = 0; i < datasetsToCreate.Count; i++)
            {
                Dataset dataset = SetupDataset(datasetsToCreate[i]);

                newDatasets.Add(dataset);
            }

            bulkInsert.Insert(newDatasets, "dbo.Datasets", DatasetSqlMap.ColumnMapping());
        }

        private void DeleteAnalysis()
        {
            db.Database.ExecuteSqlCommand("TRUNCATE TABLE Datasets");
        }

        private Dataset SetupDataset(Result result)
        {
            Dataset dataset = new Dataset();
            dataset.ResultId = result.ResultId;
            dataset.Division = result.Division;
            dataset.HomeTeam = result.HomeTeam;
            dataset.AwayTeam = result.AwayTeam;
            dataset.HomeGoals = result.FullTimeHomeGoals;
            dataset.AwayGoals = result.FullTimeAwayGoals;
            dataset.Date = result.Date;
            dataset.HomeSuperiority = GetSuperiority(result.HomeTeam, true, result.Date);
            dataset.AwaySuperiority = GetSuperiority(result.AwayTeam, false, result.Date);
            dataset.Actual = result.FullTimeResult;

            return dataset;
        }

        private FixtureRating SetupFixtureRating(Fixture fixture)
        {
            FixtureRating fixtureRating = new FixtureRating(fixture);

            fixtureRating.HomeSuperiority = GetSuperiority(fixture.HomeTeam, true);
            fixtureRating.AwaySuperiority = GetSuperiority(fixture.AwayTeam, false);

            var matchedResults = analysis.Where(x => x.Division == fixture.Division && x.MatchRating == fixtureRating.MatchRating).ToList();
            var previousResults = analysis.Where(x => x.HomeTeam == fixture.HomeTeam && x.AwayTeam == fixture.AwayTeam && x.Date < fixture.Date && x.Date > fixture.Date.AddYears(-5)).OrderByDescending(x => x.Date).Take(5).ToList();

            matchedResults.AddRange(previousResults);
            matchedResults.AddRange(previousResults);
            matchedResults.AddRange(previousResults);

            if (matchedResults.Count > 0)
            {
                fixtureRating.HomePercent = decimal.Divide(matchedResults.Where(x => x.Actual == "H").Count(), matchedResults.Count);
                fixtureRating.DrawPercent = decimal.Divide(matchedResults.Where(x => x.Actual == "D").Count(), matchedResults.Count);
                fixtureRating.AwayPercent = decimal.Divide(matchedResults.Where(x => x.Actual == "A").Count(), matchedResults.Count);

                fixtureRating.BothTeamsScoreYesPercent = decimal.Divide(matchedResults.Where(x => x.BothTeamsScored).Count(), matchedResults.Count);
                fixtureRating.BothTeamsScoreNoPercent = decimal.Divide(matchedResults.Where(x => !x.BothTeamsScored).Count(), matchedResults.Count);

                fixtureRating.TwoGoalsYesPercent = decimal.Divide(matchedResults.Where(x => x.TwoGoals).Count(), matchedResults.Count);
                fixtureRating.TwoGoalsNoPercent = decimal.Divide(matchedResults.Where(x => !x.TwoGoals).Count(), matchedResults.Count);

                fixtureRating.ThreeGoalsYesPercent = decimal.Divide(matchedResults.Where(x => x.ThreeGoals).Count(), matchedResults.Count);
                fixtureRating.ThreeGoalsNoPercent = decimal.Divide(matchedResults.Where(x => !x.ThreeGoals).Count(), matchedResults.Count);
            }

            fixtureRating.Valid = matchedResults.Count > validThreshold ? true : false;

            if (test)
            {
                fixtureRating.Actual = fixture.Actual;
                fixtureRating.ActualBTTS = fixture.ActualBTTS;
                fixtureRating.ActualTwoGoals = fixture.ActualTwoGoals;
                fixtureRating.ActualThreeGoals = fixture.ActualThreeGoals;
            }

            return fixtureRating;
        }

        private int GetSuperiority(string teamName, bool home, DateTime? date = null)
        {
            var validResults = results.Where(x => x.Date < (date != null ? date : currentDate) && (home ? x.HomeTeam : x.AwayTeam) == teamName).OrderByDescending(x => x.Date).Take(matchesToInclude).ToList();

            return validResults.Sum(x => (home ? x.FullTimeHomeGoals : x.FullTimeAwayGoals)) - validResults.Sum(x => (home ? x.FullTimeAwayGoals : x.FullTimeHomeGoals));
        }

        private void SetupTestFixtures()
        {
            DateTime startDate = new DateTime(DateTime.Now.Year - 3, 9, 1);

            var testFixtures = db.Results.AsNoTracking().Where(x => x.Date >= startDate && x.Date.Month != 7 && x.Date.Month != 8).ToList();

            for (int i = 0; i < testFixtures.Count; i++)
            {
                Fixture fixture = new Fixture
                {
                    Division = testFixtures[i].Division,
                    Date = testFixtures[i].Date,
                    HomeTeam = testFixtures[i].HomeTeam,
                    AwayTeam = testFixtures[i].AwayTeam,
                    Actual = testFixtures[i].FullTimeResult,
                    ActualBTTS = testFixtures[i].FullTimeHomeGoals > 0 && testFixtures[i].FullTimeAwayGoals > 0 ? "Yes" : "No",
                    ActualTwoGoals = testFixtures[i].FullTimeHomeGoals + testFixtures[i].FullTimeAwayGoals >= 2 ? "Yes" : "No",
                    ActualThreeGoals = testFixtures[i].FullTimeHomeGoals + testFixtures[i].FullTimeAwayGoals >= 3 ? "Yes" : "No",
                    HomeOddsAverage = testFixtures[i].HomeOddsAverage,
                    DrawOddsAverage = testFixtures[i].DrawOddsAverage,
                    AwayOddsAverage = testFixtures[i].AwayOddsAverage
                };

                fixtures.Add(fixture);
            }
        }
    }
}