using Core.Prodocers;
using Core.Security;
using Core.Services.Interfaces;
using DataLayer.Entities.Store;
using DataLayer.Entities.Supplementary;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Web.Areas.Manage.Controllers
{
    [Area("Manage")]
    [Authorize]
    [PermissionCheckerByPermissionName("productgroups")]
    public class ProductGroupsController : Controller
    {
       
        private readonly IProductService _productService;

        public ProductGroupsController(IProductService productService)
        {
            _productService = productService;
        }

        // GET: Manage/ProductGroups
        public async Task<IActionResult> Index()
        {
            return View(await _productService.GetProductGroupsAsync());
        }

        // GET: Manage/ProductGroups/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var productGroup = await _productService.GetProductGroupByIdAsync((int)id);
            if (productGroup == null)
            {
                return NotFound();
            }

            return View(productGroup);
        }

        // GET: Manage/ProductGroups/Create
        public async  Task<IActionResult> Create(int? PId)
        {
            var groups =await _productService.GetProductGroupsAsync();
           
            
            if(PId == null)
            {
                ViewData["ProductGroupId"] = new SelectList(groups, "ProductGroupId", "ProductGroupTitle");
                ViewData["ProductGroups"] = groups;
            }
            else
            {
                groups = groups.Where(w => w.ParentId == null).ToList();
                ViewData["ProductGroupId"] = new SelectList(groups, "ProductGroupId", "ProductGroupTitle",(int)PId);
                ViewData["ProductGroups"] = groups;
            }
            var banners = await _productService.GetBannersAsync();
            banners = banners.Where(w => w.IsActive).ToList();
            ViewData["Banners"] = banners.ToList();
            ProductGroup productGroup = new()
            {
                ParentId = PId
            };
            return View(productGroup);
        }

        // POST: Manage/ProductGroups/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProductGroup productGroup, IFormFile ProductGroupImage)
        {
            ViewData["ProductGroupId"] = new SelectList(await _productService.GetProductGroupsAsync(), "ProductGroupId", "ProductGroupTitle",productGroup.ProductGroupId);
            var banners = await _productService.GetBannersAsync();
            banners = banners.Where(w => w.IsActive).ToList();
            ViewData["Banners"] = banners.ToList();
            if (!ModelState.IsValid)
            {
                return View(productGroup);
            }
            if (await _productService.GetProductGroupByEnTitleAsync(productGroup.ProductEnGroupTitle) != null)
            {
                ModelState.AddModelError("ProductEnGroupTitle", "نام تکراری است !");
                return View(productGroup);
            }
            if (await _productService.ExistProductGroupCodeAsync(productGroup.ProductGroupMark))
            {
                ModelState.AddModelError("ProductGroupMark", "کد شناسه گروه تکراری است !");
                return View(productGroup);
            }
            string ImagePath = string.Empty;
            string ImageName = string.Empty;
            if (ProductGroupImage != null)
            {

                if (ProductGroupImage.Length > .05 * 1024 * 1024)
                {
                    ModelState.AddModelError("ProductGroupImage", "حجم تصویر از 50 کیلو بایت نمی تواند بیشتر باشد ");
                    return View(productGroup);
                }
                ImageName = ProductGroupImage.FileName;
                ImagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/productGroups", ImageName);
                using var stream = new FileStream(ImagePath, FileMode.Create);
                ProductGroupImage.CopyTo(stream);

            }
           
            productGroup.ProductGroupImage = ImageName;
            _productService.CreateProductGroup(productGroup);
            await _productService.SaveChangesAsync();

            return RedirectToAction(nameof(Index));


        }

        // GET: Manage/ProductGroups/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var productGroup = await _productService.GetProductGroupByIdAsync((int)id);
            if (productGroup == null)
            {
                return NotFound();
            }
            var grList = await _productService.GetProductGroupsAsync();
            grList.Insert(0, new ProductGroup() { ProductGroupTitle = "بدون والد | در صورت نیاز انتخاب کنید" });
            ViewData["ProductGroupId"] = new SelectList(grList, "ProductGroupId", "FullPro", productGroup.ParentId);
            var banners = await _productService.GetBannersAsync();            
            banners = banners.Where(w => w.IsActive).ToList();
            ViewData["Banners"] = banners.ToList();
            
            return View(productGroup);
        }

        // POST: Manage/ProductGroups/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id,  ProductGroup productGroup, IFormFile ProductGroupImage)
        {
            if (id != productGroup.ProductGroupId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (await _productService.GetProductGroupByEnTitleAsync(productGroup.ProductEnGroupTitle) != null)
                    {
                        ProductGroup productGroup1 = await _productService.GetProductGroupByEnTitleAsync(productGroup.ProductEnGroupTitle);
                        if(productGroup1.ProductGroupId !=productGroup.ProductGroupId)
                        {
                            ModelState.AddModelError("ProductEnGroupTitle", "نام تکراری است !");
                            return View(productGroup);
                        }
                        _productService.DetachProductGroup(productGroup1);
                       
                    }
                    if (await _productService.ExistProductGroupCodeAsync(productGroup.ProductGroupMark))
                    {
                        ProductGroup productGroup2 = await _productService.GetProductGroupByMarkAsync(productGroup.ProductGroupMark);
                        if(productGroup2.ProductGroupId != productGroup.ProductGroupId)
                        {
                            ModelState.AddModelError("ProductGroupMark", "کد شناسه گروه تکراری است !");
                            return View(productGroup);
                        }
                        _productService.DetachProductGroup(productGroup2);
                    }
                    string ImagePath = string.Empty;
                    if (ProductGroupImage != null)
                    {

                        if (ProductGroupImage.Length > .05 * 1024 * 1024)
                        {
                            ModelState.AddModelError("ProductGroupImage", "حجم تصویر از 50 کیلو بایت نمی تواند بیشتر باشد ");
                            return View(productGroup);
                        }
                        string ImageName = ProductGroupImage.FileName;
                        ImagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/productGroups", ImageName);
                        using (var stream = new FileStream(ImagePath, FileMode.Create))
                        {
                            ProductGroupImage.CopyTo(stream);
                        }
                        productGroup.ProductGroupImage = ImageName;
                    }
                    if(productGroup.ParentId == 0)
                    {
                        productGroup.ParentId = null;
                    }
                    _productService.EditProductGroup(productGroup);
                    await _productService.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (! await ProductGroupExists(productGroup.ProductGroupId))
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
            var grList = await _productService.GetProductGroupsAsync();
            grList.Insert(0, new ProductGroup() { ProductGroupTitle = "گروه محصول را انتخاب کنید" });
            ViewData["ProductGroupId"] = new SelectList(grList, "ProductGroupId", "FullPro", productGroup.ParentId);

            var banners = await _productService.GetBannersAsync();
            banners = banners.Where(w => w.IsActive).ToList();
            banners.Insert(0, new Banner { Comment = "بنر مورد نیاز گروه را انتخاب کنید" });
            ViewData["BannerId"] = new SelectList(await _productService.GetBannersAsync(), "Id", "Comment", productGroup.BannerId);
            return View(productGroup);
        }

        // GET: Manage/ProductGroups/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var productGroup = await _productService.GetProductGroupByIdAsync((int)id);
            if (productGroup == null)
            {
                return NotFound();
            }

            return View(productGroup);
        }

        // POST: Manage/ProductGroups/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            
            await _productService.RemoveProductGroupAsync(id);
            await _productService.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private async Task<bool> ProductGroupExists(int id)
        {
            return await _productService.ExistProductGroupAsync(id);
        }
    }
}
