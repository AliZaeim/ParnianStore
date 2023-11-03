using DataLayer.Entities.Permissions;
using DataLayer.Entities.Store;
using DataLayer.Entities.Supplementary;
using DataLayer.Entities.User;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Core.Services.Interfaces
{
    public interface IUserService
    {
        #region Generic

        public Task<InitialInfo> GetInitialInfoAsync();
        public bool SendVerificationCode(string code, string phoneNumber);
        public bool SendPassword(string pass, string phoneNumber);
        public bool SendVerification(string code, string phoneNumber);
        public bool SendUserName_and_Password(string userName, string password, string phoneNumber);
        public ContactInfo GetLastContactInfo();
        public Task<List<County>> GetCounties();
        public Task<State> GetStateByIdAsync(int sId);
        public Task<County> GetCountyByIdAsync(int cId);
        public Task<List<State>> GetStates();
        public Task<List<County>> GetCountiesofState(int sId);
        public Task<SitePage> GetSitePageByEnNameAsync(string EnName);
        public Task<Cart> GetUserCartByCookieAsync(string cartId);
        public void EditCart(Cart cart);
        public void SaveChanges();
        public Task SaveChangesAsync();
        #endregion Generic
        #region User
        public void CreateUser(User user);
        public void UpdateUser(User user);
        public Task<List<User>> GetUsersAsync();
        public Task<User> GetUserByIdAsync(int? Id);
        public Task<User> GetUserByCellphoneAsync(string cellphone);
        public Task<User> GetUserByPasswordAsync(string password);
        public Task<User> GetUserByUserName(string userName);
        public Task<List<Role>> GetRolesOfUserAsync(int userId);
        #endregion User
        #region UserRole
        public void CreateUserRole(UserRole userRole);
        public void EditUserRole(UserRole userRole);
        public Task<bool> ExistUserRole(int UserId, int RoleId);
        public Task<List<UserRole>> GetUserRolesAsync();
        public Task<UserRole> GetUserRoleById(int Id);
        #endregion UserRole
        #region Permission
        Task<List<Permission>> GetAllPermissions();
        Task<List<Permission>> GetPermissions_of_RoleByRoleId(int roleId);
        public bool CheckPermissionById(int permissionId, string userName);
        public bool CheckPermissionByName(string PermissionName, string UserName);

        #endregion
        #region RolePermission
        public void CreateRolePermission(RolePermission rolePermission);
        public void UpdateRolePermission(RolePermission rolePermission);
        public Task<List<RolePermission>> GetRolePermissionsAsync();
        public Task<RolePermission> GetRolePermissionById(int id);
        public bool ExistRolePermission(int id);
        public Task<List<RolePermission>> GetRolePermissionByRoleIdAsync(int roleId);

        
        
        //void CreatePermision(Permission permission);
        public Task<bool> AddPermissionsToRole(int roleId, List<int> permission);
        public Task<bool> RemovePermissionsFromRole(int roleId, List<int> permission);
        public Task<List<int>> PermissionsofRole(int roleId);
        public Task<bool> UpdatePermissionsRole(int roleId, List<int> Newpermissions);

        //bool CheckPermission(int permissionId, string userName);
       
        #endregion RolePermission
        #region Role
        public void CreateRole(Role role);
        public void EditRole(Role role);
        public Task<Role> GetRoleByIdAsync(int id);
        public void FullRemoveRoleById(int id);
        public Task<List<Role>> GetRolesAsync();
        public Task<List<Role>> GetRolesForRegisterAsync();
        public bool ExistRoleById(int id);
        public Task<bool> ExistRoleByName(string roleName);
        public Task<bool> RoleHasUser(int roleId);
        
        #endregion Role
        #region FAQ
        public Task<List<FAQ>> GetFAQsAsync();
        public Task<List<FAQ>> GetFAQsByNameAsync(string UserName);
        public Task<List<ContactMessage>> GetContactMessagesAsync();
        public Task<List<ContactMessage>> GetContactMessagesByNameAsync(string UserName);
        public Task<List<FAQ>> GetTodayFaqs();
        public Task<bool> ExistTodayFaq();
        #endregion
        #region ContactMessage
        public Task<List<ContactMessage>> GetTodayContactMessages();
        public Task<bool> ExistTodayContactMessage();
        #endregion
        #region EmailBank
        public void CreateEmail(EmailBank emailBank);
        public Task<List<EmailBank>> GetEmailBanksAsync();
        public void RemoveEmailById(int Id);
        public Task<bool> ExistEmail(string email);
        #endregion
        
    }
}
