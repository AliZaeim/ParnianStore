using Core.DTOs.General;
using Core.Services.Interfaces;
using DataLayer.Entities.Store;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Web.Components
{
    public class ShowPackageGroupsComponent : ViewComponent
    {
        private readonly IProductService _productService;
        public ShowPackageGroupsComponent(IProductService productService)
        {
            _productService = productService;
        }
        public async Task<IViewComponentResult> InvokeAsync(string type, bool parent = false)
        {
            List<PackageGroup> groups = await _productService.GetPackageGroupsAsync();
            groups = groups.Where(w => w.IsActive).ToList();
            //groups = groups.Where(w => w.ParentId != null).ToList();
            if (parent == false)
            {
                groups = groups.Where(w => w.ParentId == null && w.IsActive).ToList();
            }
            else
            {
                groups = groups.Where(w => w.ParentId != null && w.Parent.IsActive && w.IsActive).ToList();
            }
            groups = groups.Where(w => w.Packages.Count != 0 && w.Packages.Any(x => x.IsActive)).ToList();


            PackageGroupVM packageGroupVm = new()
            {
                PackageGroups = groups,
                Type = type
            };
            return View("/Pages/Components/_showPackageGroups.cshtml", packageGroupVm);
        }
    }
}
