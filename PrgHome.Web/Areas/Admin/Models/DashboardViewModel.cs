using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PrgHome.Web.Areas.Admin.Models
{
    public class DashboardViewModel
    {
        public IEnumerable<CategoryViewModel> Categories { get; set; }
        public IEnumerable<ArticleViewModel> Articles { get; set; }
    }
}
