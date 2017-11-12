namespace SIPCA.CLASES.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class cambioimagen : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Imagen", "ImagePath", c => c.String(maxLength: 250));
            DropColumn("dbo.Imagen", "FileType");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Imagen", "FileType", c => c.Int(nullable: false));
            DropColumn("dbo.Imagen", "ImagePath");
        }
    }
}
