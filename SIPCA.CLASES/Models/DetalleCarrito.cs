using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIPCA.CLASES.Models
{
    [Table("DetalleCarrito")]
    public class DetalleCarrito
    {
        [Key]
        public int IdDetalleCarrito { get; set; }

        [Required(ErrorMessage = "Fallo en el {0} ")]
        [Display(Name = "Cantidad")]
        public int cantidad { get; set; }

        [Required(ErrorMessage = "Fallo en el {0} ")]
        [Display(Name = "Sub Total")]
        public float SubTotal { get; set; }

        [Required]
        [ScaffoldColumn(false)]
        public bool estado { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        [ScaffoldColumn(false)]
        public DateTime Fecha { get; set; }

        [Required(ErrorMessage = "Se requiere el {0}")]
        [Display(Name = "Producto")]
        public int ProductoId { get; set; }

        //[Required(ErrorMessage = "Se requiere el {0}")]
        //[Display(Name = "Aplica IVA")]
        //public bool aplicaIVA { get; set; }

        //[Required(ErrorMessage = "Se requiere el {0}")]
        //[Display(Name = "porcentajeIVA")]
        //public decimal porcentajeIVA { get; set; }

        [ForeignKey("ProductoId")]
        public Producto Producto { get; set; }


       


        public int IdCarrito { get; set; }

        [ForeignKey("IdCarrito")]

        public virtual Carrito Carritos { get; set; }


    }
}
