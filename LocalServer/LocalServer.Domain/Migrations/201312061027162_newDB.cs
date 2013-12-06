namespace LocalServer.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class newDB : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.BatchRequests",
                c => new
                    {
                        batchRequestID = c.Int(nullable: false, identity: true),
                        timeStamp = c.DateTime(nullable: false),
                        status = c.Byte(nullable: false),
                    })
                .PrimaryKey(t => t.batchRequestID);
            
            CreateTable(
                "dbo.BatchRequestDetails",
                c => new
                    {
                        batchRequestID = c.Int(nullable: false),
                        barcode = c.String(nullable: false, maxLength: 128),
                        quantity = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.batchRequestID, t.barcode });
            
            CreateTable(
                "dbo.Categories",
                c => new
                    {
                        categoryID = c.Int(nullable: false),
                        categoryName = c.String(),
                    })
                .PrimaryKey(t => t.categoryID);
            
            CreateTable(
                "dbo.CashRegisters",
                c => new
                    {
                        cashRegisterID = c.Int(nullable: false),
                        status = c.Byte(nullable: false),
                    })
                .PrimaryKey(t => t.cashRegisterID);
            
            CreateTable(
                "dbo.Employees",
                c => new
                    {
                        employeeID = c.Int(nullable: false, identity: true),
                        hashID = c.String(),
                        role = c.String(),
                        userID = c.String(),
                        password = c.String(),
                    })
                .PrimaryKey(t => t.employeeID);
            
            CreateTable(
                "dbo.Manufacturers",
                c => new
                    {
                        manufacturerID = c.Int(nullable: false),
                        manufacturerName = c.String(),
                    })
                .PrimaryKey(t => t.manufacturerID);
            
            CreateTable(
                "dbo.Transactions",
                c => new
                    {
                        transactionID = c.Int(nullable: false),
                        date = c.DateTime(nullable: false),
                        cashierID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.transactionID);
            
            CreateTable(
                "dbo.TransactionDetails",
                c => new
                    {
                        transactionID = c.Int(nullable: false),
                        barcode = c.String(nullable: false, maxLength: 128),
                        unitSold = c.Int(nullable: false),
                        cost = c.Double(nullable: false),
                    })
                .PrimaryKey(t => new { t.transactionID, t.barcode });
            
            CreateTable(
                "dbo.Products",
                c => new
                    {
                        productID = c.Int(nullable: false, identity: true),
                        productName = c.String(),
                        barcode = c.String(),
                        categoryID = c.Int(nullable: false),
                        manufacturerID = c.Int(nullable: false),
                        sellingPrice = c.Double(nullable: false),
                        maxPrice = c.Double(nullable: false),
                        currentStock = c.Int(nullable: false),
                        minimumStock = c.Int(nullable: false),
                        bundleUnit = c.Int(nullable: false),
                        discountPercentage = c.Single(nullable: false),
                    })
                .PrimaryKey(t => t.productID);
            
            CreateTable(
                "dbo.PriceDisplays",
                c => new
                    {
                        priceDisplayID = c.Int(nullable: false),
                        barcode = c.String(),
                        status = c.Byte(nullable: false),
                    })
                .PrimaryKey(t => t.priceDisplayID);
            
            CreateTable(
                "dbo.CRSessions",
                c => new
                    {
                        CRsessionID = c.Int(nullable: false, identity: true),
                        cashRegister = c.String(),
                        userID = c.String(),
                        startTime = c.DateTime(),
                        endTime = c.DateTime(),
                    })
                .PrimaryKey(t => t.CRsessionID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.CRSessions");
            DropTable("dbo.PriceDisplays");
            DropTable("dbo.Products");
            DropTable("dbo.TransactionDetails");
            DropTable("dbo.Transactions");
            DropTable("dbo.Manufacturers");
            DropTable("dbo.Employees");
            DropTable("dbo.CashRegisters");
            DropTable("dbo.Categories");
            DropTable("dbo.BatchRequestDetails");
            DropTable("dbo.BatchRequests");
        }
    }
}
