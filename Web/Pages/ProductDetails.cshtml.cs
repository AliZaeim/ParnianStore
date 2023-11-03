using Core.DTOs.General;
using Core.Services.Interfaces;
using Core.Utility;
using DataLayer.Entities.Store;
using DataLayer.Entities.Supplementary;
using DataLayer.Entities.User;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Web.Pages
{
    public class ProductDetailsModel : PageModel
    {
        private readonly IProductService _productService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IStoreService _storeService;
        public ProductDetailsModel(IProductService productService, IStoreService storeService, IHttpContextAccessor httpContextAccessor)
        {
            _productService = productService;
            _httpContextAccessor = httpContextAccessor;
            _storeService = storeService;
        }

        public ProductDatailsVM ProductDatailsVM { get; set; } = new ProductDatailsVM();
        public DiscountOptionVM DiscountOptionVM { get; set; }
        public InitialInfo InitialInfo { get; set; }
        public int Inventory { get; set; }
        public List<Package> RelatedPackages { get; set; }
        
        public List<Dictionary<string, string>> PrNames { get; set; } = new();
        public async Task<IActionResult> OnGetAsync(string name)
        {
            CookieExtensions.SetHttpContextAccessor(_httpContextAccessor);

            name = name.Modify_Product_Name();
            name = name.Replace(" ", "-");
            Product product = await _productService.GetProductByEnName(name);

            if (product == null)
            {
                return NotFound();
            }

            RelatedPackages = await _productService.GetPackagesbyProductAsyncByName(product.ProductEnName.Replace(" ","-"));



            //return RedirectToPage("404");

            InitialInfo = await _productService.GetInitialInfoAsync();
            DiscountOptionVM = await _productService.HasProductDiscountAsync(product.ProductCode);
            Inventory = await _productService.GetProduct_Inventory_InStoreAsync(product.ProductCode);
            ProductDatailsVM.ProductId = product.ProductId;
            ProductDatailsVM.Product = product;
            ProductDatailsVM.ProductName = product.ProductName;

            List<Product> Gproducts = await _productService.GetAllProductofGroupAsync((int)product.ProductGroupId);
            Gproducts = Gproducts.Where(w => w.ProductId != product.ProductId).ToList();
            ProductDatailsVM.GroupProducts = Gproducts;
            if (User.Identity.IsAuthenticated)
            {
                User user = await _productService.GetUserByUserNameAsync(User.Identity.Name);
                if (user != null)
                {
                    ProductDatailsVM.CountInCart = _productService.GetProductCountinCart(product.ProductId, User.Identity.Name);
                    ProductDatailsVM.Name = user.Name;
                    ProductDatailsVM.Family = user.Family;
                    ProductDatailsVM.Cellphone = user.Cellphone;
                    if (ProductDatailsVM.CountInCart != 0)
                    {
                        ProductDatailsVM.CartHasProduct = true;
                    }
                }
            }
            else
            {
                Cart Cookiecart = null;
                if (CookieExtensions.ExistCookie("cartid"))
                {
                    string cid = CookieExtensions.ReadCookie("cartid");
                    Cookiecart = await _storeService.GetUserCartByCookieAsync(cid);
                    if (Cookiecart != null)
                    {
                        if (Cookiecart.IsActive && !Cookiecart.CheckOut)
                        {
                            int pcount = 0;
                            if (Cookiecart.CartItems.Any(a => a.ProductId == product.ProductId))
                            {
                                pcount = Cookiecart.CartItems.SingleOrDefault(x => x.ProductId == product.ProductId).Quantity;
                                ProductDatailsVM.CartHasProduct = true;
                            }
                            ProductDatailsVM.CountInCart = pcount;
                        }

                    }
                }
            }
            product.Views += 1;
            _productService.EditProduct(product);
            await _productService.SaveChangesAsync();
            return Page();
        }

        public async Task<IActionResult> OnPost(ProductDatailsVM productDatailsVM)
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            ProductComment productComment = new()
            {
                CreatedDate = DateTime.Now,
                Name = productDatailsVM.Name,
                Family = productDatailsVM.Family,
                ProductId = productDatailsVM.ProductId,
                Cellphone = productDatailsVM.Cellphone,
                Comment = productDatailsVM.Comment
            };
            _productService.CreateProductComment(productComment);
            await _productService.SaveChangesAsync();
            Product product = await _productService.GetProductByIdAsync(productDatailsVM.ProductId);
            TempData["saved"] = true;
            return RedirectToPage("/ProductDetails", new { name = product.ProductEnName.Replace(" ", "-") });
        }
    }
}
