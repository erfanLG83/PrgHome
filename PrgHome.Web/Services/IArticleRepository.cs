using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PrgHome.Web.Models;

namespace PrgHome.Web.Services
{
    public interface IArticleRepository
    {
        public Task<IEnumerable<TopArticleViewModel>> GetTopArticlesAsync();
        public Task<IEnumerable<TopArticleViewModel>> GetLastArticlesAsync();
        public Task<IEnumerable<CategoryWithCountViewModel>> GetCategoriesAsync();
    }
}
