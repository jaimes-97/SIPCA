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
using SIPCA.MVC.CustomFilters;

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
            if (ModelState.IsValid)
            {
                db.TipoEntregas.Add(tipoEntrega);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

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
        public ActionResult Edit([Bind(Include = "IdTipoEntrega,NombreTipoEntrega,Costo")] TipoEntrega tipoEntrega)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tipoEntrega).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
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
