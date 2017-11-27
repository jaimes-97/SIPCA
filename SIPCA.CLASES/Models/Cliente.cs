using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIPCA.CLASES.Models
{
    [Table("Cliente")]
   public class Cliente
    {
        [Key]
        public int IdCliente { get; set; }

        [Index("INDEX_CLIENTE_NOMBRE", IsUnique = true)]
        [Required(ErrorMessage = "Se requiere el {0}")]
        [Display(Name = "Nombre")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "Se requiere la {0}")]
        [Display(Name = "Dirección")]
        public  string Direccion { get; set; }

        [Required(ErrorMessage = "Se requiere la {0}")]
        [Display(Name = "Cédula")]
        public string Cedula { get; set; }

        [Required]
        [ScaffoldColumn(false)]
        [StringLength(128, ErrorMessage = "El id del usuario debe ser menor de 128 caracteres")]
        public string UserId { get; set; }

        /*[ForeignKey("UsuarioId")]
        public virtual ApplicationUser Usuario { get; set; }*/

        [ScaffoldColumn(false)]
        public bool Eliminado { get; set; }

        public virtual IEnumerable<Pedido> Pedido { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        [ScaffoldColumn(false)]
        public DateTime FechaMod { get; set; }

        [ScaffoldColumn(false)]
        [Timestamp]
        public byte[] Control { get; set; }
    }
}
