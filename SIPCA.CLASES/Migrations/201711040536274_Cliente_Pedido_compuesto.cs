namespace SIPCA.CLASES.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Cliente_Pedido_compuesto : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Cliente", "UserId", c => c.String(nullable: false, maxLength: 128));
            AddColumn("dbo.Pedido", "FechaCorte", c => c.DateTime(nullable: false));
            DropColumn("dbo.Cliente", "Correo");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Cliente", "Correo", c => c.String(nullable: false, maxLength: 250));
            DropColumn("dbo.Pedido", "FechaCorte");
            DropColumn("dbo.Cliente", "UserId");
        }
    }
}
