using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIPCA.MVC.ViewModels
{
    
    public class MarcaViewModel
    {
        [Key]
        public int IdMarca { get; set; }

        [Required(ErrorMessage = "Se requiere el {0}")]
        [Display(Name = "Nombre de la marca")]
        private string Nombre { get; set; }

        public virtual IEnumerable<ProductoViewModel> Productos { get; set; }
    }
}
