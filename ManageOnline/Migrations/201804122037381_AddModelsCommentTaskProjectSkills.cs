namespace ManageOnline.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddModelsCommentTaskProjectSkills : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CommentModels",
                c => new
                    {
                        CommentId = c.Int(nullable: false, identity: true),
                        UserId = c.Int(nullable: false),
                        CommentDescription = c.String(),
                        TaskId = c.Int(nullable: false),
                        ProjectId = c.Int(nullable: false),
                        DateWhenCommentWasAdded = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.CommentId)
                .ForeignKey("dbo.UserBasicModels", t => t.UserId, cascadeDelete: true)
                .ForeignKey("dbo.ProjectModels", t => t.ProjectId, cascadeDelete: true)
                .ForeignKey("dbo.TaskModels", t => t.TaskId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.TaskId)
                .Index(t => t.ProjectId);
            
            CreateTable(
                "dbo.ProjectModels",
                c => new
                    {
                        ProjectId = c.Int(nullable: false, identity: true),
                        ProjectTitle = c.String(),
                        ProjectDescription = c.String(),
                        ProjectCreationDate = c.DateTime(nullable: false),
                        ProjectStartDate = c.DateTime(nullable: false),
                        ProjectFinishDate = c.DateTime(nullable: false),
                        ProjectOwner_UserId = c.Int(),
                        UserBasicModel_UserId = c.Int(),
                    })
                .PrimaryKey(t => t.ProjectId)
                .ForeignKey("dbo.UserBasicModels", t => t.ProjectOwner_UserId)
                .ForeignKey("dbo.UserBasicModels", t => t.UserBasicModel_UserId)
                .Index(t => t.ProjectOwner_UserId)
                .Index(t => t.UserBasicModel_UserId);
            
            CreateTable(
                "dbo.SkillsModels",
                c => new
                    {
                        SkillId = c.Int(nullable: false, identity: true),
                        SkillName = c.Int(nullable: false),
                        ProjectModel_ProjectId = c.Int(),
                        UserBasicModel_UserId = c.Int(),
                    })
                .PrimaryKey(t => t.SkillId)
                .ForeignKey("dbo.ProjectModels", t => t.ProjectModel_ProjectId)
                .ForeignKey("dbo.UserBasicModels", t => t.UserBasicModel_UserId)
                .Index(t => t.ProjectModel_ProjectId)
                .Index(t => t.UserBasicModel_UserId);
            
            CreateTable(
                "dbo.TaskModels",
                c => new
                    {
                        TaskId = c.Int(nullable: false, identity: true),
                        TaskName = c.String(),
                        TaskDescription = c.String(),
                        TaskCreationDate = c.DateTime(nullable: false),
                        WorkerId = c.Int(nullable: false),
                        CurrentWorkerAtTask_UserId = c.Int(),
                        ProjectId_ProjectId = c.Int(),
                    })
                .PrimaryKey(t => t.TaskId)
                .ForeignKey("dbo.UserBasicModels", t => t.CurrentWorkerAtTask_UserId)
                .ForeignKey("dbo.ProjectModels", t => t.ProjectId_ProjectId)
                .Index(t => t.CurrentWorkerAtTask_UserId)
                .Index(t => t.ProjectId_ProjectId);
            
            AddColumn("dbo.UserBasicModels", "UserBasicModel_UserId", c => c.Int());
            AddColumn("dbo.UserBasicModels", "ProjectModel_ProjectId", c => c.Int());
            CreateIndex("dbo.UserBasicModels", "UserBasicModel_UserId");
            CreateIndex("dbo.UserBasicModels", "ProjectModel_ProjectId");
            AddForeignKey("dbo.UserBasicModels", "UserBasicModel_UserId", "dbo.UserBasicModels", "UserId");
            AddForeignKey("dbo.UserBasicModels", "ProjectModel_ProjectId", "dbo.ProjectModels", "ProjectId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.UserBasicModels", "ActivationCode", c => c.Guid(nullable: false));
            AddColumn("dbo.UserBasicModels", "IsEmailVerified", c => c.Boolean(nullable: false));
            DropForeignKey("dbo.CommentModels", "TaskId", "dbo.TaskModels");
            DropForeignKey("dbo.CommentModels", "ProjectId", "dbo.ProjectModels");
            DropForeignKey("dbo.CommentModels", "UserId", "dbo.UserBasicModels");
            DropForeignKey("dbo.SkillsModels", "UserBasicModel_UserId", "dbo.UserBasicModels");
            DropForeignKey("dbo.ProjectModels", "UserBasicModel_UserId", "dbo.UserBasicModels");
            DropForeignKey("dbo.UserBasicModels", "ProjectModel_ProjectId", "dbo.ProjectModels");
            DropForeignKey("dbo.TaskModels", "ProjectId_ProjectId", "dbo.ProjectModels");
            DropForeignKey("dbo.TaskModels", "CurrentWorkerAtTask_UserId", "dbo.UserBasicModels");
            DropForeignKey("dbo.SkillsModels", "ProjectModel_ProjectId", "dbo.ProjectModels");
            DropForeignKey("dbo.ProjectModels", "ProjectOwner_UserId", "dbo.UserBasicModels");
            DropForeignKey("dbo.UserBasicModels", "UserBasicModel_UserId", "dbo.UserBasicModels");
            DropIndex("dbo.TaskModels", new[] { "ProjectId_ProjectId" });
            DropIndex("dbo.TaskModels", new[] { "CurrentWorkerAtTask_UserId" });
            DropIndex("dbo.SkillsModels", new[] { "UserBasicModel_UserId" });
            DropIndex("dbo.SkillsModels", new[] { "ProjectModel_ProjectId" });
            DropIndex("dbo.ProjectModels", new[] { "UserBasicModel_UserId" });
            DropIndex("dbo.ProjectModels", new[] { "ProjectOwner_UserId" });
            DropIndex("dbo.UserBasicModels", new[] { "ProjectModel_ProjectId" });
            DropIndex("dbo.UserBasicModels", new[] { "UserBasicModel_UserId" });
            DropIndex("dbo.CommentModels", new[] { "ProjectId" });
            DropIndex("dbo.CommentModels", new[] { "TaskId" });
            DropIndex("dbo.CommentModels", new[] { "UserId" });
            DropColumn("dbo.UserBasicModels", "ProjectModel_ProjectId");
            DropColumn("dbo.UserBasicModels", "UserBasicModel_UserId");
            DropTable("dbo.TaskModels");
            DropTable("dbo.SkillsModels");
            DropTable("dbo.ProjectModels");
            DropTable("dbo.CommentModels");
        }
    }
}
