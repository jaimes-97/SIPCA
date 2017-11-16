using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIPCA.CLASES.Models
{
    [Table ("Categoria")]
    public class Categoria
    {
        [Key]
        public int IdCategoria { get; set; }

        //Para que no se repita el nombre de la categoria
        [Index("INDEX_CATEGORIA_NOMBRE", IsUnique = true)]
        [MaxLength (60, ErrorMessage ="MAximo el {0} debe contener {1} caracteres")]
        [Required(ErrorMessage = "Se requiere el nombre de la categoría")]
        [Display(Name = "Nombre")]
        public string Nombre { get; set; }
         
        //Este campo es para manejar la concurrencia
        [ScaffoldColumn(false)]
        [Timestamp]
        public byte[] Control { get; set; }

        public IEnumerable<Producto> Productos { get; set; }

        [ScaffoldColumn(false)]
        public bool Eliminado { get; set; }
    }
}
