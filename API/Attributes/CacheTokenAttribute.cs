using CORE.Abstract;
using CORE.Concrete;
using CORE.Middlewares.Translation;
using DTO.Responses;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace API.Attributes;

[AttributeUsage(AttributeTargets.Method)]
public class CacheTokenAttribute : Attribute, IAuthorizationFilter
{
    public void OnAuthorization(AuthorizationFilterContext context)
    {
        if (context.HttpContext.RequestServices.GetService(typeof(IUtilService)) is not UtilService utilService ||
            !utilService.IsTokenExistsInCache(context.HttpContext.Request.Headers.Authorization))
            context.Result =
                new UnauthorizedObjectResult(new ErrorResult(Localization.Translate(Messages.YourSessionIsClosed)));
    }
}