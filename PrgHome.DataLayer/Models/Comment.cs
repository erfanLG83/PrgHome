using System;
using System.Collections.Generic;
using System.Text;

namespace PrgHome.DataLayer.Models
{
    public class Comment
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public string Text { get; set; }
        public DateTime Date { get; set; }
        //Navigation Properties
        public int? ParentId { get; set; }
        public int ArticleId { get; set; }
        public Article Article { get; set; }
        public Comment ParentComment { get; set; }
        public List<Comment> ChildrenComments { get; set; }
    }
}
