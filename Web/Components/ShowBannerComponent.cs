using Core.Services.Interfaces;
using DataLayer.Entities.Supplementary;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Web.Components
{
    public class ShowBannerComponent : ViewComponent
    { 
        public async Task<IViewComponentResult> InvokeAsync(Banner banner)
        {
            return await Task.FromResult(View("/Pages/Components/_showBanner.cshtml", banner));
        }
    }
}
