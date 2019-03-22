using Microsoft.AspNet.Identity.EntityFramework;
using PowerLines.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace PowerLines.DAL
{
    public class PowerLinesContext : IdentityDbContext<ApplicationUser>
    {
        public PowerLinesContext()
            : base("PowerLinesContext", throwIfV1Schema: false)
        {
        }

        public DbSet<Result> Results { get; set; }

        public DbSet<Fixture> Fixtures { get; set; }

        public DbSet<Dataset> Datasets { get; set; }

        public DbSet<Bank> Banks { get; set; }

        public DbSet<Transaction> Transactions { get; set; }

        public DbSet<Tracker> Trackers { get; set; }
        
        public static PowerLinesContext Create()
        {
            return new PowerLinesContext();
        }    
    }
}