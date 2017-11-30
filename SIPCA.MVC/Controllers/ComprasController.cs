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
    public class ComprasController : Controller
    {
        private ModelContext db = new ModelContext();


        public string obtenerUltimoConsecutivo()
        {
            int ult = db.Compras.Count();
            ult += 1;
            Debug.WriteLine("conteo de registros " + ult);
            string ultimo2 = null;

            if (ult <= 9)
            {
                ultimo2 = "0000" + ult;
                return ultimo2;
            }
            if (ult > 9 && ult < 100)
            {
                ultimo2 = "000" + ult;
                return ultimo2;
            }
            if (ult > 100 && ult < 1000)
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

        // GET: Compras
        public ActionResult Index()
        {
            var compras = db.Compras.Include(c =>c.Proveedor ).Where(p => p.Eliminado == false);
            return View(compras);
        }



        // GET: Compras/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Compra compra = db.Compras.Find(id);
            if (compra == null)
            {
                return HttpNotFound();
            }
            return View(compra);
        }

        // GET: Compras/Create
        public ActionResult Create()
        {

            Compra compra = new Compra();
            compra.NCompra = obtenerUltimoConsecutivo();
            //ViewBag.NCompra = obtenerUltimoConsecutivo();
            ViewBag.Fecha = DateTime.Now;
            ViewBag.ProveedorId = new SelectList(db.Proveedores, "IdProveedor", "Nombre");
            
            return View(compra);
        }

        // POST: Compras/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "NCompra,ProveedorId,FechaEliminacion")] Compra compra)
        {
            
            compra.Eliminado = false;
            compra.FechaEliminacion = DateTime.Now;
            compra.Fecha = DateTime.Now;
            ModelState.Clear();
            TryValidateModel(compra);
            if (ModelState.IsValid)
            {
                
               
               
                db.Compras.Add(compra);
                try
                {
                    db.SaveChanges();
                    return RedirectToAction("Edit", new { controller = "Compras", action = "Edit", Id = compra.IdCompra });

                }
                catch(Exception e)
                {
                    Debug.WriteLine(e);
                }
                
                return RedirectToAction("Edit", new { controller = "Compras", action = "Edit", Id = compra.IdCompra });
            }

            ViewBag.ProveedorId = new SelectList(db.Proveedores, "IdProveedor", "Nombre", compra.ProveedorId);
            return View(compra);
        }

        // GET: Compras/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Compra compra = db.Compras.Find(id);
            if (compra == null)
            {
                return HttpNotFound();
            }
            ViewBag.ProveedorId = new SelectList(db.Proveedores, "IdProveedor", "Nombre", compra.ProveedorId);
           // ViewBag.loteId = new SelectList(db.Compras, "Id", "Name", factura.ClienteId);
            return View(compra);
        }

        public ActionResult Edit2(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Compra compra = db.Compras.Find(id);
            if (compra == null)
            {
                return HttpNotFound();
            }
            ViewBag.ProveedorId = new SelectList(db.Proveedores, "IdProveedor", "Nombre", compra.ProveedorId);
            // ViewBag.loteId = new SelectList(db.Compras, "Id", "Name", factura.ClienteId);
            return View(compra);
        }

        // POST: Compras/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "IdCompra,NCompra,Fecha,Total,ProveedorId,Eliminado,FechaEliminacion,Control")] Compra compra)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    db.Entry(compra).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }catch(Exception e)
                {
                    Debug.WriteLine("EXCEPCION " + e);
                }
               
            }
            ViewBag.ProveedorId = new SelectList(db.Proveedores, "IdProveedor", "Nombre", compra.ProveedorId);
            return View(compra);
        }

        // GET: Compras/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Compra compra = db.Compras.Find(id);
            if (compra == null)
            {
                return HttpNotFound();
            }
            return View(compra);
        }

        // POST: Compras/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            eliminarCompra(id);
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

        public int eliminarCompra(int compraId)
        {
            var compras = db.Compras.ToList();
            foreach (Compra c in compras)
            {
                if (c.IdCompra == compraId)
                {
                    c.Eliminado = true;
                  
                    db.Entry(c).State = EntityState.Modified;
                    try
                    {
                        db.SaveChanges();
                        eliminarLotes(c.IdCompra);
                        return 1;// indica que elimino logicamente

                    }
                    catch (Exception e)
                    {
                        Debug.WriteLine("EXCEPTION " + e);
                    }

                }
            }
            return 0;
        }

        public void eliminarLotes(int compraId)
        {
            var lotes = db.Lotes.ToList();
            foreach (Lote l in lotes)
            {
                if (l.CompraId == compraId)
                {
                    l.Eliminado = true;
                    db.Entry(l).State = EntityState.Modified;
                    try
                    {
                        db.SaveChanges();
                       
                        // return 1;// no se retorna para que recorra todos los registros eliminando los respectivos

                    }
                    catch (Exception e)
                    {
                        Debug.WriteLine("EXCEPTION " + e);
                    }

                }
            }
        }

       
    }
}
