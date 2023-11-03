using Core.DTOs.General;
using DataLayer.Entities.Store;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace Web.Components
{
    public class ShowProductsInColumnsComponent : ViewComponent
    {
        public IViewComponentResult Invoke(List<Product> products, string returnUrl, string title, string titleClass, string titleLineClass ,string link, int totalrecCount)
        {
            ProductsInColumnsVM productsInColumnsVM = new ProductsInColumnsVM()
            {
                Products = products,
                Title = title,
                TitleClass = titleClass,
                TitleLineClass = titleLineClass,
                Link = link,
                TotalCount = totalrecCount
            };
            return View("/Pages/Components/_showProductsInColumns.cshtml", productsInColumnsVM);
        }
    }
}
