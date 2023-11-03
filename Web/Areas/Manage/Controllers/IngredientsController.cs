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
using Microsoft.AspNetCore.Http;
using Core.DTOs.Admin;
using Core.Utility;
using Core.Security;
using Microsoft.AspNetCore.Authorization;

namespace Web.Areas.Manage.Controllers
{
    [Area("Manage")]
    [Authorize]
    [PermissionCheckerByPermissionName("ingredients")]
    public class IngredientsController : Controller
    {
        
        private readonly IProductService _productService;

        public IngredientsController(IProductService productService)
        {            
            _productService = productService;
        }

        // GET: Manage/Ingredients
        public async Task<IActionResult> Index()
        {
            return View(await _productService.GetIngredientsAsync());
        }

        // GET: Manage/Ingredients/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ingredient = await _productService.GetIngredientByIdAsync((int)id);
            if (ingredient == null)
            {
                return NotFound();
            }

            return View(ingredient);
        }

        // GET: Manage/Ingredients/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Manage/Ingredients/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Ingredient ingredient, IFormFile Image)
        {
            if (ModelState.IsValid)
            {
                if(Image == null)
                {
                    ModelState.AddModelError("Image", "تصویر انتخاب نشده است !");
                    return View(ingredient);
                }
                string[] ex = { ".jpg", ".jpeg", ".gif", ".png", ".svg", ".webp", ".avif" };
                FileValidation fileValidation = await Image.UploadedImageValidation(200, 200, 30, ex);
                if(!fileValidation.IsValid)
                {
                    ModelState.AddModelError("Image", fileValidation.Message);
                    return View(ingredient);
                }
                ingredient.Image= Image.SaveUploadedImage("wwwroot/images/productIngredients", true);
                _productService.CreateIngredient(ingredient);
                await _productService.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(ingredient);
        }

        // GET: Manage/Ingredients/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ingredient = await _productService.GetIngredientByIdAsync((int)id);
            if (ingredient == null)
            {
                return NotFound();
            }
            return View(ingredient);
        }

        // POST: Manage/Ingredients/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id,  Ingredient ingredient, IFormFile Image)
        {
            if (id != ingredient.IngredientId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if(Image != null)
                    {
                        string[] ex = { ".jpg", ".jpeg", ".gif", ".png", ".svg", ".webp", ".avif" };
                        FileValidation fileValidation = await Image.UploadedImageValidation(200, 200, 30, ex);
                        if (!fileValidation.IsValid)
                        {
                            ModelState.AddModelError("Image", fileValidation.Message);
                            return View(ingredient);
                        }
                        ingredient.Image = Image.SaveUploadedImage("wwwroot/images/productIngredients", true);
                    }
                    _productService.Updateungredient(ingredient);
                    await _productService.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!IngredientExists(ingredient.IngredientId))
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
            return View(ingredient);
        }

        // GET: Manage/Ingredients/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ingredient = await _productService.GetIngredientByIdAsync((int)id);
            if (ingredient == null)
            {
                return NotFound();
            }

            return View(ingredient);
        }

        // POST: Manage/Ingredients/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var ingredient = await _productService.GetIngredientByIdAsync(id);
            _productService.DeleteIngredient(ingredient);
            await _productService.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool IngredientExists(int id)
        {
            return _productService.ExitIngredientById(id);
        }
    }
}
