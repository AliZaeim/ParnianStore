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
    [PermissionCheckerByPermissionName("footertopcols")]
    public class ThreeColmunsController : Controller
    {
        
        private readonly IGenericService<ThreeColmun> _genericService;
        public ThreeColmunsController(IGenericService<ThreeColmun> genericService)
        {
            _genericService = genericService;           
        }

        // GET: Manage/ThreeColmuns1
        public async Task<IActionResult> Index()
        {
            return View(await _genericService.GetAllAsync());
        }

        // GET: Manage/ThreeColmuns1/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var threeColmuns = await _genericService.GetByIdAsync((int)id);
            if (threeColmuns == null)
            {
                return NotFound();
            }

            return View(threeColmuns);
        }

        // GET: Manage/ThreeColmuns1/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Manage/ThreeColmuns1/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ThreeColmun threeColmuns, IFormFile Image)
        {
            if (ModelState.IsValid)
            {
                if(Image != null)
                {
                    string[] ex = { ".jpg", ".jpeg", ".gif", ".png", ".svg", ".webp", ".avif" };
                    FileValidation ImgValid = await Image.UploadedImageValidation(50,ex);
                    if(!ImgValid.IsValid)
                    {
                        ModelState.AddModelError("Image", ImgValid.Message);
                        return View(threeColmuns);
                    }
                    threeColmuns.Image = Image.SaveUploadedImage("wwwroot/images/icons", true);
                }
                
                threeColmuns.CreatedDate = DateTime.Now;
                _genericService.Create(threeColmuns);
                await _genericService.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(threeColmuns);
        }

        // GET: Manage/ThreeColmuns1/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var threeColmuns = await _genericService.GetByIdAsync((int)id);
            if (threeColmuns == null)
            {
                return NotFound();
            }
            return View(threeColmuns);
        }

        // POST: Manage/ThreeColmuns1/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id,ThreeColmun threeColmuns,IFormFile Image)
        {
            if (id != threeColmuns.Id)
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
                        FileValidation ImgValid = await Image.UploadedImageValidation(50, ex);
                        if (!ImgValid.IsValid)
                        {
                            ModelState.AddModelError("Image", ImgValid.Message);
                            return View(threeColmuns);
                        }
                        threeColmuns.Image = Image.SaveUploadedImage("wwwroot/images/icons", true);
                    }

                    _genericService.Edit(threeColmuns);
                    await _genericService.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ThreeColmunsExists(threeColmuns.Id))
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
            return View(threeColmuns);
        }

        // GET: Manage/ThreeColmuns/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var threeColmuns = await _genericService.GetByIdAsync((int)id);
            if (threeColmuns == null)
            {
                return NotFound();
            }

            return View(threeColmuns);
        }

        // POST: Manage/ThreeColmuns1/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {

            _genericService.Delete(id);
            await _genericService.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ThreeColmunsExists(int id)
        {
            return _genericService.ExistEntity(id);
        }
    }
}
