namespace LocalServer.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _decimal : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.TransactionDetails", "cost", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.Products", "sellingPrice", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.Products", "maxPrice", c => c.Decimal(nullable: false, precision: 18, scale: 2));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Products", "maxPrice", c => c.Double(nullable: false));
            AlterColumn("dbo.Products", "sellingPrice", c => c.Double(nullable: false));
            AlterColumn("dbo.TransactionDetails", "cost", c => c.Double(nullable: false));
        }
    }
}
