namespace ManageOnline.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addAVerageRateToUser : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.UserBasicModels", "AverageRate", c => c.Double(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.UserBasicModels", "AverageRate");
        }
    }
}
