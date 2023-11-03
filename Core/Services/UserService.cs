using DataLayer.Context;
using DataLayer.Entities.Permissions;
using DataLayer.Entities.Store;
using DataLayer.Entities.Supplementary;
using DataLayer.Entities.User;
using Microsoft.EntityFrameworkCore;
using SmsIrRestfulNetCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Core.Services.Interfaces
{
    public class UserService : IUserService
    {
        private readonly MyContext _Context;
        public UserService(MyContext Context)
        {
            _Context = Context;
        }

        #region Generic
        public async Task<Cart> GetUserCartByCookieAsync(string cartId)
        {
            Cart crt = await _Context.Carts.Include(r => r.CartItems).SingleOrDefaultAsync(x => x.Id == Guid.Parse(cartId));
            return crt;
        }

        public void EditCart(Cart cart)
        {
            _Context.Carts.Update(cart);
        }
        public async Task<State> GetStateByIdAsync(int sId)
        {
            return await _Context.States.Include(x => x.Counties).SingleOrDefaultAsync(x => x.StateId == sId);
        }
        public async Task<SitePage> GetSitePageByEnNameAsync(string EnName)
        {
            return await _Context.SitePages.SingleOrDefaultAsync(x => x.EnName.Equals(EnName));
        }
        public async Task<List<County>> GetCounties()
        {
            return await _Context.Counties.Include(r => r.State).ToListAsync();
        }
        public async Task<List<State>> GetStates()
        {
            return await _Context.States.Include(r => r.Counties).ToListAsync();
        }
        public async Task<List<County>> GetCountiesofState(int sId)
        {
            State state = await _Context.States.Include(r => r.Counties).FirstOrDefaultAsync(x => x.StateId == sId);

            return state.Counties.ToList();
        }

        public bool SendVerificationCode(string code, string phoneNumber)
        {
            var token = new Token().GetToken("47c1d5e77e3ca5622a1dda26", "&*144543295858ABZ@100#");

            var restVerificationCode = new RestVerificationCode()
            {
                Code = code,
                MobileNumber = phoneNumber
            };

            var restVerificationCodeRespone = new VerificationCode().Send(token, restVerificationCode);
            if (restVerificationCode != null)
            {
                if (restVerificationCodeRespone.IsSuccessful)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }

        }
        public bool SendUserName_and_Password(string userName, string password, string phoneNumber)
        {
            var token = new Token().GetToken("47c1d5e77e3ca5622a1dda26", "&*144543295858ABZ@100#");

            var ultraFastSend = new UltraFastSend()
            {
                Mobile = long.Parse(phoneNumber),
                TemplateId = 48138,
                ParameterArray = new List<UltraFastParameters>()
            {
                new UltraFastParameters()
                {
                    Parameter="username", ParameterValue=userName

                },
                new UltraFastParameters()
                {
                     Parameter = "password" , ParameterValue = password
                }
            }.ToArray()

            };

            UltraFastSendRespone ultraFastSendRespone = new UltraFast().Send(token, ultraFastSend);

            return ultraFastSendRespone.IsSuccessful;
        }
        public bool SendPassword(string pass, string phoneNumber)
        {
            var token = new Token().GetToken("47c1d5e77e3ca5622a1dda26", "&*144543295858ABZ@100#");

            var ultraFastSend = new UltraFastSend()
            {
                Mobile = long.Parse(phoneNumber),
                TemplateId = 46671,
                ParameterArray = new List<UltraFastParameters>()
            {
                new UltraFastParameters()
                {
                    Parameter = "password" , ParameterValue = pass
                }
            }.ToArray()

            };

            UltraFastSendRespone ultraFastSendRespone = new UltraFast().Send(token, ultraFastSend);

            return ultraFastSendRespone.IsSuccessful;
        }
        public bool SendVerification(string code, string phoneNumber)
        {
            var token = new Token().GetToken("47c1d5e77e3ca5622a1dda26", "&*144543295858ABZ@100#");

            var ultraFastSend = new UltraFastSend()
            {
                Mobile = long.Parse(phoneNumber),
                TemplateId = 46669,
                ParameterArray = new List<UltraFastParameters>()
            {
                new UltraFastParameters()
                {
                     Parameter= "RegisterCode" , ParameterValue = code
                }
            }.ToArray()

            };

            UltraFastSendRespone ultraFastSendRespone = new UltraFast().Send(token, ultraFastSend);

            return ultraFastSendRespone.IsSuccessful;
        }
        public void SaveChanges()
        {
            _Context.SaveChanges();
        }

        public async Task SaveChangesAsync()
        {
            await _Context.SaveChangesAsync();
        }
        public async Task<InitialInfo> GetInitialInfoAsync()
        {
            return await _Context.InitialInfos.OrderByDescending(x => x.CreatedDate).FirstOrDefaultAsync();
        }
        public ContactInfo GetLastContactInfo()
        {
            return _Context.ContactInfos.OrderBy(r => r.Id).LastOrDefault();
        }
        public async Task<County> GetCountyByIdAsync(int cId)
        {
            return await _Context.Counties.Include(x => x.CountyId).SingleOrDefaultAsync(x => x.CountyId == cId);
        }
        #endregion Generic

        #region User
        public void CreateUser(User user)
        {
            _Context.Users.Add(user);
        }
        public async Task<User> GetUserByIdAsync(int? Id)
        {
            if (Id == null)
                return null;
            return await _Context.Users.SingleOrDefaultAsync(x => x.Id == Id);
        }
        public async Task<List<User>> GetUsersAsync()
        {
            return await _Context.Users.ToListAsync();
        }
        public void UpdateUser(User user)
        {
            _Context.Users.Update(user);
        }

        public async Task<User> GetUserByCellphoneAsync(string cellphone)
        {
            if (string.IsNullOrEmpty(cellphone))
            {
                return null;
            }
            User user = await _Context.Users.SingleOrDefaultAsync(x => x.Cellphone == cellphone);
            return user;
        }
        public async Task<User> GetUserByUserName(string userName)
        {
            if (string.IsNullOrEmpty(userName))
            {
                return null;
            }
            User user = await _Context.Users.Include(r => r.County).Include(r => r.County.State).SingleOrDefaultAsync(x => x.UserName == userName);
            return user;
        }
        public async Task<List<Role>> GetRolesOfUserAsync(int userId)
        {
            return await _Context.UserRoles.Include(x => x.User).Include(x => x.Role)
                .Where(w => w.UserId == userId).Select(x => x.Role).ToListAsync();
        }
        #endregion User
        #region UserRole
        public void CreateUserRole(UserRole userRole)
        {
            _Context.UserRoles.Add(userRole);
        }
        public void EditUserRole(UserRole userRole)
        {
            _Context.UserRoles.Update(userRole);
        }

        public async Task<bool> ExistUserRole(int UserId, int RoleId)
        {
            return await _Context.UserRoles.AnyAsync(x => x.UserId == UserId && x.RoleId == RoleId);

        }
        public async Task<List<UserRole>> GetUserRolesAsync()
        {
            return await _Context.UserRoles.Include(r => r.User).Include(r => r.User.County).Include(r => r.User.County.State).Include(r => r.Role).ToListAsync();
        }
        public async Task<UserRole> GetUserRoleById(int Id)
        {
            return await _Context.UserRoles.Include(x => x.User).Include(x => x.Role).SingleOrDefaultAsync(x => x.URId == Id);
        }
        #endregion UserRole
        #region Permissions
        public async Task<List<Permission>> GetAllPermissions()
        {
            return await _Context.Permissions.Include(r => r.Permissions).ToListAsync();
        }
        public async Task<List<Permission>> GetPermissions_of_RoleByRoleId(int roleId)
        {
            return await _Context.RolePermissions
               .Where(r => r.RoleId == roleId)
               .Select(s => s.Permission).ToListAsync();
        }
        public bool CheckPermissionById(int permissionId, string userName)
        {
            User user = _Context.Users.SingleOrDefault(u => u.UserName == userName);
            if (user == null)
                return false;

            List<int> UserRoles = _Context.UserRoles
                .Where(r => r.UserId == user.Id).Select(r => r.RoleId).ToList();

            if (!UserRoles.Any())
                return false;

            List<int> RolesPermission = _Context.RolePermissions
                .Where(p => p.PermissionId == permissionId)
                .Select(p => p.RoleId).ToList();

            return RolesPermission.Any(p => UserRoles.Contains(p));
        }
        public bool CheckPermissionByName(string PermissionName, string UserName)
        {
            User user = _Context.Users.SingleOrDefault(u => u.UserName == UserName);
            if (user == null)
                return false;

            List<int> UserRoles = _Context.UserRoles
                .Where(r => r.UserId == user.Id).Select(r => r.RoleId).ToList();

            if (!UserRoles.Any())
                return false;

            List<int> RolesPermission = _Context.RolePermissions.Include(x => x.Permission)
                .Where(p => p.Permission.PermissionName == PermissionName)
                .Select(p => p.RoleId).ToList();
            return RolesPermission.Any(p => UserRoles.Contains(p));
        }


        public async Task<User> GetUserByPasswordAsync(string password)
        {
            User user = await _Context.Users.SingleOrDefaultAsync(x => x.Password == password);
            return user;
        }

        #endregion Permissions
        #region RolePermission
        public void CreateRolePermission(RolePermission rolePermission)
        {
            _Context.RolePermissions.Add(rolePermission);
        }

        public void UpdateRolePermission(RolePermission rolePermission)
        {
            _Context.RolePermissions.Update(rolePermission);
        }

        public async Task<List<RolePermission>> GetRolePermissionsAsync()
        {
            return await _Context.RolePermissions.Include(x => x.PermissionId).Include(x => x.Role).ToListAsync();
        }
        public async Task<RolePermission> GetRolePermissionById(int id)
        {
            return await _Context.RolePermissions.Include(x => x.PermissionId).Include(x => x.Role).SingleOrDefaultAsync(x => x.PermissionId == id);
        }
        public bool ExistRolePermission(int id)
        {
            return _Context.RolePermissions.Any(x => x.PermissionId == id);
        }
        public async Task<List<RolePermission>> GetRolePermissionByRoleIdAsync(int roleId)
        {
            return await _Context.RolePermissions.Include(x => x.Role).Include(x => x.Permission)
                .Where(w => w.RoleId == roleId).ToListAsync();
        }
        public async Task<bool> AddPermissionsToRole(int roleId, List<int> permission)
        {
            List<RolePermission> rolePermissions = new();
            foreach (var p in permission)
            {
                rolePermissions.Add(new RolePermission
                {
                    PermissionId = p,
                    RoleId = roleId
                });
                
            }
            if(rolePermissions.Count!=0)
            {
                await _Context.RolePermissions.AddRangeAsync(rolePermissions);
            }
            return true;
        }
        public async Task<bool> RemovePermissionsFromRole(int roleId, List<int> permission)
        {
            List<RolePermission> rolePermissions = new();
            foreach (var p in permission)
            {
                rolePermissions.Add(new RolePermission
                {
                    PermissionId = p,
                    RoleId = roleId
                });

            }
            if (rolePermissions.Count != 0)
            {
                foreach (var item in rolePermissions)
                {
                    RolePermission remm =await _Context.RolePermissions.SingleOrDefaultAsync(x => x.RoleId == roleId && x.PermissionId == item.PermissionId);
                    if(remm != null)
                    {
                        _Context.RolePermissions.Remove(remm);
                    }
                
                }
                
            }
            return true;
        }
        public async Task<List<int>> PermissionsofRole(int roleId)
        {
            return await _Context.RolePermissions.Include(x => x.Role).Include(x => x.Permission)
                .Where(r => r.RoleId == roleId)
                .Select(r => r.PermissionId).ToListAsync();
        }

        public async Task<bool> UpdatePermissionsRole(int roleId, List<int> Newpermissions)
        {
            try
            {
                Role role = await _Context.Roles.SingleOrDefaultAsync(x => x.RoleId == roleId);
                if (role == null)
                    return false;
                List<RolePermission> rolePer = await _Context.RolePermissions.Include(x => x.Role).Include(x => x.Permission)
                                            .Where(w => w.RoleId == roleId).ToListAsync();
                List<Permission> Curpers = rolePer.Select(x => x.Permission).ToList();
                List<int> CurperIds = Curpers.Select(x => x.PermissionId).ToList();
                List<int> Remove_Newpermissions_Expect_CurperIds = CurperIds.Except(Newpermissions).ToList(); //return items from permissions that not in perIds
                List<int> Add_CurperIds_Expect_Newpermissions = Newpermissions.Except(CurperIds).ToList(); //return items from perIds that not in permissions

                bool RemRes = await RemovePermissionsFromRole(roleId, Remove_Newpermissions_Expect_CurperIds);
                bool AddRes = await AddPermissionsToRole(roleId, Add_CurperIds_Expect_Newpermissions);
                
                await _Context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                string m = ex.Message;
                return false;
            }
           
        }
        #endregion
        #region Roles
        public void CreateRole(Role role)
        {
            _Context.Roles.Add(role);
        }
        public async Task<List<Role>> GetRolesAsync()
        {
            return await _Context.Roles.Include(r => r.UserRoles).ToListAsync();
        }

        public async Task<List<Role>> GetRolesForRegisterAsync()
        {
            return await _Context.Roles.Where(w => w.UseForRegister).ToListAsync();
        }
        
        public void EditRole(Role role)
        {
            _Context.Roles.Update(role);
        }

        public async Task<Role> GetRoleByIdAsync(int id)
        {
            return await _Context.Roles.Include(x => x.UserRoles).SingleOrDefaultAsync(x => x.RoleId == id);
        }

        public void FullRemoveRoleById(int id)
        {
            Role role = _Context.Roles.Find(id);
            if(role != null)
            {
                _Context.Roles.Remove(role);
            }
        }
        public bool ExistRoleById(int id)
        {
            return _Context.Roles.Any(x => x.RoleId == id);
        }

        public async Task<bool> ExistRoleByName(string roleName)
        {            
            return await _Context.Roles.AnyAsync(x => x.RoleName.Equals(roleName.Trim()));
        }
        public async Task<bool> RoleHasUser(int roleId)
        {
            return await _Context.UserRoles.AnyAsync(x => x.RoleId == roleId);
        }

        #endregion Roles
        #region FAQ
        public async Task<List<FAQ>> GetFAQsAsync()
        {
            return await _Context.FAQs.ToListAsync();
        }

        public async Task<List<FAQ>> GetFAQsByNameAsync(string UserName)
        {
            User user = await _Context.Users.SingleOrDefaultAsync(w => w.UserName == UserName);
            if (user == null)
                return null;
            return await _Context.FAQs.Where(w => w.Cellphone == user.Cellphone).ToListAsync();
        }

        public async Task<List<ContactMessage>> GetContactMessagesAsync()
        {
            return await _Context.ContactMessages.ToListAsync();
        }

        public async Task<List<ContactMessage>> GetContactMessagesByNameAsync(string UserName)
        {
            User user = await _Context.Users.SingleOrDefaultAsync(w => w.UserName == UserName);
            if (user == null)
                return null;
            return await _Context.ContactMessages.Where(w => w.Cellphone == user.Cellphone).ToListAsync();
        }

        public async Task<List<FAQ>> GetTodayFaqs()
        {
            return await _Context.FAQs.Where(w => w.CreateDate.Date == DateTime.Now.Date).ToListAsync();
        }

        public async Task<List<ContactMessage>> GetTodayContactMessages()
        {
            return await _Context.ContactMessages.Where(w => w.CreatedDate.Date == DateTime.Now.Date).ToListAsync();
        }

        public async Task<bool> ExistTodayFaq()
        {
            return await _Context.FAQs.AnyAsync(w => w.CreateDate == DateTime.Now);
        }

        public async Task<bool> ExistTodayContactMessage()
        {
            return await _Context.ContactMessages.AnyAsync(w => w.CreatedDate == DateTime.Now);
        }
        #endregion
        #region EmailBank
        public void RemoveEmailById(int Id)
        {
            EmailBank emailBank = _Context.EmailBanks.Find(Id);
            if (emailBank != null)
            {
                _Context.EmailBanks.Remove(emailBank);
            }
        }

        public async Task<List<EmailBank>> GetEmailBanksAsync()
        {
            return await _Context.EmailBanks.ToListAsync();
        }

        public void CreateEmail(EmailBank emailBank)
        {
            _Context.EmailBanks.Add(emailBank);
        }

        public async Task<bool> ExistEmail(string email)
        {
            return await _Context.EmailBanks.AnyAsync(a => a.Email.Equals(email));
        }

        


        #endregion

    }
}
