namespace ManageOnline.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addTypesToTasks : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TaskModels", "TaskStatus", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.TaskModels", "TaskStatus");
        }
    }
}
