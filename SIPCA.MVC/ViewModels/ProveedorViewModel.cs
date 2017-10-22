using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIPCA.CLASES
{
    
 public  class ProveedorViewModel
    {
        [Key]
        public int IdProveedor { get; set;  }

        [Required(ErrorMessage ="Se requiere el nombre del proveedor ")]
        [Display(Name =" Nombre del proveedor")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "Se requiere el {0}")]
        [Display(Name = "Correo")]
        public string Correo { get; set; }

        [Required(ErrorMessage = "Se requiere la {0}")]
        [Display(Name = "Dirección")]
        public string Direccion { get; set; }

        [Required(ErrorMessage = "Se requiere el {0}")]
        [Display(Name = "Teléfono")]
        public string Telefono { get; set; }

        [ScaffoldColumn(false)]
        public bool Eliminado { get; set; }
    }
}
