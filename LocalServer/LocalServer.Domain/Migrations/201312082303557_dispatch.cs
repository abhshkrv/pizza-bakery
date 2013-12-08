namespace LocalServer.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class dispatch : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.BatchDispatches",
                c => new
                    {
                        batchDispatchID = c.Int(nullable: false, identity: true),
                        timeStamp = c.DateTime(nullable: false),
                        status = c.Byte(nullable: false),
                    })
                .PrimaryKey(t => t.batchDispatchID);
            
            CreateTable(
                "dbo.BatchDispatchDetails",
                c => new
                    {
                        batchDispatchID = c.Int(nullable: false),
                        barcode = c.String(nullable: false, maxLength: 128),
                        quantity = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.batchDispatchID, t.barcode });
            
        }
        
        public override void Down()
        {
            DropTable("dbo.BatchDispatchDetails");
            DropTable("dbo.BatchDispatches");
        }
    }
}
