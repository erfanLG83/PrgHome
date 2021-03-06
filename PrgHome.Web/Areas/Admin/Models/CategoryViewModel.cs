using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PrgHome.Web.Areas.Admin.Models
{
    public class CategoryViewModel
    {
        public int Id { get; set; }
        public int Row { get; set; }
        public string Title { get; set; }
        public int ArticleCount { get; set; }
       
    }
}
