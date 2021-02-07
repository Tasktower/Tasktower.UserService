using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Tasktower.UserService.Security.Auth
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class AuthorizeAttribute : Attribute, IAuthorizationFilter
    {
        private readonly ISet<Role> _roles;
        private readonly bool _emailVerifyRequired;

        public AuthorizeAttribute(params Role[] roles)
        {
            _roles = new HashSet<Role>(roles);
            _emailVerifyRequired = false;
        }
        public AuthorizeAttribute(bool emailVerifyRequired, params Role[] roles)
        {
            _roles = new HashSet<Role>(roles);
            _emailVerifyRequired = emailVerifyRequired;
        }
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var authContext = new AuthContext(context.HttpContext);
            var userData = authContext.UserAuthData;
            if (userData == null || !userData.XSRFToken.SequenceEqual(authContext.XSRFToken))
            {
                context.Result = new JsonResult(new { message = "Unauthorized" }) { StatusCode = StatusCodes.Status401Unauthorized };
                return;
            }
            if((_emailVerifyRequired && !userData.EmailVerified) || !userData.Roles.Any(r => _roles.Contains(r)))
            {
                context.Result = new JsonResult(new { message = "Forbidden" }) { StatusCode = StatusCodes.Status403Forbidden };
            }         
        }
    }
}
