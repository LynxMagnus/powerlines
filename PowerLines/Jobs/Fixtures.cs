using PowerLines.DAL;
using PowerLines.Inbound;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PowerLines.Jobs
{
    public static class Fixtures
    {
        public static void Update()
        {
            using (var db = new PowerLinesContext())
            {
                var fixtureService = new FixtureService(db, new FixtureReader());
                fixtureService.Upload();
            }
        }
    }
}