using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using SIPCA.MVC.ViewModels;

namespace SIPCA.MVC.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            if (Session["contador"] == null)
                Session["contador"] = 0;
            else
                if (int.Parse(Session["contador"].ToString()) == 20)
                    Session["contador"] = null;
                else
                    Session["contador"] = int.Parse(Session["contador"].ToString()) + 1;
            return View();
        }
        [Authorize(Roles = "Admin")]
        public ActionResult Index2()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        [Authorize]
        public ActionResult DefineHome()
        {
            var userId = User.Identity.GetUserId();
            var userManager =
                new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));
            var result = userManager.IsInRole(userId, "Admin");
            if (result)
                return RedirectToAction("Index2");
            else
                return RedirectToAction("Index");
        }
    }
}