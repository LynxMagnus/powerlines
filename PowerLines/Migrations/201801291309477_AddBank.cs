namespace PowerLines.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddBank : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Banks",
                c => new
                    {
                        BankId = c.Int(nullable: false, identity: true),
                        UserId = c.String(),
                    })
                .PrimaryKey(t => t.BankId);
            
            CreateTable(
                "dbo.Transactions",
                c => new
                    {
                        TransactionId = c.Int(nullable: false, identity: true),
                        BankId = c.Int(nullable: false),
                        Date = c.DateTime(nullable: false),
                        Description = c.String(),
                        Value = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.TransactionId)
                .ForeignKey("dbo.Banks", t => t.BankId, cascadeDelete: true)
                .Index(t => t.BankId);
            
            CreateTable(
                "dbo.Trackers",
                c => new
                    {
                        TrackerId = c.Int(nullable: false, identity: true),
                        UserId = c.String(),
                        FixtureId = c.Int(nullable: false),
                        Bank = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Reducer = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Predicted = c.Int(nullable: false),
                        MarketOdds = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Odds = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Outcome = c.Int(),
                    })
                .PrimaryKey(t => t.TrackerId)
                .ForeignKey("dbo.Fixtures", t => t.FixtureId, cascadeDelete: true)
                .Index(t => t.FixtureId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Trackers", "FixtureId", "dbo.Fixtures");
            DropForeignKey("dbo.Transactions", "BankId", "dbo.Banks");
            DropIndex("dbo.Trackers", new[] { "FixtureId" });
            DropIndex("dbo.Transactions", new[] { "BankId" });
            DropTable("dbo.Trackers");
            DropTable("dbo.Transactions");
            DropTable("dbo.Banks");
        }
    }
}
