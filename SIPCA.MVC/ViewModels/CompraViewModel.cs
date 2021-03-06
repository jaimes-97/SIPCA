﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIPCA.MVC.ViewModels
{
   
    public class CompraViewModel
    {
        [Key]
        public int IdCompra { get; set; }

        [Required(ErrorMessage ="Fallo en el {0}")]
        [Display(Name ="Número de compra")]
        public string NCompra { get; set;}

        [Required]
        [DataType(DataType.DateTime)]
        [ScaffoldColumn(false)]
        public DateTime Fecha { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        [ScaffoldColumn(false)]
        public DateTime FechaEliminacion { get; set; }

        [Required(ErrorMessage = "Fallo en el {0} ")]
        [Display(Name = "Total")]
        public float Total { get; set; }

        [Required(ErrorMessage = "Se requiere el {0} ")]
        [Display(Name = "Proveedor")]
        public int IdProveedor { get; set; }

        [ForeignKey("ProveedorId")]
        public virtual ProveedorViewModel Proveedor { get; set; }

        public virtual IEnumerable<LoteViewModel> Lote { get; set; }

        [ScaffoldColumn(false)]
        public bool Eliminado { get; set; }

 
    }
}
