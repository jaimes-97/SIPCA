namespace SIPCA.CLASES.Migrations
{
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

            context.TipoEntregas.AddOrUpdate(te => te.NombreTipoEntrega,
                new TipoEntrega {NombreTipoEntrega = "Domicilio, Managua", Costo = 50 },
                new TipoEntrega { NombreTipoEntrega = "Departamental", Costo = 150 },
                new TipoEntrega { NombreTipoEntrega = "Retiro en tienda", Costo = 0 }



                );
          


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
        }
    }
}
