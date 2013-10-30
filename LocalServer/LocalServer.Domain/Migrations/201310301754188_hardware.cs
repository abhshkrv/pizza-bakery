namespace LocalServer.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class hardware : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CashRegisters",
                c => new
                    {
                        cashRegisterID = c.Int(nullable: false),
                        status = c.Byte(nullable: false),
                    })
                .PrimaryKey(t => t.cashRegisterID);
            
            CreateTable(
                "dbo.PriceDisplays",
                c => new
                    {
                        priceDisplayID = c.Int(nullable: false),
                        barcode = c.String(),
                        status = c.Byte(nullable: false),
                    })
                .PrimaryKey(t => t.priceDisplayID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.PriceDisplays");
            DropTable("dbo.CashRegisters");
        }
    }
}
