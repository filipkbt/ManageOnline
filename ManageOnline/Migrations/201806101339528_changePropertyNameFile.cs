namespace ManageOnline.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class changePropertyNameFile : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.FileModels", name: "ProjectId_ProjectId", newName: "Project_ProjectId");
            RenameIndex(table: "dbo.FileModels", name: "IX_ProjectId_ProjectId", newName: "IX_Project_ProjectId");
        }
        
        public override void Down()
        {
            RenameIndex(table: "dbo.FileModels", name: "IX_Project_ProjectId", newName: "IX_ProjectId_ProjectId");
            RenameColumn(table: "dbo.FileModels", name: "Project_ProjectId", newName: "ProjectId_ProjectId");
        }
    }
}
