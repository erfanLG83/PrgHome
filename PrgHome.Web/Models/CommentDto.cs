using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PrgHome.Web.Models
{
    public class CommentDto
    {
        public string UserName { get; set; }
        public string Email { get; set; }
        public string UserId { get; set; }
        public string Text { get; set; }
        public int? ParentId { get; set; }
        public int ArticleId { get; set; }
    }
}
