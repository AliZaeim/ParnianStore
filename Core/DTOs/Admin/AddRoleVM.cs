using DataLayer.Entities.User;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTOs.Admin
{
    public class AddRoleVM
    {
        public int UserId { get; set; }
        [Display(Name = "مسئولیت")]
        [Required(ErrorMessage = "لطفا {0} را انتخاب کنید")]
        public int? RoleId { get; set; }
        public string User_FullName { get; set; }

        public List<Role> Roles { get; set; }

    }
}
