namespace Iruka.EF.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CouponModelRevamp : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Coupons", "PointPrice", c => c.Int(nullable: false));
            AddColumn("dbo.Coupons", "Amount", c => c.Int(nullable: false));
            AddColumn("dbo.Coupons", "Purchased", c => c.Int(nullable: false));
            AddColumn("dbo.Coupons", "DiscountType", c => c.Int(nullable: false));
            AddColumn("dbo.Coupons", "DiscountTotal", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.Coupons", "DiscountValue", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            DropColumn("dbo.Coupons", "ServiceType");
            DropColumn("dbo.Coupons", "MaxPoint");
            DropColumn("dbo.Coupons", "Point");
            DropColumn("dbo.Coupons", "Bonus");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Coupons", "Bonus", c => c.String());
            AddColumn("dbo.Coupons", "Point", c => c.Int(nullable: false));
            AddColumn("dbo.Coupons", "MaxPoint", c => c.Int(nullable: false));
            AddColumn("dbo.Coupons", "ServiceType", c => c.String());
            DropColumn("dbo.Coupons", "DiscountValue");
            DropColumn("dbo.Coupons", "DiscountTotal");
            DropColumn("dbo.Coupons", "DiscountType");
            DropColumn("dbo.Coupons", "Purchased");
            DropColumn("dbo.Coupons", "Amount");
            DropColumn("dbo.Coupons", "PointPrice");
        }
    }
}
