using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIPCA.CLASES.Models
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

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        [DisplayFormat(DataFormatString = "{0:0.####}", ApplyFormatInEditMode = true)]
        [Display(Name = "TOTAL")]
        public decimal Total { get; set; }

        [Required(ErrorMessage = "Se requiere el {0} ")]
        [Display(Name = "Proveedor")]
        public int ProveedorId { get; set; }

        [ForeignKey("ProveedorId")]
        public virtual Proveedor Proveedor { get; set; }

        public virtual IEnumerable<Lote> Lote { get; set; }

        [ScaffoldColumn(false)]
        public bool Eliminado { get; set; }
       
        [Required]
        [DataType(DataType.DateTime)]
        [ScaffoldColumn(false)]
        public DateTime FechaEliminacion { get; set; }
    }
}
