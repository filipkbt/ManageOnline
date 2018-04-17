namespace ManageOnline.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddBoolsToProjectModel : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ProjectModels", "IsProjectAllowedToSomeone", c => c.Boolean(nullable: false));
            AddColumn("dbo.ProjectModels", "IsProjectInProgress", c => c.Boolean(nullable: false));
            AddColumn("dbo.ProjectModels", "IsProjectFinished", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.ProjectModels", "IsProjectFinished");
            DropColumn("dbo.ProjectModels", "IsProjectInProgress");
            DropColumn("dbo.ProjectModels", "IsProjectAllowedToSomeone");
        }
    }
}
