namespace Greenpeace_Advisory.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class init : DbMigration
    {
        public override void Up()
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
                .PrimaryKey(t => t.AuditTrailID)
                .ForeignKey("dbo.Advisories", t => t.AdvisoryId, cascadeDelete: true)
                .Index(t => t.AdvisoryId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AuditTrails", "AdvisoryId", "dbo.Advisories");
            DropIndex("dbo.AuditTrails", new[] { "AdvisoryId" });
            DropTable("dbo.AuditTrails");
        }
    }
}
