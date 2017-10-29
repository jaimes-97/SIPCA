namespace SIPCA.CLASES.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Migration3 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.DetallePedido", "Eliminado", c => c.Boolean(nullable: false));
            AddColumn("dbo.Pedido", "Eliminado", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Pedido", "Eliminado");
            DropColumn("dbo.DetallePedido", "Eliminado");
        }
    }
}
