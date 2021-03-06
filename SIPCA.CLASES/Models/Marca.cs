﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIPCA.CLASES.Models
{
    [Table("Marca")]
    public class Marca
    {
        [Key]
        public int IdMarca { get; set; }

        [Index("INDEX_MARCA_NOMBRE", IsUnique = true)]
        [Required(ErrorMessage = "Se requiere el {0}")]
        [Display(Name = "Nombre de la marca")]
        public string Nombre { get; set; }

        public virtual IEnumerable<Producto> Productos { get; set; }

        [ScaffoldColumn(false)]
        [Timestamp]
        public byte[] Control { get; set; }

        [ScaffoldColumn(false)]
        public bool Eliminado { get; set; }
    }
}
