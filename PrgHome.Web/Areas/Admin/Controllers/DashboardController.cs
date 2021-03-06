using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PrgHome.DataLayer.Models;
using PrgHome.DataLayer.UnitOfWork;
using PrgHome.Web.Areas.Admin.Models;

namespace PrgHome.Web.Areas.Admin.Controllers
{
    public class DashboardController : Controller
    {
        IUnitOfWork _unitOfWork;
        public DashboardController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<IActionResult> Index()
        {
            DashboardViewModel model = new DashboardViewModel();
            var categoryRep = _unitOfWork
                .GetRepository<Category>();
            var articleRep = _unitOfWork
                .GetRepository<Article>();
            #region Get Categories
            var categories = await categoryRep
                .FindAllAsync();
            model.Categories = categories.Take(5).Select(n => new CategoryViewModel
            {
                Title = n.Title,
                ArticleCount = articleRep.GetCount(n => n.CategoryId == n.Id)
            });
            #endregion
            #region Get Articles
            var articles = await articleRep.FindAllAsync(false);
            articles = await articleRep.GetAllReferencePropertyAsync(articles.Take(4), n => n.Category);
            model.Articles = articles.Select(n => new ArticleViewModel
            {
                Title = n.Title,
                CategoryTitle = n.Category.Title,
                Image = n.Image,
                IsPublish =n.IsPublish,
                PublishDate = n.PublishDate,
                TimeToRead=n.TimeToRead,
                View=n.View
            });
            #endregion
            return View(model);
        }
    }
}