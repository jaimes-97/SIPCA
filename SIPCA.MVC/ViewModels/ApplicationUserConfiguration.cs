using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Web;

namespace SIPCA.MVC.ViewModels
{
    public class ApplicationUserConfiguration : EntityTypeConfiguration<ApplicationUser>
    {


        public ApplicationUserConfiguration()
        {

            Property(au => au.FirstName).HasMaxLength(20).IsRequired();
            Property(au => au.LastName).HasMaxLength(20).IsOptional();
            Ignore(au => au.RoleList);
        }
    }
}