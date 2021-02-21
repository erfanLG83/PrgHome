using System;
using System.Collections.Generic;
using System.Text;

namespace PrgHome.DataLayer.Models
{
    public class Category
    {
        public int Id { get; set; }
        public string Title { get; set; }
        #region Navigation Properties
        public List<Article> Articles { get; set; }
        #endregion
    }
}
