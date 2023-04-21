using CORE.Abstract;
using CORE.Concrete;
using CORE.Config;
using ENTITIES.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace API.Attributes;

public class RbacAttribute : AuthorizeAttribute, IAuthorizationFilter
{
    private readonly UserType[] _role;

    public RbacAttribute(params UserType[] role)
    {
        _role = role;
    }

    public void OnAuthorization(AuthorizationFilterContext context)
    {
        var configSettings = (context.HttpContext.RequestServices.GetService(typeof(ConfigSettings)) as ConfigSettings)!;
        var utilService = (context.HttpContext.RequestServices.GetService(typeof(IUtilService)) as UtilService)!;

        string? jwtToken = context.HttpContext.Request.Headers[configSettings.AuthSettings.HeaderName];

        var validationResult = utilService.GetRoleFromToken(jwtToken);

        if (_role.All(m => m.ToString() != validationResult))
            context.Result = new ForbidResult();
    }
}