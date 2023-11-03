using Core.Security;
using Core.Services.Interfaces;
using Core.Utility;
using DataLayer.Entities.Supplementary;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Web.Areas.Manage.Controllers
{
    [Area("Manage")]
    [Authorize]
    [PermissionCheckerByPermissionName("initialinfo")]
    public class InitialInfosController : Controller
    {
        private readonly IGenericService<InitialInfo> _genericService;
        public InitialInfosController(IGenericService<InitialInfo> genericService)
        {
            _genericService = genericService;
        }
        public async Task<IActionResult> Index()
        {
            return View(await _genericService.GetAllAsync());
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(InitialInfo initialInfo,IFormFile SiteLogo,IFormFile SiteThumbLogo,IFormFile FavIcon)
        {
            if(!ModelState.IsValid)
            {
                return View(initialInfo);
            }
            string[] ex = { ".jpg", ".jpeg", ".gif", ".png", ".svg" };
            if(SiteLogo == null)
            {
                ModelState.AddModelError("SiteLogo", "لوگوی سایت را انتخاب کنید !");
                return View(initialInfo);
            }
            if(FavIcon == null)
            {
                ModelState.AddModelError("FavIcon", "فاو آیکن سایت را انتخاب کنید !");
                return View(initialInfo);
            }
            if(SiteLogo.Length > .01*1024*1024)
            {
                ModelState.AddModelError("SiteLogo", "حجم فایل بیشتر از 10 کیلوبایت است !");
                return View(initialInfo);
            }
            if (FavIcon.Length > .005 * 1024 * 1024)
            {
                ModelState.AddModelError("FavIcon", "حجم فایل بیشتر از 5 کیلوبایت است !");
                return View(initialInfo);
            }
            if(System.IO.File.Exists("wwwroot/images/icons/" + SiteLogo.FileName))
            {
                System.IO.File.Delete("wwwroot/images/icons/" + SiteLogo.FileName);
            }
            if (System.IO.File.Exists("wwwroot/" + FavIcon.FileName))
            {
                System.IO.File.Delete("wwwroot/" + FavIcon.FileName);
            }
            initialInfo.SiteLogo = SiteLogo.SaveUploadedImage("wwwroot/images/icons/",true);
            initialInfo.FavIcon = FavIcon.SaveUploadedImage("wwwroot",true);
            initialInfo.CreatedDate = DateTime.Now;
            _genericService.Create(initialInfo);
            await _genericService.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var intialInfo = await _genericService.GetByIdAsync((int)id);
            if (intialInfo == null)
            {
                return NotFound();
            }
            return View(intialInfo);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id,InitialInfo initialInfo, IFormFile SiteLogo, IFormFile SiteThumbLogo, IFormFile FavIcon)
        {
            if(id != initialInfo.Id)
            {
                return NotFound();
            }
            if(!ModelState.IsValid)
            {
                return View(initialInfo);
            }
            if(SiteLogo != null)
            {
                if (SiteLogo.Length > .01 * 1024 * 1024)
                {
                    ModelState.AddModelError("SiteLogo", "حجم فایل بیشتر از 10 کیلوبایت است !");
                    return View(initialInfo);
                }
            }
            if(FavIcon != null)
            {
                if (FavIcon.Length > .005 * 1024 * 1024)
                {
                    ModelState.AddModelError("FavIcon", "حجم فایل بیشتر از 5 کیلوبایت است !");
                    return View(initialInfo);
                }
            }
            if(SiteLogo != null)
            {
                if (System.IO.File.Exists("wwwroot/images/icons" + SiteLogo.FileName))
                {
                    System.IO.File.Delete("wwwroot/images/icons" + SiteLogo.FileName);
                }
                initialInfo.SiteLogo = SiteLogo.SaveUploadedImage("wwwroot/images/icons", true);
            }
            if(FavIcon !=null)
            {
                if (System.IO.File.Exists("wwwroot/" + FavIcon.FileName))
                {
                    System.IO.File.Delete("wwwroot/" + FavIcon.FileName);
                }
                initialInfo.FavIcon = FavIcon.SaveUploadedImage("wwwroot", true);
            }
            
            
            _genericService.Edit(initialInfo);
            await _genericService.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

    }
}
