﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PrgHome.DataLayer.Models;
using PrgHome.DataLayer.Repository;
using PrgHome.DataLayer.UnitOfWork;
using PrgHome.Web.Classes;
using PrgHome.Web.Models;

namespace PrgHome.Web.Controllers
{
    public class ArticlesController : Controller
    {
        private readonly IUnitOfWork _uow;
        private readonly IRepositoryBase<Article> _articleRep;
        private readonly IRepositoryBase<Category> _categoryRep;
        private readonly ConvertDate _convert;
        public ArticlesController(IUnitOfWork uow , ConvertDate convert)
        {
            _convert = convert;
            _uow = uow;
            _articleRep = _uow.GetRepository<Article>();
            _categoryRep = _uow.GetRepository<Category>();
        }
        [Route("{Contoller}")]
        public async Task<IActionResult> Index(int row =5,int index =1)
        {

            int count = 0;
            var articles = await _articleRep.FindByConditionAsync(n => n.IsPublish, n => n.OrderByDescending(x => x.PublishDate));
            articles = Pagination.GetData<Article>(articles, ref count, row, index);
            articles = await _articleRep.GetAllReferencePropertyAsync(articles, n => n.Category);
            var model = articles.Select(n => new TopArticleViewModel
            {
                CategoryTitle = n.Category.Title,
                Description = n.Description,
                Image = n.Image,
                PublishDate = _convert.ConvertMiladiToShamsi(n.PublishDate.Value, "yyyy/MM/dd"),
                TimeToRead = n.TimeToRead.Value,
                Title = n.Title,
                View = n.View
            }); 
            count = count % row == 0 ? count / row : (count / row) + 1;
            ViewBag.Pagination = new Pagination(count, index, row, "/articles", "&raquo;", "&laquo");
            return View(model);
        }
        [Route("articles/show/{title}")]
        public async Task<IActionResult> ShowArticle(string title)
        {
            var articles = await _articleRep.FindAllAsync(false);
            Article article = articles.FirstOrDefault(n => n.Title == title);
            if (article==null || !article.IsPublish)
            {
                return NotFound();
            }
            article = await _articleRep.GetReferencePropertyAsync(article, n => n.Category);
            ShowArticleViewModel model = new ShowArticleViewModel(article);
            return View(model);
        }
        [Route("articles/category/{title}")]
        public async Task<IActionResult> ShowArticlesByCategory(string title , int row =5,int index=1)
        {
            var categories = await _categoryRep.FindAllAsync(false);
            var category = categories.FirstOrDefault(n => n.Title == title);
            if (category==null)
            {
                return NotFound();
            }
            category.Articles = (List<Article>) await _articleRep.FindByConditionAsync(n => n.IsPublish && n.CategoryId == category.Id);
            var model = category.Articles.Select(n => new TopArticleViewModel
            {
                Description = n.Description,
                Image = n.Image,
                PublishDate = _convert.ConvertMiladiToShamsi(n.PublishDate.Value, "yyyy/MM/dd"),
                Title = n.Title,
                View = n.View
            });
            int count = 0;
            model = Pagination.GetData<TopArticleViewModel>(model,ref count, row, index);
            count = count % row == 0 ? count / row : (count / row) + 1;
            ViewBag.Pagination = new Pagination(count, index, row ,"/articles/category/"+title, "&raquo;", "&laquo");
            ViewBag.CategoryTitle = category.Title;
            return View(model);
        }
    }
}