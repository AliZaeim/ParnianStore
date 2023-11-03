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
    public class ProductTagModel : PageModel
    {
        private readonly IProductService _productService;

        public ProductTagModel(IProductService productService)
        {
            _productService = productService;
        }
        public List<Product> Products { get; set; }
        
        public string Tag { get; set; }
        [BindProperty]
        public string TagEnTitle { get; set; }
        public List<string> ProductTags { get; set; }

        public async Task OnGet(string tag)
        {
            Products = await _productService.GetProductsAsync();
            ProductTags = Products.SelectMany(x => x.TagsList).ToList();
            ProductTags = ProductTags.GroupBy(g => g.Trim())
                              .Select(o => o.FirstOrDefault()).ToList();
            Tag = tag.Trim().Replace("-", " ");
            if(tag != "all")
            {
                Products = await _productService.GetProductsByTagAsync(tag);
               
            }
            else
            {
                Products = await _productService.GetProductsAsync();
                
            }
            
           
            
        }
        
       

    }
}
