using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace PrgHome.DataLayer.IdentityClasses
{
    public class AppRole:IdentityRole
    {
        public AppRole():base()
        {
        }

        public AppRole(string name) : base(name)
        {
        }
        public string Description { get; set; }
        public virtual List<AppUserRole> UserRoles { get; set; }
    }
}
