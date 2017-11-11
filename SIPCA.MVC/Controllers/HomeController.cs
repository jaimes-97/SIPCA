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

namespace SIPCA.MVC.Controllers
{
    public class HomeController : Controller
    {
        private SIPCA.CLASES.Context.ModelContext db = new SIPCA.CLASES.Context.ModelContext();

        public ActionResult Index()
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
            List<Producto> existentes = new List<Producto>();
            foreach (Producto pro in productos)
            {
                foreach (Lote lo in lotes)
                {
                    if (pro.IdProducto == lo.ProductoId && lo.Existencia > 0)
                    {
                        existentes.Add(pro);
                    }
                }
            }


            return View(existentes);
        }
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