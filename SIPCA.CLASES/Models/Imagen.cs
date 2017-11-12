using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace SIPCA.CLASES.Models
{
    [Table("Imagen")]
    public class Imagen
    {
        [Key]
        public int IdImagen { get; set; }

        [StringLength(255)]
        public string ImageName { get; set; }
        public string ImagePath { get; set; }
    }
}
