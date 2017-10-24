using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIPCA.MVC.ViewModels
{
   
    public class DetallePedidoViewModel
    {
        [Key]
        public int IdDetallePedido { get; set; }

        [Required]
        [ScaffoldColumn(false)]
        public int IdPedido { get; set; }

        [ForeignKey("PedidoId")]
        public virtual PedidoViewModel Pedido { get; set; }


        [Required(ErrorMessage = "Se requiere la {0}")]
        [Display(Name = "Cantidad")]
        public int Cantidad { get; set; }

        [Required(ErrorMessage = "Se requiere el {0}")]
        [Display(Name = "Precio")]
        public float PrecioVendido { get; set; }

        public virtual IEnumerable<LoteDetallePedidoViewModel> LotesDetallesPedidos { get; set; }
        

    }
}
