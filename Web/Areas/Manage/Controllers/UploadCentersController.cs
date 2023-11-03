using Core.Security;
using Core.Services.Interfaces;
using DataLayer.Context;
using DataLayer.Entities.Supplementary;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.IO;
using System.Threading.Tasks;

namespace Web.Areas.Manage.Controllers
{
    [Area("Manage")]
    [Authorize]
    [PermissionCheckerByPermissionName("uploadcenter")]
    public class UploadCentersController : Controller
    {
        private readonly MyContext _context;
        private readonly IGenericService<UploadCenter> _uploadService;

        public UploadCentersController(IGenericService<UploadCenter> uploadService,MyContext context)
        {
            _context = context;
            _uploadService = uploadService;
        }

        // GET: Manage/UploadCenters
        public async Task<IActionResult> Index()
        {
            return View(await _uploadService.GetAllAsync());
        }

        
        // GET: Manage/UploadCenters/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Manage/UploadCenters/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(UploadCenter uploadCenter, IFormFile File)
        {
            if (!ModelState.IsValid)
            {
                return View(uploadCenter);
            }
            if (File == null)
            {
                ModelState.AddModelError("File", "فایل مورد نظر خود را انتخاب کنید !");
                return View(uploadCenter);
            }
            if (System.IO.File.Exists("wwwroot/UploadCenter/" + uploadCenter.FileName))
            {
                ModelState.AddModelError("FileName", "این نام قبلا ذخیره شده است !");
                return View(uploadCenter);
            }
            if (File != null)
            {
                string fileName = string.Empty;
                if (!string.IsNullOrEmpty(uploadCenter.FileName))
                {
                    fileName = uploadCenter.FileName + Path.GetExtension(File.FileName); 
                }
                else
                {
                    fileName = File.FileName ;
                }
                string filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/UploadCenter", fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    File.CopyTo(stream);
                }
                uploadCenter.File = fileName;
            }
            _uploadService.Create(uploadCenter);
            await _uploadService.SaveChangesAsync();
            return RedirectToAction(nameof(Index));

        }


        // GET: Manage/UploadCenters/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var uploadCenter = await _uploadService.GetByIdAsync((int)id);
            if (uploadCenter == null)
            {
                return NotFound();
            }
            return View(uploadCenter);
        }

        // POST: Manage/UploadCenters/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, UploadCenter uploadCenter, IFormFile File)
        {
            if (id != uploadCenter.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (File != null)
                    {
                        string fileName = string.Empty;
                        if (!string.IsNullOrEmpty(uploadCenter.FileName))
                        {
                            fileName = uploadCenter.FileName + Path.GetExtension(File.FileName); 
                        }
                        else
                        {
                            fileName = Core.Prodocers.Generators.GenerateUniqueCode() + Path.GetExtension(File.FileName);
                        }
                        string filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/UploadCenter", fileName);
                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            File.CopyTo(stream);
                        }
                        uploadCenter.File = fileName;
                    }
                    _uploadService.Edit(uploadCenter);
                    await _uploadService.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_uploadService.ExistEntity(uploadCenter.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(uploadCenter);
        }

        // GET: Manage/UploadCenters/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var uploadCenter = await _uploadService.GetByIdAsync((int)id);
            if (uploadCenter == null)
            {
                return NotFound();
            }

            return View(uploadCenter);
        }

        // POST: Manage/UploadCenters/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var uploadCenter = await _uploadService.GetByIdAsync(id);
            string fname = uploadCenter.File;
            bool ex = System.IO.File.Exists("wwwroot/UploadCenter/" + fname);
            _uploadService.Delete(uploadCenter);
            await _uploadService.SaveChangesAsync();
            if (ex)
            {
                System.IO.File.Delete("wwwroot/UploadCenter/" + fname);
            }
            return RedirectToAction(nameof(Index));
        }

        
    }
}
