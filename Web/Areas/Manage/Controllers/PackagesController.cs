using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DataLayer.Context;
using DataLayer.Entities.Store;
using Microsoft.AspNetCore.Http;
using Core.Services.Interfaces;
using Core.DTOs.Admin;
using Core.Utility;
using Microsoft.AspNetCore.Authorization;
using Core.Security;

namespace Web.Areas.Manage.Controllers
{
    [Area("Manage")]
    [Authorize]
    [PermissionCheckerByPermissionName("packages")]
    public class PackagesController : Controller
    {

        private readonly IProductService _productService;

        public PackagesController(IProductService productService)
        {
            _productService = productService;
        }

        // GET: Manage/Packages
        public async Task<IActionResult> Index()
        {
            return View(await _productService.GetPackagesAsync());
        }
        public async Task<IActionResult> ShowPackageComments(int packId)
        {
            Package package = await _productService.GetPackageByIdAsync(packId);
            if (package == null)
            {
                return NotFound();
            }

            return View(package.PackageComments);
        }
        public async Task<bool> ChangePackageStatus(int id, int status)
        {
            Package package = await _productService.GetPackageByIdAsync(id);
            if (package == null)
                return false;
            bool st = false;
            if (status == 1)
            {
                st = true;
            }
            package.IsActive = st;
            _productService.UpdatePackage(package);
            await _productService.SaveChangesAsync();
            return st;
        }
        public async Task<bool> ChangeCommentStatus(int id, int status)
        {
            PackageComment packageComment = await _productService.GetPackageCommentByIdAsync(id);
            if (packageComment == null)
                return false;
            bool st = false;
            if (status == 1)
            {
                st = true;
            }
            packageComment.IsActive = st;
            _productService.EditPackageComment(packageComment);
            await _productService.SaveChangesAsync();
            return st;

        }
        // GET: Manage/Packages/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var package = await _productService.GetPackageByIdAsync((int)id);
            if (package == null)
            {
                return NotFound();
            }

            return View(package);
        }

        // GET: Manage/Packages/Create
        public async Task<IActionResult> Create()
        {
            var products = await _productService.GetProductsAsync();
            //ViewData["Products"] = new SelectList(await _productService.GetProductsAsync(), "ProductId", "ProductName");
            ViewData["Products"] = products.ToList();
            ViewData["SelectedPIds"] = null;
            ViewData["PackageGroups"] = new SelectList(await _productService.GetPackageGroupsAsync(), "PgId", "PgTitle");
            return View();
        }

        // POST: Manage/Packages/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Package package, List<int> SelectedProducts, IFormFile PkImage, IFormFile PkSliderImage)
        {

            var products = await _productService.GetProductsAsync();
            ViewData["Products"] = products.ToList();
            ViewData["SelectedPIds"] = SelectedProducts;
            ViewData["PackageGroups"] = new SelectList(await _productService.GetPackageGroupsAsync(), "PgId", "PgTitle");
            if (ModelState.IsValid)
            {
                if (PkSliderImage == null)
                {
                    ModelState.AddModelError("PkSliderImage", "تصویر اسلایدی انتخاب نشده است !");
                    return View(package);
                }
                if (PkImage == null)
                {
                    ModelState.AddModelError("PkImage", "تصویر انتخاب نشده است !");
                    return View(package);
                }
                string[] ex = { ".jpg", ".jpeg", ".gif", ".png", ".svg", ".webp", ".avif" };
                FileValidation sliderValidation = await PkSliderImage.UploadedImageValidation(50, ex);
                if (!sliderValidation.IsValid)
                {
                    ModelState.AddModelError("PkSliderImage", sliderValidation.Message);
                    return View(package);
                }
                FileValidation fileValidation = await PkImage.UploadedImageValidation(50, ex);
                if (!fileValidation.IsValid)
                {
                    ModelState.AddModelError("PkImage", fileValidation.Message);
                    return View(package);
                }
                package.CreatedDate = DateTime.Now;
                package.PkSliderImage = PkSliderImage.SaveUploadedImage("wwwroot/images/packages", true);
                package.PkImage = PkImage.SaveUploadedImage("wwwroot/images/packages", true);
                await _productService.CreatePackageAsync(package, SelectedProducts);
                await _productService.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            else
            {
                var errors = ModelState.Select(x => x.Value.Errors)
                           .Where(y => y.Count > 0)
                           .ToList();
            }



            return View(package);
        }

        // GET: Manage/Packages/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var package = await _productService.GetPackageByIdAsync((int)id);
            if (package == null)
            {
                return NotFound();
            }
            var products = await _productService.GetProductsAsync();
            ViewData["Products"] = products.ToList();
            ViewData["SelectedPIds"] = package.PackageProducts.Select(x => x.ProductId).ToList();
            ViewData["PackageGroups"] = new SelectList(await _productService.GetPackageGroupsAsync(), "PgId", "PgTitle", package.PgId);
            return View(package);
        }

        // POST: Manage/Packages/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Package package, List<int> SelectedProducts, IFormFile PkImage, IFormFile PkSliderImage)
        {
            if (id != package.PkId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    string[] ex = { ".jpg", ".jpeg", ".gif", ".png", ".svg", ".webp", ".avif" };
                    if (PkSliderImage != null)
                    {
                        FileValidation sliderValidation = await PkSliderImage.UploadedImageValidation(50, ex);
                        if (!sliderValidation.IsValid)
                        {
                            ModelState.AddModelError("PkSliderImage", sliderValidation.Message);
                            return View(package);
                        }
                        package.PkSliderImage = PkSliderImage.SaveUploadedImage("wwwroot/images/packages", true);
                    }
                    if (PkImage != null)
                    {
                        FileValidation fileValidation = await PkImage.UploadedImageValidation(50, ex);
                        if (!fileValidation.IsValid)
                        {
                            ModelState.AddModelError("PkImage", fileValidation.Message);
                            return View(package);
                        }

                        package.PkImage = PkImage.SaveUploadedImage("wwwroot/images/packages", true);
                    }

                    _productService.UpdatePackageProducts(id, SelectedProducts);
                    _productService.UpdatePackage(package);
                    await _productService.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {

                    if (!PackageExists(package.PkId))
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
            else
            {
                var errors = ModelState.Select(x => x.Value.Errors)
                           .Where(y => y.Count > 0)
                           .ToList();
            }
            var products = await _productService.GetProductsAsync();
            ViewData["Products"] = products.ToList();
            ViewData["SelectedPIds"] = SelectedProducts;
            ViewData["PackageGroups"] = new SelectList(await _productService.GetPackageGroupsAsync(), "PgId", "PgTitle", package.PgId);
            return View(package);
        }

        // GET: Manage/Packages/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var package = await _productService.GetPackageByIdAsync((int)id);
            if (package == null)
            {
                return NotFound();
            }

            return View(package);
        }

        // POST: Manage/Packages/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var package = await _productService.GetPackageByIdAsync(id);
            _productService.RemovePackage(package);
            await _productService.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PackageExists(int id)
        {
            return _productService.ExistPackage(id);
        }
    }
}
