using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Services.Interfaces;
using DataLayer.Entities.Store;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Web.Pages
{
    public class How_To_UseModel : PageModel
    {
        private readonly IProductService _productService;
        
        public How_To_UseModel(IProductService productService)
        {
            _productService = productService;
           
        }
        public List<Product> products { get; set; }
        public List<Package> packages { get; set; }
        public string Favicon { get; set; }
        public async Task OnGet()
        {
            var initial =await _productService.GetInitialInfoAsync();
             Favicon = "favicon.png";
            if (initial != null)
            {
                Favicon = initial.FavIcon;
            }
            products =await _productService.GetProductsAsync();
            packages = await _productService.GetPackagesAsync();
            
        }
    }
}
