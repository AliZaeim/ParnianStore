using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DataLayer.Context;
using DataLayer.Entities.Supplementary;
using Core.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Core.Security;
using Microsoft.AspNetCore.Http;
using Core.DTOs.Admin;
using Core.Utility;

namespace Web.Areas.Manage.Controllers
{
    [Area("Manage")]
    [Authorize]
    [PermissionCheckerByPermissionName("notification")]
    public class NotificationsController : Controller
    {

        private readonly IGenericService<Notification> _genericService;
        public NotificationsController(IGenericService<Notification> genericService)
        {
            _genericService = genericService;

        }

        // GET: Manage/Notifications
        public async Task<IActionResult> Index()
        {
            return View(await _genericService.GetAllAsync());
        }
        public bool ChangeStatus(int id, int status)
        {
            Notification notification = _genericService.GetById(id);
            bool st = false;
            if (status == 1)
            {
                st = true;
            }
            notification.IsActive = st;
            _genericService.Edit(notification);
            _genericService.SaveChanges();
            return st;

        }
        // GET: Manage/Notifications/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var notification = await _genericService.GetByIdAsync((int)id);
            if (notification == null)
            {
                return NotFound();
            }

            return View(notification);
        }

        // GET: Manage/Notifications/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Manage/Notifications/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Notification notification,IFormFile Image)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (Image != null)
                    {
                        string[] ex = { ".jpg", ".jpeg", ".gif", ".png", ".svg" };
                        FileValidation ImageValidation = await Image.UploadedImageValidation(50, ex);
                        if (!ImageValidation.IsValid)
                        {
                            ModelState.AddModelError("Image", ImageValidation.Message);
                            return View(notification);
                        }
                        notification.Image = Image.SaveUploadedImage("wwwroot/images/notifications", true);
                    }
                    
                    notification.CreatedDate = DateTime.Now;
                    _genericService.Create(notification);
                    await _genericService.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
            }
            catch (Exception ex)
            {

                string m = ex.Message;
            }
            
            return View(notification);
        }

        // GET: Manage/Notifications/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var notification = await _genericService.GetByIdAsync((int)id);
            if (notification == null)
            {
                return NotFound();
            }
            return View(notification);
        }

        // POST: Manage/Notifications/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Notification notification, IFormFile Image)
        {

            if (id != notification.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (Image != null)
                    {
                        string[] ex = { ".jpg", ".jpeg", ".gif", ".png", ".svg" };
                        FileValidation ImageValidation = await Image.UploadedImageValidation(50, ex);
                        if (!ImageValidation.IsValid)
                        {
                            ModelState.AddModelError("Image", ImageValidation.Message);
                            return View(notification);
                        }
                        notification.Image = Image.SaveUploadedImage("wwwroot/images/notifications", true);
                    }
                    
                    _genericService.Edit(notification);
                    await _genericService.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (_genericService.ExistEntity(id))
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
            return View(notification);
        }

        // GET: Manage/Notifications/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var notification = await _genericService.GetByIdAsync((int)id);
            if (notification == null)
            {
                return NotFound();
            }

            return View(notification);
        }

        // POST: Manage/Notifications/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var notification = await _genericService.GetByIdAsync(id);
            _genericService.Delete(notification);
            await _genericService.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }


    }
}
