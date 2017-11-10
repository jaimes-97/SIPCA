using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using SIPCA.CLASES;
using SIPCA.CLASES.Context;
using PagedList;
using SIPCA.MVC.CustomFilters;

namespace SIPCA.MVC.Controllers
{
    [Authorize]
    [AuthLogAttribute(Roles = "Admin")]
    public class MarcasController : Controller
    {
        private ModelContext db = new ModelContext();

        // GET: Marcas
        public ActionResult Index(string sort, string search, int? page)
        {

            ViewBag.MarcaSort = sort == "Marcas" ? "Nombre" : "Marcas";


            ViewBag.CurrentSort = sort;
            ViewBag.CurrentSearch = search;

            //Lo utilizamos para evaluar la carga de datos de marcas
            // solo seselecionan los que no se han eliminado
            IQueryable<Marca> foundMarcas =
                db.Marcas.Where(m => m.Eliminado == false);


            //Si el campo de busqueda no esta vacio validamos que la cadena de busqueda se encuentre entre las columnas 
            // de categoria que queramos en este caso solo nombre.
            //y si se encuentra se seleccionan las filas y las columnas que contengan la cadena y asi cambia el Viewbag
            if (!string.IsNullOrEmpty(search)) foundMarcas = foundMarcas.Where(ma => ma.Nombre.Contains(search));


            //Utilizamos un switch para las columnas que queramos ordenar
            // en este caso decimos que al selecionar la columna nombre se
            //mostraram sus registros en orden desendente
            switch (sort)
            {
                case "Nombre":
                    foundMarcas = foundMarcas.OrderByDescending(ti => ti.Nombre);
                    break;

                default:
                    foundMarcas = foundMarcas.OrderBy(ti => ti.Nombre);
                    break;
            }

            int pageSize = 5;
            int pageNumber = page ?? 1;

            // retornamos la vista ya filtrada con los campos respectivos
            //con un tamaño de 5 registros por pagina
            return View(foundMarcas.ToPagedList(pageNumber, pageSize));
        }
        // GET: Marcas/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Marca marca = db.Marcas.Find(id);
            if (marca == null)
            {
                return HttpNotFound();
            }
            return View(marca);
        }

        // GET: Marcas/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Marcas/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "IdMarca,Nombre")] Marca marca)
        {
            if (ModelState.IsValid)
            {
                db.Marcas.Add(marca);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(marca);
        }

        // GET: Marcas/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Marca marca = db.Marcas.Find(id);
            if (marca == null)
            {
                return HttpNotFound();
            }
            return View(marca);
        }

        // POST: Marcas/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "IdMarca,Nombre")] Marca marca)
        {
            if (ModelState.IsValid)
            {
                db.Entry(marca).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(marca);
        }

        // GET: Marcas/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Marca marca = db.Marcas.Find(id);
            if (marca == null)
            {
                return HttpNotFound();
            }
            return View(marca);
        }

        // POST: Marcas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Marca marca = db.Marcas.Find(id);
            marca.Eliminado = true;
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
