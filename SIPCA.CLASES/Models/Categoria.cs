using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIPCA.CLASES
{
    [Table ("Categoria")]
    public class Categoria
    {

        [Key]
        public int IdCategoria { get; set; }

        [Required(ErrorMessage = "Se requiere el nombre de la categoría")]
        [Display(Name = "Nombre")]
        public string Nombre { get; set; }

        public IEnumerable<Producto> Productos { get; set; }

    }
}
