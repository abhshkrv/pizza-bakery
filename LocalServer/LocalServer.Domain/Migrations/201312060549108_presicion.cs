namespace LocalServer.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class presicion : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.TransactionDetails", "cost", c => c.Double(nullable: false));
            AlterColumn("dbo.Products", "sellingPrice", c => c.Double(nullable: false));
            AlterColumn("dbo.Products", "maxPrice", c => c.Double(nullable: false));
            AlterColumn("dbo.CRSessions", "startTime", c => c.DateTime());
            AlterColumn("dbo.CRSessions", "endTime", c => c.DateTime());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.CRSessions", "endTime", c => c.DateTime(nullable: false));
            AlterColumn("dbo.CRSessions", "startTime", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Products", "maxPrice", c => c.Single(nullable: false));
            AlterColumn("dbo.Products", "sellingPrice", c => c.Single(nullable: false));
            AlterColumn("dbo.TransactionDetails", "cost", c => c.Single(nullable: false));
        }
    }
}
