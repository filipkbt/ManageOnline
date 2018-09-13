namespace ManageOnline.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class removeProjectCategoriesProjectIdFromProjectModels : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.ProjectModels", "ProjectCategory");
        }
        
        public override void Down()
        {
            AddColumn("dbo.ProjectModels", "ProjectCategory", c => c.Int(nullable: false));
        }
    }
}
