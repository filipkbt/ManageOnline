namespace ManageOnline.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class littleTaskEdit : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.TaskModels", name: "ProjectId_ProjectId", newName: "Project_ProjectId");
            RenameIndex(table: "dbo.TaskModels", name: "IX_ProjectId_ProjectId", newName: "IX_Project_ProjectId");
        }
        
        public override void Down()
        {
            RenameIndex(table: "dbo.TaskModels", name: "IX_Project_ProjectId", newName: "IX_ProjectId_ProjectId");
            RenameColumn(table: "dbo.TaskModels", name: "Project_ProjectId", newName: "ProjectId_ProjectId");
        }
    }
}
