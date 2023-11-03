using Core.Services.Interfaces;
using DataLayer.Entities.Supplementary;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Web.Components
{
    public class ShowCountiesComponent : ViewComponent
    {
        private readonly ICompService _compSrvice;
        public ShowCountiesComponent(ICompService compService)
        {
            _compSrvice = compService;
        }
        public async Task<IViewComponentResult> InvokeAsync(int? stateId)
        {
            List<County> counties = null;
            if(stateId == null)
            {
                counties = await _compSrvice.GetCountiesAsync();
            }
            else
            {
                counties = await _compSrvice.GetCountiesOfStateAsync((int)stateId);
            }
            counties = counties.OrderBy(x => x.CountyName).ToList();
            return View("/Pages/Components/_showCounties.cshtml", counties);
        }
    }
}
