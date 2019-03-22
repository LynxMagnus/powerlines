using Microsoft.AspNet.Identity;
using PowerLines.DAL;
using PowerLines.Models;
using PowerLines.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Authentication;
using System.Web;
using System.Web.Mvc;

namespace PowerLines.Controllers
{
    [Authorize]
    public class TrackerController : Controller
    {
        PowerLinesContext db;
        static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public TrackerController()
        {
            this.db = new PowerLinesContext();
        }

        public TrackerController(PowerLinesContext context)
        {
            this.db = context;
        }

        // GET: Tracker
        public ActionResult Index()
        {
            var userId = User.Identity.GetUserId();

            return View(db.Trackers.Where(x => x.UserId == userId).OrderByDescending(x => x.Date).ThenBy(x => x.Result == null ? 0 : 1).ThenBy(x => x.Division).ThenBy(x => x.HomeTeam).ToList());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(List<Tracker> trackers)
        {
            try
            {
                var userId = User.Identity.GetUserId();

                foreach (var tracker in trackers)
                {
                    var original = db.Trackers.Where(x => x.UserId == userId && x.TrackerId == tracker.TrackerId).FirstOrDefault();
                    var bankId = db.Banks.Where(x => x.UserId == userId).Select(x => x.BankId).FirstOrDefault();

                    if (tracker.Result != original.Result)
                    {
                        if (original.Result == null || (original.Result != null && original.Result != original.Predicted))
                        {
                            if (tracker.Result == original.Predicted)
                            {
                                Transaction transaction = new Transaction(bankId, DateTime.Now, string.Format("Winnings received for {0} v {1} - ({2})", original.HomeTeam, original.AwayTeam, original.Description), original.Potential);
                                db.Transactions.Add(transaction);
                            }
                        }
                        else
                        {
                            Transaction transaction = new Transaction(bankId, DateTime.Now, string.Format("Correction for {0} v {1} - ({2})", original.HomeTeam, original.AwayTeam, original.Description), original.Potential * -1);
                            db.Transactions.Add(transaction);
                        }

                        original.Result = tracker.Result;
                    }
                }

                db.SaveChanges();

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                log.Error(string.Format("Error saving tracker data"), ex);
                throw;
            }
        }

        public ActionResult Betslip(int fixtureId, string outcome, decimal odds)
        {
            var userId = User.Identity.GetUserId();
            var bank = db.Banks.AsNoTracking().Where(x => x.UserId == userId).FirstOrDefault();

            return View("_Betslip", new Betslip(db.Fixtures.Find(fixtureId), outcome, odds));
        }

        [HttpPost]
        public ActionResult Betslip(int fixtureId, string outcome, decimal marketOdds, decimal odds, decimal balance)
        {
            try
            {
                var userId = User.Identity.GetUserId();

                Tracker tracker = new Tracker(db.Fixtures.Find(fixtureId), outcome, marketOdds, odds, balance, 30, userId);
                db.Trackers.Add(tracker);

                var bankId = db.Banks.Where(x => x.UserId == tracker.UserId).Select(x => x.BankId).FirstOrDefault();

                Transaction transaction = new Transaction(bankId, DateTime.Now, string.Format("Bet placed on {0} v {1} - ({2})", tracker.HomeTeam, tracker.AwayTeam, tracker.Description), tracker.Stake * -1);
                db.Transactions.Add(transaction);
                db.SaveChanges();

                ViewBag.Message = "Bet placed successfully";
                return View("_Confirmation");
            }
            catch (Exception)
            {
                ViewBag.Message = "Unable to place bet";
                return View("_Confirmation");
            }
        }

        public ActionResult Recommendation(string outcome, decimal marketOdds, decimal odds)
        {
            var userId = User.Identity.GetUserId();
            var bank = db.Banks.Where(x => x.UserId == userId).FirstOrDefault();

            return View("_Recommendation", new Tracker(outcome, marketOdds, odds, bank.Balance, 30));
        }

        [HttpPost]
        public ActionResult Delete(int trackerId)
        {
            var tracker = db.Trackers.Find(trackerId);

            if (User.Identity.GetUserId() != tracker.UserId)
            {
                throw new AuthenticationException("Invalid request");
            }

            var bankId = db.Banks.Where(x => x.UserId == tracker.UserId).Select(x => x.BankId).FirstOrDefault();

            Transaction transaction = new Transaction(bankId, DateTime.Now, string.Format("Refund for deleted tracked bet - {0} v {1}", tracker.HomeTeam, tracker.AwayTeam), tracker.Result != null && tracker.Result == tracker.Predicted ? tracker.Potential : tracker.Stake);
            db.Transactions.Add(transaction);
            db.Trackers.Remove(tracker);
            db.SaveChanges();

            return Content("Ok");
        }
    }
}