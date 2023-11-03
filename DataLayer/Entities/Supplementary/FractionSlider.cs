using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DataLayer.Entities.Supplementary
{
    public class FractionSlider
    {
        public FractionSlider()
        {
            FSImages = new HashSet<FSImage>();
            FSTexts = new HashSet<FSText>();
        }
        [Key]
        public int Id { get; set; }
        [Display(Name ="عنوان")]
        [StringLength(100, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد!")]
        public string Title { get; set; }
        [Display(Name = "رتبه نمایش")]
        [StringLength(2, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد!")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public string ShowRating { get; set; }
        [Display(Name ="تاریخ ثبت")]
        public DateTime? CreatedDate { get; set; }
        [Display(Name = "فعال/غیرفعال")]
        public bool IsActive { get; set; }
        [Display(Name ="لینک")]
        [StringLength(200, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد!")]
        public string Link { get; set; }
        [Display(Name ="کلاس نمایش لینک")]
        [StringLength(50, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد!")]
        public string LinkClass { get; set; }
        [Display(Name ="متن نمایشی لینک")]
        [StringLength(100, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد!")]
        public string LinkText { get; set; }
        [Display(Name ="موقعیت نمایش لینک")]
        [StringLength(50, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد!")]
        public string LinkPostition { get; set; }
        [Display(Name = "جهت ورود-data-in")]
        [StringLength(50, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد!")]
        public string LinkDataIn { get; set; }
        [Display(Name = "جهت خروج-data-out")]
        [StringLength(50, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد!")]
        public string LinkDataOut { get; set; }
        [Display(Name = "زمان تاخیر به میلی ثانیه-data-delay")]
        [StringLength(50, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد!")]
        public string LinkDataDelay { get; set; }
        [Display(Name = "افکت حرکتی ورود-data-ease-in")]
        [StringLength(50, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد!")]
        public string LinkDataEaseIn { get; set; }

        #region Relations
        public ICollection<FSImage> FSImages { get; set; }
        public ICollection<FSText> FSTexts { get; set; }
        #endregion
    }
}
