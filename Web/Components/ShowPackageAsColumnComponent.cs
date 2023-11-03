using DataLayer.Entities.Store;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Web.Components
{
    public class ShowPackageAsColumnComponent : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync(Package package)
        {
            return await Task.FromResult(View("/Pages/Components/_showPackageAsColumn.cshtml", package));
        }
    }
}
