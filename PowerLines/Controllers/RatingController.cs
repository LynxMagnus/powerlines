using Microsoft.AspNet.Identity;
using PowerLines.BulkInsert;
using PowerLines.DAL;
using PowerLines.Models;
using PowerLines.Models.ViewModels;
using PowerLines.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PowerLines.Controllers
{
    [Authorize(Roles = "User")]
    public class RatingController : Controller
    {
        PowerLinesContext db;        
        IRatingService ratingService;

        public RatingController()
        {
            db = new PowerLinesContext();
            ratingService = new RatingService(db, new SqlBulkInsert<Dataset>(db));
        }

        [OutputCache(Duration = 60, VaryByParam = "none")]
        public ActionResult Index()
        {               
            return View(ratingService.Calculate());
        }

        [Authorize(Roles = "Admin")]
        public ActionResult Analysis(bool reset = false)
        {
            return View(new Analysis(ratingService.Calculate(true, reset)));
        }        
    }
}