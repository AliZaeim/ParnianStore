using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DataLayer.Entities.Supplementary
{
    public class ContactInfo
    {
        [Key]
        public int Id { get; set; }
        [Display(Name = "آدرس ها")]
        
        [StringLength(500, ErrorMessage = "لطفا {1} را وارد کنید")]
        public string Addresses { get; set; }
        [Display(Name = "تلفن ها")]
        
        [StringLength(500, ErrorMessage = "لطفا {1} را وارد کنید")]
        public string Phones { get; set; }
        [Display(Name = "همراه ها")]
        [StringLength(500, ErrorMessage = "لطفا {1} را وارد کنید")]
        public string Cellphones { get; set; }
        [Display(Name = "ایمیل ها")]
        [StringLength(500, ErrorMessage = "لطفا {1} را وارد کنید")]
        public string Emailes { get; set; }
        [Display(Name = "فکس ها")]
        [StringLength(500, ErrorMessage = "لطفا {1} را وارد کنید")]
        public string Faxes { get; set; }
        [Display(Name ="آدرس اینستاگرام")]
        [StringLength(100, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد!")]
        public string InstagramLink { get; set; }
        [Display(Name ="آدرس تلگرام")]
        [StringLength(100, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد!")]
        public string TelegramLink { get; set; }
        [Display(Name = "آدرس فیس بوک")]
        [StringLength(100, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد!")]
        public string FacebookLink { get; set; }
        [Display(Name = "آدرس واتزآپ")]
        [StringLength(100, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد!")]
        public string Whatsapp { get; set; }
        [Display(Name ="تاریخ ثبت")]
        public DateTime CreatedDate { get; set; }

        public IEnumerable<string> PhoneList
        {
            get { return (Phones ?? string.Empty).Split(Environment.NewLine); }
        }
        public IEnumerable<string> CellPhoneList
        {
            get { return (Cellphones ?? string.Empty).Split(Environment.NewLine); }
        }
        public IEnumerable<string> AddressList
        {
            get { return (Addresses ?? string.Empty).Split(Environment.NewLine); }
        }
        public IEnumerable<string> EmailList
        {
            get { return (Emailes ?? string.Empty).Split(Environment.NewLine); }
        }
        public IEnumerable<string> FaxList
        {
            get { return (Faxes ?? string.Empty).Split(Environment.NewLine); }
        }
    }
}
