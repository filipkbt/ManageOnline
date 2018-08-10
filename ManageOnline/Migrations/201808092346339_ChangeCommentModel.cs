namespace ManageOnline.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangeCommentModel : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.CommentModels", "ProjectId", "dbo.ProjectModels");
            DropForeignKey("dbo.CommentModels", "TaskId", "dbo.TaskModels");
            DropIndex("dbo.CommentModels", new[] { "TaskId" });
            DropIndex("dbo.CommentModels", new[] { "ProjectId" });
            RenameColumn(table: "dbo.CommentModels", name: "ProjectId", newName: "ProjectWhereCommentBelong_ProjectId");
            RenameColumn(table: "dbo.CommentModels", name: "TaskId", newName: "TaskWhereCommentBelong_TaskId");
            AlterColumn("dbo.CommentModels", "TaskWhereCommentBelong_TaskId", c => c.Int());
            AlterColumn("dbo.CommentModels", "ProjectWhereCommentBelong_ProjectId", c => c.Int());
            CreateIndex("dbo.CommentModels", "ProjectWhereCommentBelong_ProjectId");
            CreateIndex("dbo.CommentModels", "TaskWhereCommentBelong_TaskId");
            AddForeignKey("dbo.CommentModels", "ProjectWhereCommentBelong_ProjectId", "dbo.ProjectModels", "ProjectId");
            AddForeignKey("dbo.CommentModels", "TaskWhereCommentBelong_TaskId", "dbo.TaskModels", "TaskId");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.CommentModels", "TaskWhereCommentBelong_TaskId", "dbo.TaskModels");
            DropForeignKey("dbo.CommentModels", "ProjectWhereCommentBelong_ProjectId", "dbo.ProjectModels");
            DropIndex("dbo.CommentModels", new[] { "TaskWhereCommentBelong_TaskId" });
            DropIndex("dbo.CommentModels", new[] { "ProjectWhereCommentBelong_ProjectId" });
            AlterColumn("dbo.CommentModels", "ProjectWhereCommentBelong_ProjectId", c => c.Int(nullable: false));
            AlterColumn("dbo.CommentModels", "TaskWhereCommentBelong_TaskId", c => c.Int(nullable: false));
            RenameColumn(table: "dbo.CommentModels", name: "TaskWhereCommentBelong_TaskId", newName: "TaskId");
            RenameColumn(table: "dbo.CommentModels", name: "ProjectWhereCommentBelong_ProjectId", newName: "ProjectId");
            CreateIndex("dbo.CommentModels", "ProjectId");
            CreateIndex("dbo.CommentModels", "TaskId");
            AddForeignKey("dbo.CommentModels", "TaskId", "dbo.TaskModels", "TaskId", cascadeDelete: true);
            AddForeignKey("dbo.CommentModels", "ProjectId", "dbo.ProjectModels", "ProjectId", cascadeDelete: true);
        }
    }
}
