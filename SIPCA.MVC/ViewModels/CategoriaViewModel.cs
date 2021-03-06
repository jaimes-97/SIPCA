﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIPCA.MVC.ViewModels
{
   
    public class CategoriaViewModel
    {

        [Key]
        public int IdCategoria { get; set; }

        [Required(ErrorMessage = "Se requiere el nombre de la categoría")]
        [Display(Name = "Nombre")]
        public string Nombre { get; set; }

        [ScaffoldColumn(false)]
        public bool Eliminado { get; set; }



        public IEnumerable<ProductoViewModel> Productos { get; set; }

    }
}
