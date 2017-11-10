using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIPCA.CLASES.Models
{
    [Table("Configuracion")]
       public  class Configuracion
    {
        [Key]
        public int IdConfiguracion { get; set; }

        [Required(ErrorMessage = "Se requiere el {0}")]
        [Display(Name = "Porcentaje de IVA")]
        public decimal PorcentajeIVA { get; set; }
    }
}
