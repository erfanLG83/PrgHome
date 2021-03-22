using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PrgHome.DataLayer.IdentityClasses;

namespace PrgHome.Web.Areas.Admin.Models.User
{
    public class UserViewModel
    {
        public UserViewModel()
        {

        }
        public UserViewModel(AppUser user , IEnumerable<string> roles , int row)
        {
            Id = user.Id;
            Row = row;
            UserName = user.UserName;
            Email = user.Email;
            Roles = "";
            foreach (var item in roles)
            {
                Roles +=  item + ",";
            }
            if (Roles.Length > 0)
            {
                Roles = Roles.Remove(Roles.Length - 1, 1);
            }
            Image = user.Image;
            IsActive = user.IsActive;
        }
        public int Row { get; set; }
        public string Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Roles { get; set; }
        public string Image { get; set; }
        public bool IsActive { get; set; }
    }
}
