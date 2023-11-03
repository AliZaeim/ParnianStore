using Core.DTOs.Admin;
using Core.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Web.Areas.Manage.Controllers
{
    [Area("Manage")]
    [Authorize]
    [PermissionCheckerByPermissionName("ckuploadcenter")]
    public class CommonsController : Controller
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        public CommonsController(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
        }
        [HttpPost]
        public IActionResult UploadFile(IFormFile upload)
        {
            if (upload.Length <= 0)
                return null;
            
            string fileName = Path.GetFileNameWithoutExtension(upload.FileName);
            fileName += "-" + Core.Prodocers.Generators.GenerateUniqueString(5, 0, 2, 0);
            string Ex = Path.GetExtension(upload.FileName);
            fileName += Ex;
            string filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "UploadCenter/ckeditor4", fileName);
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                upload.CopyTo(stream);
            }

            string url = $"{"/UploadCenter/ckeditor4"}{fileName}";


            return Json(new { uploaded = true, url });


        }
        public IActionResult FileBrowser()
        {
            string root = Path.Combine(_webHostEnvironment.WebRootPath, "UploadCenter/ckeditor4");
            DirectoryInfo dir = new DirectoryInfo(root);
            string[] files = System.IO.Directory.GetFiles(root);
            FileBrowserVM fileBrowserVM = new()
            {
                FileInfos = dir.GetFiles().ToList()
            };

            return View(fileBrowserVM);
        }
    }
}
