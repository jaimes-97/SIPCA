﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIPCA.CLASES.Models
{
    [Table("Pedido")]
   public class Pedido
    {
        [Key]
        public int IdPedido { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        [ScaffoldColumn(false)]
        public DateTime Fecha { get; set; }

        [Required(ErrorMessage = "Se requiere el cliente")]
        [Display(Name = "Cliente")]
        public int ClienteId { get; set; }

        [ForeignKey("ClienteId")]
        public virtual Cliente Cliente { get; set; }

        [ForeignKey("TipoEntregaId")]
        public virtual TipoEntrega TipoEntrega { get; set; }

        [Required(ErrorMessage = "Se requiere el {0}")]
        [Display(Name = "Tipo de entrega")]
        public int TipoEntregaId { get; set; }

        [Required(ErrorMessage = "Fallo en el número de pedido")]
        [Display(Name = "Número de pedido")]
        public string NPedido { get; set;}

        
        [DisplayFormat(DataFormatString = "{0:0.####}", ApplyFormatInEditMode = true)]
        [Display(Name = "TOTAL")]
        public decimal Total { get; set; }

        [ScaffoldColumn(false)]
        public bool Eliminado { get; set; }

      
        [DataType(DataType.DateTime)]
        [ScaffoldColumn(false)]
        public DateTime FechaEliminacion { get; set; }

       
        [DataType(DataType.DateTime)]
        [ScaffoldColumn(false)]
        public DateTime FechaCorte { get; set; }

        public virtual IEnumerable<DetallePedido> DetallePedidos { get; set; }

        [ScaffoldColumn(false)]
        [Timestamp]
        public byte[] Control { get; set; }

        [Display(Name = "Estado")]
        public int Estado { get; set; }
        //0 cancelado indica que el pedido se entregó.
        //1 anulado indica que el pedido no se canceló en el transcurso de un día y se deben regresar los productos al inventario
        //2 activo
    }
}
