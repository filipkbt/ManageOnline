namespace ManageOnline.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangeRowNumberToNullable : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.TaskModels", "RowNumber", c => c.Int());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.TaskModels", "RowNumber", c => c.Int(nullable: false));
        }
    }
}
