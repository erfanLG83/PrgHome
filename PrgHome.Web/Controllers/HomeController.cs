using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
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
        private readonly IUnitOfWork _uow;
        private readonly IConfiguration _configuration;
        public HomeController(IArticleRepository articleRepository , IConfiguration configuration , IUnitOfWork uow)
        {
            _uow = uow;
            _configuration = configuration;
            _articleRepository = articleRepository;
        }
        private bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }
        public async Task<IActionResult> Index()
        {
            var model = await _articleRepository.GetLastArticlesAsync();
            return View(model);
        }
        [Route("JoinNewsLetters")]
        public async Task<IActionResult> JoinNewsLetters(string email)
        {
            if (IsValidEmail(email))
            {
                try
                {
                    if (!_uow._context.CommonNewsLetters.Any(n=>n.Email == email))
                    {
                        await _uow._context.CommonNewsLetters.AddAsync(new CommonNewsLetters { Email = email });
                        await _uow.Commit();
                    }
                    HttpContext.Response.Cookies.Append("NewsLetterEmail", email);
                    return Json(new JsonResponse(true));

                }
                catch (Exception)
                {
                    return Json(new JsonResponse(false));
                }
            }
            return Json(new JsonResponse(false));
        }
        [Route("ContactUs")]
        public IActionResult ContactUs() => View();


    }
}
