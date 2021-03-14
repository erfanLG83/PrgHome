using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using PrgHome.DataLayer.Models;
using PrgHome.DataLayer.Repository;
using PrgHome.DataLayer.UnitOfWork;
using PrgHome.Web.Areas.Admin.Models;
using PrgHome.Web.Classes;
using PrgHome.Web.Models;
using PrgHome.Web.Services;

namespace PrgHome.Web.Areas.Admin.Controllers
{
    public class ArticlesController : Controller
    {
        IUnitOfWork _unitOfWork;
        IRepositoryBase<Article> _articleRep;
        IFileWorker _fileWorker;
        public ArticlesController(IUnitOfWork unitOfWork, IFileWorker fileWorker)
        {
            _fileWorker = fileWorker;
            _unitOfWork = unitOfWork;
            _articleRep = _unitOfWork.GetRepository<Article>();
        }
        public async Task<IActionResult> Index(int index=1,int row=5)
        {
            int count = 0;
            var articles = Pagination.GetData<Article>(await _articleRep.FindAllAsync(false), ref count, row, index);
            int pageCount = count % row == 0 ? count / row : (count / row) + 1;
            ViewBag.PaginationModel = new Pagination(Url, pageCount, index, row, "Index", "Articles", "");
            int rowCounter = row * (index - 1);
            articles = await _articleRep.GetAllReferencePropertyAsync(articles, m => m.Category);
            var model = articles.Select(n => new ArticleViewModel
            {
                Id = n.Id,
                Image = n.Image,
                CategoryTitle = n.Category==null?null:n.Category.Title,
                IsPublish = n.IsPublish,
                PublishDate = n.PublishDate,
                TimeToRead = n.TimeToRead,
                Title = n.Title,
                View = n.View,
                Row = ++rowCounter
            }).ToList();
            return View(model);
        }
        public IActionResult Create()
        {
            ViewBag.Categories = new SelectList(_unitOfWork.GetRepository<Category>().FindAll(), "Id", "Title");
            return View();
        }
        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<IActionResult> Create(ArticleDto article)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Categories = new SelectList(_unitOfWork.GetRepository<Category>().FindAll(), "Id", "Title");
                //article.FormFile
                return View(article);
            }
            ModelValid model = new ModelValid();
            if (article.IsPublish)
            {
                model = article.IsValid(article);
            }
            if (!model.Valid)
            {
                foreach (var item in model.Errors)
                {
                    ModelState.AddModelError("", item);
                }
                ViewBag.Categories = new SelectList(_unitOfWork.GetRepository<Category>().FindAll(), "Id", "Title");
                return View(article);
            }
            Article createArticle = new Article
            {
                CategoryId = article.CategoryId,
                Content = article.Content,
                Tags = article.Tags,
                Description = article.Description,
                TimeToRead = article.TimeToRead,
                IsPublish = article.IsPublish,
                Title = article.Title,
                View=0,
            };
            if (article.FormFile != null)
            {
                createArticle.Image = await _fileWorker.SaveFileAsync(article.FormFile, "Files");
            }
            if (article.IsPublish)
            {
                createArticle.PublishDate = DateTime.Now;
            }
            await _articleRep.CreateAsync(createArticle);
            await _unitOfWork.Commit();
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Edit(int id)
        {
            Article article = await _articleRep.FindByIDAsync(id);
            if (article == null)
            {
                return NotFound();
            }
            ViewBag.Categories = new SelectList(_unitOfWork.GetRepository<Category>().FindAll(), "Id", "Title");
            return View(new ArticleDto(article));
        }
        [HttpPost]
        public async Task<IActionResult> Edit(ArticleDto article)
        {
            Article editArticle = await _articleRep.FindByIDAsync(article.Id);
            if (editArticle==null)
            {
                return View("Error");
            }
            if (!ModelState.IsValid)
            {
                ViewBag.Categories = new SelectList(_unitOfWork.GetRepository<Category>().FindAll(), "Id", "Title");
                return View(article);
            }
            ModelValid model = new ModelValid();
            if (article.IsPublish)
            {
                model = article.IsValid(article);
            }
            if (!model.Valid)
            {
                foreach (var item in model.Errors)
                {
                    ModelState.AddModelError("", item);
                }
                ViewBag.Categories = new SelectList(_unitOfWork.GetRepository<Category>().FindAll(), "Id", "Title");
                return View(article);
            }
            if (article.FormFile != null)
            {
                if (editArticle.Image!=null)
                {
                    await _fileWorker.RemoveFileAsync(editArticle.Image, "Files");
                }
                editArticle.Image = await _fileWorker.SaveFileAsync(article.FormFile, "Files");
            }
            if (article.IsPublish)
            {
                editArticle.PublishDate = DateTime.Now;
            }
            editArticle.IsPublish = article.IsPublish;
            editArticle.Tags = article.Tags;
            editArticle.TimeToRead = article.TimeToRead;
            editArticle.Title = article.Title;
            editArticle.CategoryId = article.CategoryId;
            editArticle.Content = article.Content;
            editArticle.Description = article.Description;
            _articleRep.Update(editArticle);
            await _unitOfWork.Commit();
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> Details(int id)
        {
            Article article = await _articleRep.FindByIDAsync(id);
            if (article==null)
            {
                return NotFound();
            }
            ArticleDetailsViewModel model = new ArticleDetailsViewModel(article);
            if (!article.CategoryId.HasValue)
            {
                var category = await _unitOfWork._context.Categories.FindAsync(id);
                model.CategoryTitle = category.Title;
            }
            model.CommentCount = _unitOfWork._context.Comments.Count(n => n.ArticleId == id);
            return View(model);
        }
        public async Task<IActionResult> Delete(int id)
        {
            Article article = await _articleRep.FindByIDAsync(id);
            if (article==null)
            {
                return NotFound();
            }
            ArticleDetailsViewModel model = new ArticleDetailsViewModel(article);
            return View(model);
        }
        public async Task<IActionResult> DeleteConfirm(int id)
        {
            Article article = await _articleRep.FindByIDAsync(id);
            if (article == null)
            {
                return NotFound();
            }
            if (article.Image!=null)
            {
                await _fileWorker.RemoveFileAsync(article.Image, "Files");
            }
            _articleRep.Delete(article);
            await _unitOfWork.Commit();
            return RedirectToAction(nameof(Index));
        }
    }
}