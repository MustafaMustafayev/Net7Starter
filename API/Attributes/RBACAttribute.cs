using CORE.Abstract;
using CORE.Concrete;
using CORE.Config;
using CORE.Enums;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace API.Attributes
{
    public class RBACAttribute : AuthorizeAttribute, IAuthorizationFilter
    {
        private readonly ERole[] _role;
        public RBACAttribute(params ERole[] role)
        {
            _role = role;
        }
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var configSettings = (context.HttpContext.RequestServices.GetService(typeof(ConfigSettings)) as ConfigSettings)!;
            var utilService = (context.HttpContext.RequestServices.GetService(typeof(IUtilService)) as UtilService)!;

            string? jwtToken = context.HttpContext.Request.Headers[configSettings.AuthSettings.HeaderName];

            var validationResult = utilService.GetRoleFromToken(jwtToken);

            if (!_role.Any(m => m.ToString() == validationResult))
                context.Result = new ForbidResult();
        }
    }
}
