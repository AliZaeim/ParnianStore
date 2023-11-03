using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Core.DTOs.Account
{
    public class LoginVM
    {
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [Display(Name = "نام کاربری یا شماره تلفن همراه")]
        
        public string UserName { get; set; }
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [Display(Name = "رمزعبور")]
        public string Password { get; set; }
        [Display(Name = "مرا بخاطر بسپار")]
        public bool Remember { get; set; }

        public string  Returl { get; set; }

    }
}
