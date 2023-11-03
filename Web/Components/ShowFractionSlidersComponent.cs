using Core.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Web.Components
{
    public class ShowFractionSlidersComponent : ViewComponent
    {
        private readonly ICompService _compService;
        public ShowFractionSlidersComponent(ICompService compService)
        {
            _compService = compService;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            List<DataLayer.Entities.Supplementary.FractionSlider> sliders = await _compService.GetFractionSlidersAsync();
            sliders = sliders.Where(w => w.IsActive).ToList();
            sliders = sliders.Where(w => w.FSImages.Any(a => a.IsActive) || w.FSTexts.Any(a => a.IsActive)).ToList();
            
            
            return View("/Pages/Components/_showFractionSliders.cshtml", sliders);
        }
    }
}
