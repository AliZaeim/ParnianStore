using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataLayer.Entities.Supplementary
{
    public class FSImage
    {
        [Key]
        public int Id { get; set; }
        [Display(Name ="عکس")]
        public string Image { get; set; }
        [Display(Name ="عرض")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public int? Width { get; set; }
        [Display(Name ="ارتفاع")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public int? Height { get; set; }
        [Display(Name ="مختصات-data-position")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [StringLength(50, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد!")]
        public string DataPosition { get; set; }
        [Display(Name = "رتبه ورود-data-step")]
        [StringLength(2, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد!")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public string DataStep { get; set; }
        [Display(Name = "رتبه نمایش")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public int? ShowRating { get; set; }
        [Display(Name ="جهت ورود-data-in")]
        [StringLength(50, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد!")]
        public string DataIn { get; set; }
        [Display(Name = "جهت خروج-data-out")]
        [StringLength(50, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد!")]
        public string DataOut { get; set; }
        [Display(Name ="زمان تاخیر به میلی ثانیه-data-delay")]
        [StringLength(50, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد!")]
        public string DataDelay { get; set; }
        [Display(Name ="افکت حرکتی ورود-data-ease-in")]
        [StringLength(50, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد!")]
        public string DataEaseIn { get; set; }
        [Display(Name ="فعال/غیرفعال")]
        public bool IsActive { get; set; }
        [NotMapped]
        public string ReturnUrl { get; set; }
        [Display(Name ="اسلاید")]
        public int FSId { get; set; }
        #region Relations
        [ForeignKey(nameof(FSId))]
        [Display(Name = "اسلاید")]
        public FractionSlider FractionSlider { get; set; }
        #endregion

    }
}
