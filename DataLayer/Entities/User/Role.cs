using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DataLayer.Entities.User
{
    public class Role
    {
        public Role()
        {
            UserRoles = new HashSet<UserRole>();
        }
        [Key]
        public int RoleId { get; set; }
        [Display(Name = "نام نقش")]
        [MaxLength(30, ErrorMessage = "{0} نمی تواند بیشتر از {1} باشد!")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public string RoleName { get; set; }
        [Display(Name = "عنوان نقش")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(30, ErrorMessage = "{0} نمی تواند بیشتر از {1} باشد!")]
        public string RoleTitle { get; set; }
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [Display(Name = "ضریب نقش")]
        public float RoleRate { get; set; } = 1;
        [Display(Name ="استفاده برای ثبت نام")]
        public bool UseForRegister { get; set; }
        [Display(Name ="وضعیت حذف")]
        public bool IsDeleted { get; set; }
        
        #region Relations
        public virtual ICollection<UserRole> UserRoles { get; set; }
       
        #endregion
    }
}
