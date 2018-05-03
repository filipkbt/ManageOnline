namespace ManageOnline.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addSkillsToUser : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.UserBasicModels", "Skills", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.UserBasicModels", "Skills");
        }
    }
}
