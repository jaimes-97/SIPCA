using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIPCA.CLASES.Models
{
    [Table ("TipoEntrega")]
  public  class TipoEntrega
    {
        [Key]
        public int IdTipoEntrega { get; set; }

        [Index("INDEX_TIPO_ENTREGA_NOMBRE", IsUnique = true)]
        [Required(ErrorMessage = "Se requiere el {0}")]
        [Display(Name = "Tipo de Entrega")]
        public string NombreTipoEntrega { get; set; }

        [Required(ErrorMessage ="Se requiere el costo")]
        [Display(Name ="Costo")]
        public float Costo { get; set; }

        [ScaffoldColumn(false)]
        public bool Eliminado { get; set; }

        [ScaffoldColumn(false)]
        [Timestamp]
        public byte[] Control { get; set; }

    }
}
