﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIPCA.CLASES.Models
{
    [Table ("DetallePedido")]
    public class DetallePedido
    {
        [Key]
        public int IdDetallePedido { get; set; }

        [Required]
        [ScaffoldColumn(false)]
        public int PedidoId { get; set; }

        [ForeignKey("PedidoId")]
        public virtual Pedido Pedido { get; set; }

        [Required(ErrorMessage = "Se requiere la {0}")]
        [Display(Name = "Cantidad")]
        public int Cantidad { get; set; }

        [Required(ErrorMessage = "Se requiere el {0}")]
        [Display(Name = "Aplica IVA")]
        public bool aplicaIVA { get; set; }

        [Required(ErrorMessage = "Se requiere el {0}")]
        [Display(Name = "porcentajeIVA")]
        public decimal porcentajeIVA { get; set; }

        [Required(ErrorMessage = "Se requiere el {0}")]
        [Display(Name = "Precio")]
        public float PrecioVendido { get; set; }

        [ScaffoldColumn(false)]
        public bool Eliminado { get; set; }

        public virtual IEnumerable<LoteDetallePedido> LotesDetallesPedidos { get; set; }

         [Required(ErrorMessage = "Se requiere el {0}")]
        [Display(Name = "Producto")]
        public int ProductoId { get; set; }

        [ForeignKey("ProductoId")]
        public Producto Producto { get; set; }
    }
}
