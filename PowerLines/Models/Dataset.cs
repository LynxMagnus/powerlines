using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PowerLines.Models
{
    public class Dataset
    {
        public int DatasetId { get; set; }

        public int ResultId { get; set; }

        public string Division { get; set; }

        public string HomeTeam { get; set; }

        public string AwayTeam { get; set; }

        public DateTime Date { get; set; }

        public int HomeSuperiority { get; set; }

        public int AwaySuperiority { get; set; }

        public int HomeGoals { get; set; }

        public int AwayGoals { get; set; }

        public int MatchRating
        {
            get
            {
                return HomeSuperiority - AwaySuperiority;
            }
        }

        public bool BothTeamsScored
        {
            get
            {
                return HomeGoals > 0 && AwayGoals > 0 ? true : false;
            }
        }

        public bool TwoGoals
        {
            get
            {
                return HomeGoals + AwayGoals >= 2;
            }
        }

        public bool ThreeGoals
        {
            get
            {
                return HomeGoals + AwayGoals >= 3;
            }
        }

        public string Actual { get; set; }
    }
}