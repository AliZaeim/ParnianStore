using DataLayer.Entities.Store;
using DataLayer.Entities.Supplementary;
using DataLayer.Entities.User;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Core.DTOs.Admin
{
    public class AdminOrderVM
    {
        [Display(Name = "نام خریدار")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public string BuyerName { get; set; }
        [Display(Name = "نام خانوادگی خریدار")]
        [StringLength(100, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد!")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public string BuyerFamily { get; set; }
        [Display(Name = "تلفن همراه خریدار")]
        [StringLength(100, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد!")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public string BuyerCellphone { get; set; }
        [Display(Name = "خرید توسط خود سایت")]
        public bool BuyWithSystem { get; set; } = true;
        [Display(Name = "نام تحویل گیرنده")]
        [StringLength(100, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد!")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public string RecipientName { get; set; }
        [Display(Name = "نام خانوادگی تحویل گیرنده")]
        [StringLength(100, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد!")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public string RecipientFamily { get; set; }
        [Display(Name = "تلفن همراه تحویل گیرنده")]
        [StringLength(100, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد!")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public string RecipientPhone { get; set; }
        [Display(Name = "استان")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public int? StateId { get; set; }
        [Display(Name = "شهرستان")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public int? CountyId { get; set; }

        [Display(Name = "آدرس")]
        [StringLength(100, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد!")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public string Address { get; set; }
        [Display(Name = "کد پستی")]
        [StringLength(100, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد!")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public string PostalCode { get; set; }

        
        [Required(ErrorMessage = "هیچ محصولی به سفارش اضافه نشده است !")]
        public List<int> SelectedProducts { get; set; }
        public List<int> PCount { get; set; }
        public List<Product> Products { get; set; }
        public List<Package> Packages { get; set; }
        public List<State> States { get; set; }
        public List<County> Counties { get; set; }
        public User User { get; set; }
        public bool Saved { get; set; }

    }
}
