namespace PowerLines.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddDatasetDate : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Datasets", "Date", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Datasets", "Date");
        }
    }
}
