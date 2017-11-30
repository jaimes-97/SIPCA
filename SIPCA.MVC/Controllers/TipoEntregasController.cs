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
using SIPCA.MVC.CustomFilters;
using System.Data.SqlClient;

namespace SIPCA.MVC.Controllers
{
    [Authorize]
    [AuthLogAttribute(Roles = "Admin")]
    public class TipoEntregasController : Controller
    {
        private ModelContext db = new ModelContext();

        // GET: TipoEntregas
        public ActionResult Index()
        {
            return View(db.TipoEntregas.Where(t => t.Eliminado == false).ToList());
        }

        // GET: TipoEntregas/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TipoEntrega tipoEntrega = db.TipoEntregas.Find(id);
            if (tipoEntrega == null)
            {
                return HttpNotFound();
            }
            return View(tipoEntrega);
        }

        // GET: TipoEntregas/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: TipoEntregas/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "IdTipoEntrega,NombreTipoEntrega,Costo")] TipoEntrega tipoEntrega)
        {
            try
            {
                if (ModelState.IsValid)
                 {
                db.TipoEntregas.Add(tipoEntrega);
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
            return View(tipoEntrega);
        }

        // GET: TipoEntregas/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TipoEntrega tipoEntrega = db.TipoEntregas.Find(id);
            if (tipoEntrega == null)
            {
                return HttpNotFound();
            }
            return View(tipoEntrega);
        }

        // POST: TipoEntregas/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "IdTipoEntrega,NombreTipoEntrega,Costo, Control")] TipoEntrega tipoEntrega)
        {
            try { 
            if (ModelState.IsValid)
            {
                db.Entry(tipoEntrega).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
             }
            catch(Exception ex){
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
            return View(tipoEntrega);
        }

        // GET: TipoEntregas/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TipoEntrega tipoEntrega = db.TipoEntregas.Find(id);
            if (tipoEntrega == null)
            {
                return HttpNotFound();
            }
            return View(tipoEntrega);
        }

        // POST: TipoEntregas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            TipoEntrega tipoEntrega = db.TipoEntregas.Find(id);
            tipoEntrega.Eliminado = true;
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
