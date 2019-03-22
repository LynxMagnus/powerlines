using PowerLines.BulkInsert;
using PowerLines.DAL;
using PowerLines.Inbound;
using PowerLines.Models;
using PowerLines.Services;
using System.Web;
using System.Web.Mvc;

namespace PowerLines.Controllers
{
    [Authorize(Roles = "Admin")]
    public class UploadController : Controller
    {
        PowerLinesContext db;
        IResultService resultService;
        IFixtureService fixtureService;        

        public UploadController()
        {
            db = new PowerLinesContext();
            resultService = new ResultService(db, new ResultReader(), new RatingService(db, new SqlBulkInsert<Dataset>(db)), new SqlBulkInsert<Result>(db));
            fixtureService = new FixtureService(db, new FixtureReader());            
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Upload(string submit)
        {
            int resultsAdded;

            switch (submit)
            {
                case "All Seasons":
                    resultsAdded = resultService.Upload();
                    TempData["Message"] = resultsAdded == 0 ? "No new results available" : string.Format("{0} results added",resultsAdded);
                    break;
                case "Current Season":
                    resultsAdded = resultService.Upload(true);
                    TempData["Message"] = resultsAdded == 0 ? "No new results available" : string.Format("{0} results added", resultsAdded);
                    break;
                case "Latest Fixtures":
                    int fixturesAdded = fixtureService.Upload();
                    TempData["Message"] = fixturesAdded == 0 ? "No new fixtures available" : string.Format("{0} fixtures added", fixturesAdded);
                    break;
                default:
                    break;
            }

            return RedirectToAction("Index", "Home");
        }
    }
}