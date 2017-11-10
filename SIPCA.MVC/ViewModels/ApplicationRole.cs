using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SIPCA.MVC.ViewModels
{
    public class ApplicationRole : IdentityRole
    {
        public ApplicationRole()
        {
        }

        public ApplicationRole(string name)
           : base(name)
        {

        }

    }
}