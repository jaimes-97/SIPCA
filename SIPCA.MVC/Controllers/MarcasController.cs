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
using System.Data.SqlClient;
using SIPCA.MVC.Reportes.Diseño;

namespace SIPCA.MVC.Controllers
{
    [Authorize]
    [AuthLogAttribute(Roles = "Admin")]
    public class MarcasController : Controller
    {
        private ModelContext db = new ModelContext();

        // GET: Marcas
        public ActionResult Index(string sort, string search, int? page)
        {

            ViewBag.MarcaSort = sort == "Marcas" ? "Nombre" : "Marcas";


            ViewBag.CurrentSort = sort;
            ViewBag.CurrentSearch = search;

            //Lo utilizamos para evaluar la carga de datos de marcas
            // solo seselecionan los que no se han eliminado
            IQueryable<Marca> foundMarcas =
                db.Marcas.Where(m => m.Eliminado == false);


            //Si el campo de busqueda no esta vacio validamos que la cadena de busqueda se encuentre entre las columnas 
            // de categoria que queramos en este caso solo nombre.
            //y si se encuentra se seleccionan las filas y las columnas que contengan la cadena y asi cambia el Viewbag
            if (!string.IsNullOrEmpty(search)) foundMarcas = foundMarcas.Where(ma => ma.Nombre.Contains(search));


            //Utilizamos un switch para las columnas que queramos ordenar
            // en este caso decimos que al selecionar la columna nombre se
            //mostraram sus registros en orden desendente
            switch (sort)
            {
                case "Nombre":
                    foundMarcas = foundMarcas.OrderByDescending(ti => ti.Nombre);
                    break;

                default:
                    foundMarcas = foundMarcas.OrderBy(ti => ti.Nombre);
                    break;
            }

            int pageSize = 5;
            int pageNumber = page ?? 1;

            // retornamos la vista ya filtrada con los campos respectivos
            //con un tamaño de 5 registros por pagina
            return View(foundMarcas.ToPagedList(pageNumber, pageSize));
        }
        // GET: Marcas/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Marca marca = db.Marcas.Find(id);
            if (marca == null)
            {
                return HttpNotFound();
            }
            return View(marca);
        }

        // GET: Marcas/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Marcas/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Nombre")] Marca marca)
        {
            try { 
            if (ModelState.IsValid)
            {
                db.Marcas.Add(marca);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            }
            catch(Exception ex)
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
                return View(marca);
        }

        // GET: Marcas/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Marca marca = db.Marcas.Find(id);
            if (marca == null)
            {
                return HttpNotFound();
            }
            return View(marca);
        }

        // POST: Marcas/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "IdMarca,Nombre")] Marca marca)
        {
            try
            {
            if (ModelState.IsValid)
            {
                db.Entry(marca).State = EntityState.Modified;
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
            return View(marca);
        }

        // GET: Marcas/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Marca marca = db.Marcas.Find(id);
            if (marca == null)
            {
                return HttpNotFound();
            }
            return View(marca);
        }

        // POST: Marcas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Marca marca = db.Marcas.Find(id);
            marca.Eliminado = true;
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

            return File("~/Reportes/Generado/Marcas.pdf", "application/pdf", "Marcas.pdf");
        }

        public ActionResult DescargarXLS(string sort, string search)
        {
            GenerarReporte(Server.MapPath("~/Reportes/Generado/"), ConvertirUrlRelativaToUrlAbsoluta("Reportes/"), "XLS", sort, search);

            return File("~/Reportes/Generado/Marcas.xls", "application/vnd.ms-excel", "Marcas.xls");
        }



        public string GenerarReporte(string urlAbs, string urlRel, string Report, string sort, string search)
        {
            urlAbs = (urlAbs == null ? "" : urlAbs);

            ViewBag.CategoriaSort = sort == "Marcas" ? "Nombre" : "Marcas";
            string urlDir = "";

            IQueryable<Marca> Marcas = db.Marcas.Where(t => t.Eliminado == false);

            if (!string.IsNullOrEmpty(search)) Marcas = Marcas.Where(ti => ti.Nombre.Contains(search));

            switch (sort)
            {
                case "Nombre":
                    Marcas = Marcas.OrderByDescending(ti => ti.Nombre);
                    break;

                default:
                    Marcas = Marcas.OrderBy(ti => ti.Nombre);
                    break;
            }

            List<Marca> ListaMarcas = Marcas.ToList();

            System.Data.DataTable tTemp = new System.Data.DataTable();
            tTemp.Columns.Add("IdMarca", System.Type.GetType("System.Int32"));
            tTemp.Columns.Add("Nombre", System.Type.GetType("System.String"));
            //tTemp.Columns.Add("Active", System.Type.GetType("System.Boolean"));

            foreach (Marca item in ListaMarcas)
            {
                System.Data.DataRow r = tTemp.NewRow();

                r["IdMarca"] = item.IdMarca;
                r["Nombre"] = item.Nombre;
                // r["Active"] = item.Active;

                tTemp.Rows.Add(r);
            }

            CrystalDecisions.CrystalReports.Engine.ReportDocument rpt = new MarcaRepo();

            #region PDF
            if (Report == "PDF")
            {

                if (!System.IO.File.Exists(urlAbs + "Marcas.pdf"))
                {
                    rpt.SetDataSource(tTemp);
                    rpt.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat,
                        urlAbs + "Marcas.pdf");
                    urlDir = urlRel + "Marcas.pdf";

                    //this.DownloadPDF();
                    rpt.Close();
                    rpt.Dispose();
                }
                else
                {
                    try
                    {
                        System.IO.File.Delete(@urlAbs + "Marcas.pdf");

                        rpt.SetDataSource(tTemp);
                        rpt.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat,
                            urlAbs + "Marcas.pdf");
                        urlDir = urlRel + "Marcas.pdf";

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
                if (!System.IO.File.Exists(urlAbs + "Marcas.xls"))
                {
                    rpt.SetDataSource(tTemp);
                    rpt.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.Excel,
                        urlAbs + "Marcas.xls");
                    urlDir = urlRel + "Marcas.xls";

                    //this.DownloadXLS();
                    rpt.Close();
                    rpt.Dispose();
                }
                else
                {
                    try
                    {
                        System.IO.File.Delete(@urlAbs + "Marcas.xls");

                        rpt.SetDataSource(tTemp);
                        rpt.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.Excel,
                            urlAbs + "Marcas.xls");
                        urlDir = urlRel + "Marcas.xls";

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
