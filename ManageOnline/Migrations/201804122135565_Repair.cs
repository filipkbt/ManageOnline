namespace ManageOnline.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Repair : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.ProjectModels", "ProjectTitle", c => c.String(nullable: false));
            AlterColumn("dbo.ProjectModels", "ProjectDescription", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.ProjectModels", "ProjectDescription", c => c.String());
            AlterColumn("dbo.ProjectModels", "ProjectTitle", c => c.String());
        }
    }
}
