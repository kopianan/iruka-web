namespace Iruka.EF.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class add_priority : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Events", "Priority", c => c.Int(nullable: false));
            AddColumn("dbo.Products", "Priority", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Products", "Priority");
            DropColumn("dbo.Events", "Priority");
        }
    }
}
