using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PrgHome.Web.Models
{
    public class LoginUserDto
    {
        public string ReturnUrl { get; set; }
        [Display(Name = "نام کاربری")]
        [Required(ErrorMessage ="لطفا {0} را وارد کنید")]
        [MinLength(3,ErrorMessage ="لطفا {0} را با حداقل 3 کرکتر وارد کنید")]
        public string UserName { get; set; }

        [Display(Name = "کلمه عبور")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MinLength(8, ErrorMessage = "لطفا {0} را با حداقل 8 کرکتر وارد کنید")]
        public string Password { get; set; }
        public bool RememberMe { get; set; }
    }
}
