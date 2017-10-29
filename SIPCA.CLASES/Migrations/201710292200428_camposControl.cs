namespace SIPCA.CLASES.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class camposControl : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Categoria", "Eliminado", c => c.Boolean(nullable: false));
            AddColumn("dbo.Cliente", "FechaMod", c => c.DateTime(nullable: false));
            AddColumn("dbo.Compra", "Eliminado", c => c.Boolean(nullable: false));
            AddColumn("dbo.Compra", "FechaEliminacion", c => c.DateTime(nullable: false));
            AddColumn("dbo.Pedido", "FechaEliminacion", c => c.DateTime(nullable: false));
            AddColumn("dbo.TipoEntrega", "Eliminado", c => c.Boolean(nullable: false));
            AddColumn("dbo.Marca", "Eliminado", c => c.Boolean(nullable: false));
            AddColumn("dbo.LoteDetallePedido", "Eliminado", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.LoteDetallePedido", "Eliminado");
            DropColumn("dbo.Marca", "Eliminado");
            DropColumn("dbo.TipoEntrega", "Eliminado");
            DropColumn("dbo.Pedido", "FechaEliminacion");
            DropColumn("dbo.Compra", "FechaEliminacion");
            DropColumn("dbo.Compra", "Eliminado");
            DropColumn("dbo.Cliente", "FechaMod");
            DropColumn("dbo.Categoria", "Eliminado");
        }
    }
}
