using Core.Services.Interfaces;
using DataLayer.Entities.Supplementary;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Web.Components
{
    public class ShowBoxBannersNextSliderComponent: ViewComponent
    {
        private readonly IGenericService<BannerNextSlider> _genericService;
        public ShowBoxBannersNextSliderComponent(IGenericService<BannerNextSlider> genericService)
        {
            _genericService = genericService;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            List<BannerNextSlider> bannerNextSliders =(List<BannerNextSlider>) await _genericService.GetAllAsync();
            BannerNextSlider bannerNextSlider = bannerNextSliders.Where(w => w.IsActive).OrderByDescending(r => r.CreatedDate).FirstOrDefault();
            return View("/Pages/Components/_showBannerNextSliderBox.cshtml", bannerNextSlider);
        }
    }
}
