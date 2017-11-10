namespace SIPCA.MVC.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using ViewModels;

    internal sealed class Configuration : DbMigrationsConfiguration<SIPCA.MVC.ViewModels.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(SIPCA.MVC.ViewModels.ApplicationDbContext context)
        {
            var userManager =
                new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));

            userManager.UserValidator = new UserValidator<ApplicationUser>(userManager)
            {
                AllowOnlyAlphanumericUserNames = false
            };

            var roleManager = 
                new RoleManager<ApplicationRole>(new RoleStore<ApplicationRole>(new ApplicationDbContext()));

            string email = "sistemacamina@gmail.com";
            string password = "Sipca123#";
            string nombreUsuario = "admon";
            string roleName = "Admin";

            var role = roleManager.FindByName(roleName);
            if(role == null)
            {
                role = new ApplicationRole(roleName);
                var roleResult = roleManager.Create(role);
            }
            var user = userManager.FindByEmail(email);
            if(user == null)
            {
                user = new ApplicationUser { UserName = email, Email = email, NombreUsuario = nombreUsuario};
                var result = userManager.Create(user, password);
                result = userManager.SetLockoutEnabled(user.Id, false);
            }

            var rolesForUser = userManager.GetRoles(user.Id);

            if (!rolesForUser.Contains(role.Name))
            {
                var result = userManager.AddToRole(user.Id, role.Name);
            }

        }
    }
}
