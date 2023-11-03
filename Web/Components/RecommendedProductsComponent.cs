using Core.DTOs.General;
using DataLayer.Entities.Store;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Web.Components
{
    public class RecommendedProductsComponent : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync(List<Product> products,string title)
        {
            RecProductsVM recProductsVM = new()
            {
                Products = products,
                Title = title
            };
            return await Task.FromResult(View("/Pages/Components/_showRecommendedProducts.cshtml", recProductsVM));
        }
    }
    
}
