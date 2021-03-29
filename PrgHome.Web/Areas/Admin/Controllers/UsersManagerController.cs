using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PrgHome.DataLayer.IdentityClasses;
using PrgHome.DataLayer.IdentityServices;
using PrgHome.Web.Areas.Admin.Models.User;
using PrgHome.Web.Classes;
using PrgHome.Web.Models;
using PrgHome.Web.Services;

namespace PrgHome.Web.Areas.Admin.Controllers
{

    [Authorize(Roles = "مدیر")]
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
        private async Task SetRoles()
        {
            IEnumerable<AppRole> roles = await _roleManager.GetRoles();
            ViewBag.Roles = roles.Select(n => n.Name);
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
            await SetRoles();
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
                    Popup.PopupModel = new Popup("افزودن کاربر", $"کاربر {user.Username} با موفقیت اضافه شد.", IconType.Success);
                    if (!string.IsNullOrEmpty(user.UserRoles))
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
            await SetRoles();
            return View(user);
        }
        public async Task<IActionResult> Edit(string id)
        {
            AppUser user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                Popup.PopupModel = new Popup("خطا", $"کاربری با نشانی '{id}' پیدا نشده!", IconType.Error);
                return RedirectToAction("Index");
            }
            await SetRoles();

            return View(new UserEditDto(user, await _userManager.GetRolesAsync(user)));

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(UserEditDto user)
        {
            AppUser editUser = await _userManager.FindByIdAsync(user.Id);
            if (user.ImageFile != null)
                editUser.Image = await _fileWorker.EncodeFormFile(user.ImageFile);
            else if (user.LastImageDeleted)
                editUser.Image = null;
            bool updated = false;
            if (ModelState.IsValid)
            {
                editUser.Email = user.Email;
                editUser.EmailConfirmed = user.EmailConfirmed;
                editUser.LockoutEnabled = user.IsLockout;
                editUser.UserName = user.Username;
                editUser.IsActive = user.IsActive;
                editUser.PhoneNumber = user.PhoneNumber;
                var result = await _userManager.UpdateAsync(editUser);
                updated = true;
                if (result.Succeeded)
                {
                    Popup.PopupModel = new Popup("ویرایش کاربر", $"ویرایش کاربر {editUser.UserName} با موفقیت صورت گرفت", IconType.Success);
                    #region update user roles
                    var roles = user.UserRoles.Split(',');
                    var recentRoles = await _userManager.GetRolesAsync(editUser);
                    var deletedRoles = recentRoles.Except(roles).ToArray();
                    var addedRoles = roles.Except(recentRoles).ToArray();
                    if (deletedRoles.Length != 0)
                    {
                        result = await _userManager.RemoveFromRolesAsync(editUser, deletedRoles);
                    }
                    if (addedRoles.Length != 0)
                    {
                        result = await _userManager.AddToRolesAsync(editUser, addedRoles);
                    }
                    #endregion
                    if (!result.Succeeded)
                    {
                        Popup.PopupModel = new Popup("ویرایش کاربر", $"در هنگام افزودن نقش به کاربر {editUser.UserName} خطایی صورت گرفت.", IconType.Error);
                        if (result.Errors.Any())
                        {
                            Popup.PopupModel.Message += "\n\n" + result.Errors.First().Description;
                        }
                    }
                    return RedirectToAction("Index");
                }
                foreach (var item in result.Errors)
                {
                    ModelState.AddModelError(item.Code, item.Description);
                }
            }
            if (updated && user.LastImageDeleted)
            {
                var result = await _userManager.UpdateAsync(editUser);
                foreach (var item in result.Errors)
                {
                    ModelState.AddModelError(item.Code, item.Description);
                }
            }
            user.LastImageDeleted = false;
            user.LastImage = editUser.Image;
            await SetRoles();
            return View(user);
        }
        public async Task<IActionResult> Delete(string id)
        {
            AppUser user = await _userManager.FindByIdAsync(id);
            if (user==null)
            {
                return Json(new JsonResponse(false,"کاربر پیدا نشد!"));
            }
            var result = await _userManager.DeleteAsync(user);
            if (result.Succeeded)
            {
                return Json(new JsonResponse(true));
            }
            JsonResponse response = new JsonResponse(false);
            if (result.Errors.Any())
            {
                response.ErrorMessage = result.Errors.First().Description;
            }
            return Json(response);
        }
        public async Task<IActionResult> Details(string id)
        {
            AppUser user = await _userManager.FindByIdAsync(id);
            if (user==null)
            {
                Popup.PopupModel = new Popup("خطا", $"کاربری با نشانی '{id}' پیدا نشده!", IconType.Error);
                return RedirectToAction("Index");
            }
            UserDetailsViewModel model = new UserDetailsViewModel(user, await _userManager.GetRolesAsync(user));
            return View(model);
        }
    }
}