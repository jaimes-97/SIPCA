namespace SIPCA.CLASES.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class fechaE : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Compra", "FechaEliminacion");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Compra", "FechaEliminacion", c => c.DateTime(nullable: false));
        }
    }
}
