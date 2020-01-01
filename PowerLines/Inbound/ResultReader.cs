using PowerLines.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;

namespace PowerLines.Inbound
{
    public class ResultReader : IResultReader
    {        
        public List<Result> Read(string filePath)
        {
            List<Result> results = new List<Result>();

            CultureInfo culture = new CultureInfo("en-GB");

            using (var reader = new StreamReader(filePath))
            {
                var header = reader.ReadLine();
                var headers = header.Split(',');

                int time = -1;
                int homeAverage = -1;
                int drawAverage = -1;
                int awayAverage = -1;

                for (int i = 0; i < headers.Length; i++)
                {
                    switch (headers[i])
                    {
                        case "Time":
                            time = i;
                            break;
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

                    if(time == -1)
                    {
                        var valuesList = values.ToList();
                        valuesList.Insert(2, "0");
                        values = valuesList.ToArray();
                    }

                    if (values.Length > 6 && !string.IsNullOrEmpty(values[7]))
                    {
                        results.Add(new Result
                        {
                            Division = values[0].Trim(),
                            Date = DateTime.Parse(values[1].Trim(), culture.DateTimeFormat),
                            HomeTeam = values[3].Trim(),
                            AwayTeam = values[4].Trim(),
                            FullTimeHomeGoals = int.Parse(values[5].Trim()),
                            FullTimeAwayGoals = int.Parse(values[6].Trim()),
                            FullTimeResult = values[7].Trim(),
                            HalfTimeHomeGoals = values.Length > 8 && !string.IsNullOrEmpty(values[8].Trim()) ? int.Parse(values[8].Trim()) : 0,
                            HalfTimeAwayGoals = values.Length > 8 && !string.IsNullOrEmpty(values[9].Trim()) ? int.Parse(values[9].Trim()) : 0,
                            HalfTimeResult = values.Length > 8 && !string.IsNullOrEmpty(values[10].Trim()) ? values[10].Trim() : null,
                            HomeOddsAverage = homeAverage == -1 ? 0 : !string.IsNullOrEmpty(values[homeAverage].Trim()) ? decimal.Parse(values[homeAverage].Trim()) : 0,
                            DrawOddsAverage = drawAverage == -1 ? 0 : !string.IsNullOrEmpty(values[drawAverage].Trim()) ? decimal.Parse(values[drawAverage].Trim()) : 0,
                            AwayOddsAverage = awayAverage == -1 ? 0 : !string.IsNullOrEmpty(values[awayAverage].Trim()) ? decimal.Parse(values[awayAverage].Trim()) : 0
                        });
                    }
                }
            }

            return results;
        }
    }
}