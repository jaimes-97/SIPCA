namespace SIPCA.CLASES.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class dropColumnCarritoDetalle : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.DetalleCarrito", "aplicaIVA");
            DropColumn("dbo.DetalleCarrito", "porcentajeIVA");
        }
        
        public override void Down()
        {
            AddColumn("dbo.DetalleCarrito", "porcentajeIVA", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.DetalleCarrito", "aplicaIVA", c => c.Boolean(nullable: false));
        }
    }
}
