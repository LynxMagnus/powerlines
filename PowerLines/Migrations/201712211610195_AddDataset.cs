namespace PowerLines.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddDataset : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Datasets",
                c => new
                    {
                        DatasetId = c.Int(nullable: false, identity: true),
                        ResultId = c.Int(nullable: false),
                        Division = c.String(),
                        HomeSuperiority = c.Int(nullable: false),
                        AwaySuperiority = c.Int(nullable: false),
                        Actual = c.String(),
                    })
                .PrimaryKey(t => t.DatasetId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Datasets");
        }
    }
}
