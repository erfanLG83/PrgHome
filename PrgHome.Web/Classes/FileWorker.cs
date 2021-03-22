using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using PrgHome.DataLayer.Repository;
using PrgHome.DataLayer.UnitOfWork;
using PrgHome.Web.Services;

namespace PrgHome.Web.Classes
{
    public class FileWorker:IFileWorker
    {
        private readonly IWebHostEnvironment _env;
        private readonly IRepositoryBase<PrgHome.DataLayer.Models.File> _repository;
        private readonly IUnitOfWork _unitOfWork;
        public FileWorker(IWebHostEnvironment env ,IUnitOfWork unitOfWork)
        {
            _env = env;
            _unitOfWork = unitOfWork;
            _repository = unitOfWork.GetRepository<DataLayer.Models.File>();
        }
        public async Task<string> SaveFileAsync(IFormFile file,string path)
        {
            path = Path.Combine(_env.WebRootPath, path);
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            string type = Path.GetExtension(file.FileName).ToLower();
            string fileName = Guid.NewGuid() + type;
            path = Path.Combine(path, fileName);
            using (var stream = new FileStream(path, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }
            await _repository.CreateAsync(new DataLayer.Models.File
            {
                Name = fileName,
                Size=file.Length/1000 + " kb",
                Type=type
            }) ;

            await _unitOfWork.Commit();
            return fileName;
        }
        public async Task RemoveFileAsync(string fileName,string path)
        {
            path = Path.Combine(_env.WebRootPath, path, fileName);
            var file = await _repository.FindByIDAsync(fileName);
            if (file==null)
            {
                throw new Exception("File Not Find In Database");
            }
            File.Delete(path);
            _repository.Delete(
                file
                );
            await _unitOfWork.Commit();
        }
        public async Task<string> EncodeFormFile(IFormFile file)
        {
            byte[] bytes;
            using (Stream stream = file.OpenReadStream())
            {
                using MemoryStream ms = new MemoryStream();
                await stream.CopyToAsync(ms);
                bytes = ms.ToArray();
            }
            return $"data:{file.ContentType};base64,{Convert.ToBase64String(bytes)}";
        }
    }
}
