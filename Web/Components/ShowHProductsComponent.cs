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
    public class ShowHProductsComponent : ViewComponent
    {
        
        public async Task<IViewComponentResult> InvokeAsync(List<Product> products,string returnUrl,string title,string titleClass,string link, int totalrecCount,string image,string bgClass)
        {
            HProductComponentVM hProductsComponentVM = new ()
            {
                Products = products,
                ReturnUrl = returnUrl,
                Title = title,
                TitleClass = titleClass,
                Link = link,
                TotalCount=totalrecCount,
                Image = image,
                BgClass = bgClass
                
            };
            return await Task.FromResult(View("/Pages/Components/_showHProducts.cshtml", hProductsComponentVM));
        }
    }
}
