using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using PrgHome.DataLayer.IdentityClasses;

namespace PrgHome.Web.Areas.Admin.Models
{
    public class RoleDto
    {
        public RoleDto()
        {

        }
        public RoleDto(AppRole role)
        {
            Id = role.Id;
            Name = role.Name;
            Description = role.Description;
        }
        public string Id { get; set; }
        [Display(Name = "نام نقش")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(250,ErrorMessage ="لطفا {0} را کمتر از 50 کرکتر وارد کنید")]
        public string Name { get; set; }

        [MaxLength(250, ErrorMessage = "لطفا {0} را کمتر از 350 کرکتر وارد کنید")]
        public string Description { get; set; }
    }
}
