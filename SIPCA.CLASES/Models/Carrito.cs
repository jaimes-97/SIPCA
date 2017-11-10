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

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        [DisplayFormat(DataFormatString = "{0:0.####}", ApplyFormatInEditMode = true)]
        [Display(Name = "TOTAL")]
        public decimal Total { get; set; }

        [Required]
        [ScaffoldColumn(false)]
        public bool estado { get; set; }

        // acá debe ir el id del usuario que se tomará de la tabla de usuario que se genera por identity
        [Required(ErrorMessage = "Se requiere el {0}")]
        [Display(Name = "Usuario")]
        public string ApplicationUserId { get; set; }

        public IEnumerable<DetalleCarrito> DetalleCarritos { get; set; }


        [Required(ErrorMessage = "Se requiere la {0}")]
        [Display(Name = "Cliente")]
        public int ClienteId { get; set; }

        [ForeignKey("ClienteId")]
         public virtual Cliente Cliente { get; set; }

    }
}
