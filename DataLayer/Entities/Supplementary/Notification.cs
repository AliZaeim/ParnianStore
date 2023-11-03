using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Entities.Supplementary
{
    public class Notification
    {
        [Key]
        public int Id { get; set; }
        [Display(Name ="عنوان")]        
        [StringLength(50, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد!")]
        public string Title { get; set; }
        [Display(Name ="متن")]        
        [StringLength(500, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد!")]
        public string Text { get; set; }
        [Display(Name ="عرض")]        
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public int? Width { get; set; }
        
        [Display(Name ="حاشیه داخلی")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [StringLength(20, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد!")]
        public string Padding { get; set; }

        [Display(Name ="لینک")]
        [StringLength(200, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد!")]
        public string Link { get; set; }
        [Display(Name ="تصویر")]
        [StringLength(100, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد!")]
        public string Image { get; set; }
        [Display(Name ="عرض تصویر")]
        [StringLength(50, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد!")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public string ImageWidth { get; set; }
        [Display(Name ="ارتفاع تصویر")]
        [StringLength(50, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد!")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public string ImageHeight { get; set; }
        [Display(Name ="موقعیت نمایش")]
        [StringLength(100, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد!")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public string Position { get; set; }
        [Display(Name = "مدت زمان نمایش به میلی ثانیه")]
        public int? Timer { get; set; } 
        [Display(Name ="نمایش نوار وضعیت")]
        public bool ShowProgressBar { get; set; }
        [Display(Name ="نمایش دکمه بستن")]
        public bool ShowCloseButton { get; set; }
        [Display(Name ="نمایش دکمه تایید")]
        public bool ShowConfirmButton { get; set; }
        [Display(Name ="متن دکمه تایید")]
        [StringLength(100, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد!")]
        public string ConfirmButtonText { get; set; }
        [Display(Name ="تاریخ ثبت")]
        public DateTime? CreatedDate { get; set; }
        [Display(Name ="فعال / غیرفعال")]
        public bool IsActive { get; set; }
    }
}
