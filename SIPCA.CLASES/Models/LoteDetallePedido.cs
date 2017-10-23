using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIPCA.CLASES
{
    [Table("LoteDetallePedido")]
    public class LoteDetallePedido
    {
        [Key]
        public int IdLoteDetallePedido { get; set; }

        [Required]
        public int CantidadRestar { get; set; }

        [Required(ErrorMessage = "Se requiere el {0}")]
        [Display(Name = "Lote")]
        public int LoteID { get; set; }

        [ForeignKey("LoteID")]
        public virtual Lote Lote { get; set; }


        [Required(ErrorMessage = "Se requiere el {0}")]
        [Display(Name = "Lote")]
        public int DetallePedidoId { get; set; }

        [ForeignKey("DetallePedidoId")]
        public virtual DetallePedido DetallePedido { get; set; }

    }
}
