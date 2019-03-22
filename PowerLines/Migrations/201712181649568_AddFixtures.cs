namespace PowerLines.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddFixtures : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Fixtures",
                c => new
                    {
                        FixtureId = c.Int(nullable: false, identity: true),
                        Division = c.String(),
                        Date = c.DateTime(nullable: false),
                        HomeTeam = c.String(),
                        AwayTeam = c.String(),
                    })
                .PrimaryKey(t => t.FixtureId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Fixtures");
        }
    }
}
