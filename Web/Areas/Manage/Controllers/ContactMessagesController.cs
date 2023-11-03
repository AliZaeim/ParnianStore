using Core.Security;
using Core.Services;
using Core.Services.Interfaces;
using DataLayer.Entities.Supplementary;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Web.Areas.Manage.Controllers
{
    [Area("Manage")]
    [Authorize]
    [PermissionCheckerByPermissionName("contactusermessage")]
    public class ContactMessagesController : Controller
    {
        // GET: ContactMessagesController
        private readonly IGenericService<ContactMessage> _genericService;
        public ContactMessagesController(IGenericService<ContactMessage> genericService)
        {
            _genericService = genericService;
        }
        public async Task<ActionResult> Index()
        {
            return View(await _genericService.GetAllAsync());
        }

        // GET: ContactMessagesController/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var contactMessage = await _genericService.GetByIdAsync((int)id);
            if (contactMessage == null)
            {
                return NotFound();
            }

            return View(contactMessage);
        }

        // GET: ContactMessagesController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ContactMessagesController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: ContactMessagesController/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var contactMessage = await _genericService.GetByIdAsync((int)id);
            if (contactMessage == null)
            {
                return NotFound();
            }
            return View(contactMessage);
        }

        // POST: ContactMessagesController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(int id, ContactMessage contactMessage)
        {
            if (id != contactMessage.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _genericService.Edit(contactMessage);
                    await _genericService.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_genericService.ExistEntity(contactMessage.Id))
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
            return View(contactMessage);
        }

        // GET: ContactMessagesController/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var contactMessage = await _genericService.GetByIdAsync((int)id);
            if (contactMessage == null)
            {
                return NotFound();
            }

            return View(contactMessage);
        }

        // POST: ContactMessagesController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
