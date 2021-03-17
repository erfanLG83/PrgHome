using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity;

namespace PrgHome.DataLayer.IdentityClasses
{
    public class AppUserRole:IdentityUserRole<string>
    {
        public AppRole Role { get; set; }
        public IdentityUser User { get; set; }
    }
}
