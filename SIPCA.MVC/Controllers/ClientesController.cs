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
using Microsoft.AspNet.Identity;

namespace SIPCA.MVC.Controllers
{
    [Authorize]
    public class ClientesController : Controller
    {
        private ModelContext db = new ModelContext();

        // GET: Clientes
        [Authorize(Roles = "Admin")]
        public ActionResult Index()
        {
            return View(db.Clientes.Where(c => c.Eliminado == false).ToList());
        }

        // GET: Clientes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Cliente cliente = db.Clientes.Find(id);
            if (cliente == null)
            {
                return HttpNotFound();
            }
            return View(cliente);
        }

        // GET: Clientes/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Clientes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "IdCliente,Nombre,Direccion,Cedula,Eliminado,FechaMod,UserId")] Cliente cliente)
        {
            var userId = User.Identity.GetUserId();
            Cliente cl = db.Clientes.Where(c => c.UserId == userId).Where(c => c.Eliminado == false).FirstOrDefault();
            if(cl != null)
            {
                ViewBag.ErrorMesage = "¡Usted ya es un cliente activo!";
                return View(cliente);
            }
            cliente.UserId = userId;
            ModelState.Clear();
            TryValidateModel(cliente);
            if (ModelState.IsValid)
            {
                cliente.FechaMod = System.DateTime.Now;
                db.Clientes.Add(cliente);
                db.SaveChanges();
                return RedirectToAction("Index", "Home");
            }

            return View(cliente);
        }

        // GET: Clientes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Cliente cliente = db.Clientes.Find(id);
            if (cliente == null)
            {
                return HttpNotFound();
            }
            return View(cliente);
        }

        // POST: Clientes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "IdCliente,Nombre,Direccion,Cedula,Eliminado,FechaMod,UserId")] Cliente cliente)
        {
            if (ModelState.IsValid)
            {
                cliente.FechaMod = System.DateTime.Now;
                db.Entry(cliente).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(cliente);
        }

        // GET: Clientes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Cliente cliente = db.Clientes.Find(id);
            if (cliente == null)
            {
                return HttpNotFound();
            }
            return View(cliente);
        }

        // POST: Clientes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Cliente cliente = db.Clientes.Find(id);
            db.Clientes.Remove(cliente);
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
