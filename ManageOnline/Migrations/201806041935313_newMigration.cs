namespace ManageOnline.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class newMigration : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ProjectModels", "ProjectResponsibilities", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.ProjectModels", "ProjectResponsibilities");
        }
    }
}
