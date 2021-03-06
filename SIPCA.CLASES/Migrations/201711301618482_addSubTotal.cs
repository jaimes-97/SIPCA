namespace SIPCA.CLASES.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addSubTotal : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Lote", "Subtotal", c => c.Decimal(nullable: false, precision: 18, scale: 2));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Lote", "Subtotal");
        }
    }
}
