using SIPCA.CLASES.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIPCA.CLASES.Models
{
    [Table ("Producto")]
    public class Producto
    {
        [Key]
        public  int IdProducto { get; set; }

        [Required(ErrorMessage = "Se requiere la {0}")]
        [Display(Name = "Categoría")]
        public int CategoriaId { get; set; }

        [ForeignKey("CategoriaId")]
        public virtual Categoria Categoria { get; set; }

        [Required(ErrorMessage = "Se requiere el {0}")]
        [Display(Name = "Nombre del producto")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "Se requiere la {0}")]
        [Display(Name = "Marca")]
        public int MarcaId { get; set; }

        [ForeignKey("MarcaId")]
        public virtual Marca Marca { get; set; }

        public IEnumerable<Lote> Lotes { get; set; }

        [ScaffoldColumn(false)]
        public bool Eliminado { get; set; }

        [Required(ErrorMessage = "Se requiere la {0}")]
        [Display(Name = "Precio Venta")]
        public float PrecioVenta { get; set; }

        public IEnumerable<DetalleCarrito> DetalleCarritosId { get; set; }

        public int? ImagenId { get; set; }
      
        public virtual Imagen Imagen { get; set; }
    }
}
