namespace LocalServer.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class namechange : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CRSessions",
                c => new
                    {
                        CRsessionID = c.Int(nullable: false, identity: true),
                        cashRegister = c.String(),
                        userID = c.String(),
                        startTime = c.DateTime(nullable: false),
                        endTime = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.CRsessionID);
            
            AddColumn("dbo.Employees", "hashID", c => c.String());
            AddColumn("dbo.Employees", "role", c => c.String());
            AddColumn("dbo.Employees", "userID", c => c.String());
            AddColumn("dbo.Employees", "password", c => c.String());
            DropTable("dbo.Sessions");
        }
        
        public override void Down()
        {
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
            
            DropColumn("dbo.Employees", "password");
            DropColumn("dbo.Employees", "userID");
            DropColumn("dbo.Employees", "role");
            DropColumn("dbo.Employees", "hashID");
            DropTable("dbo.CRSessions");
        }
    }
}
