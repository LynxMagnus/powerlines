namespace PowerLines.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddBTTS : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Datasets", "HomeGoals", c => c.Int(nullable: false));
            AddColumn("dbo.Datasets", "AwayGoals", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Datasets", "AwayGoals");
            DropColumn("dbo.Datasets", "HomeGoals");
        }
    }
}
