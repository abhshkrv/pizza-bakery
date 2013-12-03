namespace LocalServer.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class session : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Employees",
                c => new
                    {
                        employeeID = c.Int(nullable: false, identity: true),
                    })
                .PrimaryKey(t => t.employeeID);
            
            CreateTable(
                "dbo.Sessions",
                c => new
                    {
                        sessionID = c.Int(nullable: false, identity: true),
                        cashRegister = c.String(),
                        userID = c.String(),
                        startTime = c.DateTime(nullable: false),
                        endTime = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.sessionID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Sessions");
            DropTable("dbo.Employees");
        }
    }
}
