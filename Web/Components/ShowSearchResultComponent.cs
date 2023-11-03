using Core.DTOs.General;
using DataLayer.Entities.Store;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Web.Components
{
    public class ShowSearchResultComponent: ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync(SearchVM searchVM)
        {
            return await Task.FromResult(View("/Pages/Components/_showSearchResult.cshtml", searchVM));
        }
    }
}
