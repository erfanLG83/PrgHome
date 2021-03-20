using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PrgHome.DataLayer.IdentityClasses;

namespace PrgHome.Web.Areas.Admin.Models
{
    public class RoleDetailsViewModel
    {
        public RoleDetailsViewModel()
        {

        }
        public RoleDetailsViewModel(AppRole role)
        {
            Id = role.Id;
            Title = role.Name;
            UsersCount = role.UserRoles.Count;
            Description = role.Description;
        }
        public string Id { get; set; }
        public string Title { get; set; }
        public int UsersCount { get; set; }
        public string Description { get; set; }
    }
}
