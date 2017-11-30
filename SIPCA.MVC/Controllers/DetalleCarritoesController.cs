using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using SIPCA.CLASES.Context;
using SIPCA.CLASES.Models;

namespace SIPCA.MVC.Controllers
{
    [Authorize]
    public class DetalleCarritoesController : Controller
    {
        private ModelContext db = new ModelContext();

        // GET: DetalleCarritoes
        public ActionResult Index()
        {
            var userId = User.Identity.GetUserId();
            Cliente cl = db.Clientes.Where(c => c.UserId == userId).Where(c => c.Eliminado == false).FirstOrDefault();
            if(cl == null)
            {
                return RedirectToAction("Create", "Clientes");
            }
            Carrito carrito = db.Carritos.Where(c => c.ClienteId == cl.IdCliente).FirstOrDefault();
            var detallesCarritos = db.DetallesCarritos.Include(d => d.Producto).Where(d => d.IdCarrito == carrito.IdCarrito);
            Session["contador"] = db.DetallesCarritos.Where(d => d.IdCarrito == carrito.IdCarrito).Count();
            return View(detallesCarritos.ToList());
        }

        // GET: DetalleCarritoes/Create
        public ActionResult Create()
        {
            ViewBag.IdCarrito = new SelectList(db.Carritos, "IdCarrito", "ApplicationUserId");
            ViewBag.ProductoId = new SelectList(db.Productos, "IdProducto", "Nombre");
            return View();
        }

        // POST: DetalleCarritoes/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public ActionResult Create([Bind(Include = "cantidad,ProductoId, SubTotal")] DetalleCarrito detalleCarrito)
        {

            if (ModelState.IsValid)
            {
                var userId = User.Identity.GetUserId();
                Cliente cl = db.Clientes.Where(c => c.UserId == userId).Where(c => c.Eliminado == false).FirstOrDefault();
                if (cl == null)
                {
                    return RedirectToAction("Create", "Clientes");
                }
                Carrito carrito = db.Carritos.Where(c =>c.ClienteId == cl.IdCliente).FirstOrDefault();
                if (carrito == null) {
                    carrito = new Carrito();
                    carrito.Fecha = DateTime.Now;
                    carrito.estado = true;
                    carrito.ApplicationUserId = userId;
                    carrito.ClienteId = cl.IdCliente;
                    db.Carritos.Add(carrito);
                    db.SaveChanges();
                }
                detalleCarrito.SubTotal = detalleCarrito.SubTotal * detalleCarrito.cantidad;
                detalleCarrito.estado = true;
                detalleCarrito.Fecha = DateTime.Now;
                detalleCarrito.IdCarrito = carrito.IdCarrito;
                if (db.DetallesCarritos.Where(d => d.ProductoId == detalleCarrito.ProductoId && d.IdCarrito == detalleCarrito.IdCarrito).Count() > 0)
                {
                    Debug.WriteLine("llego1 create");
                    DetalleCarrito dc = db.DetallesCarritos.Where(d => d.ProductoId == detalleCarrito.ProductoId && d.IdCarrito == detalleCarrito.IdCarrito).FirstOrDefault();
                    dc.SubTotal = dc.SubTotal / dc.cantidad;
                    dc.cantidad += detalleCarrito.cantidad;
                    return EditDetalleCarrito(dc);
                }
                //var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));
                db.DetallesCarritos.Add(detalleCarrito);
                db.SaveChanges();

                return RedirectToAction("Index");
            }

            //ViewBag.IdCarrito = new SelectList(db.Carritos, "IdCarrito", "ApplicationUserId", detalleCarrito.IdCarrito);
            //ViewBag.ProductoId = new SelectList(db.Productos, "IdProducto", "Nombre", detalleCarrito.ProductoId);
            return RedirectToAction("Index, Home");
        }

        public ActionResult EditDetalleCarrito(DetalleCarrito dc)
        {
            dc.SubTotal = dc.SubTotal * dc.cantidad;
            db.Entry(dc).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        // GET: DetalleCarritoes/Edit/5
        public ActionResult Edit(int? id)
        {
            Debug.WriteLine("llego2 get");
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DetalleCarrito detalleCarrito = db.DetallesCarritos.Find(id);
            if (detalleCarrito == null)
            {
                return HttpNotFound();
            }
            detalleCarrito.SubTotal = detalleCarrito.SubTotal / detalleCarrito.cantidad;
            Producto p = db.Productos.Where(pr => pr.IdProducto == detalleCarrito.ProductoId).FirstOrDefault();
            detalleCarrito.Producto = p;
            return View(detalleCarrito);
        }

        // POST: DetalleCarritoes/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "IdDetalleCarrito,cantidad,SubTotal,estado,Fecha,ProductoId,IdCarrito")] DetalleCarrito detalleCarrito)
        {
            Debug.WriteLine("llego2 post");
            if (ModelState.IsValid)
            {
                return EditDetalleCarrito(detalleCarrito);
            }
            return View(detalleCarrito);
        }

        // GET: DetalleCarritoes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DetalleCarrito detalleCarrito = db.DetallesCarritos.Find(id);
            if (detalleCarrito == null)
            {
                return HttpNotFound();
            }
            Producto p = db.Productos.Where(pr => pr.IdProducto == detalleCarrito.ProductoId).FirstOrDefault();
            detalleCarrito.Producto = p;
            return View(detalleCarrito);
        }

        // POST: DetalleCarritoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            DetalleCarrito detalleCarrito = db.DetallesCarritos.Find(id);
            db.DetallesCarritos.Remove(detalleCarrito);
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
