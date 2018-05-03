namespace ManageOnline.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class small : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TaskModels", "CurrentWorkerAtTaskId", c => c.Int());
            DropColumn("dbo.TaskModels", "CurrentWorketAtTaskId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.TaskModels", "CurrentWorketAtTaskId", c => c.Int(nullable: false));
            DropColumn("dbo.TaskModels", "CurrentWorkerAtTaskId");
        }
    }
}
