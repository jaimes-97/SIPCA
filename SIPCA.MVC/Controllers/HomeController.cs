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
using SIPCA.CLASES.Context;

namespace SIPCA.MVC.Controllers
{
    public class HomeController : Controller
    {
        private SIPCA.CLASES.Context.ModelContext db = new SIPCA.CLASES.Context.ModelContext();


        public ActionResult Index(int? categoria)
        {
            var productos = db.Productos.ToList();
            var lotes = db.Lotes.ToList();
            var imagenes = db.imagenes.ToList();
            var categorias = from cat in db.Categorias where cat.Eliminado== false select cat;

            ModeloIndex modelo_index = new ModeloIndex();
            List<Producto> existentes = new List<Producto>();
            List<Producto> SliderProductos = new List<Producto>();

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
                        SliderProductos.Add(pro);
                        if (categoria == null)
                            existentes.Add(pro);
                        else {
                            if (pro.CategoriaId == categoria)
                                existentes.Add(pro);
                        }
                    }
                }
            }
            ViewBag.slider = crearSlider(SliderProductos);
            modelo_index.Productos = existentes;
            modelo_index.categorias = categorias.ToList();

            return View(modelo_index);
        }

        private List<Producto> crearSlider(List<Producto> productos) {
            var items = 4;
            var cantidadProductos = productos.Count();
            if (cantidadProductos <= items)
                return productos;
            List<Producto> slider = new List<Producto>();
            Random r = new Random();
            for (int i = 0; i < items; i++)
            {
                bool estaEnLista = true;
                while (estaEnLista)
                {
                    int random = r.Next(cantidadProductos);
                    Debug.WriteLine("numero random: "+random);
                    Producto p = productos.ElementAt(random);
                    if (slider.Count() == 0 || slider.Find(pr => pr.IdProducto == p.IdProducto) == null)
                    {
                        slider.Add(p);
                        estaEnLista = false;
                    }
                }
            }
            return slider;
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
            using (SIPCA.CLASES.Context.ModelContext db = new ModelContext())
            {
                var userId = User.Identity.GetUserId();
                CLASES.Models.Cliente cl = db.Clientes.Where(c => c.UserId == userId).Where(c => c.Eliminado == false).FirstOrDefault();
                if (cl != null)
                {
                    Carrito carrito = db.Carritos.Where(c => c.ClienteId == cl.IdCliente).FirstOrDefault();
                    try {

                        Session["contador"] = db.DetallesCarritos.Where(d => d.IdCarrito == carrito.IdCarrito).Count();
                    }catch(Exception e)
                    {
                        Session["contador"] = 0;
                    }
                       
                }
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
}