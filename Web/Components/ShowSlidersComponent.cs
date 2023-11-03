using Core.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Web.Components
{
    public class ShowSlidersComponent: ViewComponent
    {
        private readonly ICompService _compService;
        public ShowSlidersComponent(ICompService compService)
        {
            _compService = compService;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            List<DataLayer.Entities.Supplementary.Slider> sliders = await _compService.GetSlidersAsync();
            sliders = sliders.Where(w => w.SliderIsActive==true).ToList();
            sliders = sliders.Where(w => w.SliderStartActiveDate == null ||( w.SliderStartActiveDate <= DateTime.Now) && w.SliderEndActiveDate >= DateTime.Now).ToList();

            return View("/Pages/Components/_showSliders.cshtml", sliders);
        }
    }
}
