using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTOs.Admin
{
    public class DiscountCoupenVM
    {
        public int Id { get; set; }
        [Display(Name = "کد تخفیف")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MinLength(6, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد!")]
        
        public string Code { get; set; }
        [Display(Name = "توضیحات")]
        [StringLength(200, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد!")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public string Comment { get; set; }
        [Display(Name = "تعداد")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public int? Number { get; set; }
        [Display(Name = "درصد تخفیف")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public float Percent { get; set; }
        [RegularExpression(@"^$|^([1۱][۰-۹ 0-9]{3}[/\/]([0 ۰][۱-۶ 1-6])[/\/]([0 ۰][۱-۹ 1-9]|[۱۲12][۰-۹ 0-9]|[3۳][01۰۱])|[1۱][۰-۹ 0-9]{3}[/\/]([۰0][۷-۹ 7-9]|[1۱][۰۱۲012])[/\/]([۰0][1-9 ۱-۹]|[12۱۲][0-9 ۰-۹]|(30|۳۰)))$", ErrorMessage = "تاریخ وارد شده نامعتبر است.")]
        [Display(Name = "تاریخ پایان")]
        public string EndDate { get; set; }
        [Display(Name = "زمان پایان")]
        [RegularExpression("(([01][0-9])|(2[0-3])):([0-5][0-9])$", ErrorMessage = "زمان نامعتبر است !")]
        public string EndTime { get; set; }
        [Display(Name = "فعال/غیرفعال")]
        public bool IsActive { get; set; }
    }
}
