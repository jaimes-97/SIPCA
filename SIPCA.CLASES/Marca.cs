using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIPCA.CLASES
{
    [Table("Marca")]
    public class Marca
    {
        [Key]
        public int IdMarca { get; set; }

        [Required(ErrorMessage = "Se requiere el {0}")]
        [Display(Name = "Nombre de la marca")]
        private string Nombre { get; set; }

        public virtual IEnumerable<Producto> Productos { get; set; }
    }
}
