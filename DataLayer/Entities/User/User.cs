using DataLayer.Entities.Store;
using DataLayer.Entities.Supplementary;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataLayer.Entities.User
{
    public class User
    {
        public User()
        {
            UserRoles = new HashSet<UserRole>();
        }
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(100, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد!")]
        [Display(Name = "نام")]
        public string Name { get; set; }
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(100, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد!")]
         [Display(Name = "نام خانوادگی")]
        public string Family { get; set; }
        
        [Display(Name = "تلفن همراه")] 
        [StringLength(20, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد!")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public string Cellphone { get; set; }
        
        [Display(Name = "تاریخ تولد")]
        public DateTime? BirthDate { get; set; }
        [Display(Name = "ایمیل")]
        [StringLength(200, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد!")]
        public string Email { get; set; }
        [Display(Name = "نام کاربری")]
        [StringLength(20, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد!")]
        [MinLength(6, ErrorMessage = "{0} نمی تواند کمتر از {1} کاراکتر باشد!")]
        
        public string UserName { get; set; }
        [Display(Name = "رمز عبور")]
        [MinLength(8, ErrorMessage = "{0} نمی تواند کمتر از {1} کاراکتر باشد!")]
        [StringLength(200, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد!")]
        public string Password { get; set; }
        [StringLength(30, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد!")]
        [Display(Name = "شماره کارت")]
        public string UserCreditCardNumber { get; set; }
        [StringLength(50, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد!")]
        [Display(Name = "حساب بانکی")]
        public string UserBankAccountNumber { get; set; }
        [Display(Name = "جنسبت")]
        [StringLength(50, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد!")]
        public string Sex { get; set; }
        [Display(Name ="کد تجاری")]
        [StringLength(50, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد!")]
        public string ReferralCode { get; set; }
        [Display(Name ="فعال/غیرفعال")]
        public bool IsActive { get; set; }
        
        [Display(Name ="تاریخ ثبت نام")]
        public DateTime RegisteredDate { get; set; }
        [Display(Name ="آدرس")]
        [StringLength(200, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد!")]
        public string Address { get; set; }
        [Display(Name ="کد پستی")]
        [StringLength(30, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد!")]
        public string PostalCode { get; set; }
        [NotMapped]
        [Display(Name = "نام کامل")]
        public string FullName    // the FullName property
        {
            get
            {
                return Name.Trim() + " " + Family.Trim();
            }
        }
        [Display(Name = "شهرستان")]
        
        public int? CountyId { get; set; }
        #region Relations
        [Display(Name = "شهرستان")]
        [ForeignKey("CountyId")]
        public County County { get; set; }
        public virtual ICollection<UserRole> UserRoles { get; set; }
        #endregion


    }
   
}
