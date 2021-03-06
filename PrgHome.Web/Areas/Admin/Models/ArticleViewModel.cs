using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace PrgHome.Web.Areas.Admin.Models
{
    public class ArticleViewModel
    {
        public string CategoryTitle { get; set; }
        public string Title { get; set; }
        public string Image { get; set; }
        public int View { get; set; }
        public int? TimeToRead { get; set; }
        public DateTime? PublishDate { get; set; }
        public bool IsPublish { get; set; }
    }
}
