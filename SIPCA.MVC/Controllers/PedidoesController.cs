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
    public class PedidoesController : Controller
    {
        private ModelContext db = new ModelContext();

        // GET: Pedidoes
        public ActionResult Index()
        {
            System.Diagnostics.Debug.WriteLine("el número de pedido es " + obtenerUltimoConsecutivo());

            var pedidos = db.Pedidos.Include(p => p.Cliente).Include(p => p.TipoEntrega);
            return View(pedidos.ToList());
        }

        // GET: Pedidoes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Pedido pedido = db.Pedidos.Find(id);
            if (pedido == null)
            {
                return HttpNotFound();
            }
            return View(pedido);
        }

        // GET: Pedidoes/Create
        public ActionResult Create()
        {
            ViewBag.ClienteId = new SelectList(db.Clientes, "IdCliente", "Nombre");
            ViewBag.TipoEntregaId = new SelectList(db.TipoEntregas, "IdTipoEntrega", "NombreTipoEntrega");
            return View();
        }

        // POST: Pedidoes/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "IdPedido,Fecha,ClienteId,TipoEntregaId,NPedido,Total,Eliminado,FechaEliminacion,FechaCorte,Control")] Pedido pedido)
        {
            if (ModelState.IsValid)
            {
                pedido.Eliminado = false;
                pedido.FechaEliminacion = System.DateTime.Now;
                pedido.Fecha = System.DateTime.Now;
                pedido.FechaCorte = pedido.Fecha.AddDays(1);
                db.Pedidos.Add(pedido);
                db.SaveChanges();

                return RedirectToAction("Edit", new { controller = "Pedidoes", action = "Edit", Id = pedido.IdPedido });
            }

            ViewBag.ClienteId = new SelectList(db.Clientes, "IdCliente", "Nombre", pedido.ClienteId);
            ViewBag.TipoEntregaId = new SelectList(db.TipoEntregas, "IdTipoEntrega", "NombreTipoEntrega", pedido.TipoEntregaId);
            return View(pedido);
        }
            

            public string obtenerUltimoConsecutivo()
                 {
                    int ult= db.Pedidos.Count();
                     string ultimo2=null;
                     
                     if (ult <= 9)
                      {
                        ultimo2 = "0000" + ult;
                        return ultimo2;
                         }
                      if (ult > 9 && ult<100)
                      {
                         ultimo2 = "000" + ult;
                         return ultimo2;
                     }
                     if (ult >100 && ult<1000)
                     {
                        ultimo2 = "00" + ult;
                        return ultimo2;
                     }
                    if (ult > 1000 && ult < 10000)
                     {
                         ultimo2 = "0" + ult;
                         return ultimo2;

                    }
                     if (ult > 10000)
                     {
                         ultimo2 = ult.ToString();
                        return ultimo2;
                     }
                     return ultimo2;
                }
                
        
        // GET: Pedidoes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Pedido pedido = db.Pedidos.Find(id);
            if (pedido == null)
            {
                return HttpNotFound();
            }
            ViewBag.ClienteId = new SelectList(db.Clientes, "IdCliente", "Nombre", pedido.ClienteId);
            ViewBag.TipoEntregaId = new SelectList(db.TipoEntregas, "IdTipoEntrega", "NombreTipoEntrega", pedido.TipoEntregaId);
            return View(pedido);
        }

        // POST: Pedidoes/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "IdPedido,Fecha,ClienteId,TipoEntregaId,NPedido,Total,Eliminado,FechaEliminacion,FechaCorte,Control")] Pedido pedido)
        {
            if (ModelState.IsValid)
            {
                db.Entry(pedido).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ClienteId = new SelectList(db.Clientes, "IdCliente", "Nombre", pedido.ClienteId);
            ViewBag.TipoEntregaId = new SelectList(db.TipoEntregas, "IdTipoEntrega", "NombreTipoEntrega", pedido.TipoEntregaId);
            return View(pedido);
        }

        // GET: Pedidoes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Pedido pedido = db.Pedidos.Find(id);
            if (pedido == null)
            {
                return HttpNotFound();
            }
            return View(pedido);
        }

        // POST: Pedidoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Pedido pedido = db.Pedidos.Find(id);
            db.Pedidos.Remove(pedido);
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
