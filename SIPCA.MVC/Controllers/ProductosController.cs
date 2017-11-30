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
using PagedList;
using SIPCA.MVC.CustomFilters;
using System.Diagnostics;
using System.Data.Entity.Migrations;
using System.Data.SqlClient;

namespace SIPCA.MVC.Controllers
{
    [Authorize]
    [AuthLogAttribute(Roles = "Admin")]
    public class ProductosController : Controller
    {
        private ModelContext db = new ModelContext();
        private Producto productoGlobal = new Producto();

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
            if (!string.IsNullOrEmpty(search))
            {
                foundProductos = foundProductos.Where(ma => ma.Nombre.Contains(search) || ma.Marca.Nombre.Contains(search) || ma.Categoria.Nombre.Contains(search) || ma.PrecioVenta.ToString().Contains(search));
            }


            //Utilizamos un switch para las columnas que queramos ordenar
            // en este caso decimos que al selecionar la columna nombre se
            //mostraram sus registros en orden desendente
            switch (sort)
            {
                case "nombre":
                    foundProductos = foundProductos.OrderByDescending(ti => ti.Nombre);
                    break;

                case "categoria":
                    foundProductos = foundProductos.OrderByDescending(ti => ti.Categoria.Nombre);
                    break;

                case "marca":
                    foundProductos = foundProductos.OrderByDescending(ti => ti.Marca.Nombre);
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
            if(producto.ImagenId != null)
            {
                Imagen imagen =db.imagenes.Find(producto.ImagenId);
                producto.Imagen = imagen;
            }
            if (producto == null)
            {
                return HttpNotFound();
            }
            return View(producto);
        }
        // GET: Productos/DetailsCarProduct/1
        [AllowAnonymous]
        public ActionResult DetailsCarProduct(int? id)
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
            if (producto.ImagenId != null)
            {
                Imagen imagen = db.imagenes.Find(producto.ImagenId);
                producto.Imagen = imagen;
            }
            producto.Lotes = db.Lotes.Where(l => l.ProductoId == producto.IdProducto && l.Eliminado == false && l.Existencia > 0).ToList();

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
        public ActionResult Create([Bind(Include = "CategoriaId,Nombre,MarcaId,Eliminado,PrecioVenta")] Producto producto, HttpPostedFileBase upload)
        {
            try
            {
                if (ModelState.IsValid)
            {
                SIPCA.CLASES.Models.Imagen imagen = new Imagen();
                if (upload != null && upload.ContentLength > 0)
                {

                    string fileName = System.IO.Path.GetFileName(upload.FileName);
                    imagen.ImageName = fileName;
                    imagen.ImagePath = "~/Imagenes/" +fileName;

                    fileName = System.IO.Path.Combine(Server.MapPath("~/Imagenes"), fileName);
                    //string path = System.IO.Path.Combine(Server.MapPath("~/Imagenes"), pic);
                    upload.SaveAs(fileName);
                    producto.Imagen = imagen;

                    //using (SIPCA.CLASES.Context.ModelContext db = new ModelContext())
                    //{
                    //    db.imagenes.Add(imagen);
                    //    db.SaveChanges();
                    //}


                }


                db.Productos.Add(producto);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            }
            catch (Exception ex)
            {
                var e = ex.GetBaseException() as SqlException;
                if (e != null)
                    switch (e.Number)
                    {
                        case 2601:
                            TempData["MsgErrorClassAgrups"] = "El registro ya existe.";
                            break;
                        default:
                            TempData["MsgErrorClassAgrups"] = "Error al guardar registro.";
                            break;
                    }
            }
            ViewBag.ClassDanger = "alert alert-danger";

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
            productoGlobal = producto;
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
        public ActionResult Edit([Bind(Include = "IdProducto,CategoriaId,Nombre,MarcaId,Eliminado,PrecioVenta,ImagenId,Imagen,Control")] SIPCA.CLASES.Models.Producto producto, HttpPostedFileBase upload)
        {
            try{
                if (ModelState.IsValid){

                    #region Editar imagen
                    Debug.WriteLine("Entra el if del model state ");
                    SIPCA.CLASES.Models.Imagen imagen = new Imagen();
                    if (upload != null && upload.ContentLength > 0)
                    {
                        Debug.WriteLine("Entra el if del upload ");
                        //eliminar imagen
                        if (producto.Imagen == null)
                        {
                            var imagenes = db.imagenes.ToList();
                            var productos = db.Productos.ToList();

                            foreach (Producto p in productos)
                            {
                                Debug.WriteLine("p.id " + p.IdProducto + " productoglobal iid " + producto.IdProducto);
                                if (p.IdProducto == producto.IdProducto)
                                {
                                    if(p.Imagen != null && p.ImagenId !=null)
                                    {
                                        Debug.WriteLine("Imagen del producto db " + p.Imagen.ImageName + " " + p.ImagenId);
                                        producto.Imagen = p.Imagen;
                                        producto.ImagenId = p.ImagenId;
                                        Debug.WriteLine("Imagen del producto local " + producto.ImagenId + " " + producto.Imagen.ImageName);
                                    }
                              
                                }
                            }

                            Debug.WriteLine("entra al if del elimiando");
                            foreach (Imagen img in imagenes)
                            {
                                if (img.IdImagen == producto.ImagenId)
                                {
                                    producto.Imagen = img;
                                }
                            }
                        }

                        if(producto.Imagen != null)
                        {
                            Debug.WriteLine("id imagen " + producto.ImagenId);
                            Debug.WriteLine("nombre imagen " + producto.Imagen.ImageName);
                            string file = System.IO.Path.Combine(HttpContext.Server.MapPath("~/Imagenes"), producto.Imagen.ImageName);
                            if (System.IO.File.Exists(file))
                            {
                                System.IO.File.Delete(file);
                            }
                        }
                        //fin eliminar imagen

                        string fileName = System.IO.Path.GetFileName(upload.FileName);
                        imagen.ImageName = fileName;
                        imagen.ImagePath = "~/Imagenes/" + fileName;
                        fileName = System.IO.Path.Combine(Server.MapPath("~/Imagenes"), fileName);

                        //string path = System.IO.Path.Combine(Server.MapPath("~/Imagenes"), pic);
                        Debug.WriteLine("file name " + fileName);
                        Debug.WriteLine("nombre imagen " + imagen.ImageName);
                        Debug.WriteLine("ruta imagen " + imagen.ImagePath);
                        upload.SaveAs(fileName);
                        producto.Imagen = imagen;

                        using (SIPCA.CLASES.Context.ModelContext db = new ModelContext())
                        {
                            db.imagenes.Add(imagen);
                            db.SaveChanges();
                            producto.Imagen.IdImagen = imagen.IdImagen;
                            producto.ImagenId = imagen.IdImagen;
                        }
                    }
                        #endregion
                        //db.Entry(producto).State = EntityState.Modified;
                        db.Set<Producto>().AddOrUpdate(producto);
                        db.SaveChanges();
                    return RedirectToAction("Index");   
                }
            } catch (Exception ex) {
                var e = ex.GetBaseException() as SqlException;
                if (e != null)
                    switch (e.Number){
                        case 2601:
                            TempData["MsgErrorClassAgrups"] = "El registro ya existe.";
                            break;
                        default:
                            TempData["MsgErrorClassAgrups"] = "Error al guardar registro.";
                            break;
                    }
            }
            ViewBag.ClassDanger = "alert alert-danger";
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

            if (producto.Imagen == null)
            {
                Debug.WriteLine("iimagennn " + producto.ImagenId);
                var imagenes = db.imagenes.ToList();
                foreach (Imagen img in imagenes)
                {
                    if (img.IdImagen == producto.ImagenId)
                    {
                        producto.Imagen = img;
                    }
                }
                Debug.WriteLine("imgenn buena " + producto.Imagen.ImageName);
                string file = System.IO.Path.Combine(HttpContext.Server.MapPath("~/Imagenes"), producto.Imagen.ImageName);
                if (System.IO.File.Exists(file))
                {
                    System.IO.File.Delete(file);
                }
            }
            if(producto.Imagen != null)
            {
                string file = System.IO.Path.Combine(HttpContext.Server.MapPath("~/Imagenes"), producto.Imagen.ImageName);
                if (System.IO.File.Exists(file))
                {
                    System.IO.File.Delete(file);
                }
            }
          
       
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
