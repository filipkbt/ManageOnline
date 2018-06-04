namespace ManageOnline.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addResponsibilitiesToOffer : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.OfferToProjectModels", "Responsibilities", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.OfferToProjectModels", "Responsibilities");
        }
    }
}
