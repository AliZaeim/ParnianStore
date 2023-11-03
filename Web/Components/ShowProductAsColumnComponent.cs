using Core.Services.Interfaces;
using DataLayer.Entities.Store;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Web.Components
{
    public class ShowProductAsColumnComponent:ViewComponent
    {        
        public IViewComponentResult Invoke(Product product)
        {           
            return View("/Pages/Components/_showProductAsColumn.cshtml", product);
        }
    }
}
