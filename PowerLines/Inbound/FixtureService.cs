using PowerLines.DAL;
using PowerLines.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;

namespace PowerLines.Inbound
{
    public class FixtureService : IFixtureService
    {
        static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        PowerLinesContext db;
        IFixtureReader fixtureReader;
        int fixturesAdded = 0;

        string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Uploads");

        public FixtureService(PowerLinesContext context, IFixtureReader fixtureReader)
        {
            db = context;
            this.fixtureReader = new FixtureReader();
        }

        public int Upload()
        {
            string source = "http://www.football-data.co.uk/fixtures.csv";

            string filePath = Path.Combine(path, string.Format("Fixtures_{0}.csv", DateTime.Now.ToString("yyyyMMddHHmmss")));

            try
            {
                using (var client = new WebClient())
                {
                    client.DownloadFile(string.Format(source), filePath);
                }

                SaveFixtures(fixtureReader.Read(filePath));
                DeleteFixtures();

                File.Delete(filePath);
            }
            catch (Exception ex)
            {
                log.Error("Error uploading fixtures", ex);
            }
            finally
            {
                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                }
            }

            return fixturesAdded;
        }

        private void SaveFixtures(List<Fixture> fixtures)
        {
            var existingFixtures = db.Fixtures.AsNoTracking().ToList();
            
            foreach (var fixture in fixtures)
            {
                if (fixture.Date.Date >= DateTime.Now.Date && !existingFixtures.Any(x => x.HomeTeam == fixture.HomeTeam && x.AwayTeam == fixture.AwayTeam && x.Date == fixture.Date))
                {
                    db.Fixtures.Add(fixture);
                    fixturesAdded++;
                }
            }

            db.SaveChanges();            
        }

        private void DeleteFixtures()
        {
            if (fixturesAdded > 0)
            {
                DateTime currentDate = DateTime.Now.Date;

                var fixtures = db.Fixtures.Where(x => x.Date < currentDate).ToList();

                foreach (var fixture in fixtures)
                {
                    db.Fixtures.Remove(fixture);
                }

                db.SaveChanges();
            }
        }
    }
}