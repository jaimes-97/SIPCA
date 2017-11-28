using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using SIPCA.MVC.ViewModels;
using SIPCA.CLASES.Models;
using PagedList;

namespace SIPCA.MVC.Controllers
{
    public class HomeController : Controller
    {
        private SIPCA.CLASES.Context.ModelContext db = new SIPCA.CLASES.Context.ModelContext();


        public ActionResult Index(int? categoria)
        {
            if (Session["contador"] == null)
                Session["contador"] = 0;
            else
                if (int.Parse(Session["contador"].ToString()) == 20)
                    Session["contador"] = null;
                else
                    Session["contador"] = int.Parse(Session["contador"].ToString()) + 1;

            var productos = db.Productos.ToList();
            var lotes = db.Lotes.ToList();
            var imagenes = db.imagenes.ToList();
            var categorias = from cat in db.Categorias where cat.Eliminado== false select cat;

            ModeloIndex modelo_index = new ModeloIndex();
            List<Producto> existentes = new List<Producto>();
            

            foreach (Producto pro in productos)
            {
                foreach (Lote lo in lotes)
                {
                    if (pro.IdProducto == lo.ProductoId && lo.Existencia > 0)
                    {
                        if (pro.Imagen == null && pro.ImagenId!=null)
                        {
                            foreach(Imagen img in imagenes)
                            {
                                if (pro.ImagenId == img.IdImagen)
                                {
                                    pro.Imagen = img;
                                }
                            }
                        }
                        if (categoria == null)
                            existentes.Add(pro);
                        else {
                            if (pro.CategoriaId == categoria)
                                existentes.Add(pro);
                        }
                    }
                }
            }
            modelo_index.Productos = existentes;
            modelo_index.categorias = categorias.ToList();

            return View(modelo_index);
        }


        //[HttpPost]
        //public ActionResult Index(int idCategoria)
        //{
  
        //   var id_categoria = idCategoria;
        //    Debug.WriteLine("llega la mierda " + id_categoria);

        //    return View();
        //}

        [Authorize(Roles = "Admin")]
        public ActionResult Index2()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        [Authorize]
        public ActionResult DefineHome()
        {
            var userId = User.Identity.GetUserId();
            var userManager =
                new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));
            var result = userManager.IsInRole(userId, "Admin");
            if (result)
                return RedirectToAction("Index2");
            else
                return RedirectToAction("Index");
        }
    }
}