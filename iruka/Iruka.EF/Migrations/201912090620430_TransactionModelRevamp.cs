namespace Iruka.EF.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TransactionModelRevamp : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Transactions", "CustomerId", c => c.Guid(nullable: false));
            AddColumn("dbo.Transactions", "CouponId", c => c.Guid());
            AddColumn("dbo.Transactions", "SubTotal", c => c.Double(nullable: false));
            AddColumn("dbo.Transactions", "Total", c => c.Double(nullable: false));
            AddColumn("dbo.Transactions", "Notes", c => c.String());
            AddColumn("dbo.Transactions", "EarnedPoint", c => c.Int(nullable: false));
            CreateIndex("dbo.Transactions", "CouponId");
            AddForeignKey("dbo.Transactions", "CouponId", "dbo.Coupons", "Id");
            DropColumn("dbo.Transactions", "ServiceType");
            DropColumn("dbo.Transactions", "Description");
            DropColumn("dbo.Transactions", "Point");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Transactions", "Point", c => c.Int(nullable: false));
            AddColumn("dbo.Transactions", "Description", c => c.String());
            AddColumn("dbo.Transactions", "ServiceType", c => c.String());
            DropForeignKey("dbo.Transactions", "CouponId", "dbo.Coupons");
            DropIndex("dbo.Transactions", new[] { "CouponId" });
            DropColumn("dbo.Transactions", "EarnedPoint");
            DropColumn("dbo.Transactions", "Notes");
            DropColumn("dbo.Transactions", "Total");
            DropColumn("dbo.Transactions", "SubTotal");
            DropColumn("dbo.Transactions", "CouponId");
            DropColumn("dbo.Transactions", "CustomerId");
        }
    }
}
