namespace ManageOnline.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addMoreFieldToUserBasicModel : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.UserBasicModels", "DisplayedRole", c => c.String());
            AddColumn("dbo.UserBasicModels", "MobileNumber", c => c.String());
            AddColumn("dbo.UserBasicModels", "Description", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.UserBasicModels", "Description");
            DropColumn("dbo.UserBasicModels", "MobileNumber");
            DropColumn("dbo.UserBasicModels", "DisplayedRole");
        }
    }
}
