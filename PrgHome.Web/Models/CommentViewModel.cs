using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PrgHome.DataLayer.Models;
using PrgHome.Web.Classes;

namespace PrgHome.Web.Models
{
    public class CommentViewModel
    {
        public CommentViewModel()
        {

        }
        public CommentViewModel(Comment comment,in ConvertDate convert)
        {
            Id = comment.Id;
            ParentId = comment.ParentId;
            Name = comment.Name;
            Text = comment.Text;
            Date = convert.ConvertMiladiToShamsi(comment.Date,"yyyy/MM/dd");
        }
        public int Id { get; set; }
        public int? ParentId { get; set; }
        public string Name { get; set; }
        public string Date { get; set; }
        public string Text { get; set; }
    }
}
