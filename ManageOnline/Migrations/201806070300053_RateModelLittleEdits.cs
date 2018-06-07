namespace ManageOnline.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RateModelLittleEdits : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.RateModels", "AverageRate", c => c.Single(nullable: false));
            AlterColumn("dbo.RateModels", "WantToCoworkAgain", c => c.Int(nullable: false));
            DropColumn("dbo.RateModels", "ChanceToBeHiredAgain");
        }
        
        public override void Down()
        {
            AddColumn("dbo.RateModels", "ChanceToBeHiredAgain", c => c.Int());
            AlterColumn("dbo.RateModels", "WantToCoworkAgain", c => c.Int());
            AlterColumn("dbo.RateModels", "AverageRate", c => c.Int(nullable: false));
        }
    }
}
