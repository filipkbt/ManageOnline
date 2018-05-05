namespace ManageOnline.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class prepareTasksToKanbanBoard : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TaskModels", "RowNumber", c => c.Int(nullable: false));
            AddColumn("dbo.TaskModels", "ColumnNumber", c => c.Int(nullable: false));
            AddColumn("dbo.TaskModels", "UserWhoAddTask_UserId", c => c.Int());
            CreateIndex("dbo.TaskModels", "UserWhoAddTask_UserId");
            AddForeignKey("dbo.TaskModels", "UserWhoAddTask_UserId", "dbo.UserBasicModels", "UserId");
            DropColumn("dbo.TaskModels", "WorkerId");
            DropColumn("dbo.TaskModels", "CurrentWorkerAtTaskId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.TaskModels", "CurrentWorkerAtTaskId", c => c.Int());
            AddColumn("dbo.TaskModels", "WorkerId", c => c.Int(nullable: false));
            DropForeignKey("dbo.TaskModels", "UserWhoAddTask_UserId", "dbo.UserBasicModels");
            DropIndex("dbo.TaskModels", new[] { "UserWhoAddTask_UserId" });
            DropColumn("dbo.TaskModels", "UserWhoAddTask_UserId");
            DropColumn("dbo.TaskModels", "ColumnNumber");
            DropColumn("dbo.TaskModels", "RowNumber");
        }
    }
}
