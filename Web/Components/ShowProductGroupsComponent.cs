using Core.DTOs.General;
using Core.Services.Interfaces;
using DataLayer.Entities.Store;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Web.Components
{
    public class ShowProductGroupsComponent : ViewComponent
    {
        private readonly IProductService _productService;
        public ShowProductGroupsComponent(IProductService productService)
        {
            _productService = productService;
        }
        public async Task<IViewComponentResult> InvokeAsync(string type,bool parent=false)
        {
            List<ProductGroup> groups = await _productService.GetProductGroupsAsync();
            
            if (parent==true)
            {
                groups = groups.Where(w => w.ParentId == null && w.IsActive).ToList();
            }
            else
            {
                groups = groups.Where(w => w.ParentId != null && w.Parent.IsActive && w.IsActive).ToList();
            }
            groups = groups.Where(w => w.Products.Count != 0 && w.Products.Any(x =>x.IsActive )).ToList();

            
            GroupMenuVM groupMenuVM = new()
            {
                ProductGroups = groups,
                Type = type
            };
            return View("/Pages/Components/_showProductGroups.cshtml",groupMenuVM);
        }
    }
}
