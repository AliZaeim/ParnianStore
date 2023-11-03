using Core.Services.Interfaces;
using DataLayer.Entities.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Threading.Tasks;

namespace Core.Security
{
    public class PermissionCheckerByPermissionIdAttribute : AuthorizeAttribute, IAuthorizationFilter
    {
        private IUserService _userService;
        private readonly int _permissionId = 0;
        public PermissionCheckerByPermissionIdAttribute( int permissionId)
        {
            
            _permissionId = permissionId;
        }
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            _userService = (IUserService)context.HttpContext.RequestServices.GetService(typeof(IUserService));
            if (context.HttpContext.User.Identity.IsAuthenticated)
            {
                string username = context.HttpContext.User.Identity.Name;                
                if (!_userService.CheckPermissionById(_permissionId, username))
                {
                    context.Result = new ForbidResult();
                }
            }
            else
            {
                context.Result = new RedirectResult("/Login");
            }
        }

        
    }
}
