using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PrgHome.DataLayer.UnitOfWork;
using PrgHome.DataLayer.Repository;
using PrgHome.DataLayer.Models;
using PrgHome.Web.Classes;
using PrgHome.Web.Areas.Admin.Models;

namespace PrgHome.Web.Areas.Admin.Controllers
{
    public class CategoriesController : Controller
    {
        IUnitOfWork _unitOfWork;
        IRepositoryBase<Category> _categoryRep;   
        public CategoriesController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _categoryRep = _unitOfWork.GetRepository<Category>();
        }
        public async Task<IActionResult> Index(int index = 1, int row = 5)
        {
            int count = 0;
            var categories = Pagination.GetData<Category>(await _categoryRep.FindAllAsync(),ref count,row,index);
            int pageCount = count % row == 0 ? count / row : (count / row) + 1;
            ViewBag.PaginationModel = new Pagination(Url,pageCount,index,row,"Index","Categories","");
            int rowCounter = row * (index-1);
            return View(
                categories.Select(n => new CategoryViewModel
                {
                    Row=++rowCounter,
                    Id = n.Id,
                    Title = n.Title,
                    ArticleCount = _unitOfWork.GetRepository<Article>().GetCount(m=>m.CategoryId==n.Id)
                })) ;
        }
        public IActionResult Create() => View();
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CategoryDto category)
        {
            await _categoryRep.CreateAsync(new Category { 
                Title=category.Title
            });
            await _unitOfWork.Commit();
            return RedirectToAction("index");
        }
        public async Task<IActionResult> Details(int id)
        {
            var category = await _categoryRep.FindByIDAsync(id);
            if (category == null)
                return NotFound();
            CategoryViewModel model = new CategoryViewModel
            {
                Id = category.Id,
                Title = category.Title,
                ArticleCount = _unitOfWork.GetRepository<Article>().GetCount(n => n.CategoryId == category.Id)
            };
            return View(model);
        }
        public async Task<IActionResult> Edit(int id)
        {
            var category = await _categoryRep.FindByIDAsync(id);
            if (category==null)
            {
                return null;
            }
            CategoryDto model = new CategoryDto
            {
                Id=category.Id,
                Title=category.Title
            };
            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(CategoryDto dto)
        {
            if (!ModelState.IsValid)
            {
                return View(dto);
            }
            var category = await _categoryRep.FindByIDAsync(dto.Id);
            if (category==null)
            {
                return NotFound();
            }
            category.Title = dto.Title;
            _categoryRep.Update(category);
            await _unitOfWork.Commit();
            return RedirectToAction("index");

        }
        public async Task<IActionResult> Delete(int id)
        {
            var category = await _categoryRep.FindByIDAsync(id);
            if (category == null)
            {
                return NotFound();
            }
            return View(new CategoryViewModel
            {
                Id = category.Id,
                ArticleCount = _unitOfWork.GetRepository<Article>().GetCount(n=>n.CategoryId==category.Id),
                Title=category.Title
            });
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(CategoryViewModel model)
        {
            var category = await _categoryRep.FindByIDAsync(model.Id);
            if (category==null)
            {
                return NotFound();
            }
            _categoryRep.Delete(category);
            await _unitOfWork.Commit();
            return RedirectToAction("Index");
        }
    }
}