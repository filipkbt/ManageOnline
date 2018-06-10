namespace ManageOnline.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateTask : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.TaskModels", "TaskFinishDate", c => c.DateTime());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.TaskModels", "TaskFinishDate", c => c.DateTime(nullable: false));
        }
    }
}
