using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using SIPCA.CLASES.Context;
using SIPCA.CLASES.Models;

namespace SIPCA.MVC.Controllers
{
    public class DetallePedidoesController : Controller
    {
        private ModelContext db = new ModelContext();

        // GET: DetallePedidoes
        public ActionResult Index(int pedidoId)
        {
            ViewBag.PedidoId = pedidoId;
          
           
            var detallePedidos = db.DetallePedidos.Include(d => d.Pedido).Where(f => f.PedidoId == pedidoId).OrderBy(f => f.IdDetallePedido).ToList(); ;
            return PartialView("_index", detallePedidos);
        }

        // GET: DetallePedidoes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DetallePedido detallePedido = db.DetallePedidos.Find(id);
            if (detallePedido == null)
            {
                return HttpNotFound();
            }
            return View(detallePedido);
        }

        // GET: DetallePedidoes/Create
        public ActionResult Create(int pedidoId)
        {
            ViewBag.PedidoId = pedidoId;
            ViewBag.ProductoId = new SelectList(db.Productos, "IdProducto", "Nombre");
            DetallePedido nuevoDetallePedido = new DetallePedido();
            nuevoDetallePedido.PedidoId = pedidoId;

            return PartialView("_Create", nuevoDetallePedido); 
           
            
        }

        // POST: DetallePedidoes/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "IdDetallePedido,PedidoId,Cantidad,aplicaIVA,porcentajeIVA,PrecioVendido,Eliminado,ProductoId")] DetallePedido detallePedido)
        {
            detallePedido.Eliminado = false;
            System.Diagnostics.Debug.WriteLine("valor de producto obtenido " + detallePedido.ProductoId);
            if (ModelState.IsValid)
            {
                db.DetallePedidos.Add(detallePedido);
                db.SaveChanges();
                return Json(new { success = true });
            }

            ViewBag.PedidoId = new SelectList(db.Pedidos, "IdPedido", "NPedido", detallePedido.PedidoId);
            return View(detallePedido);
        }

        // GET: DetallePedidoes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DetallePedido detallePedido = db.DetallePedidos.Find(id);
            if (detallePedido == null)
            {
                return HttpNotFound();
            }
            ViewBag.PedidoId = new SelectList(db.Pedidos, "IdPedido", "NPedido", detallePedido.PedidoId);
            return View(detallePedido);
        }

        // POST: DetallePedidoes/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "IdDetallePedido,PedidoId,Cantidad,aplicaIVA,porcentajeIVA,PrecioVendido,Eliminado")] DetallePedido detallePedido)
        {
            if (ModelState.IsValid)
            {
                db.Entry(detallePedido).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.PedidoId = new SelectList(db.Pedidos, "IdPedido", "NPedido", detallePedido.PedidoId);
            return View(detallePedido);
        }

        // GET: DetallePedidoes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DetallePedido detallePedido = db.DetallePedidos.Find(id);
            if (detallePedido == null)
            {
                return HttpNotFound();
            }
            return View(detallePedido);
        }

        // POST: DetallePedidoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            DetallePedido detallePedido = db.DetallePedidos.Find(id);
            db.DetallePedidos.Remove(detallePedido);
            db.SaveChanges();
            return RedirectToAction("Index");
        }


        public float GetProductInfo(int productId)
        {
            float price = 0;
            Producto productInfo = db.Productos.FirstOrDefault(p => p.IdProducto == productId && p.Eliminado == false);
            if (productInfo != null) price = productInfo.PrecioVenta;
            return price;
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
