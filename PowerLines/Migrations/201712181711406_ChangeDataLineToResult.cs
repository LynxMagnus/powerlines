namespace PowerLines.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangeDataLineToResult : DbMigration
    {
        public override void Up()
        {
            RenameColumn("dbo.DataLines", "DataLineId", "ResultId");
            RenameTable("dbo.DataLines", "Results");
        }
        
        public override void Down()
        {
            RenameColumn("dbo.Results", "ResultId", "DataLineId");
            RenameTable("dbo.Results", "DataLines");
        }
    }
}
