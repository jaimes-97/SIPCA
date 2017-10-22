using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIPCA.CLASES
{
    
    public class LoteViewModel
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
        public int IdCompra { get; set; }
        
        [ForeignKey("CompraId")]
        public CompraViewModel Compra { get; set; }

        [Required (ErrorMessage ="Se requiere el {0}")]
        [Display (Name ="Producto")]
        public int IdProducto { get; set; }

        [ForeignKey("ProductoId")]
        public ProductoViewModel Producto { get; set; }


        public IEnumerable<LoteDetallePedidoViewModel> LotesDetallesPedidos { get; set; }




    }
}
