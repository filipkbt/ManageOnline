namespace ManageOnline.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addRoles : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.UserBasicModels", "Role", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.UserBasicModels", "Role");
        }
    }
}
