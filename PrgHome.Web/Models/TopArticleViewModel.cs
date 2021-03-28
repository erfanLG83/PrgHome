using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PrgHome.Web.Models
{
    public class TopArticleViewModel
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
        public string PublishDate { get; set; }
        public string CategoryTitle { get; set; }
        public int TimeToRead { get; set; }
        public int View { get; set; }
    }
}
