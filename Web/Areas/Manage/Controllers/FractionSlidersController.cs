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
    [PermissionCheckerByPermissionName("fractionslider")]
    public class FractionSlidersController : Controller
    {
       
        private readonly ICompService _compService;

        public FractionSlidersController(ICompService compService)
        {
            _compService = compService;            
        }

        // GET: Manage/FractionSliders
        public async Task<IActionResult> Index()
        {
            return View(await _compService.GetFractionSlidersAsync());
        }
        public async Task<bool> ChangeStatus(int id, int status)
        {
            FractionSlider fractionSlider = await _compService.GetFractionByIdAsync(id);
            bool st = false;
            if (status == 1)
            {
                st = true;
            }
            fractionSlider.IsActive = st;
            _compService.UpdateFractionSlider(fractionSlider);
            await _compService.SaveChangesAsync();
            return st;

        }
        // GET: Manage/FractionSliders/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var fractionSlider = await _compService.GetFractionByIdAsync((int)id);
            if (fractionSlider == null)
            {
                return NotFound();
            }

            return View(fractionSlider);
        }

        // GET: Manage/FractionSliders/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Manage/FractionSliders/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(FractionSlider fractionSlider)
        {
            if (ModelState.IsValid)
            {
                fractionSlider.CreatedDate = DateTime.Now;
                _compService.CreateFractionSlider(fractionSlider);                
                await _compService.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(fractionSlider);
        }

        // GET: Manage/FractionSliders/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var fractionSlider = await _compService.GetFractionByIdAsync((int)id);
            if (fractionSlider == null)
            {
                return NotFound();
            }
            return View(fractionSlider);
        }

        // POST: Manage/FractionSliders/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id,FractionSlider fractionSlider)
        {
            if (id != fractionSlider.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _compService.UpdateFractionSlider(fractionSlider);
                    await _compService.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FractionSliderExists(fractionSlider.Id))
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
            return View(fractionSlider);
        }

        // GET: Manage/FractionSliders/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var fractionSlider = await _compService.GetFractionByIdAsync((int)id);
            if (fractionSlider == null)
            {
                return NotFound();
            }

            return View(fractionSlider);
        }

        // POST: Manage/FractionSliders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var fractionSlider = await _compService.GetFractionByIdAsync(id);
            _compService.RemoveFractionSlider(fractionSlider);
            await _compService.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool FractionSliderExists(int id)
        {
            var fractions= _compService.GetFractionSlidersAsync().Result;
            return fractions.Any(x => x.Id == id);
        }
    }
}
