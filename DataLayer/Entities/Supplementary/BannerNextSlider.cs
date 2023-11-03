using System;
using System.ComponentModel.DataAnnotations;

namespace DataLayer.Entities.Supplementary
{
    public class BannerNextSlider
    {
        [Key]
        public int Id { get; set; }
        [Display(Name ="بنر اول")]
        [StringLength(200, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد!")]       
        public string Banner1 { get; set; }
        [Display(Name ="توضیحات بنر اول")]
        [StringLength(100, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد!")]
        public string Comment1 { get; set; }
        [Display(Name ="لینک بنر اول")]
        [StringLength(200, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد!")]
        public string Banner1Link { get; set; }
        [Display(Name = "بنر دوم")]
        [StringLength(200, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد!")]
        public string Banner2 { get; set; }
        [Display(Name = "توضیحات بنر دوم")]
        [StringLength(100, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد!")]
        public string Comment2 { get; set; }
        [Display(Name = "لینک بنر دوم")]
        [StringLength(200, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد!")]
        public string Banner2Link { get; set; }
        [Display(Name ="فعال / غیرفعال")]
        public bool IsActive { get; set; }
        [Display(Name ="تاریخ ثبت")]
        public DateTime? CreatedDate { get; set; }
        [Display(Name ="بازدید")]
        public int Views { get; set; }
    }
}
