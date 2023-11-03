using Core.DTOs.Admin;
using Core.Security;
using Core.Services.Interfaces;
using Core.Utility;
using DataLayer.Entities.Supplementary;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace Web.Areas.Manage.Controllers
{
    [Area("Manage")]
    [Authorize]
    [PermissionCheckerByPermissionName("fractionslider")]
    public class FSImagesController : Controller
    {

        private readonly ICompService _compService;
        public FSImagesController(ICompService compService)
        {

            _compService = compService;
        }

        // GET: Manage/FSImages
        public async Task<IActionResult> Index()
        {
            var fSImages = await _compService.GetFSImagesAsync();
            return View(fSImages);
        }
        public async Task<bool> ChangeStatus(int id, int status)
        {
            FSImage fSImage = await _compService.GetFSImageByIdAsync(id);
            bool st = false;
            if (status == 1)
            {
                st = true;
            }
            fSImage.IsActive = st;
            _compService.UpdateFSImage(fSImage);
            await _compService.SaveChangesAsync();
            return st;

        }
        // GET: Manage/FSImages/Details/5
        public async Task<IActionResult> Details(int? id, string retUrl)
        {
            if (id == null)
            {
                return NotFound();
            }

            var fSImage = await _compService.GetFSImageByIdAsync((int)id);
            if (fSImage == null)
            {
                return NotFound();
            }
            fSImage.ReturnUrl = retUrl;
            return View(fSImage);
        }

        // GET: Manage/FSImages/Create
        public async Task<IActionResult> Create(int? fsId,string retUrl)
        {
            var fs = await _compService.GetFractionSlidersAsync();
            ViewData["FSId"] = new SelectList(fs.Where(w => w.IsActive).ToList(), "Id", "Title", fsId);
            FSImage fSImage = new();
            fSImage.ReturnUrl = retUrl;
            return View(fSImage);
        }

        // POST: Manage/FSImages/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(FSImage fSImage, IFormFile Image)
        {

            if (ModelState.IsValid)
            {
                string[] ex = { ".jpg", ".jpeg", ".gif", ".png", ".svg", ".webp", ".avif" };
                FileValidation imageValidation = await Image.UploadedImageValidation(100, ex);
                if (!imageValidation.IsValid)
                {
                    var fsl = await _compService.GetFractionSlidersAsync();
                    ViewData["FSId"] = new SelectList(fsl.Where(w => w.IsActive).ToList(), "Id", "Title",fSImage.FSId);
                    ModelState.AddModelError("Image", imageValidation.Message);
                    return View(fSImage);

                }
                fSImage.Image = Image.SaveUploadedImage("wwwroot/images/sliders/fs", true);
                _compService.CreateFSImage(fSImage);
                await _compService.SaveChangesAsync();
                if (!string.IsNullOrEmpty(fSImage.ReturnUrl))
                {
                    return Redirect(fSImage.ReturnUrl.Replace("-", "/"));
                }
                else
                {
                    return RedirectToAction(nameof(Index));
                }

            }
            var fs = await _compService.GetFractionSlidersAsync();
            ViewData["FSId"] = new SelectList(fs.Where(w => w.IsActive).ToList(), "Id", "Title");
            return View(fSImage);
        }

        // GET: Manage/FSImages/Edit/5
        public async Task<IActionResult> Edit(int? id, string retUrl)
        {
            if (id == null)
            {
                return NotFound();
            }

            var fSImage = await _compService.GetFSImageByIdAsync((int)id);
            if (fSImage == null)
            {
                return NotFound();
            }
            var fs = await _compService.GetFractionSlidersAsync();
            ViewData["FSId"] = new SelectList(fs.Where(w => w.IsActive).ToList(), "Id", "Title",fSImage.FSId);
            fSImage.ReturnUrl = retUrl;
            return View(fSImage);
        }

        // POST: Manage/FSImages/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, FSImage fSImage, IFormFile Image)
        {
            if (id != fSImage.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (Image != null)
                    {
                        string[] ex = { ".jpg", ".jpeg", ".gif", ".png", ".svg", ".webp", ".avif" };
                        FileValidation imageValidation = await Image.UploadedImageValidation(100, ex);
                        if (!imageValidation.IsValid)
                        {
                            var fsT = await _compService.GetFractionSlidersAsync();
                            ViewData["FSId"] = new SelectList(fsT.Where(w => w.IsActive).ToList(), "Id", "Title");
                            ModelState.AddModelError("Image", imageValidation.Message);
                            return View(fSImage);

                        }
                        fSImage.Image = Image.SaveUploadedImage("wwwroot/images/sliders/fs", true);
                    }

                    _compService.UpdateFSImage(fSImage);
                    await _compService.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FSImageExists(fSImage.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                if (!string.IsNullOrEmpty(fSImage.ReturnUrl))
                {

                    return Redirect(fSImage.ReturnUrl.Replace("-", "/"));
                }
                else
                {
                    return RedirectToAction(nameof(Index));
                }

            }
            var fs = await _compService.GetFractionSlidersAsync();
            ViewData["FSId"] = new SelectList(fs.Where(w => w.IsActive).ToList(), "Id", "Title");
            return View(fSImage);
        }

        // GET: Manage/FSImages/Delete/5
        public async Task<IActionResult> Delete(int? id, string retUrl)
        {
            if (id == null)
            {
                return NotFound();
            }

            var fSImage = await _compService.GetFSImageByIdAsync((int)id);

            if (fSImage == null)
            {
                return NotFound();
            }
            fSImage.ReturnUrl = retUrl;
            return View(fSImage);
        }

        // POST: Manage/FSImages/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id, string ReturnUrl)
        {
            var fSImage = await _compService.GetFSImageByIdAsync(id);
            _compService.RemoveFSImage(fSImage);
            await _compService.SaveChangesAsync();
            if (string.IsNullOrEmpty(ReturnUrl))
            {
                return RedirectToAction(nameof(Index));
            }
            else
            {
                return Redirect(ReturnUrl.Replace("-", "/"));
            }

        }

        private bool FSImageExists(int id)
        {
            var fsimages = _compService.GetFSImagesAsync().Result;
            return fsimages.Any(x => x.IsActive && x.FractionSlider.IsActive && x.Id == id);
        }
    }
}
