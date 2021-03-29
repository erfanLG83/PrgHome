using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PrgHome.DataLayer.Models;
using PrgHome.DataLayer.Repository;
using PrgHome.DataLayer.UnitOfWork;
using PrgHome.Web.Classes;
using PrgHome.Web.Models;
using PrgHome.Web.Services;

namespace PrgHome.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly IArticleRepository _articleRepository;
        public HomeController(IArticleRepository articleRepository)
        {
            _articleRepository = articleRepository;
        }
        public async Task<IActionResult> Index()
        {
            var model = await _articleRepository.GetLastArticlesAsync();
            return View(model);
        }
        [Route("ContactUs")]
        public IActionResult ContactUs() => View();

    }
}
