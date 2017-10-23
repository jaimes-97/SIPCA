using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIPCA.CLASES
{
    [Table ("Compra")]
    public class Compra
    {
        [Key]
        public int IdCompra { get; set; }

        [Required(ErrorMessage ="Fallo en el {0}")]
        [Display(Name ="Número de compra")]
        public string NCompra { get; set;}

        [Required]
        [DataType(DataType.DateTime)]
        [ScaffoldColumn(false)]
        public DateTime Fecha { get; set; }

        [Required(ErrorMessage = "Fallo en el {0} ")]
        [Display(Name = "Total")]
        public float Total { get; set; }

        [Required(ErrorMessage = "Se requiere el {0} ")]
        [Display(Name = "Proveedor")]
        public int ProveedorId { get; set; }

        [ForeignKey("ProveedorId")]
        public virtual Proveedor Proveedor { get; set; }

        public virtual IEnumerable<Lote> Lote { get; set; }
    }
}
