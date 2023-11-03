using System.ComponentModel.DataAnnotations;

namespace DataLayer.Entities.Supplementary
{
    public class UploadCenter
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [StringLength(200, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد!")]
        [Display(Name = "عنوان")]
        public string Title { get; set; }
        [StringLength(100, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد!")]
        [Display(Name = "فایل")]
        public string File { get; set; }
        [Display(Name = "توضیحات")]
        [StringLength(300, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد!")]
        public string Comment { get; set; }
        [Display(Name = "نام فایل")]
        [StringLength(30, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد!")]
        public string FileName { get; set; }
    }
}
