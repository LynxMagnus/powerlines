namespace PowerLines.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemoveFixtureFromTracker : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Trackers", "FixtureId", "dbo.Fixtures");
            DropIndex("dbo.Trackers", new[] { "FixtureId" });
            AddColumn("dbo.Trackers", "Date", c => c.DateTime(nullable: false));
            AddColumn("dbo.Trackers", "Division", c => c.String());
            AddColumn("dbo.Trackers", "HomeTeam", c => c.String());
            AddColumn("dbo.Trackers", "AwayTeam", c => c.String());
            DropColumn("dbo.Trackers", "FixtureId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Trackers", "FixtureId", c => c.Int(nullable: false));
            DropColumn("dbo.Trackers", "AwayTeam");
            DropColumn("dbo.Trackers", "HomeTeam");
            DropColumn("dbo.Trackers", "Division");
            DropColumn("dbo.Trackers", "Date");
            CreateIndex("dbo.Trackers", "FixtureId");
            AddForeignKey("dbo.Trackers", "FixtureId", "dbo.Fixtures", "FixtureId", cascadeDelete: true);
        }
    }
}
