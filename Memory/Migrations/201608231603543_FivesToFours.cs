namespace Memory.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FivesToFours : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Stats", "isFour", c => c.Boolean(nullable: false));
            DropColumn("dbo.Stats", "isFive");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Stats", "isFive", c => c.Boolean(nullable: false));
            DropColumn("dbo.Stats", "isFour");
        }
    }
}