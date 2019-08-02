using PowerLines.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;

namespace PowerLines.Inbound
{
    public class FixtureReader : IFixtureReader
    {
        public List<Fixture> Read(string filePath)
        {
            List<Fixture> fixtures = new List<Fixture>();

            CultureInfo culture = new CultureInfo("en-GB");

            using (var reader = new StreamReader(filePath))
            {
                var header = reader.ReadLine();
                var headers = header.Split(',');

                int homeAverage = -1;
                int drawAverage = -1;
                int awayAverage = -1;

                for(int i = 0; i< headers.Length; i++)
                {
                    switch(headers[i])
                    {
                        case "BbAvH":
                            homeAverage = i;
                            break;
                        case "BbAvD":
                            drawAverage = i;
                            break;
                        case "BbAvA":
                            awayAverage = i;
                            break;
                        default:
                            break;
                    }
                }
                
                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    var values = line.Split(',');

                    fixtures.Add(new Fixture
                    {
                        Division = values[0].Trim(),
                        Date = DateTime.Parse(values[1].Trim(), culture.DateTimeFormat).Add(TimeSpan.Parse(values[2])),
                        HomeTeam = values[3].Trim(),
                        AwayTeam = values[4].Trim(),
                        HomeOddsAverage = homeAverage == -1 ? 0 : !string.IsNullOrEmpty(values[homeAverage].Trim()) ? decimal.Parse(values[homeAverage].Trim()) : 0,
                        DrawOddsAverage = drawAverage == -1 ? 0 : !string.IsNullOrEmpty(values[drawAverage].Trim()) ? decimal.Parse(values[drawAverage].Trim()) : 0,
                        AwayOddsAverage = awayAverage == -1 ? 0 : !string.IsNullOrEmpty(values[awayAverage].Trim()) ? decimal.Parse(values[awayAverage].Trim()) : 0
                    });

                }
            }

            return fixtures;
        }
    }
}
