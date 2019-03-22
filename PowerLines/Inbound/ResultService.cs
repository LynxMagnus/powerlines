using PowerLines.BulkInsert;
using PowerLines.BulkInsert.Maps;
using PowerLines.DAL;
using PowerLines.Models;
using PowerLines.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;

namespace PowerLines.Inbound
{
    public class ResultService : IResultService
    {
        static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        PowerLinesContext db;
        IResultReader resultReader;
        IRatingService ratingService;
        IBulkInsert<Result> bulkInsert;
        int resultsAdded = 0;
        List<Result> inboundResults = new List<Result>();
        
        string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Uploads");

        public ResultService(PowerLinesContext context, IResultReader resultReader, IRatingService ratingService, IBulkInsert<Result> bulkInsert)
        {
            db = context;
            this.resultReader = resultReader;
            this.ratingService = ratingService;
            this.bulkInsert = bulkInsert;
        }

        private List<string> leagues = new List<string>
        {
            "E0",
            "E1",
            "E2",
            "E3",
            "EC",
            "SC0",
            "SC1",
            "SC2",
            "SC3",
            "D1",
            "D2",
            "I1",
            "I2",
            "SP1",
            "SP2",
            "F1",
            "F2",
            "N1",
            "B1",
            "P1",
            "T1",
            "G1"
        };

        public int Upload(bool currentSeasonOnly = false)
        {
            string source = "http://www.football-data.co.uk/mmz4281/{0}/{1}.csv";

            int firstSeason = GetFirstSeasonYear(currentSeasonOnly);
            int lastSeason = GetLastSeasonYear();
                        
            while (firstSeason < lastSeason)
            {
                foreach (var league in leagues)
                {
                    string filePath = Path.Combine(path, string.Format("DataLines_{0}_{1}_{2}.csv", league, firstSeason, DateTime.Now.ToString("yyyyMMddHHmmss")));

                    try
                    {
                        using (var client = new WebClient())
                        {
                            client.DownloadFile(string.Format(source, GetSeason(firstSeason % 100), league), filePath);
                        }

                        inboundResults.AddRange(resultReader.Read(filePath));                        
                    }
                    catch (Exception ex)
                    {
                        log.Error(string.Format("Error uploading results for {0}", league), ex);
                    }
                    finally
                    {
                        if (File.Exists(filePath))
                        {
                            File.Delete(filePath);
                        }
                    }
                }

                firstSeason++;
            }

            SaveResults();

            ratingService.RunAnalysis();

            return resultsAdded;
        }

        private void SaveResults()
        {
            var existingResults = db.Results.AsNoTracking().ToList();
            var newResults = inboundResults.Except(existingResults, new ResultComparer())
                .GroupBy(x => new { x.HomeTeam, x.AwayTeam, x.Date }).Select(g => g.First()).ToList();
            
            resultsAdded = newResults.Count;

            bulkInsert.Insert(newResults, "dbo.Results", ResultSqlMap.ColumnMapping());
        }
        
        private string GetSeason(int firstYear)
        {
            int secondYear = firstYear + 1;

            return string.Format("{0}{1}", firstYear.ToString("D2"), secondYear.ToString("D2"));
        }

        private int GetFirstSeasonYear(bool currentSeasonOnly = false)
        {
            if (!currentSeasonOnly)
            {
                return 1993;
            }

            DateTime current = DateTime.Now;

            if (current.Month <= 5)
            {
                return current.Year - 1;
            }
            return current.Year;
        }

        private int GetLastSeasonYear()
        {
            DateTime current = DateTime.Now;

            if (current.Month <= 5)
            {
                return current.Year;
            }
            return current.Year + 1;
        }
    }
}