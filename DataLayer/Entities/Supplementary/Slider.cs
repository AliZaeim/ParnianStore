using System;
using System.ComponentModel.DataAnnotations;

namespace DataLayer.Entities.Supplementary
{
    public class Slider
    {
        [Key]
        public int SliderId { get; set; }
        [Display(Name = "تصویر")]
        [MaxLength(50, ErrorMessage = "{0} نمی تواند بیشتر از {1} باشد!")]
        
        public string SliderImage { get; set; }
        
        [Display(Name ="عنوان")]
        [StringLength(100, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد!")]
        public string SliderTitle { get; set; }
        [Display(Name = "کلاس عنوان")]
        [StringLength(50, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد!")]
        public string SliderTitleClass { get; set; }
        [Display(Name ="افکت نمایشی عنوان")]
        [StringLength(100, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد!")]
        public string SliderTitle_DataAppear { get; set; }
        [Display(Name = "توضیح")]
        [StringLength(100, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد!")]
        public string SliderComment { get; set; }
        [Display(Name = "کلاس توضیح")]
        [StringLength(50, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد!")]
        public string SliderCommentClass { get; set; }
        [Display(Name = "افکت نمایشی توضیح")]
        [StringLength(100, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد!")]
        public string SliderComment_DataAppear { get; set; }
        
        [MaxLength(100, ErrorMessage = "{0} نمی تواند بیشتر از {1} باشد!")]
        [Display(Name = "لینک")]
        public string SliderLink { get; set; }
        [Display(Name ="متن لینک")]
        public string SliderLinkText { get; set; }
        [Display(Name = "کلاس لینک")]
        [StringLength(50, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد!")]
        public string SliderLinkClass { get; set; }
        [Display(Name = "افکت نمایشی لینک")]
        [StringLength(100, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد!")]
        public string SliderLink_DataAppear { get; set; }
        [Display(Name ="تاریخ ثبت")]
        public DateTime? SliderRegisterDateTime { get; set; }
        [Display(Name ="فعال/غیرفعال")]
        public bool SliderIsActive { get; set; }
        [Display(Name ="تاریخ شروع نمایش")]
        public DateTime? SliderStartActiveDate { get; set; }
        [Display(Name ="تاریخ پایان نمایش")]
        public DateTime? SliderEndActiveDate { get; set; }
        [Display(Name ="بازدید")]
        public int Views { get; set; }
    }
}
