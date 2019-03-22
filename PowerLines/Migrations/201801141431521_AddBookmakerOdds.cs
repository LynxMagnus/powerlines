namespace PowerLines.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddBookmakerOdds : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Fixtures", "B365H", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.Fixtures", "B365D", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.Fixtures", "B365A", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.Fixtures", "BWH", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.Fixtures", "BWD", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.Fixtures", "BWA", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.Fixtures", "IWH", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.Fixtures", "IWD", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.Fixtures", "IWA", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.Fixtures", "LBH", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.Fixtures", "LBD", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.Fixtures", "LBA", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.Fixtures", "PSH", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.Fixtures", "PSD", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.Fixtures", "PSA", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.Fixtures", "WHH", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.Fixtures", "WHD", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.Fixtures", "WHA", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.Fixtures", "VCH", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.Fixtures", "VCD", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.Fixtures", "VCA", c => c.Decimal(nullable: false, precision: 18, scale: 2));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Fixtures", "VCA");
            DropColumn("dbo.Fixtures", "VCD");
            DropColumn("dbo.Fixtures", "VCH");
            DropColumn("dbo.Fixtures", "WHA");
            DropColumn("dbo.Fixtures", "WHD");
            DropColumn("dbo.Fixtures", "WHH");
            DropColumn("dbo.Fixtures", "PSA");
            DropColumn("dbo.Fixtures", "PSD");
            DropColumn("dbo.Fixtures", "PSH");
            DropColumn("dbo.Fixtures", "LBA");
            DropColumn("dbo.Fixtures", "LBD");
            DropColumn("dbo.Fixtures", "LBH");
            DropColumn("dbo.Fixtures", "IWA");
            DropColumn("dbo.Fixtures", "IWD");
            DropColumn("dbo.Fixtures", "IWH");
            DropColumn("dbo.Fixtures", "BWA");
            DropColumn("dbo.Fixtures", "BWD");
            DropColumn("dbo.Fixtures", "BWH");
            DropColumn("dbo.Fixtures", "B365A");
            DropColumn("dbo.Fixtures", "B365D");
            DropColumn("dbo.Fixtures", "B365H");
        }
    }
}
