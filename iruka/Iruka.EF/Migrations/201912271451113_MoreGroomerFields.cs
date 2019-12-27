namespace Iruka.EF.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MoreGroomerFields : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "YearsOfExperience", c => c.Int(nullable: false));
            AddColumn("dbo.AspNetUsers", "Availability", c => c.Boolean(nullable: false));
            AddColumn("dbo.AspNetUsers", "Styling", c => c.Int(nullable: false));
            AddColumn("dbo.AspNetUsers", "Clipping", c => c.Int(nullable: false));
            AddColumn("dbo.AspNetUsers", "KeyFeatures", c => c.String());
            AddColumn("dbo.AspNetUsers", "CoverageArea", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "CoverageArea");
            DropColumn("dbo.AspNetUsers", "KeyFeatures");
            DropColumn("dbo.AspNetUsers", "Clipping");
            DropColumn("dbo.AspNetUsers", "Styling");
            DropColumn("dbo.AspNetUsers", "Availability");
            DropColumn("dbo.AspNetUsers", "YearsOfExperience");
        }
    }
}
