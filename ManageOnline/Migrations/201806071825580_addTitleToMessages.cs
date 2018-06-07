namespace ManageOnline.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addTitleToMessages : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.MessageModels", "Title", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.MessageModels", "Title");
        }
    }
}
