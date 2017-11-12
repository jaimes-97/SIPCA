namespace SIPCA.CLASES.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Imagen : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Imagen",
                c => new
                    {
                        IdImagen = c.Int(nullable: false, identity: true),
                        ImageName = c.String(maxLength: 255),
                        FileType = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.IdImagen);
            
            AddColumn("dbo.Producto", "ImagenId", c => c.Int());
            AddColumn("dbo.Producto", "Imagen_IdImagen", c => c.Int());
            CreateIndex("dbo.Producto", "Imagen_IdImagen");
            AddForeignKey("dbo.Producto", "Imagen_IdImagen", "dbo.Imagen", "IdImagen");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Producto", "Imagen_IdImagen", "dbo.Imagen");
            DropIndex("dbo.Producto", new[] { "Imagen_IdImagen" });
            DropColumn("dbo.Producto", "Imagen_IdImagen");
            DropColumn("dbo.Producto", "ImagenId");
            DropTable("dbo.Imagen");
        }
    }
}
