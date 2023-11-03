using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DataLayer.Context;
using DataLayer.Entities.Store;
using Core.Services.Interfaces;

namespace Web.Areas.Manage.Controllers
{
    [Area("Manage")]
    public class PackageGroupsController : Controller
    {
        
        private readonly IProductService _productService;
        public PackageGroupsController(IProductService productService)
        {            
            _productService = productService;
        }

        // GET: Manage/PackageGroups
        public async Task<IActionResult> Index()
        {            
            return View(await _productService.GetPackageGroupsAsync());
        }

        // GET: Manage/PackageGroups/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var packageGroup = await _productService.GetPackageGroupByIdAsync((int)id);
            if (packageGroup == null)
            {
                return NotFound();
            }

            return View(packageGroup);
        }

        // GET: Manage/PackageGroups/Create
        public async Task<IActionResult> Create()
        {
            ViewData["banners"] = await _productService.GetBannersAsync();
            ViewData["groups"] = await _productService.GetPackageGroupsAsync();            
            return View();
        }

        // POST: Manage/PackageGroups/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create( PackageGroup packageGroup)
        {
            if (ModelState.IsValid)
            {
                _productService.CreatePackageGroup(packageGroup);
                await _productService.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["banners"] = await _productService.GetBannersAsync();
            ViewData["groups"] = await _productService.GetPackageGroupsAsync();
            
            return View(packageGroup);
        }

        // GET: Manage/PackageGroups/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var packageGroup = await _productService.GetPackageGroupByIdAsync((int)id);
            if (packageGroup == null)
            {
                return NotFound();
            }
            ViewData["banners"] = await _productService.GetBannersAsync();
            ViewData["groups"] = await _productService.GetPackageGroupsAsync();
            return View(packageGroup);
        }

        // POST: Manage/PackageGroups/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, PackageGroup packageGroup)
        {
            if (id != packageGroup.PgId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _productService.UpdatePackageGroup(packageGroup);
                    await _productService.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PackageGroupExists(packageGroup.PgId))
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
            ViewData["banners"] = await _productService.GetBannersAsync();
            ViewData["groups"] = await _productService.GetPackageGroupsAsync();
            return View(packageGroup);
        }

        // GET: Manage/PackageGroups/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var packageGroup = await _productService.GetPackageGroupByIdAsync((int)id);
            if (packageGroup == null)
            {
                return NotFound();
            }

            return View(packageGroup);
        }

        // POST: Manage/PackageGroups/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var packageGroup = await _productService.GetPackageGroupByIdAsync(id);
            _productService.RemovePackageGroup(packageGroup);
            await _productService.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PackageGroupExists(int id)
        {
            return _productService.ExistPackageGroup(id);
        }
    }
}
