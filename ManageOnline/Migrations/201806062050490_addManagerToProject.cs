namespace ManageOnline.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addManagerToProject : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ProjectModels", "Manager_UserId", c => c.Int());
            CreateIndex("dbo.ProjectModels", "Manager_UserId");
            AddForeignKey("dbo.ProjectModels", "Manager_UserId", "dbo.UserBasicModels", "UserId");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ProjectModels", "Manager_UserId", "dbo.UserBasicModels");
            DropIndex("dbo.ProjectModels", new[] { "Manager_UserId" });
            DropColumn("dbo.ProjectModels", "Manager_UserId");
        }
    }
}
