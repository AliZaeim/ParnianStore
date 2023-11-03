using DataLayer.Entities.Store;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DataLayer.Entities.Supplementary
{
    public class Banner
    {
        [Key]
        public int Id { get; set; }
        [Display(Name ="توضیحات")]
        [StringLength(200, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد!")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public string Comment { get; set; }
        [Display(Name ="تصویر دسکتاپی بنر اول")]        
        [StringLength(100, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد!")]
        public string Banner1 { get; set; }
        
        [Display(Name = "تصویر موبایلی بنر اول")]        
        [StringLength(100, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد!")]
        public string Banner1Mobile { get; set; }
        [Display(Name ="لینک بنر اول")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [StringLength(100, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد!")]
        public string Banner1_link { get; set; }
        [Display(Name ="توضیحات تصویر اول")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [StringLength(120, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد!")]
        public string Banner1_Alt { get; set; }
        [Display(Name = "تصویر دسکتاپی بنر دوم")]       
        public string Banner2 { get; set; }
        
        [Display(Name = "تصویر موبایلی بنر دوم")]        
        [StringLength(100, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد!")]
        public string Banner2Mobile { get; set; }
        [Display(Name ="لینک بنر دوم")]        
        public string Banner2_link { get; set; }
        [Display(Name = "توضیحات تصویر دوم")]       
        [StringLength(120, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد!")]
        public string Banner2_Alt { get; set; }
        [Display(Name = "تصویر دسکتاپی بنر سوم")]
        public string Banner3 { get; set; }
        
        [Display(Name = "تصویر موبایلی بنر سوم")]        
        [StringLength(100, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد!")]
        public string Banner3Mobile { get; set; }
        [Display(Name = "لینک بنر سوم")]
        public string Banner3_link { get; set; }
        [Display(Name = "توضیحات تصویر سوم")]
        [StringLength(120, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد!")]
        public string Banner3_Alt { get; set; }
        [Display(Name = "تصویر دسکتاپی بنر چهارم")]
        public string Banner4 { get; set; }
       
        [Display(Name = "تصویر موبایلی بنر چهارم")]       
        [StringLength(100, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد!")]
        public string Banner4Mobile { get; set; }
        [Display(Name = "لینک بنر چهارم")]
        public string Banner4_link { get; set; }
        [Display(Name = "توضیحات تصویر چهارم")]
        [StringLength(120, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد!")]
        public string Banner4_Alt { get; set; }
        [Display(Name ="فعال/غیرفعال")]
        public bool IsActive { get; set; }
        [Display(Name ="تاریخ ثبت")]
        public DateTime? CreatedDate { get; set; }

        #region Relations
        public ICollection<ProductGroup> ProductGroups { get; set; }
        #endregion
    }
}
