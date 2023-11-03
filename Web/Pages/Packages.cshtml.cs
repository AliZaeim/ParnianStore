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
    public class PackagesModel : PageModel
    {
        private readonly IProductService _productService;
        public PackagesModel(IProductService productService)
        {
            _productService = productService;
        }
        public List<Package> Packages { get; set; }
        public List<PackageGroup> PackageGroups { get; set; }
        public int? CurrentPackageGroupId { get; set; }
        public PackageGroup CurrentPackageGroup { get; set; }
        public bool AllP { get; set; }
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public int TotalCount { get; set; }
        public int RecPerPage { get; set; }
        public SitePage SitePage { get; set; }
        public string FavIcon { get; set; } = "favicon.png";
        public async Task OnGet(string gname, int? pageId)
        {
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
            RecPerPage = recPerPage;
            CurrentPage = zpage;
            PackageGroups = await _productService.GetPackageGroupsAsync();
            Packages = await _productService.GetPackagesAsync();
            if(Packages != null)
            {
                TotalCount = Packages.Count;
            }
           
            if (!string.IsNullOrEmpty(gname))
            {
                CurrentPackageGroup = await _productService.GetPackageGroupByEnNameAsync(gname);
                if (CurrentPackageGroup != null)
                {
                    CurrentPackageGroupId = CurrentPackageGroup.PgId;
                    Packages = CurrentPackageGroup.Packages.ToList();
                    if (Packages != null)
                    {
                        Packages = Packages.Where(w => w.IsActive).ToList();
                    }
                }


            }
            if (Packages != null)
            {
                
                if (Packages.Count % recPerPage == 0)
                {
                    TotalPages = Packages.Count / recPerPage;
                }
                else
                {
                    TotalPages = (Packages.Count / recPerPage) + 1;
                }
            }
            Packages = Packages.Skip((zpage - 1) * recPerPage).Take(recPerPage).ToList();
        }
        [BindProperty]
        public int? PageId { get; set; }
        [BindProperty]
        public string Grname { get; set; }
        public async Task<IActionResult> OnPostAsync()
        {
            int zpage = PageId.GetValueOrDefault(1);
            int recPerPage = 12;
            RecPerPage = recPerPage;
            PackageGroups = await _productService.GetPackageGroupsAsync();
            List<Package> packages = packages = await _productService.GetPackagesAsync();
            PackageGroup packageGroup = null;


            if (packages != null)
            {
                packages = packages.Where(w => w.IsActive).ToList();
            }
            TotalCount = packages.Count;
            if (!string.IsNullOrEmpty(Grname))
            {
                packageGroup = await _productService.GetPackageGroupByEnNameAsync(Grname);
                CurrentPackageGroupId = packageGroup.PgId;
                CurrentPackageGroup = packageGroup;
                if (packageGroup != null)
                {
                    packages = packageGroup.Packages.ToList();
                }
                if (packages != null)
                {
                    packages = packages.Where(w => w.IsActive).ToList();
                }

            }
            CurrentPage = zpage;

            if (packages != null)
            {
                
                if (packages.Count % recPerPage == 0)
                {
                    TotalPages = packages.Count / recPerPage;
                }
                else
                {
                    TotalPages = (packages.Count / recPerPage) + 1;
                }
                packages = packages.Skip((zpage - 1) * recPerPage).Take(recPerPage).ToList();
            }
            Packages = packages.ToList();
            return Page();
        }
    }
}
