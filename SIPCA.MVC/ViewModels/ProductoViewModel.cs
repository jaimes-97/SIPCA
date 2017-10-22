using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIPCA.CLASES
{
    
    public class ProductoViewModel
    {
        [Key]
        public  int IdProducto { get; set; }

        [Required(ErrorMessage = "Se requiere la {0}")]
        [Display(Name = "Categoría")]
        public int IdCategoria { get; set; }

        [ForeignKey("CategoriaId")]
        public virtual CategoriaViewModel Categoria { get; set; }

        [Required(ErrorMessage = "Se requiere el {0}")]
        [Display(Name = "Nombre del producto")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "Se requiere la {0}")]
        [Display(Name = "Marca")]
        public int IdMarca { get; set; }

        [ForeignKey("MarcaId")]
        public virtual MarcaViewModel Marca { get; set; }

        public IEnumerable<LoteViewModel> Lotes { get; set; }

        [ScaffoldColumn(false)]
        public bool Eliminado { get; set; }






    }
}
