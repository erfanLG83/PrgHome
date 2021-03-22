using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace PrgHome.Web.Services
{
    public interface IFileWorker
    {
        public Task<string> SaveFileAsync(IFormFile file, string path);
        public Task RemoveFileAsync(string fileName, string path);
        public Task<string> EncodeFormFile(IFormFile file);
    }
}
