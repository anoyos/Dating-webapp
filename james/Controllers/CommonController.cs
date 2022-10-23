using james.Helpers.General;
using james.Models.DB;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace james.Controllers
{
    public class CommonController : Controller
    {
        private readonly IHostingEnvironment _hostingEnvironment;
        private DbContextOptions<DBContext> _dbOptions;
        public CommonController(IHostingEnvironment hostingEnvironment, DbContextOptions<DBContext> dbOptions)
        {
            _hostingEnvironment = hostingEnvironment;
            _dbOptions = dbOptions;
        }

        public IActionResult Index()
        {
            return View();
        }
        public async Task<string> UploadFile(IFormFile file)
        {
            string filename = null;
            try
            {
                if (true)
                {
                    string s = file.FileName;
                    string extension = Path.GetExtension(file.FileName);
                    filename = DateTime.Now.ToString("yyyyMMddhhmmmss") + extension;
                    string webRootPath = _hostingEnvironment.WebRootPath;
                    string contentRootPath = _hostingEnvironment.ContentRootPath;

                    var folder = Path.Combine(webRootPath, "img", "upload");
                    string filePath = Path.Combine(folder, filename);
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await file.CopyToAsync(fileStream);
                    }
                    return Common.Serialize(filename);                
                }
            }
            catch (Exception ex)
            {
                return Common.Serialize(null);
            }
        }
        public async Task<string> UploadMultiFile(List<IFormFile> files)
        {

            List<string> filesName = new List<string> { };
            try
            {
               
                if (true)
                {
                    foreach (var file in files)
                    {
                        string filename = null;
                        string s = file.FileName;
                        string extension = Path.GetExtension(file.FileName);
                        filename = Guid.NewGuid().ToString() + extension;
                        string webRootPath = _hostingEnvironment.WebRootPath;
                        string contentRootPath = _hostingEnvironment.ContentRootPath;

                        var folder = Path.Combine(webRootPath, "img", "upload");
                        string filePath = Path.Combine(folder, filename);
                        using (var fileStream = new FileStream(filePath, FileMode.Create))
                        {
                            await file.CopyToAsync(fileStream);
                        }
                        filesName.Add(filename);
                    }
                    return Common.Serialize(filesName);
                }
            }
            catch (Exception ex)
            {
                return Common.Serialize(null);
            }
        }
        
    }
}
