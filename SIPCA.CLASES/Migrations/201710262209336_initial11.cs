namespace SIPCA.CLASES.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initial11 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Categoria",
                c => new
                    {
                        IdCategoria = c.Int(nullable: false, identity: true),
                        Nombre = c.String(nullable: false, maxLength: 250),
                    })
                .PrimaryKey(t => t.IdCategoria);
            
            CreateTable(
                "dbo.Cliente",
                c => new
                    {
                        IdCliente = c.Int(nullable: false, identity: true),
                        Nombre = c.String(nullable: false, maxLength: 250),
                        Direccion = c.String(nullable: false, maxLength: 250),
                        Cedula = c.String(nullable: false, maxLength: 250),
                        Correo = c.String(nullable: false, maxLength: 250),
                        IdUsuario = c.Int(nullable: false),
                        Eliminado = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.IdCliente);
            
            CreateTable(
                "dbo.Compra",
                c => new
                    {
                        IdCompra = c.Int(nullable: false, identity: true),
                        NCompra = c.String(nullable: false, maxLength: 250),
                        Fecha = c.DateTime(nullable: false),
                        Total = c.Single(nullable: false),
                        ProveedorId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.IdCompra)
                .ForeignKey("dbo.Proveedor", t => t.ProveedorId)
                .Index(t => t.ProveedorId);
            
            CreateTable(
                "dbo.Proveedor",
                c => new
                    {
                        IdProveedor = c.Int(nullable: false, identity: true),
                        Nombre = c.String(nullable: false, maxLength: 250),
                        Correo = c.String(nullable: false, maxLength: 250),
                        Direccion = c.String(nullable: false, maxLength: 250),
                        Telefono = c.String(nullable: false, maxLength: 250),
                        Eliminado = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.IdProveedor);
            
            CreateTable(
                "dbo.DetallePedido",
                c => new
                    {
                        IdDetallePedido = c.Int(nullable: false, identity: true),
                        PedidoId = c.Int(nullable: false),
                        Cantidad = c.Int(nullable: false),
                        PrecioVendido = c.Single(nullable: false),
                    })
                .PrimaryKey(t => t.IdDetallePedido)
                .ForeignKey("dbo.Pedido", t => t.PedidoId)
                .Index(t => t.PedidoId);
            
            CreateTable(
                "dbo.Pedido",
                c => new
                    {
                        IdPedido = c.Int(nullable: false, identity: true),
                        Fecha = c.DateTime(nullable: false),
                        ClienteId = c.Int(nullable: false),
                        TipoEntregaId = c.Int(nullable: false),
                        NPedido = c.String(nullable: false, maxLength: 250),
                        Total = c.Single(nullable: false),
                    })
                .PrimaryKey(t => t.IdPedido)
                .ForeignKey("dbo.Cliente", t => t.ClienteId)
                .ForeignKey("dbo.TipoEntrega", t => t.TipoEntregaId)
                .Index(t => t.ClienteId)
                .Index(t => t.TipoEntregaId);
            
            CreateTable(
                "dbo.TipoEntrega",
                c => new
                    {
                        IdTipoEntrega = c.Int(nullable: false, identity: true),
                        NombreTipoEntrega = c.String(nullable: false, maxLength: 250),
                        Costo = c.Single(nullable: false),
                    })
                .PrimaryKey(t => t.IdTipoEntrega);
            
            CreateTable(
                "dbo.LoteDetallePedido",
                c => new
                    {
                        IdLoteDetallePedido = c.Int(nullable: false, identity: true),
                        CantidadRestar = c.Int(nullable: false),
                        LoteID = c.Int(nullable: false),
                        DetallePedidoId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.IdLoteDetallePedido)
                .ForeignKey("dbo.DetallePedido", t => t.DetallePedidoId)
                .ForeignKey("dbo.Lote", t => t.LoteID)
                .Index(t => t.LoteID)
                .Index(t => t.DetallePedidoId);
            
            CreateTable(
                "dbo.Lote",
                c => new
                    {
                        IdLote = c.Int(nullable: false, identity: true),
                        Existencia = c.Int(nullable: false),
                        Cantidad = c.Int(nullable: false),
                        CompraId = c.Int(nullable: false),
                        ProductoId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.IdLote)
                .ForeignKey("dbo.Compra", t => t.CompraId)
                .ForeignKey("dbo.Producto", t => t.ProductoId)
                .Index(t => t.CompraId)
                .Index(t => t.ProductoId);
            
            CreateTable(
                "dbo.Producto",
                c => new
                    {
                        IdProducto = c.Int(nullable: false, identity: true),
                        CategoriaId = c.Int(nullable: false),
                        Nombre = c.String(nullable: false, maxLength: 250),
                        MarcaId = c.Int(nullable: false),
                        Eliminado = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.IdProducto)
                .ForeignKey("dbo.Categoria", t => t.CategoriaId)
                .ForeignKey("dbo.Marca", t => t.MarcaId)
                .Index(t => t.CategoriaId)
                .Index(t => t.MarcaId);
            
            CreateTable(
                "dbo.Marca",
                c => new
                    {
                        IdMarca = c.Int(nullable: false, identity: true),
                        Nombre = c.String(nullable: false, maxLength: 250),
                    })
                .PrimaryKey(t => t.IdMarca);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.LoteDetallePedido", "LoteID", "dbo.Lote");
            DropForeignKey("dbo.Lote", "ProductoId", "dbo.Producto");
            DropForeignKey("dbo.Producto", "MarcaId", "dbo.Marca");
            DropForeignKey("dbo.Producto", "CategoriaId", "dbo.Categoria");
            DropForeignKey("dbo.Lote", "CompraId", "dbo.Compra");
            DropForeignKey("dbo.LoteDetallePedido", "DetallePedidoId", "dbo.DetallePedido");
            DropForeignKey("dbo.DetallePedido", "PedidoId", "dbo.Pedido");
            DropForeignKey("dbo.Pedido", "TipoEntregaId", "dbo.TipoEntrega");
            DropForeignKey("dbo.Pedido", "ClienteId", "dbo.Cliente");
            DropForeignKey("dbo.Compra", "ProveedorId", "dbo.Proveedor");
            DropIndex("dbo.Producto", new[] { "MarcaId" });
            DropIndex("dbo.Producto", new[] { "CategoriaId" });
            DropIndex("dbo.Lote", new[] { "ProductoId" });
            DropIndex("dbo.Lote", new[] { "CompraId" });
            DropIndex("dbo.LoteDetallePedido", new[] { "DetallePedidoId" });
            DropIndex("dbo.LoteDetallePedido", new[] { "LoteID" });
            DropIndex("dbo.Pedido", new[] { "TipoEntregaId" });
            DropIndex("dbo.Pedido", new[] { "ClienteId" });
            DropIndex("dbo.DetallePedido", new[] { "PedidoId" });
            DropIndex("dbo.Compra", new[] { "ProveedorId" });
            DropTable("dbo.Marca");
            DropTable("dbo.Producto");
            DropTable("dbo.Lote");
            DropTable("dbo.LoteDetallePedido");
            DropTable("dbo.TipoEntrega");
            DropTable("dbo.Pedido");
            DropTable("dbo.DetallePedido");
            DropTable("dbo.Proveedor");
            DropTable("dbo.Compra");
            DropTable("dbo.Cliente");
            DropTable("dbo.Categoria");
        }
    }
}
