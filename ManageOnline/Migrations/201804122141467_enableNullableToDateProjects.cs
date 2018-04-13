namespace ManageOnline.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class enableNullableToDateProjects : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.ProjectModels", "ProjectStartDate", c => c.DateTime());
            AlterColumn("dbo.ProjectModels", "ProjectFinishDate", c => c.DateTime());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.ProjectModels", "ProjectFinishDate", c => c.DateTime(nullable: false));
            AlterColumn("dbo.ProjectModels", "ProjectStartDate", c => c.DateTime(nullable: false));
        }
    }
}
