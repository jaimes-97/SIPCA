namespace SIPCA.CLASES.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class removeComputed : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Pedido", "Total", c => c.Decimal(nullable: false, precision: 18, scale: 2));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Pedido", "Total", c => c.Decimal(nullable: false, precision: 18, scale: 2));
        }
    }
}
