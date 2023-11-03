using Core.DTOs.Account;
using Core.Services.Interfaces;
using DataLayer.Entities.User;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Threading.Tasks;

namespace Web.Pages.Account
{
    public class RegisterModel : PageModel
    {
        private readonly IUserService _userService;
        public RegisterModel(IUserService userService)
        {
            _userService = userService;
        }

        [BindProperty]
        public RegisterVM RegisterVM { get; set; }
        public void OnGetAsync(string ReturnUrl)
        {
            RegisterVM = new();            
            RegisterVM.RetUrl = ReturnUrl;
        }
        
        public async Task<IActionResult> OnPostAsync(RegisterVM RegisterVM)
        {
            if (!ModelState.IsValid)
                return Page();

            User xUser = await _userService.GetUserByCellphoneAsync(RegisterVM.Cellphone);
            if (xUser != null)
            {
                ModelState.AddModelError("registerVM.Cellphone", "این شماره تلفن قبلا ثبت شده است !");
                return Page();
            }
            if(await _userService.GetUserByUserName(RegisterVM.UserName) != null)
            {
                ModelState.AddModelError("registerVM.UserName", "این کد کاربری قبلا ثبت شده است !");
                return Page();
            }
            string pass = Core.Prodocers.Generators.GenerateUniqueString(0, 0, 8, 0);
            User user = new()
            {
                Name = RegisterVM.Name,
                Family = RegisterVM.Family,
                Password = pass,
                Cellphone = RegisterVM.Cellphone,
                UserName = RegisterVM.UserName,
                IsActive = true,
               
                RegisteredDate = DateTime.Now
            };
            
            //_userService.CreateUser(user);
            UserRole userRole = new()
            {
                User = user,
                RoleId = 2,
                RegisterDate = DateTime.Now,
                IsActive = true
                
            };
            _userService.CreateUserRole(userRole);
            await _userService.SaveChangesAsync();

            //Todo Send password to cellphone
            _userService.SendPassword(pass, RegisterVM.Cellphone);

            if (string.IsNullOrEmpty(RegisterVM.RetUrl))
            {
                return Redirect("/Manage/Home/Index");
            }
            else
            {
                return Redirect(RegisterVM.RetUrl);
            }
            
            
        }
    }
}
