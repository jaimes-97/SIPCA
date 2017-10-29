using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

using System.Threading.Tasks;


namespace SIPCA.CLASES.Models
{
    [Table("Carrito")]
    public class Carrito
    {
        [Key]
        public int IdCarrito { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        [ScaffoldColumn(false)]
        public DateTime Fecha { get; set; }

        [Required(ErrorMessage = "Fallo en el {0} ")]
        [Display(Name = "Total")]
        public float Total { get; set; }

        [Required]
        [ScaffoldColumn(false)]
        public bool estado { get; set; }

        // acá debe ir el id del usuario que se tomará de la tabla de usuario que se genera por identity
        [Required(ErrorMessage = "Se requiere el {0}")]
        [Display(Name = "Usuario")]
        public string ApplicationUserId { get; set; }

        //[ForeignKey("UserId")]
        //public virtual ApplicationUser ApplicationUser { get; set; }


        public IEnumerable<DetalleCarrito> DetalleCarritos { get; set; }






    }
}
