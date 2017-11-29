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
using System.Data.SqlClient;
using SIPCA.MVC.Reportes.Diseño;

namespace SIPCA.MVC.Controllers
{
    [Authorize]
    [AuthLogAttribute(Roles = "Admin")]
    public class CategoriasController : Controller
    {
        private ModelContext db = new ModelContext();

        // GET: Categorias
        public ActionResult Index(string sort, string search, int? page)
        {

            ViewBag.CategoriaSort = sort == "Categorias" ? "Nombre" : "Categorias";


            ViewBag.CurrentSort = sort;
            ViewBag.CurrentSearch = search;

            //Lo utilizamos para evaluar la carga de datos de categorias
            // solo seselecionan los que no se han eliminado
            IQueryable<Categoria> foundCategorias =
                db.Categorias.Where(c => c.Eliminado == false);


            //Si el campo de busqueda no esta vacio validamos que la cadena de busqueda se encuentre entre las columnas 
            // de categoria que queramos en este caso solo nombre.
            //y si se encuentra se seleccionan las filas y las columnas que contengan la cadena y asi cambia el Viewbag
            if (!string.IsNullOrEmpty(search)) foundCategorias = foundCategorias.Where(ca => ca.Nombre.Contains(search));


            //Utilizamos un switch para las columnas que queramos ordenar
            // en este caso decimos que al selecionar la columna nombre se
            //mostraram sus registros en orden desendente
            switch (sort)
            {
                case "Nombre":
                    foundCategorias = foundCategorias.OrderByDescending(ti => ti.Nombre);
                    break;

                default:
                    foundCategorias = foundCategorias.OrderBy(ti => ti.Nombre);
                    break;
            }

            int pageSize = 5;
            int pageNumber = page ?? 1;

            // retornamos la vista ya filtrada con los campos respectivos
            //con un tamaño de 5 registros por pagina
            return View(foundCategorias.ToPagedList(pageNumber, pageSize));
        }
        // GET: Categorias
        //public ActionResult Index()
        //{
        //    return View(db.Categorias.Where(c => c.Eliminado == false).ToList());
        //}

        // GET: Categorias/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Categoria categoria = db.Categorias.Find(id);
            if (categoria == null)
            {
                return HttpNotFound();
            }
            return View(categoria);
        }

        // GET: Categorias/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Categorias/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Nombre,Eliminado")] Categoria categoria)
        {
                try{
                    if (ModelState.IsValid){
                        db.Categorias.Add(categoria);
                        db.SaveChanges();
                        return RedirectToAction("Index");
                       }
                    }
                catch (Exception ex){
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
                return View(categoria);
        }

        // GET: Categorias/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Categoria categoria = db.Categorias.Find(id);
            if (categoria == null)
            {
                return HttpNotFound();
            }
            return View(categoria);
        }

        // POST: Categorias/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "IdCategoria,Nombre, Control")] Categoria categoria)
        {
            try{
                if (ModelState.IsValid){
                    db.Entry(categoria).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            catch(Exception ex){
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
            return View(categoria);
        }

        // GET: Categorias/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Categoria categoria = db.Categorias.Find(id);
            if (categoria == null)
            {
                return HttpNotFound();
            }
            return View(categoria);
        }

        // POST: Categorias/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Categoria categoria = db.Categorias.Find(id);
            categoria.Eliminado = true;
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

        private string ConvertirUrlRelativaToUrlAbsoluta(string relativeUrl)
        {
            string strUrl = "";
            if (Request.IsSecureConnection)
                strUrl = string.Format("https://{0}{1}{2}", Request.Url.Host, Request.Url.Port == 80 ? "" : ":" + Request.Url.Port.ToString(), "/");
            else
                strUrl = string.Format("http://{0}{1}{2}", Request.Url.Host, Request.Url.Port == 80 ? "" : ":" + Request.Url.Port.ToString(), "/");
            strUrl = strUrl + relativeUrl;
            return strUrl;
        }

        public ActionResult DescargarPDF(string sort, string search)
        {
            GenerarReporte(Server.MapPath("~/Reportes/Generado/"), ConvertirUrlRelativaToUrlAbsoluta("Reportes/"), "PDF", sort, search);

            return File("~/Reportes/Generado/Categorias.pdf", "application/pdf", "Categorias.pdf");
        }

        public ActionResult DescargarXLS(string sort, string search)
        {
            GenerarReporte(Server.MapPath("~/Reportes/Generado/"), ConvertirUrlRelativaToUrlAbsoluta("Reportes/"), "XLS", sort, search);

            return File("~/Reportes/Generado/Categorias.xls", "application/vnd.ms-excel", "Categorias.xls");
        }



        public string GenerarReporte(string urlAbs, string urlRel, string Report, string sort, string search)
        {
            urlAbs = (urlAbs == null ? "" : urlAbs);

            string urlDir = "";

            IQueryable<Categoria> Categorias = db.Categorias.Where(t => t.Eliminado == false);

            if (!string.IsNullOrEmpty(search)) Categorias = Categorias.Where(ti => ti.Nombre.Contains(search));

            switch (sort)
            {
                case "Categorias_desc":
                    Categorias = Categorias.OrderByDescending(ti => ti.Nombre);
                    break;

                default:
                    Categorias = Categorias.OrderBy(ti => ti.Nombre);
                    break;
            }

            List<Categoria> ListaCategorias = Categorias.ToList();

            System.Data.DataTable tTemp = new System.Data.DataTable();
            tTemp.Columns.Add("IdCategoria", System.Type.GetType("System.Int32"));
            tTemp.Columns.Add("Nombre", System.Type.GetType("System.String"));
            //tTemp.Columns.Add("Active", System.Type.GetType("System.Boolean"));

            foreach (Categoria item in ListaCategorias)
            {
                System.Data.DataRow r = tTemp.NewRow();

                r["IdCategoria"] = item.IdCategoria;
                r["Nombre"] = item.Nombre;
               // r["Active"] = item.Active;

                tTemp.Rows.Add(r);
            }

            CrystalDecisions.CrystalReports.Engine.ReportDocument rpt = new CategoriaRepo();

            #region PDF
            if (Report == "PDF")
            {
                
                if (!System.IO.File.Exists(urlAbs + "Categorias.pdf"))
                {
                    rpt.SetDataSource(tTemp);
                    rpt.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat,
                        urlAbs + "Categorias.pdf");
                    urlDir = urlRel + "Categorias.pdf";

                    //this.DownloadPDF();
                    rpt.Close();
                    rpt.Dispose();
                }
                else
                {
                    try
                    {
                        System.IO.File.Delete(@urlAbs + "Categorias.pdf");

                        rpt.SetDataSource(tTemp);
                        rpt.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat,
                            urlAbs + "Categorias.pdf");
                        urlDir = urlRel + "Categorias.pdf";

                        //DownloadPDF();
                        rpt.Close();
                        rpt.Dispose();
                    }
                    catch (System.IO.IOException e)
                    {
                        Console.WriteLine(e.Message);
                        return "";
                    }
                }
            }
            #endregion

            #region XLS
            else
            {

                //
                if (!System.IO.File.Exists(urlAbs + "Categorias.xls"))
                {
                    rpt.SetDataSource(tTemp);
                    rpt.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.Excel,
                        urlAbs + "Categorias.xls");
                    urlDir = urlRel + "Categorias.xls";

                    //this.DownloadXLS();
                    rpt.Close();
                    rpt.Dispose();
                }
                else
                {
                    try
                    {
                        System.IO.File.Delete(@urlAbs + "Categorias.xls");

                        rpt.SetDataSource(tTemp);
                        rpt.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.Excel,
                            urlAbs + "Categorias.xls");
                        urlDir = urlRel + "Categorias.xls";

                        //DownloadXLS();
                        rpt.Close();
                        rpt.Dispose();
                    }
                    catch (System.IO.IOException e)
                    {
                        Console.WriteLine(e.Message);
                        return "";
                    }
                }
            }
            #endregion

            return urlDir;
        }
    }
}
