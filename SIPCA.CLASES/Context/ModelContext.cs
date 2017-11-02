using SIPCA.CLASES.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIPCA.CLASES.Context
{
   public class ModelContext : DbContext
    {
        public ModelContext() : base("DefaultConnection"){
        }

        public DbSet<Categoria> Categorias { get; set; }
        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<Compra> Compras { get; set; }
        public DbSet<DetallePedido> DetallePedidos { get; set; }
        public DbSet<Lote> Lotes { get; set; }
        public DbSet<LoteDetallePedido> LoteDetallePedidos { get; set; }
        public DbSet<Marca> Marcas { get; set; }
        public DbSet<Pedido> Pedidos { get; set; }
        public DbSet<Producto> Productos { get; set; }
        public DbSet<Proveedor> Proveedores { get; set; }
        public DbSet<TipoEntrega> TipoEntregas { get; set; }
        public DbSet<Carrito> Carritos { get; set; }
        public DbSet<DetalleCarrito> DetallesCarritos { get; set; }
        public DbSet<Configuracion> configuraciones { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            #region EntityConfiguration
            base.OnModelCreating(modelBuilder);
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
            modelBuilder.Conventions.Remove<ManyToManyCascadeDeleteConvention>();
            /*modelBuilder.Properties().Where(p => p.Name == p.ReflectedType.Name + "Id").
                Configure(p => p.IsKey());*/
            modelBuilder.Properties<string>().Configure(p => p.HasColumnType("nvarchar"));
            modelBuilder.Properties<string>().Configure(p => p.HasMaxLength(250));
            #endregion

            #region EntityConfiguration
            //modelBuilder.Configurations.Add(new BancoConfig());
            //modelBuilder.Configurations.Add(new TipoBancoConfig());
            #endregion
        }

        public override int SaveChanges()
        {
            foreach (var entry in ChangeTracker.Entries().Where(entry => entry.GetType()
                .GetProperty("DateCreation") != null))
            {
                if (entry.State == EntityState.Added)
                    entry.Property("DateCreation").CurrentValue = System.DateTime.Now;
            }
            foreach (var entry in ChangeTracker.Entries().Where(entry => entry.GetType()
                .GetProperty("DateMod") != null))
            {
                if (entry.State == EntityState.Added)
                    entry.Property("DateMod").CurrentValue = System.DateTime.Now;
                if (entry.State == EntityState.Modified)
                    entry.Property("DateMod").CurrentValue = System.DateTime.Now;
            }
            return base.SaveChanges();
        }

       
    }
}
