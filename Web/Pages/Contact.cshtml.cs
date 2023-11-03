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
    public class ContactModel : PageModel
    {
        
        private readonly IGenericService<ContactMessage> _genericServie;
        private readonly IGenericService<ContactInfo> _contactInfo;
        public ContactModel(IGenericService<ContactMessage> genericService,IGenericService<ContactInfo> contactInfo)
        {
            _genericServie = genericService;
            _contactInfo = contactInfo;
        }
        [BindProperty]
        public ContactMessage ContactMessage { get; set; }
        public ContactInfo ContactInfo { get; set; }
        public async Task<IActionResult> OnGet()
        {
            var contacts = await _contactInfo.GetAllAsync();
            ContactInfo = contacts.OrderByDescending(x => x.Id).FirstOrDefault();
            return Page();
        } 
        public async Task<IActionResult> OnPostAsync()
        {
            if(!ModelState.IsValid)
            {
                return Page();
            }
            ContactMessage.CreatedDate = DateTime.Now;
            _genericServie.Create(ContactMessage);
            await _genericServie.SaveChangesAsync();
            ModelState.Clear();
            TempData["message"] = "پیام شما با موفقیت ثبت شد";
            return RedirectToPage("Contact");
        }
    }
}
