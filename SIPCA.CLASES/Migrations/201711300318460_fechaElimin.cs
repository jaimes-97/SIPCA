namespace SIPCA.CLASES.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class fechaElimin : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Compra", "FechaEliminacion", c => c.DateTime(nullable: true));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Compra", "FechaEliminacion");
        }
    }
}
