namespace SIPCA.CLASES.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class cambioCliente : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Carrito", "ClienteId", c => c.Int(nullable: false));
            CreateIndex("dbo.Carrito", "ClienteId");
            AddForeignKey("dbo.Carrito", "ClienteId", "dbo.Cliente", "IdCliente");
            DropColumn("dbo.Cliente", "IdUsuario");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Cliente", "IdUsuario", c => c.Int(nullable: false));
            DropForeignKey("dbo.Carrito", "ClienteId", "dbo.Cliente");
            DropIndex("dbo.Carrito", new[] { "ClienteId" });
            DropColumn("dbo.Carrito", "ClienteId");
        }
    }
}
