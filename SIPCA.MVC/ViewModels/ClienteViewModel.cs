using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIPCA.CLASES
{
    
   public class Cliente
    {
        [Key]
        public int IdCliente { get; set; }

        [Required(ErrorMessage = "Se requiere el {0}")]
        [Display(Name = "Nombre")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "Se requiere la {0}")]
        [Display(Name = "Dirección")]
        public  string Direccion { get; set; }

        [Required(ErrorMessage = "Se requiere la {0}")]
        [Display(Name = "Cédula")]
        public string Cedula { get; set; }

        [Required(ErrorMessage = "Se requiere el {0}")]
        [Display(Name = "Correo")]
        public  string Correo { get; set; }


        public int IdUsuario { get; set; }

        [ScaffoldColumn(false)]
        public bool Eliminado { get; set; }

        public virtual IEnumerable<PedidoViewModel> Pedido { get; set; }
      
    }
}
