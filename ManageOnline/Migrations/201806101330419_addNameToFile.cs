namespace ManageOnline.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addNameToFile : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.FileModels", "FileName", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.FileModels", "FileName");
        }
    }
}
