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
using Core.Security;
using Core.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Core.DTOs.Admin;
using Core.Utility;

namespace Web.Areas.Manage.Controllers
{
    [Area("Manage")]
    [Authorize]
    [PermissionCheckerByPermissionName("insta")]

    public class InstagramsController : Controller
    {
       
        private readonly IGenericService<Instagram> _instaService;
        public InstagramsController(IGenericService<Instagram> instaService)
        {
            _instaService = instaService;
           
        }

        // GET: Manage/Instagrams
        public async Task<IActionResult> Index()
        {
            return View(await _instaService.GetAllAsync());
        }

        // GET: Manage/Instagrams/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var instagram = await _instaService.GetByIdAsync((int)id);
            if (instagram == null)
            {
                return NotFound();
            }

            return View(instagram);
        }

        // GET: Manage/Instagrams/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Manage/Instagrams/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Instagram instagram , IFormFile Image)
        {
            if (ModelState.IsValid)
            {
                if(Image == null)
                {
                    ModelState.AddModelError("Image", "تصویر انتخاب نشده است !");
                    return View();
                }
                string[] ex = { ".jpg", ".jpeg", ".gif", ".png", ".svg", ".webp", ".avif" };
                FileValidation ImageValidation = await Image.UploadedImageValidation(50,ex);
                if(!ImageValidation.IsValid)
                {
                    ModelState.AddModelError("Image", ImageValidation.Message);
                    return View();
                }
                instagram.Image = Image.SaveUploadedImage("wwwroot/images/instagrams/", true);
                instagram.CreatedDate = DateTime.Now;
                _instaService.Create(instagram);
                await _instaService.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(instagram);
        }

        // GET: Manage/Instagrams/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var instagram = await _instaService.GetByIdAsync((int)id);
            if (instagram == null)
            {
                return NotFound();
            }
            return View(instagram);
        }

        // POST: Manage/Instagrams/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Instagram instagram, IFormFile Image)
        {
            if (id != instagram.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if(Image != null)
                    {
                        string[] ex = { ".jpg", ".jpeg", ".gif", ".png", ".svg", ".webp", ".avif" };
                        FileValidation fileValidation =await Image.UploadedImageValidation(50, ex);
                        if(!fileValidation.IsValid)
                        {
                            ModelState.AddModelError("Image", fileValidation.Message);
                            return View();
                        }
                        instagram.Image = Image.SaveUploadedImage("wwwroot/images/instagrams/", true);
                    }
                    instagram.CreatedDate = DateTime.Now;
                    _instaService.Edit(instagram);
                    await _instaService.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!InstagramExists(instagram.Id))
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
            return View(instagram);
        }

        // GET: Manage/Instagrams/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var instagram = await _instaService.GetByIdAsync((int)id);
            if (instagram == null)
            {
                return NotFound();
            }

            return View(instagram);
        }

        // POST: Manage/Instagrams/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var instagram = await _instaService.GetByIdAsync(id);
            _instaService.Delete(instagram);
            await _instaService.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool InstagramExists(int id)
        {
            return _instaService.ExistEntity(id);
        }
    }
}
