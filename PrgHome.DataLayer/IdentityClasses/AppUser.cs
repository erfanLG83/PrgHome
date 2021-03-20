using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity;

namespace PrgHome.DataLayer.IdentityClasses
{
    public class AppUser:IdentityUser
    {
        public string Image { get; set; }
        public virtual List<AppUserRole> UserRoles { get; set; }
    }
}
