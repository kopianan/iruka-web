namespace Iruka.EF.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ProductAddedToCoupon : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Coupons", "CouponType", c => c.Int(nullable: false));
            AddColumn("dbo.Coupons", "FreeProduct", c => c.String());
            DropColumn("dbo.Coupons", "DiscountTotal");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Coupons", "DiscountTotal", c => c.Double(nullable: false));
            DropColumn("dbo.Coupons", "FreeProduct");
            DropColumn("dbo.Coupons", "CouponType");
        }
    }
}
