namespace ManageOnline.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class fix : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.OfferToProjectModels", "UserWhoAddOfferId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.OfferToProjectModels", "UserWhoAddOfferId", c => c.Int(nullable: false));
        }
    }
}
