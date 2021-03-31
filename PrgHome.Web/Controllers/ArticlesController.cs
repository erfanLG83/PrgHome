using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
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
        private readonly IRepositoryBase<Comment> _commentRep;
        private readonly ConvertDate _convert;
        public ArticlesController(IUnitOfWork uow, ConvertDate convert)
        {
            _convert = convert;
            _uow = uow;
            _articleRep = _uow.GetRepository<Article>();
            _categoryRep = _uow.GetRepository<Category>();
            _commentRep = _uow.GetRepository<Comment>();
        }
        [Route("articles")]
        public async Task<IActionResult> Index(int row = 5, int index = 1)
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
            if (article == null || !article.IsPublish)
            {
                return NotFound();
            }
            article = await _articleRep.GetReferencePropertyAsync(article, n => n.Category);
            ShowArticleViewModel model = new ShowArticleViewModel(article);
            var comments = await _commentRep.FindByConditionAsync(n => n.ArticleId == article.Id, n => n.OrderByDescending(x => x.Date));
            model.Comments = comments.Select(n => new CommentViewModel(n, in _convert)).ToList();
            #region Update View
            article.View++;
            _articleRep.Update(article);
            await _uow.Commit();
            #endregion
            return View(model);
        }
        [Route("articles/category/{title}")]
        public async Task<IActionResult> ShowArticlesByCategory(string title, int row = 5, int index = 1)
        {
            var categories = await _categoryRep.FindAllAsync(false);
            var category = categories.FirstOrDefault(n => n.Title == title);
            if (category == null)
            {
                return NotFound();
            }
            category.Articles = (List<Article>)await _articleRep.FindByConditionAsync(n => n.IsPublish && n.CategoryId == category.Id);
            var model = category.Articles.Select(n => new TopArticleViewModel
            {
                Description = n.Description,
                Image = n.Image,
                PublishDate = _convert.ConvertMiladiToShamsi(n.PublishDate.Value, "yyyy/MM/dd"),
                Title = n.Title,
                View = n.View,
                CategoryTitle = title,
                TimeToRead = n.TimeToRead.Value
            });
            int count = 0;
            model = Pagination.GetData<TopArticleViewModel>(model, ref count, row, index);
            count = count % row == 0 ? count / row : (count / row) + 1;
            ViewBag.Pagination = new Pagination(count, index, row, "/articles/category/" + title, "&raquo;", "&laquo");
            ViewBag.CategoryTitle = category.Title;
            return View(model);
        }
        [Route("search")]
        public async Task<IActionResult> Search(string q, int row = 5, int index = 1)
        {
            if (string.IsNullOrEmpty(q))
            {
                return RedirectToAction("Index", "Home");
            }
            var list = await _articleRep.FindByConditionAsync(n => n.IsPublish);
            list = await _articleRep.GetAllReferencePropertyAsync(list, n => n.Category);
            string qUppred = q.ToUpper();
            var model = list.Where(n =>
                n.Tags.ToUpper().Contains(qUppred)
                || n.Title.ToUpper().Contains(qUppred)
                || n.Description.ToUpper().Contains(qUppred)
                || n.Category.Title.ToUpper().Contains(qUppred)
                ).Select(n => new TopArticleViewModel
                {
                    CategoryTitle = n.Category.Title,
                    Description = n.Description,
                    Image = n.Image,
                    PublishDate = _convert.ConvertMiladiToShamsi(n.PublishDate.Value, "yyyy/MM/dd"),
                    TimeToRead = n.TimeToRead.Value,
                    Title = n.Title,
                    View = n.View
                });
            int count = 0;
            model = Pagination.GetData<TopArticleViewModel>(model, ref count, row, index);
            count = count % row == 0 ? count / row : (count / row) + 1;
            ViewBag.Query = q;
            ViewBag.Pagination = new Pagination(count, index, row, "/search?q=" + q, "&raquo;", "&laquo");
            return View(model);

        }
        public async Task<IActionResult> AddComment(CommentDto comment)
        {
            Comment createComment = new Comment
            {
                Text = comment.Text,
                ArticleId = comment.ArticleId,
                ParentId = comment.ParentId,
                Date = DateTime.Now,
                Email = comment.Email,
                Name = comment.UserName
            };
            try
            {
                await _commentRep.CreateAsync(createComment);
                await _uow.Commit();
                return Json(new JsonResponse
                {
                    Success = true,
                    Data = new
                    {
                        date = _convert.ConvertMiladiToShamsi(createComment.Date, "yyyy/MM/dd"),
                        id = createComment.Id
                    }
                });
            }
            catch (Exception err)
            {
                return Json(new JsonResponse { Success = false, ErrorMessage = err.Message });
            }
        }
        [Authorize(Roles = "مدیر")]
        public async Task<IActionResult> RemoveComment(int id)
        {
            Comment comment = await _commentRep.FindByIDAsync(id);
            if (comment == null)
            {
                return Json(new JsonResponse(false));
            }
            var comments = await _commentRep.FindByConditionAsync(n => n.ParentId.HasValue && n.ParentId.Value == id);
            _commentRep.DeleteRange(comments);
            await _uow.Commit();
            _commentRep.Delete(comment);
            await _uow.Commit();
            return Json(new JsonResponse(true));
        }
    }
}