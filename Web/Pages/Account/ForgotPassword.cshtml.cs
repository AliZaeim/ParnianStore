using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Core.DTOs.Account;
using Core.Services.Interfaces;
using DataLayer.Entities.User;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Web.Pages.Account
{
    public class ForgotPasswordModel : PageModel
    {
        private readonly IUserService _userService;
        public ForgotPasswordModel(IUserService userService)
        {
            _userService = userService;
        }
        [BindProperty]
        [RegularExpression("^[0][1-9]\\d{9}$|^[1-9]\\d{9}$", ErrorMessage = " شماره تلفن همراه نا معتبر است !")]
        [Display(Name = "تلفن همراه")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public string Cellphone { get; set; }
        public string Message { get; set; }
        public void OnGet()
        {
        }
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
                return Page();
            User xUser = await _userService.GetUserByCellphoneAsync(Cellphone);
            if(xUser == null)
            {                
                ModelState.AddModelError("Cellphone", "کاربری با این شماره تلفن یافت نشد !");
                return Page();
            }
            //Todo Send password
            bool Issend =_userService.SendPassword(xUser.Password, Cellphone);
            if(Issend==true)
            {
                TempData["SendPassword"] = "true";
            }

            return Redirect("/Index");
        }
    }
}
