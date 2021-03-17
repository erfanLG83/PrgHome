using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace PrgHome.DataLayer.IdentityClasses
{
    public class AppRole:IdentityRole
    {
        public string Description { get; set; }
    }
}
