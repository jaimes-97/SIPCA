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
using System.Diagnostics;

namespace SIPCA.MVC.Controllers
{
    public class LotesController : Controller
    {
        private ModelContext db = new ModelContext();

        // GET: Lotes
        public ActionResult Index(int compraId)
        {
            ViewBag.CompraId = compraId;
            var lotes = db.Lotes.Include(l => l.Compra).Include(l => l.Producto).Where(l =>l.CompraId ==compraId).OrderBy(l =>l.IdLote).ToList();
            List<Lote> detalles = new List<Lote>();
            foreach (Lote l in lotes)
            {
                if (l.Eliminado == false)
                {
                    detalles.Add(l);
                }
            }
            
            return PartialView("_Index",detalles);
        }


        public ActionResult Index2(int compraId)
        {
            ViewBag.CompraId = compraId;
            var lotes = db.Lotes.Include(l => l.Compra).Include(l => l.Producto).Where(l => l.CompraId == compraId).OrderBy(l => l.IdLote).ToList();
            List<Lote> detalles = new List<Lote>();
            foreach (Lote l in lotes)
            {
                if (l.Eliminado == false)
                {
                    detalles.Add(l);
                }
            }

            return PartialView("_Index2", detalles);
        }

        // GET: Lotes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Lote lote = db.Lotes.Find(id);
            if (lote == null)
            {
                return HttpNotFound();
            }
            return View(lote);
        }

        // GET: Lotes/Create
        public ActionResult Create(int compraId)
        {
            ViewBag.CompraId = compraId;
          
            ViewBag.ProductoId = new SelectList(db.Productos, "IdProducto", "Nombre");
            Lote nuevoLote = new Lote();
            nuevoLote.CompraId = compraId;
            return PartialView("_Create",nuevoLote);
        }

        // POST: Lotes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult Create( Lote lote)
        {
            //[Bind(Include = "Cantidad,CompraId,ProductoId,aplicaIVA,porcentajeIVA,Costo,Subtotal")]
            Debug.WriteLine("lote> "+lote.Cantidad);
            lote.Existencia = lote.Cantidad;
            lote.Eliminado = false;
            ModelState.Clear();
            TryValidateModel (lote);
            if (ModelState.IsValid)
            {
                
                db.Lotes.Add(lote);
                db.SaveChanges();
                return Json(new { success = true });
            }

            ViewBag.CompraId = new SelectList(db.Compras, "IdCompra", "NCompra", lote.CompraId);
            ViewBag.ProductoId = new SelectList(db.Productos, "IdProducto", "Nombre", lote.ProductoId);
            return View(lote);
        }

        // GET: Lotes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Lote lote = db.Lotes.Find(id);
            if (lote == null)
            {
                return HttpNotFound();
            }
            ViewBag.CompraId = new SelectList(db.Compras, "IdCompra", "NCompra", lote.CompraId);
            ViewBag.ProductoId = new SelectList(db.Productos, "IdProducto", "Nombre", lote.ProductoId);
            return View(lote);
        }

        // POST: Lotes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "IdLote,Existencia,Cantidad,CompraId,ProductoId,aplicaIVA,porcentajeIVA,Eliminado")] Lote lote)
        {
            if (ModelState.IsValid)
            {
                db.Entry(lote).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CompraId = new SelectList(db.Compras, "IdCompra", "NCompra", lote.CompraId);
            ViewBag.ProductoId = new SelectList(db.Productos, "IdProducto", "Nombre", lote.ProductoId);
            return View(lote);
        }

        // GET: Lotes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Lote lote = db.Lotes.Find(id);
            int idCompra=buscarCompraId(lote.IdLote);
            if (idCompra != 0)
            {
                lote.CompraId =idCompra;
            }
                if (lote == null)
            {
                return HttpNotFound();
            }
            return View(lote);
        }

        public int buscarCompraId(int idLote)
        {

            var detalles = db.Lotes.ToList();

            foreach (Lote l in detalles)
            {
                if (l.IdLote == idLote)
                {
                    return l.CompraId;
                }
            }

            return 0;
        }

        // POST: Lotes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
          try
            {
                eliminarLote(id);

            }

            catch (Exception e)
            {
                Debug.WriteLine("EXCEPTION " + e);
            }
            int compraId =buscarCompraId(id);
            return RedirectToAction("Edit", new { controller = "Compras", action = "Edit", Id = compraId });
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }


        public int eliminarLote(int id)
        {
            Debug.WriteLine("ID " + id);
            var lotes = db.Lotes.ToList();
            foreach (Lote l in lotes)
            {
                if (l.IdLote == id)
                {
                    l.Eliminado = true;
                    db.Entry(l).State = EntityState.Modified;
                    try
                    {
                        db.SaveChanges();
                        decimal total = calcularTotal(l.Cantidad, l.IdLote, l.aplicaIVA, l.porcentajeIVA);
                        actualizarTotalCompra(total, l.CompraId, 2);
                        anularLote(l.IdLote);
                        return 1;//

                    }
                    catch (Exception e)
                    {
                        Debug.WriteLine("EXCEPTION " + e);
                    }
                }
            }
            return 0;
        }
        public decimal calcularTotal(int cantidad, int idLote, bool aplicaIva, decimal porcentajeIva)
        {
            decimal total;
            decimal porcentaje = porcentajeIva;
            decimal precio = GetProductInfo(idLote);
            if (aplicaIva == true)
            {
                total = (cantidad * precio) + ((cantidad * precio) * (porcentaje / 100));
                return total;
            }
            else
            {
                total = cantidad * precio;
                return total;
            }

        }

        public decimal GetProductInfo(int idLote)
        {
            decimal price = 0;
            Lote productInfo = db.Lotes.FirstOrDefault(l => l.IdLote == idLote && l.Eliminado == false);
            if (productInfo != null) price = productInfo.Costo;
            return price;
        }

        public int actualizarTotalCompra(decimal total, int compraId, int caso)
        {
            var compras = db.Compras.ToList();

            foreach (Compra c in compras)
            {
                if (c.IdCompra == compraId && caso == 1)
                {
                    c.Total += (decimal)total;
                    System.Diagnostics.Debug.WriteLine("El total es " + c.Total);
                    if (ModelState.IsValid)
                    {
                        db.Entry(c).State = EntityState.Modified;
                        db.SaveChanges();
                        return 1;//indica que actualiza el total
                    }
                }
                else if (c.IdCompra == compraId && caso == 2)
                {
                    c.Total -= (decimal)total;
                    System.Diagnostics.Debug.WriteLine("El total es " + c.Total);
                    if (ModelState.IsValid)
                    {
                        db.Entry(c).State = EntityState.Modified;
                        db.SaveChanges();
                        return 1;//indica que actualiza el total
                    }
                }
            }
            return 0; // indica que no actualiza el total.

        }

        public void anularLote(int idLote)
        {
            var lotes = db.Lotes.ToList();
            
            foreach (Lote l in lotes)
            {
                if (l.IdLote == idLote)
                {
                    l.Eliminado = true;
                

                    db.Entry(l).State = EntityState.Modified;
                    db.SaveChanges();


                }
            }
        }


    }
}
