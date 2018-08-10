namespace ManageOnline.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddTaskStartDate : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TaskModels", "TaskStartDate", c => c.DateTime());
        }
        
        public override void Down()
        {
            DropColumn("dbo.TaskModels", "TaskStartDate");
        }
    }
}
