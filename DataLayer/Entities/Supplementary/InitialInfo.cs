using System;
using System.ComponentModel.DataAnnotations;

namespace DataLayer.Entities.Supplementary
{
    public class InitialInfo
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [StringLength(200, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد!")]
        [Display(Name ="نام کارخانه")]
        public string Title { get; set; }
        [Display(Name ="تلفن")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [StringLength(100, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد!")]
        public string Phone { get; set; }
        [Display(Name ="کد پستی")]
        [StringLength(100, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد!")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public string PostalCode { get; set; }
        [Display(Name ="نام استان")]
        [StringLength(100, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد!")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public string StateName { get; set; }
        [Display(Name ="نام شهرستان")]
        [StringLength(100, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد!")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public string CountyName { get; set; }
        [Display(Name ="آدرس")]
        [StringLength(300, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد!")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public string Address { get; set; }
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [StringLength(100, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد!")]
        [Display(Name = "واحد پول")]
        public string SiteCurrency { get; set; }        
        [StringLength(100, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد!")]
        [Display(Name = "لوگو")]
        public string SiteLogo { get; set; }       
        [StringLength(100, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد!")]
        [Display(Name = "لوگوی کوچک ")]
        public string SiteThumbLogo { get; set; }
        
        [StringLength(100, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد!")]
        [Display(Name = "فاوآیگن")]
        public string FavIcon { get; set; }
        
        [Display(Name ="شعار عالیان")]
        [StringLength(200, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد!")]
        public string Motto { get; set; }
        /// <summary>
        /// درصدی که باعث افزایش مبلغ تخفیف می شود و قابل استفاده جهت خرید محصول می باشد
        /// </summary>
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [Display(Name = "درصد افزایش تخفیف")]
        public float PercentageIncreaseDiscount { get; set; }
        [Display(Name ="حداقل مبلغ خرید برای ارسال رایگان")]        
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public int? FreeShipping_MinimumPurchaseAmount { get; set; }
        [Display(Name ="تاریخ ثبت")]
        public DateTime CreatedDate { get; set; }

    }
}
