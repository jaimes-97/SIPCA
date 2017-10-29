namespace SIPCA.CLASES.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Migration2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Producto", "PrecioVenta", c => c.Single(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Producto", "PrecioVenta");
        }
    }
}
