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
using Microsoft.AspNet.Identity;

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
            List<DetallePedido> detalles = new List<DetallePedido>();
            foreach(DetallePedido dp in detallePedidos)
            {
                if (dp.Eliminado == false)
                {
                    detalles.Add(dp);
                }
            }
            return PartialView("_index", detalles);
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






        
        public ActionResult CreateCarritoPedido(int Id)
        {
           
            var userId = User.Identity.GetUserId();
            Cliente cl = db.Clientes.Where(c => c.UserId == userId).Where(c => c.Eliminado == false).FirstOrDefault();
            if (cl == null)
            {
                return RedirectToAction("Create", "Clientes");
            }
            Carrito carrito = db.Carritos.Where(c => c.ClienteId == cl.IdCliente).FirstOrDefault();
            var detallesCarritos = db.DetallesCarritos.Include(d => d.Producto).Where(d => d.IdCarrito == carrito.IdCarrito).ToList();

            foreach (var dc in detallesCarritos)
            {
                DetallePedido dp = new DetallePedido();
                dc.Producto.Lotes = db.Lotes.Where(l => l.ProductoId == dc.Producto.IdProducto && l.Eliminado == false && l.Existencia > 0).ToList();
                dp.PedidoId = Id;
                
                dp.ProductoId = dc.Producto.IdProducto;
                dp.Cantidad = dc.cantidad;
                dp.PrecioVendido = dc.Producto.PrecioVenta;
                dp.Eliminado = false;
                foreach (var l in dc.Producto.Lotes)
                {
                    dp.aplicaIVA = l.aplicaIVA;
                    dp.porcentajeIVA = l.porcentajeIVA;
                    break;
                }

                db.DetallePedidos.Add(dp);
                try
                {
                    db.SaveChanges();
                    float total = calcularTotal(dp.Cantidad, dp.ProductoId, dp.aplicaIVA, dp.porcentajeIVA);
                    int resultado = actualizarTotalPedido(total, dp.PedidoId, 1);
                    Debug.WriteLine("RESULTADO DE LA ACTUA DEL TOTAL PEDIDO " + resultado);
                    seleccionarLotes(dp.Cantidad, dp.ProductoId, dp.IdDetallePedido);

                  
                }
                catch (Exception e)
                {
                    System.Diagnostics.Debug.WriteLine("EXCEPTION " + e);
                }
            
            //



        }


            return RedirectToAction("Details", new { controller = "Pedidoes", action = "Details", Id = Id });

        }





        public void anularPedidoDeta(int idDetalle)
        {
            var lotesDetalles = db.LoteDetallePedidos.ToList();
            var lotes = db.Lotes.ToList();
            foreach(LoteDetallePedido ldp in lotesDetalles)
            {
                if (ldp.DetallePedidoId == idDetalle)
                {
                    ldp.Eliminado = true;
                    foreach(Lote l in lotes)
                    {
                        if (l.IdLote == ldp.LoteID)
                        {
                            l.Existencia += ldp.CantidadRestar;
                            actualizarLote(l);

                         

                        }
                    }

                    db.Entry(ldp).State = EntityState.Modified;
                    db.SaveChanges();


                }
            }
        }

        public void seleccionarLotes(int cantidad,int idProducto,int detallePedido)
        {
            int cantidadRestante = cantidad;
            do
            {
                Lote lote = obtenerLote(idProducto);
                if (cantidadRestante > lote.Existencia)
                {
                    cantidadRestante = cantidadRestante - lote.Existencia;
                    actualizarLoteDetalle(lote.IdLote, detallePedido, lote.Existencia);
                    lote.Existencia = 0;
                    actualizarLote(lote);
                   
                }

                if (cantidadRestante <= lote.Existencia)
                {
                    actualizarLoteDetalle(lote.IdLote, detallePedido, cantidadRestante);
                    lote.Existencia -= cantidadRestante;
                    cantidadRestante = 0;
                    actualizarLote(lote);
                }

            } while (!(cantidadRestante==0));
            
        }

        public void actualizarLote(Lote l)
        {

            Lote lote = db.Lotes.Find(l.IdLote);
            lote = l;


           
            db.Entry(l).State = EntityState.Modified;
            db.SaveChanges();
           
        }
        public void actualizarLoteDetalle(int id_lote,int detallePedido,int cantidadRest)
        {
            try
            {
                LoteDetallePedido ldp = new LoteDetallePedido();
                ldp.LoteID = id_lote;
                ldp.DetallePedidoId = detallePedido;
                ldp.CantidadRestar = cantidadRest;
                ldp.Eliminado = false;


                db.LoteDetallePedidos.Add(ldp);
                db.SaveChanges();
               

            }
            catch (Exception e)
            {
                Debug.WriteLine("EXCEPTION " + e);
            }
        }

        public Lote obtenerLote(int productoId)
        {
            var lotes = db.Lotes.ToList();
            foreach(Lote l in lotes)
            {
                if (l.Existencia > 0 && l.ProductoId==productoId)
                {
                    return l;
                }

            }
            return null;
            
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
            System.Diagnostics.Debug.WriteLine("valor de idPedido obtenido " + detallePedido.PedidoId);

            if (ModelState.IsValid)
            {
                db.DetallePedidos.Add(detallePedido);
                try
                {
                    db.SaveChanges();
                    float total = calcularTotal(detallePedido.Cantidad, detallePedido.ProductoId, detallePedido.aplicaIVA, detallePedido.porcentajeIVA);
                  int resultado=  actualizarTotalPedido(total, detallePedido.PedidoId,1);
                    Debug.WriteLine("RESULTADO DE LA ACTUA DEL TOTAL PEDIDO " + resultado);
                    seleccionarLotes(detallePedido.Cantidad, detallePedido.ProductoId, detallePedido.IdDetallePedido);
                    
                    return Json(new { success = true });
                }
                catch (Exception e)
                {
                    System.Diagnostics.Debug.WriteLine("EXCEPTION " + e);
                }
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

        public int buscarPedidoId(int idDetalle)
        {
       
            var detalles = db.DetallePedidos.ToList();
          
                foreach(DetallePedido dp in detalles)
                {
                    if (dp.IdDetallePedido == idDetalle)
                    {
                        return dp.PedidoId;
                    }
                }
            
            return 0;
        }
        // GET: DetallePedidoes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DetallePedido detallePedido = db.DetallePedidos.Find(id);
            int pedidoId = buscarPedidoId(detallePedido.IdDetallePedido);
            if(pedidoId != 0)
            {
                detallePedido.PedidoId = pedidoId;
            }

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

            try
            {
                eliminarDetalle(id);

                
              

            }
            catch (Exception e)
            {
                Debug.WriteLine("EXCEPTION " + e);
            }

            // DetallePedido detallePedido = db.DetallePedidos.Find(id);
            // db.DetallePedidos.Remove(detallePedido);
            // db.SaveChanges();
            int pedidoId = buscarPedidoId(id);
            return RedirectToAction("Edit", new { controller = "Pedidoes", action = "Edit", Id = pedidoId });
        }

        public int eliminarDetalle(int id)
        {
            Debug.WriteLine("ID " + id);
            var detallePedidos = db.DetallePedidos.ToList();
            foreach(DetallePedido dp in detallePedidos)
            {
                if (dp.IdDetallePedido==id)
                {
                    dp.Eliminado = true;
                    db.Entry(dp).State = EntityState.Modified;
                    try
                    {
                        db.SaveChanges();
                        float total=calcularTotal(dp.Cantidad, dp.ProductoId, dp.aplicaIVA, dp.porcentajeIVA);
                        actualizarTotalPedido(total, dp.PedidoId, 2);
                        anularPedidoDeta(dp.IdDetallePedido);
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

        public float calcularTotal(float cantidad, int productoId,bool aplicaIva,decimal porcentajeIva)
        {
            float total;
            float porcentaje = (float)porcentajeIva;
            float precio = GetProductInfo(productoId);
            if (aplicaIva == true)
            {
                total = (cantidad * precio) + ((cantidad * precio)* (porcentaje/100));
                return total;
            }else
            {
                total = cantidad * precio;
                return total;
            }
           
        }

        public int actualizarTotalPedido(float total,int pedidoId,int caso)
        {
            var pedidos = db.Pedidos.ToList();

            foreach(Pedido p in pedidos)
            {
                if (p.IdPedido == pedidoId && caso==1)
                {
                    p.Total += (decimal)total;
                    System.Diagnostics.Debug.WriteLine("El total es " + p.Total);
                    if (ModelState.IsValid)
                    {
                        db.Entry(p).State = EntityState.Modified;
                        db.SaveChanges();
                        return 1;//indica que actualiza el total
                    }
                }else if (p.IdPedido == pedidoId && caso == 2)
                {
                    p.Total -= (decimal)total;
                    System.Diagnostics.Debug.WriteLine("El total es " + p.Total);
                    if (ModelState.IsValid)
                    {
                        db.Entry(p).State = EntityState.Modified;
                        db.SaveChanges();
                        return 1;//indica que actualiza el total
                    }
                }
            }
            return 0; // indica que no actualiza el total.

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
