using Core.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Core.Security
{
    public class PermissionCheckerByPermissionNameAttribute : AuthorizeAttribute, IAuthorizationFilter
    {
        private IUserService _userService;
        private readonly string _permissionName;
        public PermissionCheckerByPermissionNameAttribute(string permissionName)
        {

            _permissionName = permissionName;
        }
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            _userService = (IUserService)context.HttpContext.RequestServices.GetService(typeof(IUserService));
            if (context.HttpContext.User.Identity.IsAuthenticated)
            {
                string username = context.HttpContext.User.Identity.Name;
                if (!_userService.CheckPermissionByName(_permissionName, username))
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
