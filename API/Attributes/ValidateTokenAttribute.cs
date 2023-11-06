using BLL.Abstract;
using CORE.Abstract;
using CORE.Config;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace API.Attributes;

// [AttributeUsage(AttributeTargets.Method)]
public class ValidateTokenAttribute : Attribute, IAuthorizationFilter
{
    public void OnAuthorization(AuthorizationFilterContext context)
    {
        var hasAllowAnonymous = context.ActionDescriptor.EndpointMetadata.OfType<AllowAnonymousAttribute>().Any();
        if (hasAllowAnonymous) return;

        var configSettings =
            (context.HttpContext.RequestServices
            .GetService(typeof(ConfigSettings)) as ConfigSettings)!;
        var tokenService = (context.HttpContext.RequestServices
            .GetService(typeof(ITokenService)) as ITokenService)!;
        var utilService = (context.HttpContext.RequestServices
            .GetService(typeof(IUtilService)) as IUtilService)!;

        string? jwtToken = context.HttpContext.Request
            .Headers[configSettings.AuthSettings.HeaderName];
        string? refreshToken = context.HttpContext.Request
            .Headers[configSettings.AuthSettings.RefreshTokenHeaderName];

        jwtToken = utilService.TrimToken(jwtToken);

        var validationResult = tokenService
            .CheckValidationAsync(jwtToken, refreshToken!).Result;

        if (!validationResult.Success)
            context.Result = new UnauthorizedObjectResult(validationResult);
    }
}