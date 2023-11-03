using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Entities.Store
{
    public class DiscountCoupen
    {
        [Key]
        public int Id { get; set; }
        [Display(Name ="کد تخفیف")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [StringLength(20, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد!")]
        public string Code { get; set; }
        [Display(Name ="توضیحات")]
        [StringLength(200, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد!")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public string Comment { get; set; }
        [Display(Name ="تعداد")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public int? Number { get; set; }
        [Display(Name ="درصد تخفیف")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public float Percent { get; set; }
        [Display(Name ="تاریخ پایان اعتبار")]        
        public DateTime? ExpiredDate { get; set; }
        [Display(Name ="فعال/غیرفعال")]
        public bool IsActive { get; set; }
        
    }
}
