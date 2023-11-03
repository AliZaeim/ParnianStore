using DataLayer.Entities.User;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
namespace Core.DTOs.Account
{
    public class RegisterVM
    {
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [Display(Name = "نام کاربری")]
        [MinLength(6, ErrorMessage = "{0} نمی تواند کمتر از {1} کاراکتر باشد!")]
        public string UserName { get; set; }
        [Display(Name = "نام")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public string Name { get; set; }

        [Display(Name = "نام خانوادگی")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public string Family { get; set; }
        [Display(Name = "تلفن همراه")]
        [RegularExpression("^[0][1-9]\\d{9}$|^[1-9]\\d{9}$", ErrorMessage = " شماره تلفن همراه نا معتبر است !")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public string Cellphone { get; set; }
        
        public string RetUrl { get; set; }
        
    }
}
