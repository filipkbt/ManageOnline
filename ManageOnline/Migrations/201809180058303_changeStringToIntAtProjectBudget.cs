namespace ManageOnline.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class changeStringToIntAtProjectBudget : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.ProjectModels", "ProjectBudget", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.ProjectModels", "ProjectBudget", c => c.String());
        }
    }
}
