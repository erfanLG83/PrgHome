using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PrgHome.DataLayer.Models;
using PrgHome.Web.Classes;

namespace PrgHome.Web.Models
{
    public class ShowArticleViewModel
    {
        public ShowArticleViewModel(Article article)
        {
            ConvertDate convert = new ConvertDate();
            Id = article.Id;
            Title = article.Title;
            Date = convert.ConvertMiladiToShamsi(article.PublishDate.Value,"yyyy/MM/dd");
            Content = article.Content;
            CategoryTitle = article.Category.Title;
            View = article.View;
            TimeToRead = article.TimeToRead.Value;
            Image = "/Files/" + article.Image;
            if (string.IsNullOrEmpty(article.Tags))
            {
                Tags = new List<string>();
            }
            else
            {
                Tags = article.Tags.Split(',').ToList();
                for (int i = 0; i < Tags.Count; i++)
                {
                    Tags[i] = Tags[i].Replace(' ', '_');
                }
            }
        }
        public int Id { get; set; }
        public string Title { get; set; }
        public string Image { get; set; }
        public string Date { get; set; }
        public string Content { get; set; }
        public string CategoryTitle { get; set; }
        public int View { get; set; }
        public int TimeToRead { get; set; }
        public List<CommentViewModel> Comments { get; set; }
        public List<string> Tags { get; set; }
    }
}
