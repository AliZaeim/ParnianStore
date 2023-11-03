using Core.DTOs.Account;
using Core.Services.Interfaces;
using DataLayer.Entities.User;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Web.Pages.Account
{
    public class LoginModel : PageModel
    {
        private readonly IUserService _userService;
        public LoginModel(IUserService userService)
        {
            _userService = userService;
        }

        [BindProperty]
        public LoginVM LoginVM { get; set; } = new LoginVM();
        
        public void OnGet(string ReturnUrl)
        {
            LoginVM.Returl = ReturnUrl;
        }
        public async Task<IActionResult> OnPostAsync(LoginVM loginVM)
        {
            if (!ModelState.IsValid)
                return Page();

            User userbyName = await _userService.GetUserByUserName(loginVM.UserName);
            User userbycellphone = await _userService.GetUserByCellphoneAsync(loginVM.UserName);
            if (userbyName == null && userbycellphone == null)
            {
                ModelState.AddModelError("loginVM.UserName", "نام کاربری یا رمز عبور نامعتبر است !");
                return Page();
            }
            User userPassword = await _userService.GetUserByPasswordAsync(loginVM.Password);
            if (userPassword == null)
            {
                ModelState.AddModelError("loginVM.UserName", "نام کاربری یا رمز عبور نامعتبر است !");
                return Page();
            }
            if(userbyName != null)
            {
                if (userbyName != userPassword)
                {
                    ModelState.AddModelError("loginVM.UserName", "نام کاربری یا رمز عبور نامعتبر است !");
                    return Page();
                }
            }
            if(userbycellphone != null)
            {
                if (userbycellphone != userPassword)
                {
                    ModelState.AddModelError("loginVM.UserName", "نام کاربری یا رمز عبور نامعتبر است !");
                    return Page();
                }
            }
            
            if (userPassword.IsActive)
            {
                var claims = new List<Claim>()
                {
                    new Claim(ClaimTypes.NameIdentifier, userPassword.Id.ToString()),
                    new Claim(ClaimTypes.Name, userPassword.UserName),                   
                    new Claim("fullname", userPassword.FullName)

                };
                var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var principal = new ClaimsPrincipal(identity);
                var properties = new AuthenticationProperties
                {
                    IsPersistent = loginVM.Remember
                };
                await HttpContext.SignInAsync(principal, properties);
            }
            
            if(string.IsNullOrEmpty(loginVM.Returl))
            {
                return Redirect("/Manage/Home/Index");
            }
            else
            {
                return Redirect(loginVM.Returl);
            }
           
        }
    }
}
