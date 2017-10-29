using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIPCA.MVC.ViewModels
{
    
  public  class TipoEntregaViewModel
    {
        [Key]
        public int IdTipoEntrega { get; set; }


        [Required(ErrorMessage ="Se requiere el costo")]
        [Display(Name ="Costo")]
        public float Costo { get; set; }

        [ScaffoldColumn(false)]
        public bool Eliminado { get; set; }



    }
}
