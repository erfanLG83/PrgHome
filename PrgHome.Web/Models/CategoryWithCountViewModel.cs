using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PrgHome.Web.Models
{
    public class CategoryWithCountViewModel
    {
        public string CategoryTitle { get; set; }
        public int ArticleCount { get; set; }
        public int Id { get; set; }
    }
}
