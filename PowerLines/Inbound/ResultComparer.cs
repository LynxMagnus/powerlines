using PowerLines.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PowerLines.Inbound
{
    public class ResultComparer : IEqualityComparer<Result>
    {
        public bool Equals(Result x, Result y)
        {
            return x.Date == y.Date && x.HomeTeam == y.HomeTeam && x.AwayTeam == y.AwayTeam;
        }

        public int GetHashCode(Result obj)
        {
            int hash = 17;
            hash = hash * 23 + (obj.Date).GetHashCode();
            hash = hash * 23 + (obj.HomeTeam).GetHashCode();
            hash = hash * 23 + (obj.AwayTeam).GetHashCode();
            return hash;
        }
    }

}