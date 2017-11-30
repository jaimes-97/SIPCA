namespace SIPCA.CLASES.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class prueba : DbMigration
    {
        public override void Up()
        {
           
            DropColumn("dbo.Lote", "Subtotal");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Lote", "Subtotal", c => c.Decimal(nullable: false, precision: 18, scale: 2));
         
        }
    }
}
