using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.DTOs.General;
using Core.Services.Interfaces;
using DataLayer.Entities.Store;
using DataLayer.Entities.Supplementary;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Web.Pages
{
    public class ProductsModel : PageModel
    {
        private readonly IProductService _productService;
        public ProductsModel(IProductService productService)
        {
            _productService = productService;
        }
        public ProductsVM ProductsVM { get; set; } = new ProductsVM();
        public SitePage SitePage { get; set; }
        public string FavIcon { get; set; } = "favicon.png";
        public bool ExistPackage { get; set; }
        public async Task<IActionResult> OnGetAsync(string gname, int? pageId)
        {
            SitePage sitePage = await _productService.GetSitePageByEnNameAsync("products");
            var initial = await _productService.GetInitialInfoAsync();
            if (initial != null)
            {
                if (!string.IsNullOrEmpty(initial.FavIcon))
                {
                    FavIcon = initial.FavIcon;
                }

            }
            int zpage = pageId.GetValueOrDefault(1);
            PageId = zpage;
           
            int recPerPage = 12;
            ProductsVM.RecPerPage = recPerPage;
            ProductsVM.ProductGroups = await _productService.GetProductGroupsAsync();
            List<Product> products = null;            
            ProductGroup productGroup = null;
            if (string.IsNullOrEmpty(gname))
            {
                products = await _productService.GetProductsAsync();
                if (products != null)
                {
                    products = products.Where(w => w.IsActive).ToList();
                }
            }
            else
            {
                productGroup = await _productService.GetProductGroupByEnTitleAsync(gname);
                ProductsVM.CurrentPoductGroupId = productGroup.ProductGroupId;
                ProductsVM.CurrentProductGroup = productGroup;
                if (productGroup != null)
                {
                    products = await _productService.GetAllProductofGroupAsync(productGroup.ProductGroupId);
                }
                if (products != null)
                {
                    products = products.Where(w => w.IsActive).ToList();
                }
            }
            ProductsVM.CurrentPage = zpage;

            if (products != null)
            {
                ProductsVM.TotalCount = products.Count;
                if (products.Count % recPerPage == 0)
                {
                    ProductsVM.TotalPages = products.Count / recPerPage;
                }
                else
                {
                    ProductsVM.TotalPages = (products.Count / recPerPage) + 1;
                }

                products = products.Skip((zpage - 1) * recPerPage).Take((zpage) * recPerPage).ToList();
            }
            ProductsVM.Products = products;
            ExistPackage = await _productService.ExisActivePackageAsync();
            return Page();

        }

        [BindProperty]
        public int? PageId { get; set; }
        [BindProperty]
        public string Grname { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            int zpage = PageId.GetValueOrDefault(1);
            int recPerPage = 12;
            ProductsVM.RecPerPage = recPerPage;
            ProductsVM.ProductGroups = await _productService.GetProductGroupsAsync();
            List<Product> products = await _productService.GetProductsAsync();
            ProductGroup productGroup = null;


            if (products != null)
            {
                products = products.Where(w => w.IsActive).ToList();
            }
            ProductsVM.TotalCount = products.Count;
            if (!string.IsNullOrEmpty(Grname))
            {
                productGroup = await _productService.GetProductGroupByEnTitleAsync(Grname);
                ProductsVM.CurrentPoductGroupId = productGroup.ProductGroupId;
                ProductsVM.CurrentProductGroup = productGroup;
                if (productGroup != null)
                {
                    products = await _productService.GetAllProductofGroupAsync(productGroup.ProductGroupId);
                }
                if (products != null)
                {
                    products = products.Where(w => w.IsActive).ToList();
                }
            }
            ProductsVM.CurrentPage = zpage;
            if (products != null)
            {
                
                if (products.Count % recPerPage == 0)
                {
                    ProductsVM.TotalPages = products.Count / recPerPage;
                }
                else
                {
                    ProductsVM.TotalPages = (products.Count / recPerPage) + 1;
                }
                products = products.Skip((zpage - 1) * recPerPage).Take(recPerPage).ToList();
            }
            ProductsVM.Products = products;
            return Page();
        }
    }
}
