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
using PagedList;
using SIPCA.MVC.CustomFilters;

namespace SIPCA.MVC.Controllers
{
    [Authorize]
    [AuthLogAttribute(Roles = "Admin")]
    public class ProductosController : Controller
    {
        private ModelContext db = new ModelContext();

        // GET: Productos
        public ActionResult Index(string sort, string search, int? page)
        {
            ViewBag.ProductoSort = sort == "Productos" ? "nombre" : "Productos";
            ViewBag.MarcaSort = sort == "Productos" ? "categoria" : "Productos";
            ViewBag.CategoriaSort = sort == "Productos" ? "marca" : "Productos";
            ViewBag.PrecioSort = sort == "Productos" ? "precioventa" : "Productos";

            ViewBag.CurrentSort = sort;
            ViewBag.CurrentSearch = search;

            //Lo utilizamos para evaluar la carga de datos de marcas
            // solo seselecionan los que no se han eliminado
            IQueryable<Producto> foundProductos = db.Productos.Where(p => p.Eliminado == false);

            //Si el campo de busqueda no esta vacio validamos que la cadena de busqueda se encuentre entre las columnas 
            // de categoria que queramos en este caso solo nombre.
            //y si se encuentra se seleccionan las filas y las columnas que contengan la cadena y asi cambia el Viewbag
            if (!string.IsNullOrEmpty(search)) foundProductos = foundProductos.Where(ma => ma.Nombre.Contains(search));


            //Utilizamos un switch para las columnas que queramos ordenar
            // en este caso decimos que al selecionar la columna nombre se
            //mostraram sus registros en orden desendente
            switch (sort)
            {
                case "nombre":
                    foundProductos = foundProductos.OrderByDescending(ti => ti.Nombre);
                    break;

                case "categoria":
                    foundProductos = foundProductos.OrderByDescending(ti => ti.Categoria);
                    break;

                case "marca":
                    foundProductos = foundProductos.OrderByDescending(ti => ti.Marca);
                    break;

                case "precioventa":
                    foundProductos = foundProductos.OrderByDescending(ti => ti.PrecioVenta);
                    break;

                default:
                    foundProductos = foundProductos.OrderBy(ti => ti.Nombre);
                    break;
            }

            int pageSize = 5;
            int pageNumber = page ?? 1;

            // retornamos la vista ya filtrada con los campos respectivos
            //con un tamaño de 5 registros por pagina
            return View(foundProductos.ToPagedList(pageNumber, pageSize));
        }

        // GET: Productos/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Producto producto = db.Productos.Find(id);
            if (producto == null)
            {
                return HttpNotFound();
            }
            return View(producto);
        }

        // GET: Productos/Create
        public ActionResult Create()
        {
            ViewBag.CategoriaId = new SelectList(db.Categorias, "IdCategoria", "Nombre");
            ViewBag.MarcaId = new SelectList(db.Marcas, "IdMarca", "Nombre");
            return View();
        }

        // POST: Productos/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "IdProducto,CategoriaId,Nombre,MarcaId,Eliminado,PrecioVenta")] Producto producto)
        {
            if (ModelState.IsValid)
            {
                db.Productos.Add(producto);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CategoriaId = new SelectList(db.Categorias, "IdCategoria", "Nombre", producto.CategoriaId);
            ViewBag.MarcaId = new SelectList(db.Marcas, "IdMarca", "Nombre", producto.MarcaId);
            return View(producto);
        }

        // GET: Productos/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Producto producto = db.Productos.Find(id);
            if (producto == null)
            {
                return HttpNotFound();
            }
            ViewBag.CategoriaId = new SelectList(db.Categorias, "IdCategoria", "Nombre", producto.CategoriaId);
            ViewBag.MarcaId = new SelectList(db.Marcas, "IdMarca", "Nombre", producto.MarcaId);
            return View(producto);
        }

        // POST: Productos/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "IdProducto,CategoriaId,Nombre,MarcaId,Eliminado,PrecioVenta")] Producto producto)
        {
            if (ModelState.IsValid)
            {
                db.Entry(producto).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CategoriaId = new SelectList(db.Categorias, "IdCategoria", "Nombre", producto.CategoriaId);
            ViewBag.MarcaId = new SelectList(db.Marcas, "IdMarca", "Nombre", producto.MarcaId);
            return View(producto);
        }

        // GET: Productos/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Producto producto = db.Productos.Find(id);
            if (producto == null)
            {
                return HttpNotFound();
            }
            return View(producto);
        }

        // POST: Productos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Producto producto = db.Productos.Find(id);
            producto.Eliminado = true;
       
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
