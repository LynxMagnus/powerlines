namespace PowerLines.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddTeamDataset : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Datasets", "HomeTeam", c => c.String());
            AddColumn("dbo.Datasets", "AwayTeam", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Datasets", "AwayTeam");
            DropColumn("dbo.Datasets", "HomeTeam");
        }
    }
}
