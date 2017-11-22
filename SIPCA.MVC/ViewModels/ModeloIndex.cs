using SIPCA.CLASES.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SIPCA.MVC.ViewModels
{
    public class ModeloIndex
    {
        public List<Producto> Productos { get;  set; }
        public List<Categoria> categorias { get;  set; }
    }
}