using Core.DTOs.Admin;
using Core.Security;
using Core.Services.Interfaces;
using Core.Utility;
using DataLayer.Entities.Store;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Web.Areas.Manage.Controllers
{
    [Area("Manage")]
    [Authorize]
    [PermissionCheckerByPermissionName("products")]
    public class ProductsController : Controller
    {

        private readonly IProductService _productService;

        public ProductsController(IProductService productService)
        {
            _productService = productService;

        }

        // GET: Manage/ProductS
        public async Task<IActionResult> Index()
        {
            return View(await _productService.GetProductsAsync());
        }
        public async Task<IActionResult> ShowProductComments(int productId)
        {
            Product product = await _productService.GetProductByIdAsync(productId);
            if (product == null)
            {
                return NotFound();
            }

            return View(product.ProductComments);
        }
        public async Task<bool> ChangeProductStatus(int id, int status)
        {
            Product product = await _productService.GetProductByIdAsync(id);
            if (product == null)
                return false;
            bool st = false;
            if (status == 1)
            {
                st = true;
            }
            product.IsActive = st;
            _productService.EditProduct(product);
            await _productService.SaveChangesAsync();
            return st;
        }
        public async Task<bool> ChangeCommentStatus(int id, int status)
        {
            ProductComment productComment = await _productService.GetProductCommentByIdAsync(id);
            if (productComment == null)
                return false;
            bool st = false;
            if (status == 1)
            {
                st = true;
            }
            productComment.IsActive = st;
            _productService.EditProductComment(productComment);
            await _productService.SaveChangesAsync();
            return st;

        }

        // GET: Manage/ProductS/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _productService.GetProductByIdAsync((int)id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // GET: Manage/Product/Create
        //image size in product images list page : 720*960
        //image sizes in product details page : main : 1200*1600 and beside : 320*427
        //thumb feathured image sizes in blog 112*150
        public async Task<IActionResult> Create()
        {
            ViewData["ProductGroupId"] = new SelectList(await _productService.GetProductGroupsAsync(), "ProductGroupId", "FullPro");

            ViewData["Ingredients"] = await _productService.GetIngredientsAsync();
            ViewData["SelectedIngs"] = null;
            return View();
        }

        // POST: Manage/ProductS/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Product product, List<int> SelectedIngredients, IFormFile ProductListImage, IFormFile ProductImage)
        {
            ViewData["ProductGroupId"] = new SelectList(await _productService.GetProductGroupsAsync(), "ProductGroupId", "FullPro", product.ProductGroupId);

            ViewData["Ingredients"] = await _productService.GetIngredientsAsync();
            ViewData["SelectedIngs"] = SelectedIngredients;
            if (ModelState.IsValid)
            {
                Product productwc = await _productService.GetProductWithCodeAsync(product.ProductCode);
                if (productwc != null)
                {
                    ModelState.AddModelError("ProductCode", "کد محصول تکراری است !");
                    return View(product);
                }
                string[] ex = { ".jpg", ".jpeg", ".gif", ".png", ".svg", ".webp", ".avif" };
                FileValidation ProductListImageValidation = await ProductListImage.UploadedImageValidation(240, 300, 20, ex);
                if (!ProductListImageValidation.IsValid)
                {
                    ModelState.AddModelError("ProductListImage", ProductListImageValidation.Message);
                    return View(product);
                }
                FileValidation ProductImageValidation = await ProductImage.UploadedImageValidation(565, 690, 30, ex);
                if (!ProductImageValidation.IsValid)
                {
                    ModelState.AddModelError("ProductImage", ProductImageValidation.Message);
                    return View(product);
                }



                product.ProductListImage = ProductListImage.SaveUploadedImage("wwwroot/images/products", true);
                product.ProductImage = ProductImage.SaveUploadedImage("wwwroot/images/products", true);
                product.ProductBlogThumb = ProductImage.SaveImageWithNewDimension(112, 137, "wwwroot/images/products/thumbs", Path.GetFileNameWithoutExtension(ProductImage.FileName) + "-thumb");

                product.CreatedDate = DateTime.Now;
               
                _productService.CreateProduct(product);
                await _productService.SaveChangesAsync();
                bool Res = await _productService.UpdateProductIngredients(product.ProductId, SelectedIngredients);
                if (Res == true)
                {
                    await _productService.SaveChangesAsync();
                }
                return RedirectToAction("Index");
            }


            return View(product);
        }

        public JsonResult GetProductCode(int pgId)
        {
            string code = _productService.GetProductCodeAsync(pgId).Result;
            return Json(code);
        }
        // GET: Manage/ProductS/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _productService.GetProductByIdAsync((int)id);
            if (product == null)
            {
                return NotFound();
            }
            ViewData["ProductGroupId"] = new SelectList(await _productService.GetProductGroupsAsync(), "ProductGroupId", "ProductGroupTitle", product.ProductGroupId);

            ViewData["Ingredients"] = await _productService.GetIngredientsAsync();
            ViewData["SelectedIngs"] = product.ProductIngredients.Select(x => x.IngredientId).ToList();


            return View(product);
        }

        // POST: Manage/ProductS/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Product product, List<int> SelectedIngredients, IFormFile ProductListImage, IFormFile ProductImage)
        {
            try
            {
                if (id != product.ProductId)
                {
                    return NotFound();
                }
                ViewData["ProductGroupId"] = new SelectList(await _productService.GetProductGroupsAsync(), "ProductGroupId", "ProductGroupTitle", product.ProductGroupId);
                ViewData["Ingredients"] = await _productService.GetIngredientsAsync();
                ViewData["SelectedIngs"] = SelectedIngredients;

                if (ModelState.IsValid)
                {
                    bool det = false;
                    try
                    {
                        if (await _productService.ExistProductByCodeAsync(product.ProductCode))
                        {
                            Product exProduct = await _productService.GetProductWithCodeAsync(product.ProductCode);
                            if (exProduct.ProductId != product.ProductId)
                            {
                                ModelState.AddModelError("ProductCode", "کد محصول تکراری است !");
                                return View(product);
                            }
                            det = _productService.DetachProduct(exProduct);

                        }
                        if (SelectedIngredients.Count == 0)
                        {
                            ModelState.AddModelError("SelectedIngredients", "هیچ جزء سازنده ای انتخاب نشده است !");
                            return View(product);
                        }

                        string[] ex = { ".jpg", ".jpeg", ".gif", ".png", ".svg", ".webp", ".avif" };
                        FileValidation ProductListImageValidation = await ProductListImage.UploadedImageValidation(240, 300, 20, ex);
                        FileValidation ProductImageValidation = await ProductImage.UploadedImageValidation(565, 690, 30, ex);

                        if (ProductListImage != null)
                        {
                            if (!ProductListImageValidation.IsValid)
                            {
                                ModelState.AddModelError("ProductListImage", ProductListImageValidation.Message);
                                return View(product);
                            }
                        }
                        if (ProductImage != null)
                        {
                            if (!ProductImageValidation.IsValid)
                            {
                                ModelState.AddModelError("ProductImage", ProductImageValidation.Message);
                                return View(product);
                            }
                        }
                        if (ProductListImage != null)
                        {
                            product.ProductListImage = ProductListImage.SaveUploadedImage("wwwroot/images/products", true);
                        }
                        if (ProductImage != null)
                        {
                            string prevProductImage = "image";// product.ProductImage;
                            string prevthumb = product.ProductBlogThumb;
                            if (!string.IsNullOrEmpty(prevProductImage))
                            {
                                string path1 = @"wwwroot/images/products" + prevProductImage;
                                if (!System.IO.File.Exists(path1))
                                {
                                    System.IO.File.Delete(path1);
                                }
                                string path2 = @"wwwroot/images/products/thumbs" + prevthumb;
                                if (!System.IO.File.Exists(path2))
                                {
                                    System.IO.File.Delete(path2);
                                }
                            }
                            product.ProductImage = ProductImage.SaveUploadedImage("wwwroot/images/products", true);
                            product.ProductBlogThumb = ProductImage.SaveImageWithNewDimension(112, 137, "wwwroot/images/products/thumbs", Path.GetFileNameWithoutExtension(ProductImage.FileName) + "-thumb");

                        }

                        _productService.EditProduct(product);
                        bool Res = await _productService.UpdateProductIngredients(product.ProductId, SelectedIngredients);
                        await _productService.SaveChangesAsync();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!await ProductExistsAsync(product.ProductId))
                        {
                            return NotFound();
                        }
                        else
                        {
                            throw;
                        }
                    }

                    return RedirectToAction("Index");
                }


                return View(product);
            }
            catch (Exception ex)
            {

                string m = ex.Message;
                return NotFound();
            }
        }

        // GET: Manage/ProductS/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _productService.GetProductByIdAsync((int)id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // POST: Manage/Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var product = await _productService.GetProductByIdAsync(id);
            product.IsDeleted = true;
            _productService.EditProduct(product);
            await _productService.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        private async Task<bool> ProductExistsAsync(int id)
        {
            return await _productService.ExistProductAsync(id);
        }
    }
}
