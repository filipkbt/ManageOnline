namespace ManageOnline.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddUserPhoto : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.UserBasicModels", "UserPhoto", c => c.Binary());
        }
        
        public override void Down()
        {
            DropColumn("dbo.UserBasicModels", "UserPhoto");
        }
    }
}
