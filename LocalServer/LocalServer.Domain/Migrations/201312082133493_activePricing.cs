namespace LocalServer.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class activePricing : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Products", "temporaryStock", c => c.Int(nullable: false));
            AddColumn("dbo.Products", "afterUpdateStock", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Products", "afterUpdateStock");
            DropColumn("dbo.Products", "temporaryStock");
        }
    }
}
