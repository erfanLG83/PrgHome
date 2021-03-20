using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PrgHome.DataLayer.IdentityClasses;
using PrgHome.DataLayer.IdentityServices;
using PrgHome.DataLayer.UnitOfWork;
using PrgHome.Web.Areas.Admin.Models;
using PrgHome.Web.Classes;
using PrgHome.Web.Models;

namespace PrgHome.Web.Areas.Admin.Controllers
{
    public class RolesManagerController : Controller
    {
        private readonly IAppRoleManager _roleManager;
        public RolesManagerController(IAppRoleManager roleManager)
        {
            _roleManager = roleManager;
        }
        public async Task<IActionResult> Index(int index=1,int row =5)
        {
            int count = 0;
            var roles = Pagination.GetData<AppRole>(await _roleManager.GetRoles(), ref count, row, index);
            int rowCounter = (index - 1) * row;
            IEnumerable<RoleViewModel> model = roles.Select(n => new RoleViewModel
            {
                Id = n.Id,
                Row = ++rowCounter,
                Title = n.Name,
                UsersCount = n.UserRoles.Count
            });
            count = count % row == 0 ? count / row : (count / row) + 1;
            ViewBag.PaginationModel = new Pagination(this.Url, count, index, row
                ,action: "Index"
                ,controller: "RolesManager");
            return View(model);
        }
        public async Task<IActionResult> Details(string id)
        {
            var role = await _roleManager.FindByIdWithUserRolesAsync(id);
            if (role==null)
            {
                return NotFound();
            }
            return View(
                new RoleDetailsViewModel(role)
                );
        }
        public IActionResult Create() => View();
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(RoleDto role)
        {
            if (ModelState.IsValid)
            {
                var result = await _roleManager.CreateAsync(new AppRole { Name = role.Name, Description = role.Description });
                if (result.Succeeded)
                {
                    return RedirectToAction("Index");
                }
                foreach (var item in result.Errors)
                {
                    ModelState.AddModelError(item.Code, item.Description);
                }
            }

            return View(role);
        }
        public async Task<IActionResult> Edit(string id)
        {
            var role = await _roleManager.FindByIdAsync(id);
            if (role==null)
            {
                return NotFound();
            }
            return View(new RoleDto(role));
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(RoleDto role)
        {
            AppRole editRole = await _roleManager.FindByIdAsync(role.Id);
            if (editRole==null)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                editRole.Name = role.Name;
                editRole.Description = role.Description;

                var result = await _roleManager.UpdateAsync(editRole);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index");
                }
                foreach (var item in result.Errors)
                {
                    ModelState.AddModelError(item.Code, item.Description);
                }
            }
            return View(role);
        }
        public async Task<IActionResult> Delete(string id)
        {
            var role = await _roleManager.FindByIdAsync(id);
            if (role == null)
            {
                return Json(new JsonResponse(false, "در حذف نقش مشکلی  پیش اومد!"));
            }
            var result = await _roleManager.DeleteAsync(role);
            if (result.Succeeded)
            {
                return Json(new JsonResponse(true));
            }
            foreach (var item in result.Errors)
            {
                ModelState.AddModelError(item.Code, item.Description);
            }
            var response = new JsonResponse(false);
            if (result.Errors.Any())
            {
                response.ErrorMessage = result.Errors.First().Description;
            }
            return Json(response);
        }
    }
}