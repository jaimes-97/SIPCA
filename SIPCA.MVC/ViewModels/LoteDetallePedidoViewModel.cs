using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIPCA.MVC.ViewModels
{
    
    public class LoteDetallePedidoViewModel
    {
        [Key]
        public int IdLoteDetallePedido { get; set; }

        [Required]
        public int CantidadRestar { get; set; }

        [Required(ErrorMessage = "Se requiere el {0}")]
        [Display(Name = "Lote")]
        public int IdLote { get; set; }

        [ForeignKey("LoteID")]
        public virtual LoteViewModel Lote { get; set; }


        [Required(ErrorMessage = "Se requiere el {0}")]
        [Display(Name = "Lote")]
        public int IdDetallePedido { get; set; }

        [ForeignKey("DetallePedidoId")]
        public virtual DetallePedidoViewModel DetallePedido { get; set; }

    }
}
