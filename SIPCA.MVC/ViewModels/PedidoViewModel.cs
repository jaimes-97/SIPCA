using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIPCA.MVC.ViewModels
{
   public class PedidoViewModel
    {
        [Key]
        public int IdPedido { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        [ScaffoldColumn(false)]
        public DateTime Fecha { get; set; }

        [Required(ErrorMessage = "Se requiere el cliente")]
        [Display(Name = "Cliente")]
        public int IdCliente { get; set; }

        [ForeignKey("ClienteId")]
        public virtual Cliente Cliente { get; set; }

        [ForeignKey("TipoEntregaId")]
        public virtual TipoEntregaViewModel TipoEntrega { get; set; }

        [Required(ErrorMessage = "Se requiere el {0}")]
        [Display(Name = "Tipo de entrega")]
        public int TipoEntregaId { get; set; }

        [Required(ErrorMessage = "Fallo en el número de pedido")]
        [Display(Name = "Número de pedido")]
        public string NPedido { get; set;}

        [Required(ErrorMessage = "Se requiere el total")]
        [Display(Name = "Total")]
        public float Total { get; set; }

        public virtual IEnumerable<DetallePedidoViewModel> DetallePedidos { get; set; }

        [ScaffoldColumn(false)]
        public bool Eliminado { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        [ScaffoldColumn(false)]
        public DateTime FechaEliminacion { get; set; }

    }
}
