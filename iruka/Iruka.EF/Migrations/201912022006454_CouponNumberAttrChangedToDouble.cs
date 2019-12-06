namespace Iruka.EF.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CouponNumberAttrChangedToDouble : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Coupons", "DiscountValue", c => c.Double(nullable: false));
            AlterColumn("dbo.Coupons", "DiscountTotal", c => c.Double(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Coupons", "DiscountTotal", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.Coupons", "DiscountValue", c => c.Decimal(nullable: false, precision: 18, scale: 2));
        }
    }
}
