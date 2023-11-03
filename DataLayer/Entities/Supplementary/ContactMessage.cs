using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataLayer.Entities.Supplementary
{
    public class ContactMessage
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [StringLength(200, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد!")]
        [Display(Name = "نام کامل")]
        public string FullName { get; set; }
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [StringLength(11, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد!")]
        [RegularExpression("^[0][1-9]\\d{9}$|^[1-9]\\d{9}$", ErrorMessage = " شماره تلفن همراه نا معتبر است !")]
        [Display(Name = "تلفن همراه")]
        public string Cellphone { get; set; }
        [Display(Name = "ایمیل")]
        [StringLength(200, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد!")]
        public string Email { get; set; }
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [StringLength(200, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد!")]        
        [Display(Name = "موضوع پیام")]        
        public string Title { get; set; }
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [Display(Name = "پــیام")]
        public string Message { get; set; }
        [Display(Name ="تاریخ ثبت")]
        public DateTime CreatedDate { get; set; }
        [NotMapped]
        public IList<string> MessageLines
        {
            get { return (Message ?? string.Empty).Split(Environment.NewLine); }
        }
       
    }
}
