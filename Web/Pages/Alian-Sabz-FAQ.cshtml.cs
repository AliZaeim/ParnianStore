using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.DTOs.General;
using Core.Services.Interfaces;
using DataLayer.Entities.Supplementary;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Web.Pages
{
    public class FAQModel : PageModel
    {
        private readonly IGenericService<FAQ> _genericServie;
        private readonly IUserService _userService;
        public FAQModel(IGenericService<FAQ> genericService,IUserService userService)
        {
            _genericServie = genericService;
            _userService = userService;
        }
        [BindProperty]
        public FaqVM FaqVM { get; set; } = new() ;
        public async Task OnGet()
        {
            if(User.Identity.IsAuthenticated)
            {
                var user = await _userService.GetUserByUserName(User.Identity.Name);
                if(user != null)
                {
                    FaqVM.FullName = user.FullName;
                    FaqVM.Cellphone = user.Cellphone;
                    FaqVM.Email = user.Email;
                }
            }
            FaqVM.FAQs = _genericServie.GetAll().Where(w => !string.IsNullOrEmpty(w.Reply) && w.IsActive).ToList();
        }
        public async Task<IActionResult> OnPostAsync(FaqVM faqVM)
        {
            if(!ModelState.IsValid)
            {
                return Page();
            }
            if(string.IsNullOrEmpty(faqVM.Cellphone) && string.IsNullOrEmpty(faqVM.Email))
            {
                ModelState.AddModelError("faqVM.Cellphone", "شماره همراه یا ایمیل خود را وارد کنید !");
                return Page();
            }
            FAQ fAQ = new()
            {
                CreateDate = DateTime.Now,
                FullName = faqVM.FullName,
                Cellphone = faqVM.Cellphone,
                Email = faqVM.Email,
                Question = faqVM.Question
            };
            _genericServie.Create(fAQ);
            await _genericServie.SaveChangesAsync();
            ModelState.Clear();
            TempData["message"] = "سوال شما با موفقیت ثبت شد";
            return RedirectToPage("AlianSabz_FAQ");
        }
    }
}
