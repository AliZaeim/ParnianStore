using Core.Services.Interfaces;
using DataLayer.Entities.Supplementary;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Web.Pages
{    
    public class About_alian_sabzModel : PageModel
    {
        private readonly IGenericService<ContactInfo> _contactServie;
        private readonly ICompService _compService;
        private readonly IGenericService<Instagram> _instaService;
        public About_alian_sabzModel(IGenericService<ContactInfo> contactService,ICompService compService,IGenericService<Instagram> instaService)
        {
            _contactServie = contactService;
            _compService = compService;
            _instaService = instaService;
        }
        public ContactInfo ContactInfo { get; set; }
        public List<AboutUsfaq> AboutFaqs { get; set; }
        public List<Instagram> Instagrams { get; set; }
        public SitePage SitePage { get; set; }
        public string FavIcon { get; set; } = "favicon.png";
        public async Task OnGet()
        {
            var contacts = await _contactServie.GetAllAsync();
            SitePage = await _compService.GetSitePageByEnNameAsync("about");
            var initial = await _compService.GetInitialInfoAsync();
            if (initial != null)
            {
                FavIcon = initial.FavIcon;
            }
            ContactInfo = contacts.OrderByDescending(x => x.Id).FirstOrDefault();
            AboutFaqs =await _compService.GetAboutUsfaqsAsync();
            AboutFaqs = AboutFaqs.Where(w => w.IsActive).ToList();
            var ins = await _instaService.GetAllAsync();
            Instagrams = ins.ToList();
        }
    }
}
