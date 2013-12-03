namespace LocalServer.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class nullable : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.CRSessions", "startTime", c => c.DateTime());
            AlterColumn("dbo.CRSessions", "endTime", c => c.DateTime());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.CRSessions", "endTime", c => c.DateTime(nullable: false));
            AlterColumn("dbo.CRSessions", "startTime", c => c.DateTime(nullable: false));
        }
    }
}
