using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PrgHome.Web.Areas.Admin.Models
{
    public class CategoryDto
    {
        public int Id { get; set; }
        [Display(Name = "عنوان دسته بندی")]
        [Required(ErrorMessage ="لطفا {0} را وارد کنید")]
        [MinLength(2,ErrorMessage ="لطفا {0} را با حداقل 2 کرکتر وارد کنید")]
        [MaxLength(250,ErrorMessage ="لطفا {0} را کمتر از 250 کرکتر وارد کنید")]
        public string Title { get; set; }
    }
}
