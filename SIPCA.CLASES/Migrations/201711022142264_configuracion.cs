namespace SIPCA.CLASES.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class configuracion : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Configuracion",
                c => new
                    {
                        IdConfiguracion = c.Int(nullable: false, identity: true),
                        PorcentajeIVA = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.IdConfiguracion);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Configuracion");
        }
    }
}
