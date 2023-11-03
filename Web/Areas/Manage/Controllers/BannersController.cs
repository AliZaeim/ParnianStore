using Core.DTOs.Admin;
using Core.Security;
using Core.Services.Interfaces;
using Core.Utility;
using DataLayer.Entities.Supplementary;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace Web.Areas.Manage.Controllers
{
    [Area("Manage")]
    [Authorize]
    [PermissionCheckerByPermissionName("pintrobanner")]
    public class BannersController : Controller
    {
        
        private readonly IProductService _productService;
        public BannersController(IProductService productService)
        {           
            _productService = productService;
        }

        // GET: Manage/Banners
        public async Task<IActionResult> Index()
        {
            return View(await _productService.GetBannersAsync());
        }
        public async Task<bool> ChangeStatus(int id, int status)
        {
            Banner banner =await _productService.GetBannerByIdAsync(id);
            bool st = false;
            if (status == 1)
            {
                st = true;
            }
            banner.IsActive = st;
            _productService.UpdateBanner(banner);
            await _productService.SaveChangesAsync();
            return st;

        }
        // GET: Manage/Banners/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var banner = await _productService.GetBannerByIdAsync((int)id);
            if (banner == null)
            {
                return NotFound();
            }

            return View(banner);
        }

        // GET: Manage/Banners/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Manage/Banners/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Banner banner,IFormFile Banner1,IFormFile Banner1Mobile, IFormFile Banner2, IFormFile Banner2Mobile, IFormFile Banner3,  IFormFile Banner3Mobile, IFormFile Banner4,  IFormFile Banner4Mobile)
        {
            if (ModelState.IsValid)
            {
                int Dw = 450; int Dh = 250;                
                int Mw = 450; int Mh = 250;
                if(Banner1 !=null && Banner2 == null && Banner3== null && Banner4 == null)
                {
                    Dw = 1650;
                }
                if (Banner1 != null && Banner2 != null && Banner3 == null && Banner4 == null)
                {
                    Dw = 820;
                }
                if (Banner1 != null && Banner2 != null && Banner3 != null && Banner4 == null)
                {
                    Dw = 545;
                }
                if (Banner1 != null && Banner2 != null && Banner3 != null && Banner4 != null)
                {
                    Dw = 362;
                }
                if (Banner1 == null)
                {
                    ModelState.AddModelError("Banner1", "حتما باید بنر اول تصویر داشته باشد!");
                    return View(banner);
                }
                string[] ex = { ".jpg", ".jpeg", ".gif", ".png", ".svg", ".webp", ".avif" };
                FileValidation Banner1ImageValidation = await Banner1.UploadedImageValidation(Dw, Dh, 20, ex);
                if (!Banner1ImageValidation.IsValid)
                {
                    ModelState.AddModelError("Banner1", Banner1ImageValidation.Message);
                    return View(banner);
                }
                else
                {
                    banner.Banner1 = Banner1.SaveUploadedImage("wwwroot/images/banners", true);
                    
                    if (Banner1Mobile !=null)
                    {
                        FileValidation Banner1MobileImageValidation = await Banner1Mobile.UploadedImageValidation(Mw, Mh, 20, ex);
                        if (!Banner1MobileImageValidation.IsValid)
                        {
                            ModelState.AddModelError("Banner1Mobile", Banner1MobileImageValidation.Message);
                            return View(banner);
                        }
                        banner.Banner1Mobile = Banner1Mobile.SaveUploadedImage("wwwroot/images/banners/mobile", true);
                    }
                    else
                    {
                        ModelState.AddModelError("Banner1Mobile", "تصویر موبایلی بنر اول را با ابعاد مناسب انتخاب کنید !");
                        return View(banner);
                    }
                }
                if (Banner2 != null)
                {
                    FileValidation Banner2ImageValidation = await Banner2.UploadedImageValidation(Dw, Dh, 20, ex);
                    if (!Banner2ImageValidation.IsValid)
                    {
                        ModelState.AddModelError("Banner2", Banner2ImageValidation.Message);
                        return View(banner);
                    }
                    else
                    {
                        banner.Banner2 = Banner2.SaveUploadedImage("wwwroot/images/banners", true);
                        if (Banner2Mobile != null)
                        {
                            FileValidation Banner2MobileImageValidation = await Banner2Mobile.UploadedImageValidation(Mw, Mh, 20, ex);
                            if (!Banner2MobileImageValidation.IsValid)
                            {
                                ModelState.AddModelError("Banner2Mobile", Banner2MobileImageValidation.Message);
                                return View(banner);
                            }
                            banner.Banner2Mobile = Banner2Mobile.SaveUploadedImage("wwwroot/images/banners/mobile", true);
                        }
                        else
                        {
                            ModelState.AddModelError("Banner2Mobile", "تصویر موبایلی بنر دوم را با ابعاد مناسب انتخاب کنید !");
                            return View(banner);
                        }
                    }

                }
                if (Banner3 != null)
                {
                    FileValidation Banner3ImageValidation = await Banner3.UploadedImageValidation(Dw, Dh, 20, ex);
                    if (!Banner3ImageValidation.IsValid)
                    {
                        ModelState.AddModelError("Banner3", Banner3ImageValidation.Message);
                        return View(banner);
                    }
                    else
                    {
                        banner.Banner3 = Banner3.SaveUploadedImage("wwwroot/images/banners", true);
                        if (Banner3Mobile != null)
                        {
                            FileValidation Banner3MobileImageValidation = await Banner3Mobile.UploadedImageValidation(Mw, Mh, 20, ex);
                            if (!Banner3MobileImageValidation.IsValid)
                            {
                                ModelState.AddModelError("Banner3Mobile", Banner3MobileImageValidation.Message);
                                return View(banner);
                            }
                            banner.Banner3Mobile = Banner3Mobile.SaveUploadedImage("wwwroot/images/banners/mobile", true);
                        }
                        else
                        {
                            ModelState.AddModelError("Banner3Mobile", "تصویر موبایلی بنر سوم را با ابعاد مناسب انتخاب کنید !");
                            return View(banner);
                        }
                    }
                }
                if (Banner4 != null)
                {
                    FileValidation Banner4ImageValidation = await Banner4.UploadedImageValidation(Dw, Dh, 20, ex);
                    if (!Banner4ImageValidation.IsValid)
                    {
                        ModelState.AddModelError("Banner4", Banner4ImageValidation.Message);
                        return View(banner);
                    }
                    else
                    {
                        banner.Banner4 = Banner4.SaveUploadedImage("wwwroot/images/banners", true);
                        if (Banner4Mobile != null)
                        {
                            FileValidation Banner4MobileImageValidation = await Banner4Mobile.UploadedImageValidation(Mw, Mh, 20, ex);
                            if (!Banner4MobileImageValidation.IsValid)
                            {
                                ModelState.AddModelError("Banner4Mobile", Banner4MobileImageValidation.Message);
                                return View(banner);
                            }
                            banner.Banner4Mobile = Banner4Mobile.SaveUploadedImage("wwwroot/images/banners/mobile", true);
                        }
                        else
                        {
                            ModelState.AddModelError("Banner4Mobile", "تصویر موبایلی بنر چهارم را با ابعاد مناسب انتخاب کنید !");
                            return View(banner);
                        }
                    }
                }
                banner.CreatedDate = DateTime.Now;
                _productService.CreateBanner(banner);
                await _productService.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(banner);
        }

        // GET: Manage/Banners/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var banner = await _productService.GetBannerByIdAsync((int)id);
            if (banner == null)
            {
                return NotFound();
            }
            return View(banner);
        }

        // POST: Manage/Banners/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id,  Banner banner, IFormFile Banner1, IFormFile Banner1Mobile, IFormFile Banner2, IFormFile Banner2Mobile, IFormFile Banner3, IFormFile Banner3Mobile, IFormFile Banner4, IFormFile Banner4Mobile)
        {
            if (id != banner.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    int Dw = 450; int Dh = 250;
                    int Mw = 450; int Mh = 250;
                    if (Banner1 != null && Banner2 == null && Banner3 == null && Banner4 == null)
                    {
                        Dw = 1650;
                    }
                    if (Banner1 != null && Banner2 != null && Banner3 == null && Banner4 == null)
                    {
                        Dw = 820;
                    }
                    if (Banner1 != null && Banner2 != null && Banner3 != null && Banner4 == null)
                    {
                        Dw = 545;
                    }
                    if (Banner1 != null && Banner2 != null && Banner3 != null && Banner4 != null)
                    {
                        Dw = 362;
                    }
                    string[] ex = { ".jpg", ".jpeg", ".gif", ".png", ".svg", ".webp", ".avif" };

                    if (Banner1 != null)
                    {
                        FileValidation Banner1ImageValidation = await Banner1.UploadedImageValidation(Dw, Dh, 20, ex);
                        if(!Banner1ImageValidation.IsValid)
                        {
                            ModelState.AddModelError("Banner1", Banner1ImageValidation.Message);
                            return View(banner);
                        }
                        else
                        {
                            banner.Banner1 = Banner1.SaveUploadedImage("wwwroot/images/banners", true);

                            if (Banner1Mobile != null)
                            {
                                FileValidation Banner1MobileImageValidation = await Banner1Mobile.UploadedImageValidation(Mw, Mh, 20, ex);
                                if (!Banner1MobileImageValidation.IsValid)
                                {
                                    ModelState.AddModelError("Banner1Mobile", Banner1MobileImageValidation.Message);
                                    return View(banner);
                                }
                                banner.Banner1Mobile = Banner1Mobile.SaveUploadedImage("wwwroot/images/banners/mobile", true);
                            }
                            else
                            {
                                ModelState.AddModelError("Banner1Mobile", "تصویر موبایلی بنر اول را با ابعاد مناسب انتخاب کنید !");
                                return View(banner);
                            }
                        }
                    }
                    
                    if (Banner2 != null)
                    {
                        FileValidation Banner2ImageValidation = await Banner2.UploadedImageValidation(Dw, Dh, 20, ex);
                        if (!Banner2ImageValidation.IsValid)
                        {
                            ModelState.AddModelError("Banner2", Banner2ImageValidation.Message);
                            return View(banner);
                        }
                        else
                        {
                            banner.Banner2 = Banner2.SaveUploadedImage("wwwroot/images/banners", true);
                            if (Banner2Mobile != null)
                            {
                                FileValidation Banner2MobileImageValidation = await Banner2Mobile.UploadedImageValidation(Mw, Mh, 20, ex);
                                if (!Banner2MobileImageValidation.IsValid)
                                {
                                    ModelState.AddModelError("Banner2Mobile", Banner2MobileImageValidation.Message);
                                    return View(banner);
                                }
                                banner.Banner2Mobile = Banner2Mobile.SaveUploadedImage("wwwroot/images/banners/mobile", true);
                            }
                            else
                            {
                                ModelState.AddModelError("Banner2Mobile", "تصویر موبایلی بنر دوم را با ابعاد مناسب انتخاب کنید !");
                                return View(banner);
                            }
                        }

                    }
                    if (Banner3 != null)
                    {
                        FileValidation Banner3ImageValidation = await Banner3.UploadedImageValidation(Dw, Dh, 20, ex);
                        if (!Banner3ImageValidation.IsValid)
                        {
                            ModelState.AddModelError("Banner3", Banner3ImageValidation.Message);
                            return View(banner);
                        }
                        else
                        {
                            banner.Banner3 = Banner3.SaveUploadedImage("wwwroot/images/banners", true);
                            if (Banner3Mobile != null)
                            {
                                FileValidation Banner3MobileImageValidation = await Banner3Mobile.UploadedImageValidation(Mw, Mh, 20, ex);
                                if (!Banner3MobileImageValidation.IsValid)
                                {
                                    ModelState.AddModelError("Banner3Mobile", Banner3MobileImageValidation.Message);
                                    return View(banner);
                                }
                                banner.Banner3Mobile = Banner3Mobile.SaveUploadedImage("wwwroot/images/banners/mobile", true);
                            }
                            else
                            {
                                ModelState.AddModelError("Banner3Mobile", "تصویر موبایلی بنر سوم را با ابعاد مناسب انتخاب کنید !");
                                return View(banner);
                            }
                        }
                    }
                    if (Banner4 != null)
                    {
                        FileValidation Banner4ImageValidation = await Banner4.UploadedImageValidation(Dw, Dh, 20, ex);
                        if (!Banner4ImageValidation.IsValid)
                        {
                            ModelState.AddModelError("Banner4", Banner4ImageValidation.Message);
                            return View(banner);
                        }
                        else
                        {
                            banner.Banner4 = Banner4.SaveUploadedImage("wwwroot/images/banners", true);
                            if (Banner4Mobile != null)
                            {
                                FileValidation Banner4MobileImageValidation = await Banner4Mobile.UploadedImageValidation(Mw, Mh, 20, ex);
                                if (!Banner4MobileImageValidation.IsValid)
                                {
                                    ModelState.AddModelError("Banner4Mobile", Banner4MobileImageValidation.Message);
                                    return View(banner);
                                }
                                banner.Banner4Mobile = Banner4Mobile.SaveUploadedImage("wwwroot/images/banners/mobile", true);
                            }
                            else
                            {
                                ModelState.AddModelError("Banner4Mobile", "تصویر موبایلی بنر چهارم را با ابعاد مناسب انتخاب کنید !");
                                return View(banner);
                            }
                        }
                    }
                    _productService.UpdateBanner(banner);
                    await _productService.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BannerExists(banner.Id))
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
            return View(banner);
        }

        // GET: Manage/Banners/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var banner = await _productService.GetBannerByIdAsync((int)id);
            if (banner == null)
            {
                return NotFound();
            }

            return View(banner);
        }

        // POST: Manage/Banners/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var banner = await _productService.GetBannerByIdAsync(id);
            _productService.RemoveBanner(banner);
            await _productService.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BannerExists(int id)
        {
            return _productService.ExistBanner(id);
        }
    }
}
