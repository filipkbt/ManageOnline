namespace ManageOnline.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class changeUserIDToUserId : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.UserBasicModels", "IsEmailVerified", c => c.Boolean(nullable: false));
            AddColumn("dbo.UserBasicModels", "ActivationCode", c => c.Guid(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.UserBasicModels", "ActivationCode");
            DropColumn("dbo.UserBasicModels", "IsEmailVerified");
        }
    }
}
