using DataLayer.Entities.User;
using System.ComponentModel.DataAnnotations;

namespace DataLayer.Entities.Permissions
{
    public class RolePermission
    {
        [Key]
        public int RP_Id { get; set; }
        public int RoleId { get; set; }
        public int PermissionId { get; set; }
        [Display(Name ="نقش")]
        public Role Role { get; set; }
        [Display(Name ="دسترسی")]
        public Permission Permission { get; set; }
    }
}
