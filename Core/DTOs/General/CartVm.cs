using DataLayer.Entities.Store;
using DataLayer.Entities.Supplementary;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Core.DTOs.General
{
    public class CartVm
    {
        public Cart Cart { get; set; }
        public Guid CartId { get; set; }
        public List<State> States { get; set; }
        public List<County> Counties { get; set; }
        [Display(Name = "نام خریدار")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public string BuyerName { get; set; }
        [Display(Name = "نام خانوادگی خریدار")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public string BuyerFamily { get; set; }
        [Display(Name = "تلفن همراه خریدار")]
        [RegularExpression("^[0][1-9]\\d{9}$|^[1-9]\\d{9}$", ErrorMessage = " شماره تلفن همراه نا معتبر است !")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public string BuyerCellphone { get; set; }
        [Display(Name = "نام تحویل گیرنده")]
        [StringLength(100, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد!")]
        
        public string RecipientName { get; set; }
        [Display(Name = "نام خانوادگی تحویل گیرنده")]
        [StringLength(100, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد!")]
        
        public string RecipientFamily { get; set; }
        [Display(Name = "تلفن  همراه تحویل گیرنده")]
        [RegularExpression("^[0][1-9]\\d{9}$|^[1-9]\\d{9}$", ErrorMessage = " شماره تلفن همراه نا معتبر است !")]
       
        public string RecipientPhone { get; set; }
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
        
        public string PostalCode { get; set; }
        public string DiscountCode { get; set; }
        public string DiscoutCoupenMessage { get; set; }
        public double DiscountCoupenPercent { get; set; }
        public double CartTotalWithDiscount { get; set; }
    }
}
