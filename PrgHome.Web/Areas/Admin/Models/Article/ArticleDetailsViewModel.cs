using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PrgHome.DataLayer.Models;

namespace PrgHome.Web.Areas.Admin.Models
{
    public class ArticleDetailsViewModel
    {
        public ArticleDetailsViewModel()
        {

        }
        public ArticleDetailsViewModel(Article article)
        {
            Id = article.Id;
            Title = article.Title;
            Description = article.Description;
            Content = article.Content;
            Image = article.Image;
            View = article.View;
            TimeToRead = article.TimeToRead;
            PublishDate = article.PublishDate;
            IsPublish = article.IsPublish;
            if (!String.IsNullOrEmpty(article.Tags))
            {
                Tags = article.Tags.Split(',').ToList();
            }
        }
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Content { get; set; }
        public string Image { get; set; }
        public int View { get; set; }
        public int? TimeToRead { get; set; }
        public DateTime? PublishDate { get; set; }
        public bool IsPublish { get; set; }
        public List<string> Tags { get; set; }
        public string CategoryTitle { get; set; }
        public int CommentCount { get; set; }
    }
}
