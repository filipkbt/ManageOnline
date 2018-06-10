namespace ManageOnline.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addFileModel : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.FileModels",
                c => new
                    {
                        FileId = c.Int(nullable: false, identity: true),
                        FilePath = c.String(),
                        DateUploadFile = c.DateTime(nullable: false),
                        ProjectId_ProjectId = c.Int(),
                        UserWhoAddFile_UserId = c.Int(),
                    })
                .PrimaryKey(t => t.FileId)
                .ForeignKey("dbo.ProjectModels", t => t.ProjectId_ProjectId)
                .ForeignKey("dbo.UserBasicModels", t => t.UserWhoAddFile_UserId)
                .Index(t => t.ProjectId_ProjectId)
                .Index(t => t.UserWhoAddFile_UserId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.FileModels", "UserWhoAddFile_UserId", "dbo.UserBasicModels");
            DropForeignKey("dbo.FileModels", "ProjectId_ProjectId", "dbo.ProjectModels");
            DropIndex("dbo.FileModels", new[] { "UserWhoAddFile_UserId" });
            DropIndex("dbo.FileModels", new[] { "ProjectId_ProjectId" });
            DropTable("dbo.FileModels");
        }
    }
}
