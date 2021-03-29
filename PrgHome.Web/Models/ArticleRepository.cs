using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PrgHome.DataLayer.Models;
using PrgHome.DataLayer.Repository;
using PrgHome.DataLayer.UnitOfWork;
using PrgHome.Web.Classes;
using PrgHome.Web.Services;

namespace PrgHome.Web.Models
{
    public class ArticleRepository : IArticleRepository
    {
        private readonly IUnitOfWork _uow;
        private readonly IRepositoryBase<Article> _articleRep;
        private readonly IRepositoryBase<Category> _categoryRep;
        private readonly ConvertDate _convert;

        public ArticleRepository(IUnitOfWork uow, ConvertDate convert)
        {
            _convert = convert;
            _uow = uow;
            _articleRep = _uow.GetRepository<Article>();
            _categoryRep = _uow.GetRepository<Category>();
        }
        public async Task<IEnumerable<CategoryWithCountViewModel>> GetCategoriesAsync()
        {
            var categories = await _categoryRep.FindAllAsync(false);
            List<CategoryWithCountViewModel> categoriesViewModel = new List<CategoryWithCountViewModel>();
            foreach (var item in categories)
            {
                categoriesViewModel.Add(new CategoryWithCountViewModel
                {
                    Id = item.Id,
                    CategoryTitle = item.Title,
                    ArticleCount = _articleRep.GetCount(n => n.CategoryId == item.Id)
                });
            }
            return categoriesViewModel;
        }

        public async Task<IEnumerable<TopArticleViewModel>> GetTopArticlesAsync()
        {
            var articles = await _articleRep.FindByConditionAsync(n => n.IsPublish, n => n.OrderByDescending(n => n.View));
            articles = articles.Take(5);
            List<TopArticleViewModel> articleViewModels = new List<TopArticleViewModel>();
            foreach (var item in articles)
            {
                articleViewModels.Add(new TopArticleViewModel
                {
                    Description = item.Description,
                    Title = item.Title,
                    View = item.View,
                    Image = item.Image,
                    PublishDate = _convert.ConvertMiladiToShamsi(item.PublishDate.Value, "yyyy/MM/dd"),
                });
            }
            return articleViewModels;
        }

        public async Task<IEnumerable<TopArticleViewModel>> GetLastArticlesAsync()
        {
            var articles = await _articleRep.FindByConditionAsync(n => n.IsPublish, n => n.OrderByDescending(n => n.PublishDate));
            articles = articles.Take(4);
            articles = await _articleRep.GetAllReferencePropertyAsync(articles, n => n.Category);
            List<TopArticleViewModel> articleViewModels = new List<TopArticleViewModel>();
            foreach (var item in articles)
            {
                articleViewModels.Add(new TopArticleViewModel
                {
                    Description = item.Description,
                    Title = item.Title,
                    View = item.View,
                    Image = item.Image,
                    PublishDate = _convert.ConvertMiladiToShamsi(item.PublishDate.Value, "yyyy/MM/dd"),
                    TimeToRead = item.TimeToRead.Value,
                    CategoryTitle = item.Category.Title
                });
            }
            return articleViewModels;
        }
    }
}
