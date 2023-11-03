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
using Core.Security;

namespace Web.Areas.Manage.Controllers
{
    [Area("Manage")]
    [Authorize]
    [PermissionCheckerByPermissionName("contactinfo")]
    public class ContactInfoesController : Controller
    {
        
        private readonly IGenericService<ContactInfo> _genericService;
        public ContactInfoesController(IGenericService<ContactInfo> genericService)
        {           
            _genericService = genericService;
        }

        // GET: Manage/ContactInfoes
        public async Task<IActionResult> Index()
        {
            return View(await _genericService.GetAllAsync());
        }

        // GET: Manage/ContactInfoes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var contactInfo = await _genericService.GetByIdAsync((int)id);
            if (contactInfo == null)
            {
                return NotFound();
            }

            return View(contactInfo);
        }

        // GET: Manage/ContactInfoes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Manage/ContactInfoes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ContactInfo contactInfo)
        {
            if (ModelState.IsValid)
            {
                contactInfo.CreatedDate = DateTime.Now;
                _genericService.Create(contactInfo);
                await _genericService.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(contactInfo);
        }

        // GET: Manage/ContactInfoes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var contactInfo = await _genericService.GetByIdAsync((int)id);
            if (contactInfo == null)
            {
                return NotFound();
            }
            return View(contactInfo);
        }

        // POST: Manage/ContactInfoes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id,  ContactInfo contactInfo)
        {
            if (id != contactInfo.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _genericService.Edit(contactInfo);
                    await _genericService.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ContactInfoExists(contactInfo.Id))
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
            return View(contactInfo);
        }

        //// GET: Manage/ContactInfoes/Delete/5
        //public async Task<IActionResult> Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var contactInfo = await _context.ContactInfos
        //        .FirstOrDefaultAsync(m => m.Id == id);
        //    if (contactInfo == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(contactInfo);
        //}

        //// POST: Manage/ContactInfoes/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> DeleteConfirmed(int id)
        //{
        //    var contactInfo = await _context.ContactInfos.FindAsync(id);
        //    _context.ContactInfos.Remove(contactInfo);
        //    await _context.SaveChangesAsync();
        //    return RedirectToAction(nameof(Index));
        //}

        private bool ContactInfoExists(int id)
        {
            ContactInfo contactInfo = _genericService.GetById(id);
            if(contactInfo != null)
            {
                return true;
            }
            return false;
        }
    }
}
