using Core.DTOs.Admin;
using Core.Security;
using Core.Services.Interfaces;
using DataLayer.Entities.Store;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Web.Areas.Manage.Controllers
{
    [Area("Manage")]
    [Authorize]
    [PermissionCheckerByPermissionName("products")]
    public class ProductIngredientsController : Controller
    {
        private readonly IProductService _productService;
        public ProductIngredientsController(IProductService productService)
        {
            _productService = productService;
        }
        public async Task<IActionResult> Index(int? pId)
        {
            if(pId == null)
            {
                return NotFound();
            }
            List<ProductIngredient> SelectedProductIngredients = await _productService.GetProductIngredientsByProductIdAsync((int)pId);
            Product product = await _productService.GetProductByIdAsync((int)pId);
            ProductIngredientVM productIngredientVM = new()
            {
                Ingredients = await _productService.GetIngredientsAsync(),
                Product = product,
                SelectedIngredients = SelectedProductIngredients.Select(x => x.IngredientId).ToList(),
                Title = "اجزای تشکیل دهنده" + " " + product.ProductName,
                ProductId = (int)pId
            };
            return View(productIngredientVM);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(List<int> SelectedIngredients, int ProductId)
        {
            bool Res = await _productService.UpdateProductIngredients(ProductId, SelectedIngredients);
            if(Res == true)
            {
                await _productService.SaveChangesAsync();
            }
            Product product = await _productService.GetProductByIdAsync(ProductId);
            //List<RolePermission> selectedRolePermisisons = await _userService.GetRolePermissionByRoleIdAsync(RoleId);

            List<ProductIngredient> productIngredients = await _productService.GetProductIngredientsByProductIdAsync(ProductId);
            ProductIngredientVM productIngredientVM = new()
            {
                Ingredients = await _productService.GetIngredientsAsync(),
                Product = product,
                SelectedIngredients = productIngredients.Select(x => x.IngredientId).ToList(),
                Title = "اجزای تشکیل دهنده" + " " + product.ProductName,
                ProductId = ProductId
            };
            return View(productIngredientVM);
        }
    }
}
