using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using SIPCA.MVC.ViewModels;
using SIPCA;
using Microsoft.AspNet.Identity.Owin;
using System.Threading.Tasks;
using PagedList;
using SIPCA.MVC;
using SIPCA.MVC.CustomFilters;

namespace ClienteSistemaFacturacion.Controllers
{
    [Authorize]
    [AuthLogAttribute(Roles = "Admin")]
    public class ApplicationUsersController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public ApplicationUsersController()
        {
        }

        public ApplicationUsersController(ApplicationUserManager userManager, ApplicationRoleManager roleManager)
        {
            UserManager = userManager;
            RoleManager = roleManager;
        }

        private ApplicationUserManager _userManager;

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        private ApplicationRoleManager _roleManager;
        public ApplicationRoleManager RoleManager
        {
            get
            {
                return _roleManager ?? HttpContext.GetOwinContext().Get<ApplicationRoleManager>();
            }
            private set
            {
                _roleManager = value;
            }
        }


        //private ApplicationDbContext db = new ApplicationDbContext();

        // GET: ApplicationUsers
        public async Task<ActionResult> Index(String sort, string search, int? page)
        {
            ViewBag.nombreUsuario = String.IsNullOrEmpty(sort) ? "nombreU_desc" : string.Empty;
            ViewBag.emailSort = sort == "email" ? "email_desc" : "email";
            ViewBag.accessFailedSort = sort == "accessFailed" ? "accessFailed_desc" : "accessFailed";
            ViewBag.userNameSort = sort == "userName" ? "userName_desc" : "userName";   

            ViewBag.CurrentSort = sort;
            ViewBag.CurrentSearch = search;

            IQueryable<ApplicationUser> applicationUsers = db.Users;

            if (!string.IsNullOrEmpty(search)) applicationUsers = applicationUsers.Where(ii => ii.NombreUsuario.Contains(search) || ii.Email.Contains(search) || ii.UserName.Contains(search));


            switch (sort)
            {
                case "nombreU_desc":
                    applicationUsers = applicationUsers.OrderByDescending(ii => ii.NombreUsuario);
                    break;

                case "email":
                    applicationUsers = applicationUsers.OrderBy(ii => ii.Email);
                    break;

                case "email_desc":
                    applicationUsers = applicationUsers.OrderByDescending(ii => ii.Email);
                    break;

                case "accessFailed":
                    applicationUsers = applicationUsers.OrderBy(ii => ii.AccessFailedCount);
                    break;

                case "accessFailed_desc":
                    applicationUsers = applicationUsers.OrderByDescending(ii => ii.AccessFailedCount);
                    break;


                case "userName":
                    applicationUsers = applicationUsers.OrderBy(ii => ii.UserName);
                    break;

                case "userName_desc":
                    applicationUsers = applicationUsers.OrderByDescending(ii => ii.UserName);
                    break;

                default:
                    applicationUsers = applicationUsers.OrderBy(ii => ii.NombreUsuario);
                    break;
            }

            int pageSize = 10;
            int pageNumber = page ?? 1;


            return View(applicationUsers.ToPagedList(pageNumber, pageSize));
        }



        // GET: ApplicationUsers/Edit/5
        public async Task<ActionResult> Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ApplicationUser applicationUser = await UserManager.FindByIdAsync(id);
            if (applicationUser == null)
            {
                return HttpNotFound();
            }

            var userRoles = await UserManager.GetRolesAsync(applicationUser.Id);
            applicationUser.RoleList = RoleManager.Roles.ToList().Select(r => new SelectListItem
            {
                Selected = userRoles.Contains(r.Name),
                Text = r.Name,
                Value = r.Name
            });
            return View(applicationUser);
        }

        // POST: ApplicationUsers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id")] ApplicationUser applicationUser, params string[] rolesSelectedOnView)
        {
            if (ModelState.IsValid)
            {
                // If the user is currently stored having the Admin role,
                var rolesCurrentlyPersistedForUser = await UserManager.GetRolesAsync(applicationUser.Id);
                bool isThisUserAnAdmin = rolesCurrentlyPersistedForUser.Contains("Admin");

                // and the user did not have the Admin role checked,
                rolesSelectedOnView = rolesSelectedOnView ?? new string[] { };
                bool isThisUserAdminDeselected = !rolesSelectedOnView.Contains("Admin");

                // and the current stored count of users with the Admin role == 1,
                var role = await RoleManager.FindByNameAsync("Admin");
                bool isOnlyOneUserAnAdmin = role.Users.Count == 1;

                // (populate the roles list in case we have to return to the Edit view)
                applicationUser = await UserManager.FindByIdAsync(applicationUser.Id);
                applicationUser.RoleList = RoleManager.Roles.ToList().Select(x => new SelectListItem()
                {
                    Selected = rolesCurrentlyPersistedForUser.Contains(x.Name),
                    Text = x.Name,
                    Value = x.Name
                });

                // then prevent the removal of the Admin role.
                if (isThisUserAnAdmin && isThisUserAdminDeselected && isOnlyOneUserAnAdmin)
                {
                    ModelState.AddModelError("", "Al menos un usuario debe tener el rol administrador; Estas tratando de eliminar al ultimo usuario con este rol.");
                    return View(applicationUser);
                }

                var result = await UserManager.AddToRolesAsync(
                    applicationUser.Id,
                    rolesSelectedOnView.Except(rolesCurrentlyPersistedForUser).ToArray());

                if (!result.Succeeded)
                {
                    ModelState.AddModelError("", result.Errors.First());
                    return View(applicationUser);
                }

                result = await UserManager.RemoveFromRolesAsync(
                    applicationUser.Id,
                    rolesCurrentlyPersistedForUser.Except(rolesSelectedOnView).ToArray());

                if (!result.Succeeded)
                {
                    ModelState.AddModelError("", result.Errors.First());
                    return View(applicationUser);
                }

                return RedirectToAction("Index");
            }

            ModelState.AddModelError("", "Ha ocurrido un error.");
            return View(applicationUser);
        }

        //// GET: ApplicationUsers/Delete/5
        //public async Task<ActionResult> Delete(string id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    ApplicationUser applicationUser = await db.ApplicationUsers.FindAsync(id);
        //    if (applicationUser == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(applicationUser);
        //}

        //// POST: ApplicationUsers/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public async Task<ActionResult> DeleteConfirmed(string id)
        //{
        //    ApplicationUser applicationUser = await db.ApplicationUsers.FindAsync(id);
        //    db.ApplicationUsers.Remove(applicationUser);
        //    await db.SaveChangesAsync();
        //    return RedirectToAction("Index");
        //}


        public async Task<ActionResult> LockAccount([Bind(Include = "Id")] string id)
        {
            await UserManager.ResetAccessFailedCountAsync(id);
            await UserManager.SetLockoutEndDateAsync(id, DateTime.UtcNow.AddYears(100));
            return RedirectToAction("Index");
        }


        public async Task<ActionResult> UnlockAccount([Bind(Include = "Id")] string id)
        {
            await UserManager.ResetAccessFailedCountAsync(id);
            await UserManager.SetLockoutEndDateAsync(id, DateTime.UtcNow.AddYears(-1));
            return RedirectToAction("Index");
        }


        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                UserManager.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
