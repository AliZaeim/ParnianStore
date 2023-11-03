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

namespace Web.Areas.Manage.Controllers
{
    [Area("Manage")]
    [Authorize]
    [PermissionCheckerByPermissionName("aboutfaq")]
    public class AboutUsfaqsController : Controller
    {
        
        private readonly ICompService _compService;
        public AboutUsfaqsController(ICompService compService)
        {
            _compService = compService;
           
        }

        // GET: Manage/AboutUsfaqs
        public async Task<IActionResult> Index()
        {
            return View(await _compService.GetAboutUsfaqsAsync());
        }
        public async Task<bool> ChangeStatus(int id, int status)
        {
            AboutUsfaq aboutUsfaq = await _compService.GetAboutUsfaqByIdAsync(id);
            bool st = false;
            if (status == 1)
            {
                st = true;
            }
            aboutUsfaq.IsActive = st;
            _compService.UpateAboutUsfas(aboutUsfaq);
            await _compService.SaveChangesAsync();
            return st;

        }
        // GET: Manage/AboutUsfaqs/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var aboutUsfaq = await _compService.GetAboutUsfaqByIdAsync((int)id);
            if (aboutUsfaq == null)
            {
                return NotFound();
            }

            return View(aboutUsfaq);
        }

        // GET: Manage/AboutUsfaqs/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Manage/AboutUsfaqs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AboutUsfaq aboutUsfaq)
        {
            if (ModelState.IsValid)
            {
                aboutUsfaq.CreatedDate = DateTime.Now;
                _compService.CreateAboutUsfaq(aboutUsfaq);
                await _compService.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(aboutUsfaq);
        }

        // GET: Manage/AboutUsfaqs/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var aboutUsfaq = await _compService.GetAboutUsfaqByIdAsync((int)id);
            if (aboutUsfaq == null)
            {
                return NotFound();
            }
            return View(aboutUsfaq);
        }

        // POST: Manage/AboutUsfaqs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id,  AboutUsfaq aboutUsfaq)
        {
            if (id != aboutUsfaq.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if(aboutUsfaq.CreatedDate == null)
                    {
                        aboutUsfaq.CreatedDate = DateTime.Now;
                    }
                    _compService.UpateAboutUsfas(aboutUsfaq);
                    await _compService.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AboutUsfaqExists(aboutUsfaq.Id))
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
            return View(aboutUsfaq);
        }

        // GET: Manage/AboutUsfaqs/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var aboutUsfaq = await _compService.GetAboutUsfaqByIdAsync((int)id);
            if (aboutUsfaq == null)
            {
                return NotFound();
            }

            return View(aboutUsfaq);
        }

        // POST: Manage/AboutUsfaqs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var aboutUsfaq = await _compService.GetAboutUsfaqByIdAsync((int)id);
            _compService.RemoveAboutUsfaq(aboutUsfaq);
            await _compService.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AboutUsfaqExists(int id)
        {
            return _compService.ExistAboutUsfaq(id);
        }
    }
}
