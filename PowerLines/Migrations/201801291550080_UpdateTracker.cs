namespace PowerLines.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateTracker : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Trackers", "Result", c => c.String());
            AlterColumn("dbo.Trackers", "Predicted", c => c.String());
            DropColumn("dbo.Trackers", "Outcome");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Trackers", "Outcome", c => c.Int());
            AlterColumn("dbo.Trackers", "Predicted", c => c.Int(nullable: false));
            DropColumn("dbo.Trackers", "Result");
        }
    }
}
