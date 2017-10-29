namespace SIPCA.CLASES.Migrations
{
    using CLASES;
    using Context;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<SIPCA.CLASES.Context.ModelContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(SIPCA.CLASES.Context.ModelContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //
            #region Cargando_Tipo_Entrega
            context.TipoEntregas.AddOrUpdate(te => te.NombreTipoEntrega,
                new TipoEntrega {NombreTipoEntrega = "Domicilio, Managua", Costo = 50 },
                new TipoEntrega { NombreTipoEntrega = "Departamental", Costo = 150 },
                new TipoEntrega { NombreTipoEntrega = "Retiro en tienda", Costo = 0 }



                );
            context.SaveChanges();
            #endregion

            #region Cargando_Categoria
            context.Categorias.AddOrUpdate(c => c.Nombre,
              new Categoria { Nombre = "Deportivo-Damas" },
              new Categoria { Nombre = "Deportivo-Caballero" },
              new Categoria { Nombre = "Deportivo-Infantil" },
              new Categoria { Nombre = "Casual-Damas" },
              new Categoria { Nombre = "Casual-Caballero" },
              new Categoria { Nombre = "Casual-Infantil" },
              new Categoria { Nombre = "Elegante-Damas" },
              new Categoria { Nombre = "Elegante-Caballero" },
              new Categoria { Nombre = "Elegante-Infantil" },
               new Categoria { Nombre = "Varios" }
              );

            context.SaveChanges();

            #endregion

            #region Cargando_Marcas
            context.Marcas.AddOrUpdate(m => m.Nombre,
                new Marca { Nombre = "Supra"},
                new Marca { Nombre = "Addidas"},
                new Marca { Nombre = "Nike"},
                new Marca { Nombre = "Tiger"},
                new Marca { Nombre = "Converse" },
                new Marca { Nombre = "Timberland" },
                new Marca { Nombre = "Lacoste" },
                new Marca { Nombre = "Champion" },
                new Marca { Nombre = "Sperrys" },
                new Marca { Nombre = "American Eagle" },
                new Marca { Nombre = "Levi's"},
                new Marca { Nombre = "DC" },
                new Marca { Nombre = "Guess" },
                new Marca { Nombre = "Polo" },
                new Marca { Nombre = "Puma" },
                new Marca { Nombre = "Tommy Hilfiger" },
                new Marca { Nombre = "Skechers" }
                );
            #endregion

            //Faltan los Crud

            #region Cargando Producto
            context.Productos.AddOrUpdate(p => p.Nombre ,
              new Producto { Nombre = "Botines SB-2015", CategoriaId = 2 , MarcaId = 1, PrecioVenta = 3000.00f},
              new Producto { Nombre = "Zapatos Cafe-Cuerina", CategoriaId = 2, MarcaId = 17, PrecioVenta = 1500.00f },
              new Producto { Nombre = "Zapatos Celeste", CategoriaId = 2, MarcaId = 14, PrecioVenta = 1350.00f },
              new Producto { Nombre = "Zapatos Morados", CategoriaId = 1, MarcaId = 14, PrecioVenta = 1450.00f },
              new Producto { Nombre = "Pantunflas", CategoriaId = 10, MarcaId = 16, PrecioVenta = 550.00f },
              new Producto { Nombre = "Botas Cafe-Cuero", CategoriaId = 4, MarcaId = 13, PrecioVenta = 1780.00f },
              new Producto { Nombre = "Botas Negras-Cuero ", CategoriaId = 5, MarcaId = 13, PrecioVenta = 1695.00f },
              new Producto { Nombre = "Botas Negras-Cuerina", CategoriaId = 7, MarcaId = 13, PrecioVenta = 1150.00f },
              new Producto { Nombre = "Botas Cafes-Cuerina", CategoriaId = 8, MarcaId = 13, PrecioVenta = 1150.00f }
          
             
              );

            context.SaveChanges();
            #endregion

            #region Cargando_Proveedor
            context.Proveedores.AddOrUpdate(p => p.Nombre,
                new Proveedor { Nombre = "Calzado Rhino", Telefono = "22760260", Direccion = "Las Colinas, Managua", Correo= "calzaRhino@gmail.com" },
                new Proveedor { Nombre = "Calzado Chontal", Telefono = "22490090", Direccion = "Jardines De Santa Clara, Managua", Correo = "chontalCal@yahoo.com"},
                new Proveedor { Nombre = "Caribbean Shoes S. A.", Telefono = "22681347", Direccion = "Semáforos de Linda Vista 1c. al Este, Colonia Francisco Morazán, Managua 12012", Correo = "caribeanZap@gmail.com"}



                );
            context.SaveChanges();
            #endregion

            #region Cargando_Cliente
            context.Clientes.AddOrUpdate(c => c,
                new Cliente { Nombre = "Carlos Daniel Robles", Cedula= "401-051099-1004D", Correo= "daniel0599@icloud.com", Direccion= "Ticuantepe - Cont.Parque Heroes y Martires del 6 de Junio Cost/Nort", IdUsuario=1},
                new Cliente { Nombre = "Cesar Marin", Cedula = "222-2222-2222D", Correo = "tupapi@gmail.com", Direccion = "Cerca de aqui", IdUsuario = 2 }

                );
            context.SaveChanges();
            #endregion
        }
    }
}
