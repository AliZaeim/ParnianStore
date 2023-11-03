using System;
using System.ComponentModel.DataAnnotations;

namespace DataLayer.Entities.Supplementary
{
    public class UniqueKey
    {
        [Key]
        public int Id { get; set; }
        [Display(Name = "کلید")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(10, ErrorMessage = "{0} نمی تواند بیشتر از {1} باشد!")]
        public string Key { get; set; }
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [Display(Name = "نام جدول")]
        [MaxLength(20, ErrorMessage = "{0} نمی تواند بیشتر از {1} باشد!")]
        public string TableName { get; set; }
        [Display(Name = "تاریخ")]
        public DateTime DateTime { get; set; }
    }
}
