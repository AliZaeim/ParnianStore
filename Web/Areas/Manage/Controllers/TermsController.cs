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

namespace Web.Areas.Manage.Controllers
{
    [Area("Manage")]
    [Authorize]
    [PermissionCheckerByPermissionName("terms")]
    public class TermsController : Controller
    {
        
        private readonly ICompService _compService;

        public TermsController(ICompService compService)
        {
            _compService = compService;
           
        }

        // GET: Manage/Terms
        public async Task<IActionResult> Index()
        {
            return View(await _compService.GetTermsAsync());
        }

        // GET: Manage/Terms/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var terms = await _compService.GetTermsByIdAsync((int)id);
            if (terms == null)
            {
                return NotFound();
            }

            return View(terms);
        }

        // GET: Manage/Terms/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Manage/Terms/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Terms terms)
        {
            if (ModelState.IsValid)
            {
                terms.CreatedDate = DateTime.Now;
                _compService.CreateTerm(terms);
                await _compService.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(terms);
        }

        // GET: Manage/Terms/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var terms = await _compService.GetTermsByIdAsync((int)id);
            if (terms == null)
            {
                return NotFound();
            }
            return View(terms);
        }

        // POST: Manage/Terms/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Terms terms)
        {
            if (id != terms.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    terms.CreatedDate = DateTime.Now;
                    _compService.UpdateTerm(terms);
                    await _compService.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TermsExists(terms.Id))
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
            return View(terms);
        }

        // GET: Manage/Terms/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var terms = await _compService.GetTermsByIdAsync((int)id);
            if (terms == null)
            {
                return NotFound();
            }

            return View(terms);
        }

        // POST: Manage/Terms/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            
            await _compService.RemoveTermsByIdAsync(id);
            await _compService.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TermsExists(int id)
        {
            return _compService.ExistTerms(id);
        }
    }
}
