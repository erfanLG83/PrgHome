using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PrgHome.DataLayer.IdentityClasses;
using PrgHome.DataLayer.IdentityServices;
using PrgHome.Web.Areas.Admin.Models.User;
using PrgHome.Web.Classes;
using PrgHome.Web.Services;

namespace PrgHome.Web.Areas.Admin.Controllers
{
    public class UsersManagerController : Controller
    {
        private readonly IAppUserManager _userManager;
        private readonly IAppRoleManager _roleManager;
        private readonly IFileWorker _fileWorker;
        public UsersManagerController(IAppUserManager userManager, IAppRoleManager roleManager, IFileWorker fileWorker)
        {
            _fileWorker = fileWorker;
            _userManager = userManager;
            _roleManager = roleManager;
        }
        public async Task<IActionResult> Index(int index = 1, int row = 5)
        {
            int count = 0;
            IEnumerable<AppUser> users = Pagination.GetData<AppUser>(await _userManager.GetUsersAsync(), ref count, row, index);
            int pageCount = count % row == 0 ? count / row : (count / row) + 1;
            ViewBag.PaginationModel = new Pagination(Url, pageCount, index, row, action: "Index", controller: "UsersManager");
            int rowCounter = row * (index - 1);
            List<UserViewModel> model = new List<UserViewModel>();
            foreach (var item in users)
            {
                model.Add(new UserViewModel(item, await _userManager.GetRolesAsync(item), ++rowCounter));
            }
            return View(model);
        }
        public async Task<IActionResult> Create()
        {
            IEnumerable<AppRole> roles = await _roleManager.GetRoles();
            ViewBag.Roles = roles.Select(n => n.Name);
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(UserCreateDto user)
        {
            if (ModelState.IsValid)
            {
                AppUser createUser = user.GetUser();
                if (user.ImageFile != null)
                {
                    createUser.Image = await _fileWorker.EncodeFormFile(user.ImageFile);
                }
                var result = await _userManager.CreateAsync(createUser, user.Password);
                if (!result.Succeeded)
                {
                    foreach (var item in result.Errors)
                    {
                        ModelState.AddModelError(item.Code, item.Description);
                    }
                }
                else
                {
                    Popup.PopupModel = new Popup("افزودن کاربر", $"کاربر {user.Username} با موفقیا اضافه شد.", IconType.Success);
                    if (! string.IsNullOrEmpty(user.UserRoles))
                    {
                        result = await _userManager.AddToRolesAsync(createUser, user.UserRoles.Split(','));
                        if (!result.Succeeded)
                        {
                            Popup.PopupModel = new Popup("خطا در افزودن نقش", $"در هنگام افزودن نقش به کاربر {user.Username} خطایی رخ داد.", IconType.Error);
                            if (result.Errors.Count() > 0)
                            {
                                Popup.PopupModel.Message += result.Errors.First();
                            }
                        }
                    }
                    return RedirectToAction("Index");
                }

            }
            IEnumerable<AppRole> roles = await _roleManager.GetRoles();
            ViewBag.Roles = roles.Select(n => n.Name);
            return View(user);
        }
    }
}