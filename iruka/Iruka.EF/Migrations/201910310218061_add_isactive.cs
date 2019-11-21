namespace Iruka.EF.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class add_isactive : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Events", "isActive", c => c.Boolean(nullable: false));
            AddColumn("dbo.Products", "isActive", c => c.Boolean(nullable: false));
            AddColumn("dbo.AspNetUsers", "isActive", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "isActive");
            DropColumn("dbo.Products", "isActive");
            DropColumn("dbo.Events", "isActive");
        }
    }
}
