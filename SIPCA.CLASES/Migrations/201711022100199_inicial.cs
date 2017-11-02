namespace SIPCA.CLASES.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class inicial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Carrito",
                c => new
                    {
                        IdCarrito = c.Int(nullable: false, identity: true),
                        Fecha = c.DateTime(nullable: false),
                     //   Total = c.Decimal(nullable: false, precision: 18, scale: 2),
                        estado = c.Boolean(nullable: false),
                        ApplicationUserId = c.String(nullable: false, maxLength: 250),
                        ClienteId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.IdCarrito)
                .ForeignKey("dbo.Cliente", t => t.ClienteId)
                .Index(t => t.ClienteId);

            Sql(@"create function dbo.GetSumDetalleCarrito(@carritoId int)
                    returns Decimal(18,2)
                    as
                    begin 

                    declare @carritoSum Decimal(18,2)

                    select @carritoSum= SUM((p.precioventa*dc.cantidad))
                    from Carrito c inner join DetalleCarrito dc on c.IdCarrito=dc.IdCarrito
                    inner join Producto p on dc.ProductoId=p.IdProducto
                    where c.IdCarrito=@carritoId 
                    return isnull(@carritoSum,0)
                    end
                ");

            Sql(@"
                ALTER TABLE dbo.Carrito add Total AS 
                dbo.GetSumDetalleCarrito(idCarrito)
                ");

            CreateTable(
                "dbo.Cliente",
                c => new
                    {
                        IdCliente = c.Int(nullable: false, identity: true),
                        Nombre = c.String(nullable: false, maxLength: 250),
                        Direccion = c.String(nullable: false, maxLength: 250),
                        Cedula = c.String(nullable: false, maxLength: 250),
                        Correo = c.String(nullable: false, maxLength: 250),
                        Eliminado = c.Boolean(nullable: false),
                        FechaMod = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.IdCliente);
            
            CreateTable(
                "dbo.Categoria",
                c => new
                    {
                        IdCategoria = c.Int(nullable: false, identity: true),
                        Nombre = c.String(nullable: false, maxLength: 250),
                        Eliminado = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.IdCategoria);
            
            CreateTable(
                "dbo.Compra",
                c => new
                    {
                        IdCompra = c.Int(nullable: false, identity: true),
                        NCompra = c.String(nullable: false, maxLength: 250),
                        Fecha = c.DateTime(nullable: false),
                       // Total = c.Decimal(nullable: false, precision: 18, scale: 2),
                        ProveedorId = c.Int(nullable: false),
                        Eliminado = c.Boolean(nullable: false),
                        FechaEliminacion = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.IdCompra)
                .ForeignKey("dbo.Proveedor", t => t.ProveedorId)
                .Index(t => t.ProveedorId);

            Sql(@"Create FUNCTION dbo.GetSumDetalleCompra(@compraId INT)
                   Returns Decimal(18,2)
                AS
                Begin
                
                DECLARE @compraSum Decimal(18,2)
                
                select @compraSum = sum((p.PrecioVenta * l.Cantidad)+((p.PrecioVenta * l.Cantidad)*(l.porcentajeIVA/100))) 
                from lote l inner join producto p on p.IdProducto=l.ProductoId
				inner join compra c on l.CompraId=c.IdCompra
				
                where c.IdCompra = @compraId
                and l.eliminado=0

                return ISNULL(@compraSum,0)
                END
                ");

            Sql(@"
                ALTER TABLE dbo.Compra add Total AS 
                dbo.GetSumDetalleCompra(idCompra)
                ");

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
                        aplicaIVA = c.Boolean(nullable: false),
                        porcentajeIVA = c.Decimal(nullable: false, precision: 18, scale: 2),
                        PrecioVendido = c.Single(nullable: false),
                        Eliminado = c.Boolean(nullable: false),
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
                       // Total = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Eliminado = c.Boolean(nullable: false),
                        FechaEliminacion = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.IdPedido)
                .ForeignKey("dbo.Cliente", t => t.ClienteId)
                .ForeignKey("dbo.TipoEntrega", t => t.TipoEntregaId)
                .Index(t => t.ClienteId)
                .Index(t => t.TipoEntregaId);

            Sql(@"Create FUNCTION dbo.GetSumDetallePedido(@pedidoId INT)
                   Returns Decimal(18,2)
                AS
                Begin
                
                DECLARE @pedidoSum Decimal(18,2)
                
                select @pedidoSum = sum((p.PrecioVenta * dp.Cantidad)+((p.PrecioVenta * dp.Cantidad)*(dp.porcentajeIVA/100))) 
                from DetallePedido dp 
				inner join LoteDetallePedido ldp on dp.IdDetallePedido=ldp.DetallePedidoId
				inner join lote l on l.IdLote=ldp.LoteID
				inner join producto p on p.IdProducto=l.ProductoId 
				inner join Pedido pe on pe.IdPedido=dp.PedidoId
                where pe.IdPedido = @pedidoId
                and dp.eliminado=0

                return ISNULL(@pedidoSum,0)
                END
                ");

            Sql(@"
                ALTER TABLE dbo.Pedido add Total AS 
                dbo.GetSumDetallePedido(idPedido)
                ");

            CreateTable(
                "dbo.TipoEntrega",
                c => new
                    {
                        IdTipoEntrega = c.Int(nullable: false, identity: true),
                        NombreTipoEntrega = c.String(nullable: false, maxLength: 250),
                        Costo = c.Single(nullable: false),
                        Eliminado = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.IdTipoEntrega);
            
            CreateTable(
                "dbo.DetalleCarrito",
                c => new
                    {
                        IdDetalleCarrito = c.Int(nullable: false, identity: true),
                        cantidad = c.Int(nullable: false),
                        SubTotal = c.Single(nullable: false),
                        estado = c.Boolean(nullable: false),
                        Fecha = c.DateTime(nullable: false),
                        ProductoId = c.Int(nullable: false),
                        aplicaIVA = c.Boolean(nullable: false),
                        porcentajeIVA = c.Decimal(nullable: false, precision: 18, scale: 2),
                        IdCarrito = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.IdDetalleCarrito)
                .ForeignKey("dbo.Carrito", t => t.IdCarrito)
                .ForeignKey("dbo.Producto", t => t.ProductoId)
                .Index(t => t.ProductoId)
                .Index(t => t.IdCarrito);
            
            CreateTable(
                "dbo.Producto",
                c => new
                    {
                        IdProducto = c.Int(nullable: false, identity: true),
                        CategoriaId = c.Int(nullable: false),
                        Nombre = c.String(nullable: false, maxLength: 250),
                        MarcaId = c.Int(nullable: false),
                        Eliminado = c.Boolean(nullable: false),
                        PrecioVenta = c.Single(nullable: false),
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
                        Eliminado = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.IdMarca);
            
            CreateTable(
                "dbo.LoteDetallePedido",
                c => new
                    {
                        IdLoteDetallePedido = c.Int(nullable: false, identity: true),
                        CantidadRestar = c.Int(nullable: false),
                        LoteID = c.Int(nullable: false),
                        DetallePedidoId = c.Int(nullable: false),
                        Eliminado = c.Boolean(nullable: false),
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
                        aplicaIVA = c.Boolean(nullable: false),
                        porcentajeIVA = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Eliminado = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.IdLote)
                .ForeignKey("dbo.Compra", t => t.CompraId)
                .ForeignKey("dbo.Producto", t => t.ProductoId)
                .Index(t => t.CompraId)
                .Index(t => t.ProductoId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.LoteDetallePedido", "LoteID", "dbo.Lote");
            DropForeignKey("dbo.Lote", "ProductoId", "dbo.Producto");
            DropForeignKey("dbo.Lote", "CompraId", "dbo.Compra");
            DropForeignKey("dbo.LoteDetallePedido", "DetallePedidoId", "dbo.DetallePedido");
            DropForeignKey("dbo.DetalleCarrito", "ProductoId", "dbo.Producto");
            DropForeignKey("dbo.Producto", "MarcaId", "dbo.Marca");
            DropForeignKey("dbo.Producto", "CategoriaId", "dbo.Categoria");
            DropForeignKey("dbo.DetalleCarrito", "IdCarrito", "dbo.Carrito");
            DropForeignKey("dbo.DetallePedido", "PedidoId", "dbo.Pedido");
            DropForeignKey("dbo.Pedido", "TipoEntregaId", "dbo.TipoEntrega");
            DropForeignKey("dbo.Pedido", "ClienteId", "dbo.Cliente");
            DropForeignKey("dbo.Compra", "ProveedorId", "dbo.Proveedor");
            DropForeignKey("dbo.Carrito", "ClienteId", "dbo.Cliente");
            DropIndex("dbo.Lote", new[] { "ProductoId" });
            DropIndex("dbo.Lote", new[] { "CompraId" });
            DropIndex("dbo.LoteDetallePedido", new[] { "DetallePedidoId" });
            DropIndex("dbo.LoteDetallePedido", new[] { "LoteID" });
            DropIndex("dbo.Producto", new[] { "MarcaId" });
            DropIndex("dbo.Producto", new[] { "CategoriaId" });
            DropIndex("dbo.DetalleCarrito", new[] { "IdCarrito" });
            DropIndex("dbo.DetalleCarrito", new[] { "ProductoId" });
            DropIndex("dbo.Pedido", new[] { "TipoEntregaId" });
            DropIndex("dbo.Pedido", new[] { "ClienteId" });
            DropIndex("dbo.DetallePedido", new[] { "PedidoId" });
            DropIndex("dbo.Compra", new[] { "ProveedorId" });
            DropIndex("dbo.Carrito", new[] { "ClienteId" });
            DropTable("dbo.Lote");
            DropTable("dbo.LoteDetallePedido");
            DropTable("dbo.Marca");
            DropTable("dbo.Producto");
            DropTable("dbo.DetalleCarrito");
            DropTable("dbo.TipoEntrega");
            DropTable("dbo.Pedido");
            DropTable("dbo.DetallePedido");
            DropTable("dbo.Proveedor");
            DropTable("dbo.Compra");
            DropTable("dbo.Categoria");
            DropTable("dbo.Cliente");
            DropTable("dbo.Carrito");
        }
    }
}
