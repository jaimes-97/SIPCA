using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using SIPCA.MVC.CustomFilters;
using SIPCA.MVC.ViewModels;

namespace SIPCA.MVC.Controllers
{
    [Authorize]
    [AuthLogAttribute(Roles = "Admin")]
    public class ApplicationRolesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: ApplicationRoles
        public ActionResult Index()
        {
            return View(db.IdentityRoles.ToList());
        }

        // GET: ApplicationRoles/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ApplicationRole applicationRole = db.IdentityRoles.Find(id);
            if (applicationRole == null)
            {
                return HttpNotFound();
            }
            return View(applicationRole);
        }

        // GET: ApplicationRoles/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ApplicationRoles/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Name")] ApplicationRoleViewModel applicationRoleViewModel)
        {
            if (ModelState.IsValid)
            {
                ApplicationRole applicationRole = new ApplicationRole { Name = applicationRoleViewModel.Name };
                var roleManager =
                        new RoleManager<ApplicationRole>(new RoleStore<ApplicationRole>(new ApplicationDbContext()));
                var result = roleManager.Create(applicationRole);
                if (!result.Succeeded)
                {
                    ModelState.AddModelError("", result.Errors.First());
                    return View();
                }
                return RedirectToAction("Index");

            }

            return View();
        }

        // GET: ApplicationRoles/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ApplicationRole applicationRole = db.IdentityRoles.Find(id);
            if (applicationRole == null)
            {
                return HttpNotFound();
            }

            ApplicationRoleViewModel applicationRoleViewModel = new ApplicationRoleViewModel
            {
                Id = applicationRole.Id,
                Name = applicationRole.Name
            };
            return View(applicationRoleViewModel);
        }

        // POST: ApplicationRoles/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name")] ApplicationRoleViewModel applicationRoleViewModel)
        {
            if (ModelState.IsValid)
            {
                var roleManager =
                        new RoleManager<ApplicationRole>(new RoleStore<ApplicationRole>(new ApplicationDbContext()));
                ApplicationRole ApplicationRole = roleManager.FindById(applicationRoleViewModel.Id);
                string originalName = ApplicationRole.Name;

                //Si el nombre original es admin y se desea cambiar no se pueda.
                if (originalName == "Admin" && applicationRoleViewModel.Name != "Admin")
                {
                    ModelState.AddModelError("", "No puedes cambiar el nombre del role a Admin");
                    return View(applicationRoleViewModel);
                }

                //Si el nombre original no es admin y se desea poner tal no se pueda realizar.
                if (originalName != "Admin" && applicationRoleViewModel.Name == "Admin")
                {
                    ModelState.AddModelError("", "No puedes cambiar el nombre del role a Admin");
                    return View(applicationRoleViewModel);
                }

                ApplicationRole.Name = applicationRoleViewModel.Name;
                var result = roleManager.Update(ApplicationRole);
                if (!result.Succeeded)
                {
                    ModelState.AddModelError("", result.Errors.First());
                    return View();
                }

                return RedirectToAction("Index");
            }
            return View(applicationRoleViewModel);
        }

        // GET: ApplicationRoles/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ApplicationRole applicationRole = db.IdentityRoles.Find(id);
            if (applicationRole == null)
            {
                return HttpNotFound();
            }
            return View(applicationRole);
        }

        // POST: ApplicationRoles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            ApplicationRole applicationRole = db.IdentityRoles.Find(id);
            db.IdentityRoles.Remove(applicationRole);
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
