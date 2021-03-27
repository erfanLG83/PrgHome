using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PrgHome.DataLayer.Models;
using PrgHome.DataLayer.Repository;
using PrgHome.DataLayer.UnitOfWork;
using PrgHome.Web.Models;

namespace PrgHome.Web.Controllers
{
    public class ArticlesController : Controller
    {
        private readonly IUnitOfWork _uow;
        private readonly IRepositoryBase<Article> _articleRep;
        public ArticlesController(IUnitOfWork uow)
        {
            _uow = uow;
            _articleRep = _uow.GetRepository<Article>();
        }
        [Route("{Contoller}")]
        public IActionResult Index()
        {
            return View();
        }
        [Route("articles/show/{id:int}")]
        public async Task<IActionResult> ShowArticle(int id)
        {
            Article article = await _articleRep.FindByIDAsync(id);
            if (article==null || !article.IsPublish)
            {
                return NotFound();
            }
            article = await _articleRep.GetReferencePropertyAsync(article, n => n.Category);
            ShowArticleViewModel model = new ShowArticleViewModel(article);
            return View(model);
        }
    }
}