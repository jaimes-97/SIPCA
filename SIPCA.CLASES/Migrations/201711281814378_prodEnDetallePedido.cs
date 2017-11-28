namespace SIPCA.CLASES.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class prodEnDetallePedido : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.DetallePedido", "ProductoId", c => c.Int(nullable: false));
            CreateIndex("dbo.DetallePedido", "ProductoId");
            AddForeignKey("dbo.DetallePedido", "ProductoId", "dbo.Producto", "IdProducto");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.DetallePedido", "ProductoId", "dbo.Producto");
            DropIndex("dbo.DetallePedido", new[] { "ProductoId" });
            DropColumn("dbo.DetallePedido", "ProductoId");
        }
    }
}
