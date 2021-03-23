using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PrgHome.DataLayer.IdentityClasses;

namespace PrgHome.Web.Areas.Admin.Models.User
{
    public class UserDetailsViewModel
    {
        public UserDetailsViewModel(AppUser user,IEnumerable<string> roles)
        {
            Id = user.Id;
            UserName = user.UserName;
            Email = user.Email;
            PhoneNumber = user.PhoneNumber;
            Image = user.Image;
            IsActive = user.IsActive;
            Roles = roles;
            TwoFactorEnabled = user.TwoFactorEnabled;
            LockoutEnabled = user.LockoutEnabled;
            EmailConfirmed = user.EmailConfirmed;
            AccessFailedCount = user.AccessFailedCount;
            LockoutEnd = user.LockoutEnd;
        }

        public string Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Image { get; set; }
        public bool IsActive { get; set; }
        public IEnumerable<string> Roles { get; set; }
        public bool TwoFactorEnabled { get; set; }

        public bool LockoutEnabled { get; set; }

        public bool EmailConfirmed { get; set; }

        public int AccessFailedCount { get; set; }

        public DateTimeOffset? LockoutEnd { get; set; }
    }
}
