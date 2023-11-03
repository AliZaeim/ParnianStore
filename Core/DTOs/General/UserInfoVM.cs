using DataLayer.Entities.Supplementary;
using DataLayer.Entities.User;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Core.DTOs.General
{
    public class UserInfoVM
    {
        public int Id { get; set; }
        [Display(Name ="نام")]
        [StringLength(100, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد!")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]

        public string Name { get; set; }
        [Display(Name = "نام خانوادگی")]
        [StringLength(100, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد!")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public string Family { get; set; }
        [Display(Name = "تلفن همراه")]
        [RegularExpression("^[0][1-9]\\d{9}$|^[1-9]\\d{9}$", ErrorMessage = " شماره تلفن همراه نا معتبر است !")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public string Cellphone { get; set; }
        [Display(Name = "نام کاربری")]
        [StringLength(20, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد!")]
        [MinLength(6, ErrorMessage = "{0} نمی تواند کمتر از {1} کاراکتر باشد!")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public string UserName { get; set; }
        
        [Display(Name = "جنسیت")]
        [StringLength(50, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد!")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public string Sex { get; set; }

        [Required(ErrorMessage = "لطفا {0} را انتغاب کنید")]
        [Display(Name = "استان")]
        public int? SatetId { get; set; }
        [Display(Name = "شهرستان")]
        [Required(ErrorMessage = "لطفا {0} را انتخاب کنید")]
        public int? CountyId { get; set; }
        [Display(Name = "آدرس")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [StringLength(200, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد!")]
        public string Address { get; set; }
        [Display(Name = "کد پستی")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]

        public string PostalCode { get; set; }
        [Display(Name ="ایمیل")]
        [StringLength(200, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد!")]
        [DataType(DataType.EmailAddress,ErrorMessage ="آدرس ایمیل نامعتبر است !")]
        public string Email { get; set; }

        public County County { get; set; }
        public List<Role> Roles { get; set; }
       
    }
}
