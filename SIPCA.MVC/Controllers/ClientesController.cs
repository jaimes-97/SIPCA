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
using Microsoft.AspNet.Identity;
using PagedList;
using SIPCA.MVC.CustomFilters;

namespace SIPCA.MVC.Controllers
{
    [Authorize]
    public class ClientesController : Controller
    {
        private ModelContext db = new ModelContext();

        // GET: Clientes
        [Authorize]
        [AuthLogAttribute(Roles = "Admin")]
        public ActionResult Index(string sort, string search, int? page)
        {

            ViewBag.ClienteSort = sort == "Clientes" ? "nombre" : "Clientes";
            ViewBag.CedulaSort = sort == "Clientes" ? "cedula" : "Clientes";
            ViewBag.DireccionSort = sort == "Clientes" ? "direccion" : "Clientes";
         
            ViewBag.CurrentSort = sort;
            ViewBag.CurrentSearch = search;

            //Lo utilizamos para evaluar la carga de datos de marcas
            // solo seselecionan los que no se han eliminado
            IQueryable<Cliente> foundClientes = db.Clientes.Where(m => m.Eliminado == false);

            //Si el campo de busqueda no esta vacio validamos que la cadena de busqueda se encuentre entre las columnas 
            // de categoria que queramos en este caso solo nombre.
            //y si se encuentra se seleccionan las filas y las columnas que contengan la cadena y asi cambia el Viewbag
            if (!string.IsNullOrEmpty(search)) foundClientes = foundClientes.Where(ma => ma.Nombre.Contains(search));

            //Utilizamos un switch para las columnas que queramos ordenar
            // en este caso decimos que al selecionar la columna nombre se
            //mostraram sus registros en orden desendente
            switch (sort)
            {
                case "nombre":
                    foundClientes = foundClientes.OrderByDescending(ti => ti.Nombre);
                    break;
                case "cedula":
                    foundClientes = foundClientes.OrderByDescending(ti => ti.Cedula);
                    break;
                case "direccion":
                    foundClientes = foundClientes.OrderByDescending(ti => ti.Direccion);
                    break;
                default:
                    foundClientes = foundClientes.OrderBy(ti => ti.Nombre);
                    break;
            }

            int pageSize = 5;
            int pageNumber = page ?? 1;

            // retornamos la vista ya filtrada con los campos respectivos
            //con un tamaño de 5 registros por pagina
            return View(foundClientes.ToPagedList(pageNumber, pageSize));
        }

        // GET: Clientes/Details/5
        [Authorize]
        [AuthLogAttribute(Roles = "Admin")]
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
        [Authorize]
        [AuthLogAttribute(Roles = "Admin")]
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
