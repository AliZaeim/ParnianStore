using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataLayer.Entities.Supplementary
{
    public class FSText
    {
        [Key]
        public int Id { get; set; }
        [Display(Name ="متن")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [StringLength(200, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد!")]
        public string Text { get; set; }
        [Display(Name ="کلاس متن")]
        [StringLength(50, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد!")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public string TextClass { get; set; }
        [Display(Name ="مختصات-data-postion")]
        [StringLength(50, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد!")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public string DataPosition { get; set; }
        [Display(Name ="جهت ورود-data-in")]
        [StringLength(50, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد!")]
        public string DataIn { get; set; }
        [Display(Name ="جهت خروج-data-out")]
        [StringLength(50, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد!")]
        public string DataOut { get; set; }
        [Display(Name = "انمیشن ورود-data-ease-in")]
        [StringLength(50, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد!")]
        public string DataEaseIn { get; set; }
        [Display(Name ="رتبه ورود-data-step")]
        [StringLength(2, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد!")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public string DataStep { get; set; }
        [Display(Name = "رتبه نمایش")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public int? ShowRating { get; set; }
        [Display(Name ="تاخیر به میلی ثانیه-data-delay")]
        [StringLength(10, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد!")]
        public string DataDelay { get; set; }
        [Display(Name ="افکت نمایش رتبه-data-special")]
        [StringLength(50, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد!")]
        public string DataSpecial { get; set; }
        [Display(Name = "فعال/غیرفعال")]
        public bool IsActive { get; set; }
        [NotMapped]
        public string ReturnUrl { get; set; }
        [Display(Name = "اسلاید")]
        public int FSId { get; set; }
        #region Relations
        [ForeignKey(nameof(FSId))]
        [Display(Name = "اسلاید")]
        public FractionSlider FractionSlider { get; set; }
        #endregion

    }
}
