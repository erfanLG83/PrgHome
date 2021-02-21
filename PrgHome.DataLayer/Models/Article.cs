using System;
using System.Collections.Generic;
using System.Text;

namespace PrgHome.DataLayer.Models
{
    public class Article
    {
        #region Main Properties
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Content { get; set; }
        public string Image { get; set; }
        public int View { get; set; }
        public int TimeToRead { get; set; }
        public DateTime PublishDate { get; set; }
        public bool IsPublish { get; set; }
        public string Tags { get; set; }
        #endregion

        #region Navigation Properties
        public int CategoryId { get; set; }
        public Category Category { get; set; }
        #endregion
    }
}
