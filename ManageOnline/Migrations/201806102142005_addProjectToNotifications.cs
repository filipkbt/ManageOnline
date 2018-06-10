namespace ManageOnline.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addProjectToNotifications : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.NotificationModels", "Project_ProjectId", c => c.Int());
            CreateIndex("dbo.NotificationModels", "Project_ProjectId");
            AddForeignKey("dbo.NotificationModels", "Project_ProjectId", "dbo.ProjectModels", "ProjectId");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.NotificationModels", "Project_ProjectId", "dbo.ProjectModels");
            DropIndex("dbo.NotificationModels", new[] { "Project_ProjectId" });
            DropColumn("dbo.NotificationModels", "Project_ProjectId");
        }
    }
}
