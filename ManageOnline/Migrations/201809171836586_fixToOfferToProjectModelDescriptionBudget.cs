namespace ManageOnline.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class fixToOfferToProjectModelDescriptionBudget : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.OfferToProjectModels", "Budget", c => c.Int(nullable: false));
            AlterColumn("dbo.OfferToProjectModels", "Responsibilities", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.OfferToProjectModels", "Responsibilities", c => c.Int(nullable: false));
            AlterColumn("dbo.OfferToProjectModels", "Budget", c => c.Double(nullable: false));
        }
    }
}
