using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using SIPCA.CLASES.Models;
using SIPCA.CLASES.Context;
using PagedList;
using SIPCA.MVC.CustomFilters;
using System.Data.SqlClient;

namespace SIPCA.MVC.Controllers
{
    [Authorize]
    [AuthLogAttribute(Roles = "Admin")]
    public class ProveedorsController : Controller
    {
        private ModelContext db = new ModelContext();

        // GET: Proveedors
        public ActionResult Index(string sort, string search, int? page)
        {
            ViewBag.ProveedorSort = sort == "Proveedors" ? "nombre" : "Proveedors";
            ViewBag.CorreoSort = sort == "Proveedors" ? "correo" : "Proveedors";
            ViewBag.DireccionSort = sort == "Proveedors" ? "direccion" : "Proveedors";
            ViewBag.TelefonoSort = sort == "Proveedors" ? "telefono" : "Proveedors";

            ViewBag.CurrentSort = sort;
            ViewBag.CurrentSearch = search;

            //Lo utilizamos para evaluar la carga de datos de marcas
            // solo seselecionan los que no se han eliminado
            IQueryable<Proveedor> foundProveedores = db.Proveedores.Where(m => m.Eliminado == false);

            //Si el campo de busqueda no esta vacio validamos que la cadena de busqueda se encuentre entre las columnas 
            // de categoria que queramos en este caso solo nombre.
            //y si se encuentra se seleccionan las filas y las columnas que contengan la cadena y asi cambia el Viewbag
            if (!string.IsNullOrEmpty(search)) foundProveedores = foundProveedores.Where(ma => ma.Nombre.Contains(search) || ma.Correo.Contains(search) || ma.Direccion.Contains(search) || ma.Telefono.Contains(search));


            //Utilizamos un switch para las columnas que queramos ordenar
            // en este caso decimos que al selecionar la columna nombre se
            //mostraram sus registros en orden desendente
            switch (sort)
            {
                case "nombre":
                    foundProveedores = foundProveedores.OrderByDescending(ti => ti.Nombre);
                    break;

                case "correo":
                    foundProveedores = foundProveedores.OrderByDescending(ti => ti.Correo);
                    break;

                case "telefono":
                    foundProveedores = foundProveedores.OrderByDescending(ti => ti.Direccion);
                    break;

                case "direccion":
                    foundProveedores = foundProveedores.OrderByDescending(ti => ti.Telefono);
                    break;

                default:
                    foundProveedores = foundProveedores.OrderBy(ti => ti.Nombre);
                    break;
            }

            int pageSize = 5;
            int pageNumber = page ?? 1;

            // retornamos la vista ya filtrada con los campos respectivos
            //con un tamaño de 5 registros por pagina
            return View(foundProveedores.ToPagedList(pageNumber, pageSize));
        }

        // GET: Proveedors/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Proveedor proveedor = db.Proveedores.Find(id);
            if (proveedor == null)
            {
                return HttpNotFound();
            }
            return View(proveedor);
        }

        // GET: Proveedors/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Proveedors/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "IdProveedor,Nombre,Correo,Direccion,Telefono,Eliminado")] Proveedor proveedor)
        {
            try
            {
                if (ModelState.IsValid)
                 {
                db.Proveedores.Add(proveedor);
                db.SaveChanges();
                return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                var e = ex.GetBaseException() as SqlException;
                if (e != null)
                    switch (e.Number)
                    {
                        case 2601:
                            TempData["MsgErrorClassAgrups"] = "El registro ya existe.";
                            break;
                        default:
                            TempData["MsgErrorClassAgrups"] = "Error al guardar registro.";
                            break;
                    }
            }
            ViewBag.ClassDanger = "alert alert-danger";
            return View(proveedor);
        }

        // GET: Proveedors/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Proveedor proveedor = db.Proveedores.Find(id);
            if (proveedor == null)
            {
                return HttpNotFound();
            }
            return View(proveedor);
        }

        // POST: Proveedors/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "IdProveedor,Nombre,Correo,Direccion,Telefono,Eliminado")] Proveedor proveedor)
        {
            try { 
            if (ModelState.IsValid)
               {
                db.Entry(proveedor).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
               }
             }
            catch (Exception ex)
            {
                var e = ex.GetBaseException() as SqlException;
                if (e != null)
                    switch (e.Number)
                    {
                        case 2601:
                            TempData["MsgErrorClassAgrups"] = "El registro ya existe.";
                            break;
                        default:
                            TempData["MsgErrorClassAgrups"] = "Error al guardar registro.";
                            break;
                    }
}
ViewBag.ClassDanger = "alert alert-danger";
            return View(proveedor);
        }

        // GET: Proveedors/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Proveedor proveedor = db.Proveedores.Find(id);
            if (proveedor == null)
            {
                return HttpNotFound();
            }
            return View(proveedor);
        }

        // POST: Proveedors/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Proveedor proveedor = db.Proveedores.Find(id);
            proveedor.Eliminado = true;
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
