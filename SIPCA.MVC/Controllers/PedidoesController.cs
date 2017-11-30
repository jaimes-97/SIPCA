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
    public class PedidoesController : Controller
    {
        private ModelContext db = new ModelContext();

        // GET: Pedidoes
        public ActionResult Index()
        {
            System.Diagnostics.Debug.WriteLine("el número de pedido es " + obtenerUltimoConsecutivo());

            var pedidos = db.Pedidos.Include(p => p.Cliente).Include(p => p.TipoEntrega).Where(p => p.Eliminado == false && p.FechaCorte >= DateTime.Now && p.Estado == 2).ToList();
            return View(pedidos);
        }

        public ActionResult IndexVencidos()
        {
            System.Diagnostics.Debug.WriteLine("el número de pedido es " + obtenerUltimoConsecutivo());

            var pedidos = db.Pedidos.Include(p => p.Cliente).Include(p => p.TipoEntrega).Where(p => p.Eliminado == false && p.FechaCorte < DateTime.Now && p.Estado == 2).ToList();
            return View(pedidos);
        }

        public ActionResult IndexTotal()
        {
            var pedidos = db.Pedidos.Include(p => p.Cliente).Include(p => p.TipoEntrega).Where(p => p.Eliminado == false && p.Estado != 1).ToList();
            return View(pedidos);
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
            Pedido p = new Pedido();
            p.NPedido = obtenerUltimoConsecutivo();
            return View(p);
        }

        // POST: Pedidoes/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "IdPedido,Fecha,ClienteId,TipoEntregaId,NPedido,Total,Eliminado,FechaEliminacion,FechaCorte,Control,Estado")] Pedido pedido)
        {

             Debug.WriteLine("valor de tipoEntrega " + pedido.TipoEntregaId);

            if (ModelState.IsValid)
            {
                Debug.WriteLine("valor de estado "+ pedido.Estado);
                Debug.WriteLine("valor de tipoEntrega " + pedido.TipoEntregaId);

                pedido.Eliminado = false;
                pedido.FechaEliminacion = System.DateTime.Now;
                pedido.Fecha = System.DateTime.Now;
                pedido.FechaCorte = pedido.Fecha.AddDays(1);
                pedido.Total = 0;
                db.Pedidos.Add(pedido);
                try
                {
                    db.SaveChanges();
                }catch(Exception e)
                {
                    Debug.WriteLine("EXCEPTION " + e);
                }
                

               
                return RedirectToAction("Edit", new { controller = "Pedidoes", action = "Edit", Id = pedido.IdPedido });
            }

            ViewBag.ClienteId = new SelectList(db.Clientes, "IdCliente", "Nombre", pedido.ClienteId);
            ViewBag.TipoEntregaId = new SelectList(db.TipoEntregas, "IdTipoEntrega", "NombreTipoEntrega", pedido.TipoEntregaId);
            return View(pedido);
        }


        //anular pedido
        public ActionResult AnularPedido(int id)
        {
            eliminarPedido(id);
            return RedirectToAction("IndexVencidos");
        }

        //


        // cancelar pedido
        
        public ActionResult CancelarPedido(int id)
        {
            Pedido pedido = db.Pedidos.Where(p => p.IdPedido == id).FirstOrDefault();
            pedido.Estado = 0;
            db.Entry(pedido).State = EntityState.Modified;
            try
            {
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch (Exception e)
            {
                Debug.WriteLine("EXCEPTION " + e);
            }

            return RedirectToAction("Index", new { controller = "Pedidoes", action = "Index"});
        }

        //

        //crear pedido de carrito
        [HttpPost]
     
        public ActionResult CreatePedidoCarrito([Bind(Include = "TipoEntregaId,Total")] Pedido pedido)
        {

            Debug.WriteLine("valor de tipoEntrega " + pedido.TipoEntregaId);

            var userId = User.Identity.GetUserId();
            Cliente cl = db.Clientes.Where(c => c.UserId == userId).Where(c => c.Eliminado == false).FirstOrDefault();
            if (cl == null)
            {
                return RedirectToAction("Create", "Clientes");
            }

                pedido.Eliminado = false;
                pedido.Estado = 2; //Activo
                pedido.FechaEliminacion = System.DateTime.Now;
                pedido.Fecha = System.DateTime.Now;
                pedido.FechaCorte = pedido.Fecha.AddDays(2);
                pedido.Total = 0;
                pedido.NPedido = obtenerUltimoConsecutivo();
               pedido.ClienteId = cl.IdCliente;
                db.Pedidos.Add(pedido);
                try
                {
                    db.SaveChanges();
                Debug.WriteLine("ID DEL PEDIDO GENERADO " + pedido.IdPedido);
                return RedirectToAction("CreateCarritoPedido", new { controller = "DetallePedidoes", action = "CreateCarritoPedido", Id = pedido.IdPedido });
            }
                catch (Exception e)
                {
                    Debug.WriteLine("EXCEPTION " + e);
                }



                return RedirectToAction("Edit", new { controller = "Pedidoes", action = "Edit", Id = pedido.IdPedido });
            

            ViewBag.ClienteId = new SelectList(db.Clientes, "IdCliente", "Nombre", pedido.ClienteId);
            ViewBag.TipoEntregaId = new SelectList(db.TipoEntregas, "IdTipoEntrega", "NombreTipoEntrega", pedido.TipoEntregaId);
            return View(pedido);
        }

        //

        public string obtenerUltimoConsecutivo()
                 {
                    int ult= db.Pedidos.Count();
                     ult += 1;
                     Debug.WriteLine("conteo de registros " + ult);
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
                
        public ActionResult DetailPedido(int? id)
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
        public ActionResult Edit([Bind(Include = "IdPedido,Fecha,ClienteId,TipoEntregaId,NPedido,Total,Eliminado,FechaEliminacion,FechaCorte,Control,Estado")] Pedido pedido)
        {
            pedido.Eliminado = false;
            if (ModelState.IsValid)
            {
                pedido.Fecha = System.DateTime.Now;
                pedido.FechaCorte = pedido.Fecha.AddDays(2);
                pedido.FechaEliminacion = System.DateTime.Now;
                db.Entry(pedido).State = EntityState.Modified;
                try
                {
                    db.SaveChanges();
                    return RedirectToAction("Index");
                } catch (Exception e)
            {
                Debug.WriteLine("EXCEPTION " + e);
             }
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

            eliminarPedido(id);
            //Pedido pedido = db.Pedidos.Find(id);
            //db.Pedidos.Remove(pedido);
           // db.SaveChanges();
            return RedirectToAction("Index");
        }


        public void actualizarLote(Lote l)
        {

            Lote lote = db.Lotes.Find(l.IdLote);
            lote = l;



            db.Entry(l).State = EntityState.Modified;
            db.SaveChanges();

        }

        public void anularPedidoDeta(int idDetalle)
        {
            var lotesDetalles = db.LoteDetallePedidos.ToList();
            var lotes = db.Lotes.ToList();
            foreach (LoteDetallePedido ldp in lotesDetalles)
            {
                if (ldp.DetallePedidoId == idDetalle)
                {
                    
                    foreach (Lote l in lotes)
                    {
                        if (l.IdLote == ldp.LoteID && ldp.Eliminado==false)
                        {
                            l.Existencia += ldp.CantidadRestar;
                            actualizarLote(l);



                        }
                    }
                    ldp.Eliminado = true;

                    db.Entry(ldp).State = EntityState.Modified;
                    db.SaveChanges();


                }
            }
        }

        public void eliminarDetallePedido(int pedidoId)
        {
            var detallesPedidos = db.DetallePedidos.ToList();
            foreach(DetallePedido dp in detallesPedidos)
            {
                if (dp.PedidoId == pedidoId)
                {
                    dp.Eliminado = true;
                    db.Entry(dp).State = EntityState.Modified;
                    try
                    {
                        db.SaveChanges();
                        anularPedidoDeta(dp.IdDetallePedido);
                       // return 1;// no se retorna para que recorra todos los registros eliminando los respectivos

                    }
                    catch (Exception e)
                    {
                        Debug.WriteLine("EXCEPTION " + e);
                    }

                }
            }
        }

        public int eliminarPedido(int pedidoId)
        {
            var pedidos = db.Pedidos.ToList();
           
            foreach(Pedido p in pedidos)
            {
                if (p.IdPedido == pedidoId)
                {
                        p.Eliminado = true;
                        p.Estado = 3;//anulado
                        db.Entry(p).State = EntityState.Modified;
                        try
                        {
                            db.SaveChanges();
                            eliminarDetallePedido(p.IdPedido);
                           return  1;// indica que elimino logicamente
                           
                        }
                        catch (Exception e)
                        {
                            Debug.WriteLine("EXCEPTION " + e);
                        }
                    
                }
            }
            return 0;
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
