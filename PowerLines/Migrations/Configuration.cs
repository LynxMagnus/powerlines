namespace PowerLines.Migrations
{
    using Microsoft.AspNet.Identity.EntityFramework;
    using PowerLines.Models;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<PowerLines.DAL.PowerLinesContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(PowerLines.DAL.PowerLinesContext context)
        {
            context.Roles.AddOrUpdate(
              p => p.Name,
              new IdentityRole { Name = "Admin" },
              new IdentityRole { Name = "User" }
            );
        }
    }
}
