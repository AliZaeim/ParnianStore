using System;
using System.ComponentModel.DataAnnotations;

namespace DataLayer.Entities.Supplementary
{
    public class ThreeColmun
    {
        [Key]
        public int Id { get; set; }
        [Display(Name ="عنوان")]
        [StringLength(50, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد!")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public string Title { get; set; }
        [Display(Name ="متن")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [StringLength(200, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد!")]
        public string Text { get; set; }
        [Display(Name ="تصویر")]
        [StringLength(100, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد!")]
        public string Image { get; set; }
        [Display(Name ="لینک")]
        public string Link { get; set; }
        [Display(Name ="متن لینک")]
        public string LinkText { get; set; }
        [Display(Name ="فعال / غیرفعال")]
        public bool IsActive { get; set; }
        [Display(Name ="تاریخ ثبت")]
        public DateTime CreatedDate { get; set; }
    }
}
