namespace ManageOnline.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddScrumSprintModel : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ScrumSprintModels",
                c => new
                    {
                        ScrumSprintId = c.Int(nullable: false, identity: true),
                        StartScrumSprintDate = c.DateTime(nullable: false),
                        FinishScrumSprintDate = c.DateTime(nullable: false),
                        ProjectId_ProjectId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ScrumSprintId)
                .ForeignKey("dbo.ProjectModels", t => t.ProjectId_ProjectId, cascadeDelete: true)
                .Index(t => t.ProjectId_ProjectId);
            
            AddColumn("dbo.ProjectModels", "ProjectManagementMethodology", c => c.Int(nullable: false));
            AddColumn("dbo.TaskModels", "ScrumSprintWhereTaskBelong_ScrumSprintId", c => c.Int());
            AlterColumn("dbo.TaskModels", "ColumnNumber", c => c.Int());
            CreateIndex("dbo.TaskModels", "ScrumSprintWhereTaskBelong_ScrumSprintId");
            AddForeignKey("dbo.TaskModels", "ScrumSprintWhereTaskBelong_ScrumSprintId", "dbo.ScrumSprintModels", "ScrumSprintId");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.TaskModels", "ScrumSprintWhereTaskBelong_ScrumSprintId", "dbo.ScrumSprintModels");
            DropForeignKey("dbo.ScrumSprintModels", "ProjectId_ProjectId", "dbo.ProjectModels");
            DropIndex("dbo.TaskModels", new[] { "ScrumSprintWhereTaskBelong_ScrumSprintId" });
            DropIndex("dbo.ScrumSprintModels", new[] { "ProjectId_ProjectId" });
            AlterColumn("dbo.TaskModels", "ColumnNumber", c => c.Int(nullable: false));
            DropColumn("dbo.TaskModels", "ScrumSprintWhereTaskBelong_ScrumSprintId");
            DropColumn("dbo.ProjectModels", "ProjectManagementMethodology");
            DropTable("dbo.ScrumSprintModels");
        }
    }
}
