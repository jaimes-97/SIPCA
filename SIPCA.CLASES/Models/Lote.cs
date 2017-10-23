using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIPCA.CLASES
{
    [Table("Lote")]
    public class Lote
    {
        [Key]
        public int IdLote { get; set; }

        [Required]
        [ScaffoldColumn(false)]
        public int Existencia { get; set; }

        [Required(ErrorMessage = "Se requiere la {0}")]
        [Display(Name = "Cantidad")]
        public int Cantidad { get; set; }

        [Required]
        [ScaffoldColumn(false)]
        public int CompraId { get; set; }
        
        [ForeignKey("CompraId")]
        public Compra Compra { get; set; }

        [Required (ErrorMessage ="Se requiere el {0}")]
        [Display (Name ="Producto")]
        public int ProductoId { get; set; }

        [ForeignKey("ProductoId")]
        public Producto Producto { get; set; }


        public IEnumerable<LoteDetallePedido> LotesDetallesPedidos { get; set; }

    }
}
