using DataLayer.Entities.Permissions;
using DataLayer.Entities.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTOs.Admin
{
    public class RolePermissionVM
    {
        public int RoleId { get; set; }
        public Role Role { get; set; }
        public List<Permission> Permissions { get; set; }
        public List<int> SelectedPermissions { get; set; }
        public string Title { get; set; }
    }
}
