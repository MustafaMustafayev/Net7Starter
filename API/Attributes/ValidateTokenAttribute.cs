using BLL.Abstract;
using BLL.Concrete;
using CORE.Abstract;
using CORE.Concrete;
using CORE.Config;
using CORE.Constants;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace API.Attributes;

// [AttributeUsage(AttributeTargets.Method)]
public class ValidateTokenAttribute : Attribute, IAuthorizationFilter
{
    public void OnAuthorization(AuthorizationFilterContext context)
    {
        if (Constants.AllowAnonymous.Contains(context.HttpContext.Request.Path.Value)) return;

        var configSettings =
            (context.HttpContext.RequestServices.GetService(typeof(ConfigSettings)) as ConfigSettings)!;
        var tokenService = (context.HttpContext.RequestServices.GetService(typeof(ITokenService)) as TokenService)!;
        var utilService = (context.HttpContext.RequestServices.GetService(typeof(IUtilService)) as UtilService)!;

        string? jwtToken = context.HttpContext.Request.Headers[configSettings.AuthSettings.HeaderName];
        string? refreshToken = context.HttpContext.Request.Headers[configSettings.AuthSettings.RefreshTokenHeaderName];

        var validationResult = tokenService
            .CheckValidationAsync(utilService.GetTokenStringFromHeader(jwtToken), refreshToken!).Result;

        if (!validationResult.Success)
            context.Result = new UnauthorizedObjectResult(validationResult);
    }
}