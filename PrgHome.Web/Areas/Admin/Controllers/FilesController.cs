using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PrgHome.DataLayer.Models;
using PrgHome.DataLayer.Repository;
using System.Drawing;
using PrgHome.DataLayer.UnitOfWork;
using Microsoft.AspNetCore.Authorization;

namespace PrgHome.Web.Areas.Admin.Controllers
{

    [Authorize(Roles = "مدیر")]
    public class FilesController : Controller
    {
        IUnitOfWork _unitOfWork;
        IRepositoryBase<DataLayer.Models.File> _fileRep;
        IWebHostEnvironment _env;
        public FilesController(IWebHostEnvironment env , IUnitOfWork unit)
        {
            _unitOfWork = unit;
            _fileRep = unit.GetRepository<DataLayer.Models.File>();
            _env = env;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [Route("file-upload")]
        public async Task<IActionResult> UploadImage(IFormFile upload, string CKEditorFuncNum, string CKEditor, string langCode)
        {
            if (upload.Length <= 0) return null;
            string type = Path.GetExtension(upload.FileName).ToLower();
            var fileName = Guid.NewGuid() + type ;

            string baseDir = Path.Combine(
                _env.WebRootPath, "Files");

            if (!Directory.Exists(baseDir))
            {
                var info = Directory.CreateDirectory(baseDir);
            }


            var path = Path.Combine(baseDir,
                fileName);

            using (var stream = new FileStream(path, FileMode.Create))
            {
                await upload.CopyToAsync(stream);

            }



            var url = $"/Files/{fileName}";
            await _fileRep.CreateAsync(new DataLayer.Models.File
            {
                Name = fileName,
                Size = (upload.Length / 1000).ToString() + " KB",
                Type = type
            });
            await _unitOfWork.Commit();
            return Json(new { uploaded = true, url });
        }

    }
}