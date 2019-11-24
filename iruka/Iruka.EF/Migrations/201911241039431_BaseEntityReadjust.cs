namespace Iruka.EF.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class BaseEntityReadjust : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Coupons", "CreatedBy", c => c.String());
            AddColumn("dbo.Coupons", "ModifiedDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.Coupons", "ModifiedBy", c => c.String());
            AddColumn("dbo.Coupons", "IsActive", c => c.Boolean(nullable: false));
            AddColumn("dbo.Events", "CreatedBy", c => c.String());
            AddColumn("dbo.Events", "ModifiedDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.Events", "ModifiedBy", c => c.String());
            AddColumn("dbo.Products", "CreatedBy", c => c.String());
            AddColumn("dbo.Products", "ModifiedDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.Products", "ModifiedBy", c => c.String());
            AddColumn("dbo.Transactions", "CreatedBy", c => c.String());
            AddColumn("dbo.Transactions", "ModifiedDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.Transactions", "ModifiedBy", c => c.String());
            DropTable("dbo.Tests");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.Tests",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Value = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            DropColumn("dbo.Transactions", "ModifiedBy");
            DropColumn("dbo.Transactions", "ModifiedDate");
            DropColumn("dbo.Transactions", "CreatedBy");
            DropColumn("dbo.Products", "ModifiedBy");
            DropColumn("dbo.Products", "ModifiedDate");
            DropColumn("dbo.Products", "CreatedBy");
            DropColumn("dbo.Events", "ModifiedBy");
            DropColumn("dbo.Events", "ModifiedDate");
            DropColumn("dbo.Events", "CreatedBy");
            DropColumn("dbo.Coupons", "IsActive");
            DropColumn("dbo.Coupons", "ModifiedBy");
            DropColumn("dbo.Coupons", "ModifiedDate");
            DropColumn("dbo.Coupons", "CreatedBy");
        }
    }
}
