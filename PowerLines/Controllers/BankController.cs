using Microsoft.AspNet.Identity;
using PowerLines.DAL;
using PowerLines.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PowerLines.Controllers
{
    [Authorize]
    public class BankController : Controller
    {
        PowerLinesContext db;

        public BankController()
        {
            this.db = new PowerLinesContext();
        }

        public BankController(PowerLinesContext context)
        {
            this.db = context;
        }

        // GET: Bank
        public ActionResult Index()
        {
            var userId = User.Identity.GetUserId();

            return View(db.Banks.Where(x => x.UserId == userId).FirstOrDefault());
        }

        public ActionResult _CurrentBalance()
        {
            var userId = User.Identity.GetUserId();
            ViewBag.Balance = db.Banks.AsNoTracking().Where(x => x.UserId == userId).FirstOrDefault()?.Balance;

            return View();
        }

        public ActionResult Transaction(int bankId, string type)
        {
            ViewBag.Type = type;
            return View(new Transaction(bankId));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Transaction(Transaction transaction, string type)
        {
            if (ModelState.IsValid)
            {
                var userId = User.Identity.GetUserId();
                var bank = db.Banks.Where(x => x.UserId == userId).FirstOrDefault();

                transaction.BankId = bank.BankId;
                transaction.Date = DateTime.Now;
                switch(type)
                {
                    case "Deposit":
                        transaction.Description = "Funds deposited";
                        break;
                    case "Withdraw":
                        transaction.Value = transaction.Value > bank.Balance ? bank.Balance * -1 : transaction.Value * -1;
                        transaction.Description = "Funds withdrawn";
                        break;
                    default:
                        break;
                }
                db.Transactions.Add(transaction);
                db.SaveChanges();

                return RedirectToAction("Index");
            }

            ViewBag.Type = type;
            return View(transaction);
        }
    }
}