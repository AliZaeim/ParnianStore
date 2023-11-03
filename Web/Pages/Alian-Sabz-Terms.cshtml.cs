using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Services.Interfaces;
using DataLayer.Entities.Supplementary;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Web.Pages
{
    public class AlianSabz_RulesModel : PageModel
    {
        private readonly ICompService _compService;
        public AlianSabz_RulesModel(ICompService compService)
        {
            _compService = compService;
        }
        public List<Terms> Terms { get; set; }
        public async Task OnGet()
        {
            Terms = await _compService.GetTermsAsync();
            Terms = Terms.OrderBy(x => x.ShowRating).ThenBy(x => x.CreatedDate).ToList();
        }
    }
}
