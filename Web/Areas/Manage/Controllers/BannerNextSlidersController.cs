using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DataLayer.Context;
using DataLayer.Entities.Supplementary;
using Microsoft.AspNetCore.Authorization;
using Core.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Core.DTOs.Admin;
using Core.Utility;
using Core.Security;

namespace Web.Areas.Manage.Controllers
{
    [Area("Manage")]
    [Authorize]
    [PermissionCheckerByPermissionName("slidernextbanner")]
    public class BannerNextSlidersController : Controller
    {
       
        private readonly IGenericService<BannerNextSlider> _genericService;

        public BannerNextSlidersController(IGenericService<BannerNextSlider> genericService)
        {
            
            _genericService = genericService;
        }

        // GET: Manage/BannerNextSliders
        public async Task<IActionResult> Index()
        {
            return View(await _genericService.GetAllAsync());
        }
        public bool ChangeStatus(int id,int status)
        {
            BannerNextSlider bannerNextSlider = _genericService.GetById(id);
            bool st = false;
            if(status == 1)
            {
                st = true;
            }
            bannerNextSlider.IsActive = st;
            _genericService.Edit(bannerNextSlider);
            _genericService.SaveChanges();
            return st;

        }
        // GET: Manage/BannerNextSliders/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var banner = await _genericService.GetByIdAsync((int)id);
            if (banner == null)
            {
                return NotFound();
            }

            return View(banner);
        }

        // GET: Manage/BannerNextSliders/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Manage/BannerNextSliders/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(BannerNextSlider bannerNextSlider, IFormFile Banner1, IFormFile Banner2)
        {
            if (!ModelState.IsValid)
            {
                return View(bannerNextSlider);
            }
            if(Banner1 == null)
            {
                ModelState.AddModelError("Banner1", "حتما باید بنر 1 انتخاب شده باشد !");
                return View(bannerNextSlider);
            }
            string[] ex = { ".jpg", ".jpeg", ".gif", ".png", ".svg" };
            FileValidation fileValidation1 = null;
            FileValidation fileValidation2 = null;
            if(Banner1 !=null && Banner2 == null)
            {
                fileValidation1 = await Banner1.UploadedImageValidation(477, 350, 50, ex);
            }
            if (Banner1 != null && Banner2 != null)
            {
                fileValidation1 = await Banner1.UploadedImageValidation(477, 167, 25, ex);
                fileValidation2 = await Banner2.UploadedImageValidation(477, 167, 25, ex);
                
            }
            if(fileValidation1 !=null)
            {
                if(!fileValidation1.IsValid)
                {
                    ModelState.AddModelError("Banner1", fileValidation1.Message);
                    return View(bannerNextSlider);
                }
            }
            if (fileValidation2 != null)
            {
                if (!fileValidation2.IsValid)
                {
                    ModelState.AddModelError("Banner2", fileValidation2.Message);
                    return View(bannerNextSlider);
                }
            }
            bannerNextSlider.CreatedDate = DateTime.Now;
            bannerNextSlider.Banner1 = Banner1.SaveUploadedImage("wwwroot/images/banners", true);           
            bannerNextSlider.Banner2 = Banner2.SaveUploadedImage("wwwroot/images/banners", true);
            
            _genericService.Create(bannerNextSlider);
            await _genericService.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
            
        }

        // GET: Manage/BannerNextSliders/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bannerNextSlider = await _genericService.GetByIdAsync((int)id);
            if (bannerNextSlider == null)
            {
                return NotFound();
            }
            return View(bannerNextSlider);
        }

        // POST: Manage/BannerNextSliders/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id,  BannerNextSlider bannerNextSlider,IFormFile Banner1, IFormFile Banner2)
        {
            if (id != bannerNextSlider.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (Banner1 == null)
                    {
                        ModelState.AddModelError("Banner1", "حتما باید بنر 1 انتخاب شده باشد !");
                        return View(bannerNextSlider);
                    }

                    string[] ex = { ".jpg", ".jpeg", ".gif", ".png", ".svg" };
                    FileValidation fileValidation1 = null;
                    FileValidation fileValidation2 = null;
                    if (Banner1 != null && Banner2 == null)
                    {
                        fileValidation1 = await Banner1.UploadedImageValidation(477, 350, 50, ex);
                    }
                    if (Banner1 != null && Banner2 != null)
                    {
                        fileValidation1 = await Banner1.UploadedImageValidation(477, 167, 25, ex);
                        fileValidation2 = await Banner2.UploadedImageValidation(477, 167, 25, ex);

                    }
                    if (fileValidation1 != null)
                    {
                        if (!fileValidation1.IsValid)
                        {
                            ModelState.AddModelError("Banner1", fileValidation1.Message);
                            return View(bannerNextSlider);
                        }
                    }
                    if (fileValidation2 != null)
                    {
                        if (!fileValidation2.IsValid)
                        {
                            ModelState.AddModelError("Banner2", fileValidation2.Message);
                            return View(bannerNextSlider);
                        }
                    }
                    if(Banner1 != null)
                    {
                        bannerNextSlider.Banner1 = Banner1.SaveUploadedImage("wwwroot/images/banners", true);
                    }
                    if(Banner2 != null)
                    {
                        bannerNextSlider.Banner2 = Banner2.SaveUploadedImage("wwwroot/images/banners", true);
                    }
                    
                    _genericService.Edit(bannerNextSlider);
                    await _genericService.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BannerNextSliderExists(bannerNextSlider.Id))
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
            return View(bannerNextSlider);
        }

        // GET: Manage/BannerNextSliders/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bannerNextSlider = await _genericService.GetByIdAsync((int)id);
            if (bannerNextSlider == null)
            {
                return NotFound();
            }

            return View(bannerNextSlider);
        }

        // POST: Manage/BannerNextSliders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var bannerNextSlider = await _genericService.GetByIdAsync(id);
            _genericService.Delete(bannerNextSlider);
            await _genericService.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BannerNextSliderExists(int id)
        {
            return _genericService.GetAll().Any(x => x.Id == id);
        }
    }
}
