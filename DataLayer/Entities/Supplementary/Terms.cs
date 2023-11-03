using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Entities.Supplementary
{
    public class Terms
    {
        [Key]
        public int Id { get; set; }
        [Display(Name ="عنوان")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [StringLength(100, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد!")]
        public string Title { get; set; }
        [Display(Name ="تاریخ ثبت")]
        public DateTime? CreatedDate { get; set; }
        [Display(Name ="توضیحات")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public string Text { get; set; }
        [Display(Name ="رتبه نمایش")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public int? ShowRating { get; set; }
    }
}
