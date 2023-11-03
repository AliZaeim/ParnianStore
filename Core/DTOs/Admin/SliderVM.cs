using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Core.DTOs.Admin
{
    public class SliderVM
    {
        public int SliderId { get; set; }
        [Display(Name = "عنوان")]
        [StringLength(100, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد!")]
        public string SliderTitle { get; set; }
        [Display(Name = "کلاس عنوان")]
        [StringLength(50, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد!")]
        public string SliderTitleClass { get; set; }
        [Display(Name = "افکت نمایشی عنوان")]
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
        [Display(Name = "تصویر")]
        [MaxLength(50, ErrorMessage = "{0} نمی تواند بیشتر از {1} باشد!")]
        public string SliderImage { get; set; }
        [MaxLength(100, ErrorMessage = "{0} نمی تواند بیشتر از {1} باشد!")]
        [Display(Name = "لینک")]
        public string SliderLink { get; set; }
        [Display(Name = "متن لینک")]
        public string SliderLinkText { get; set; }
        [Display(Name = "کلاس لینک")]
        [StringLength(50, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد!")]
        public string SliderLinkClass { get; set; }
        [Display(Name = "افکت نمایشی لینک")]
        [StringLength(100, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد!")]
        public string SliderLink_DataAppear { get; set; }
        [Display(Name = "فعال/غیرفعال")]
        public bool SliderIsActive { get; set; }
        [RegularExpression(@"^$|^([1۱][۰-۹ 0-9]{3}[/\/]([0 ۰][۱-۶ 1-6])[/\/]([0 ۰][۱-۹ 1-9]|[۱۲12][۰-۹ 0-9]|[3۳][01۰۱])|[1۱][۰-۹ 0-9]{3}[/\/]([۰0][۷-۹ 7-9]|[1۱][۰۱۲012])[/\/]([۰0][1-9 ۱-۹]|[12۱۲][0-9 ۰-۹]|(30|۳۰)))$", ErrorMessage = "تاریخ وارد شده نامعتبر است.")]
        [Display(Name = "تاریخ شروع نمایش")]
        public string SliderStartDate { get; set; }
        [Display(Name = "زمان شروع")]
        [RegularExpression("(([01][0-9])|(2[0-3])):([0-5][0-9])$", ErrorMessage = "زمان نامعتبر است !")]
        public string SliderStartTime { get; set; }
        [RegularExpression(@"^$|^([1۱][۰-۹ 0-9]{3}[/\/]([0 ۰][۱-۶ 1-6])[/\/]([0 ۰][۱-۹ 1-9]|[۱۲12][۰-۹ 0-9]|[3۳][01۰۱])|[1۱][۰-۹ 0-9]{3}[/\/]([۰0][۷-۹ 7-9]|[1۱][۰۱۲012])[/\/]([۰0][1-9 ۱-۹]|[12۱۲][0-9 ۰-۹]|(30|۳۰)))$", ErrorMessage = "تاریخ وارد شده نامعتبر است.")]
        [Display(Name = "تاریخ پایان نمایش")]
        public string SliderEndDate { get; set; }
        [Display(Name = "زمان پایان")]
        [RegularExpression("(([01][0-9])|(2[0-3])):([0-5][0-9])$", ErrorMessage = "زمان نامعتبر است !")]
        public string SliderEndTime { get; set; }
    }
}
