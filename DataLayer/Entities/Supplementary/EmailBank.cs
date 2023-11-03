using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Entities.Supplementary
{
    public class EmailBank
    {
        [Key]
        public int Id { get; set; }
        [Display(Name ="ایمیل")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [StringLength(150, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد!")]
        public string Email { get; set; }
        [Display(Name ="تاریخ ثبت")]
        public DateTime? CreatedDate { get; set; }
    }
}
