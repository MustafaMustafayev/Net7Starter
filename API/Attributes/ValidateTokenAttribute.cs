using BLL.Abstract;
using BLL.Concrete;
using CORE.Abstract;
using CORE.Concrete;
using CORE.Config;
using CORE.Constants;
using CORE.Localization;
using DTO.Responses;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Messages = CORE.Localization.Messages;

namespace API.Attributes;

[AttributeUsage(AttributeTargets.Method)]
public class ValidateTokenAttribute : Attribute, IAuthorizationFilter
{
    public async void OnAuthorization(AuthorizationFilterContext context)
    {
        if (Constants.AllowAnonymous.Contains(context.HttpContext.Request.Path)) return;

        var configSettings =
            (context.HttpContext.RequestServices.GetService(typeof(ConfigSettings)) as ConfigSettings)!;
        var tokenService = (context.HttpContext.RequestServices.GetService(typeof(ITokenService)) as TokenService)!;
        var utilService = (context.HttpContext.RequestServices.GetService(typeof(IUtilService)) as UtilService)!;

        string? jwtToken = context.HttpContext.Request.Headers[configSettings.AuthSettings.HeaderName];
        string? refreshToken = context.HttpContext.Request.Headers[configSettings.AuthSettings.RefreshTokenHeaderName];

        var isValid = await tokenService.IsValid(utilService.GetTokenStringFromHeader(jwtToken), refreshToken!);

        if (!isValid)
            context.Result = new UnauthorizedObjectResult(new ErrorResult(Messages.YourSessionIsClosed.Translate()));
    }
}