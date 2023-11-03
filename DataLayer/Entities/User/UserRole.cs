using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataLayer.Entities.User
{
    public class UserRole
    {
        public UserRole()
        {

        }
        [Key]
        public int URId { get; set; }
        
        [Required]
        public int UserId { get; set; }
        
        [Required]
        public int RoleId { get; set; }
       
        [Required]
        public DateTime RegisterDate { get; set; }
        public bool IsActive { get; set; }
        [MaxLength(20)]
        public string UserRoleRegDiscountCode { get; set; }
        [MaxLength(100)]
        public string UserBusinessName { get; set; }
        public float UserBusinessPercent { get; set; }
        public bool IsDeleted { get; set; }
        #region Relations  
        [ForeignKey("UserId")]
        public User User { get; set; }
        [ForeignKey("RoleId")]
        public  Role Role { get; set; }
       
        #endregion
        [NotMapped]
        [Display(Name = "نام کامل")]
        public string FullName    // the FullName property
        {
            get
            {
                return User?.Name.Trim() + " " + User?.Family.Trim() + " | " +Role?.RoleTitle;
            }
        }

    }
}
