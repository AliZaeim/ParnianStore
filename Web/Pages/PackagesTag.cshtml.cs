using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Services.Interfaces;
using DataLayer.Entities.Store;
using DataLayer.Entities.Supplementary;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Web.Pages
{
    public class PackagesTagModel : PageModel
    {
        private readonly IProductService _productService;
        

        public PackagesTagModel(IProductService productService)
        {
            _productService = productService;
        }
        public List<Package> Packages { get; set; }

        public string Tag { get; set; }
        [BindProperty]
        public string TagEnTitle { get; set; }
        public List<string> PackageTags { get; set; }
        public InitialInfo InitialInfo { get; set; }
        public async Task OnGet(string tag)
        {
            InitialInfo = await _productService.GetInitialInfoAsync();
            Packages = await _productService.GetPackagesAsync();
            PackageTags = Packages.SelectMany(x => x.TagsList).ToList();
            PackageTags = PackageTags.GroupBy(g => g.Trim())
                              .Select(o => o.FirstOrDefault()).ToList();
            Tag = tag.Trim().Replace("-", " ");
            if (tag != "all")
            {
                Packages = await _productService.GetPackagesByTagAsync(tag);

            }
            
        }
    }
}
