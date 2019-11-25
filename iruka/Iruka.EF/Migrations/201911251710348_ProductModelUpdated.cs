namespace Iruka.EF.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ProductModelUpdated : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Products", "EventStatus", c => c.Int(nullable: false));
            AlterColumn("dbo.Products", "Priority", c => c.Int());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Products", "Priority", c => c.Int(nullable: false));
            DropColumn("dbo.Products", "EventStatus");
        }
    }
}
