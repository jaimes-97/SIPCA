using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SIPCA.MVC.ViewModels
{
    public class ApplicationRoleViewModel
    {
        public string Id { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Debe ingresar el nombre del Rol.")]
        [StringLength(30, ErrorMessage = "El nombre del rol debe ser 30 caracteres o menos.")]
        [Display(Name = "Nombre del Rol")]
        public string Name { get; set; }
    }
}