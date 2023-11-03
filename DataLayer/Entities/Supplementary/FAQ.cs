using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataLayer.Entities.Supplementary
{
    public class FAQ
    {
        [Key]
        public int Id { get; set; }
        [Display(Name = "تاریخ")]
        public DateTime CreateDate { get; set; }
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [Display(Name = "نام کامل")]
        [StringLength(100, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد!")]
        public string FullName { get; set; }
        [StringLength(11, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد!")]
        [RegularExpression("^[0][1-9]\\d{9}$|^[1-9]\\d{9}$", ErrorMessage = " شماره تلفن همراه نا معتبر است !")]
        [Display(Name = "تلفن همراه")]
        public string Cellphone { get; set; }
        [RegularExpression(@"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$",ErrorMessage ="ایمیل نامعتبر است !")]
        [Display(Name = "ایمیل")]
        [StringLength(100, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد!")]
        public string Email { get; set; }
        [Display(Name = "پرسش")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [StringLength(300, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد!")]
        public string Question { get; set; }
        [Display(Name = "پاسخ")]
        [StringLength(300, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد!")]
        public string Reply { get; set; }
        [Display(Name ="فعال / غیرفعال")]
        public bool IsActive { get; set; }
        [NotMapped]
        public IList<string> QuestionLines
        {
            get { return (Question ?? string.Empty).Split(Environment.NewLine); }
        }
        [NotMapped]
        public IList<string> ReplyLines
        {
            get { return (Reply ?? string.Empty).Split(Environment.NewLine); }
        }
    }
}
