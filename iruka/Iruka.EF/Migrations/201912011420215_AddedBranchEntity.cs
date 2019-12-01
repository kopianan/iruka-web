namespace Iruka.EF.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedBranchEntity : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Branches",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        BranchName = c.String(),
                        Address = c.String(),
                        City = c.String(),
                        PostalCode = c.String(),
                        Email = c.String(),
                        PhoneAreaCode = c.String(),
                        Phone = c.String(),
                        MobilePhone = c.String(),
                        Whatsapp = c.String(),
                        PointRate = c.Decimal(nullable: false, precision: 18, scale: 2),
                        CreatedDate = c.DateTime(nullable: false),
                        CreatedBy = c.String(),
                        ModifiedDate = c.DateTime(nullable: false),
                        ModifiedBy = c.String(),
                        IsActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Branches");
        }
    }
}
