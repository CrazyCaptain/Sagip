namespace Greenpeace_Advisory.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class init1 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.AuditTrails", "AdvisoryId", "dbo.Advisories");
            DropIndex("dbo.AuditTrails", new[] { "AdvisoryId" });
            AddColumn("dbo.Advisories", "Username", c => c.String());
            DropTable("dbo.AuditTrails");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.AuditTrails",
                c => new
                    {
                        AuditTrailID = c.Int(nullable: false, identity: true),
                        Username = c.String(),
                        AdvisoryId = c.Int(nullable: false),
                        TimeStamp = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.AuditTrailID);
            
            DropColumn("dbo.Advisories", "Username");
            CreateIndex("dbo.AuditTrails", "AdvisoryId");
            AddForeignKey("dbo.AuditTrails", "AdvisoryId", "dbo.Advisories", "AdvisoryId", cascadeDelete: true);
        }
    }
}
